using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalJwtAuth.Exceptions;
using MinimalJwtAuth.Models;
using MinimalJwtAuth.Services;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Cookie,
        Name = "Authorization",
        Description = "Bearer Athentication with JWT Token",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["Authorization"];
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

});

builder.Services.AddCors();

builder.Services.AddAuthorization();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.AllowAnyOrigin()
//                          .AllowAnyHeader().AllowAnyMethod();
//                      });
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();

app.UseCors(policy =>
{
    policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => "Hello from auth")
    .ExcludeFromDescription();

app.MapPost("/login",
    (UserLogin user, IUserService service) => Login(user, service))
    .Produces<string>();

app.MapPost("/refreshToken",
(OldRefreshToken oldRefreshToken, IUserService service) => RefreshToken(oldRefreshToken, service))
    .Produces<string>();

app.MapGet("/getUser",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IUserService service) => GetUser(service))
    .Produces<IEnumerable<String>>(statusCode: 200, contentType: "application/json");

app.MapPost("/create",
     (User user, IUserService service) => Create(user, service))
    .Produces<User>();

app.MapPut("/update",
     (User user, IUserService service) => Update(user, service))
    .Produces<User>();

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IUserService service) => Delete(service));
//app.MapGet("/list",
//    (IMovieService service) => List(service))
//    .Produces<List<Movie>>(statusCode: 200, contentType: "application/json");

//app.MapPut("/update",
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
//(Movie movie, IMovieService service) => Update(movie, service))
//    .Accepts<Movie>("application/json")
//    .Produces<Movie>(statusCode: 200, contentType: "application/json");

//app.MapDelete("/delete",
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
//(int id, IMovieService service) => Delete(id, service));

IResult Login(UserLogin user, IUserService service)
{
    if (!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = service.Get(user);
        if (loggedInUser is null) return Results.NotFound("User not found");

        var tokenExpiry = DateTime.UtcNow.AddMinutes(1);

        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, loggedInUser.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
            new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
            new Claim(ClaimTypes.Surname, loggedInUser.Surname),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };

        var token = new JwtSecurityToken
            (
                issuer: builder.Configuration["Jwt:Issuer"],
                audience: builder.Configuration["Jwt:Audience"],
                claims: claims,
                expires: tokenExpiry,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        loggedInUser.RefreshToken = service.GenerateRefreshToken();
        loggedInUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        var returnable = new LoggedIn
        {
            Id = loggedInUser.Id,
            Token = tokenString,
            TokenExpiry = tokenExpiry,
            RefreshToken = loggedInUser.RefreshToken,
            RefreshTokenExpiry = loggedInUser.RefreshTokenExpiryTime,
            Role = loggedInUser.Role,
            UserName = loggedInUser.Username,
            GivenName = loggedInUser.GivenName,
            Surname = loggedInUser.Surname,

        };

        return Results.Ok(returnable);
    }
    return Results.BadRequest("Invalid user credentials");
}

IResult RefreshToken(OldRefreshToken oldRefreshToken, IUserService service)
{
    var loggedInUser = service.GetCurrentUserById();
    if (loggedInUser is null) return Results.NotFound("User not found");


    if (oldRefreshToken.RefreshToken != loggedInUser.RefreshToken || loggedInUser.RefreshTokenExpiryTime <= DateTime.Now) return Results.BadRequest("Imposible to refresh token");

    var tokenExpiry = DateTime.UtcNow.AddMinutes(1);

    var claims = new[]
        {
            new Claim(ClaimTypes.Sid, loggedInUser.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
            new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
            new Claim(ClaimTypes.Surname, loggedInUser.Surname),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };

    var token = new JwtSecurityToken
        (
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: tokenExpiry,
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    loggedInUser.RefreshToken = service.GenerateRefreshToken();
    loggedInUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    var returnable = new LoggedIn
    {
        Id = loggedInUser.Id,
        Token = tokenString,
        TokenExpiry = tokenExpiry,
        RefreshToken = loggedInUser.RefreshToken,
        RefreshTokenExpiry = loggedInUser.RefreshTokenExpiryTime,
        Role = loggedInUser.Role,
        UserName = loggedInUser.Username,
        GivenName = loggedInUser.GivenName,
        Surname = loggedInUser.Surname,

    };

    return Results.Ok(returnable);

}

IResult GetUser(IUserService service)
{
    var user = service.GetCurrentUserById();
    var returnable = new User
    {
        Id = user.Id,
        Username = user.Username,
        EmailAddress = user.EmailAddress,
        GivenName = user.GivenName,
        Surname = user.Surname,
        Role = user.Role,
    };
    return Results.Ok(returnable);
}

IResult Create(User user, IUserService service)
{
    try
    {
        var result = service.Create(user);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        if (ex is UserException)
        {
            return Results.Conflict(ex.Message);
        }
        else
        {
            return Results.Problem(ex.Message);
        }
    }
}

IResult Update(User newUser, IUserService service)
{
    try
    {
        var result = service.Update(newUser);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        if (ex is UserException)
        {
            return Results.Conflict(ex.Message);
        }
        else
        {
            return Results.Problem(ex.Message);
        }
    }
}

IResult Delete(IUserService service)
{
    var result = service.Delete();
    if (!result) Results.BadRequest("Something went wrong");
    return Results.Ok(result);
}

//IResult Get(int id, IMovieService service)
//{
//    var movie = service.Get(id);
//    if (movie == null) return Results.NotFound("Movie Not Found");
//    return Results.Ok(movie);
//}

//IResult List(IMovieService service)
//{
//    var movies = service.List();

//    return Results.Ok(movies);
//}

//IResult Update(Movie newMovie, IMovieService service)
//{
//    var updatedMovie = service.Create(newMovie);

//    if (updatedMovie == null) return Results.NotFound("Movie Not found");

//    return Results.Ok(updatedMovie);
//}

//IResult Delete(int id, IMovieService service)
//{
//    var result = service.Delete(id);
//    if (!result) Results.BadRequest("Something went wrong");
//    return Results.Ok(result);
//}

app.UseSwaggerUI();

app.Run();
