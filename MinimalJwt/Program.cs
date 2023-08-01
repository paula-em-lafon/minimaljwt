using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MinimalJwt.Models;
using MinimalJwt.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Athentication with JWT Token",
        Type = SecuritySchemeType.Http
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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
      {
          options.Cookie.Name = "Authorization";
      })
  .AddJwtBearer(options =>
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
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["Jwt:Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
  };
});

builder.Services.AddCors();


builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMovieService, MovieService>();
//builder.Services.AddSingleton<IUserService, UserService>();

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
//app.UseAuthentication();

//app.MapGet("/", () => "Hello World!")
//    .ExcludeFromDescription();

//app.MapPost("/login",
//    (UserLogin user, IUserService service) => Login(user, service))
//    .Accepts<UserLogin>("application/json")
//    .Produces<string>();

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovieService service) => Create(movie, service))
    .Accepts<Movie>("application/json")
    .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IMovieService service) => Get(id, service))
    .Produces<Movie>();

app.MapGet("/list",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IMovieService service) => List(service))
    .Produces<List<Movie>>(statusCode: 200, contentType: "application/json");

app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovieService service) => Update(movie, service))
    .Accepts<Movie>("application/json")
    .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(int id, IMovieService service) => Delete(id, service));

//IResult Login(UserLogin user, IUserService service)
//{
//    if (!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
//    {
//        var loggedInUser = service.Get(user);
//        if (loggedInUser is null) return Results.NotFound("User not found");

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
//            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
//            new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
//            new Claim(ClaimTypes.Surname, loggedInUser.Surname),
//            new Claim(ClaimTypes.Role, loggedInUser.Role)
//        };

//        var token = new JwtSecurityToken
//            (
//                issuer: builder.Configuration["Jwt:Issuer"],
//                audience: builder.Configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddDays(1),
//                notBefore: DateTime.UtcNow,
//                signingCredentials: new SigningCredentials(
//                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//                    SecurityAlgorithms.HmacSha256)
//            );

//        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

//        return Results.Ok(tokenString);
//    }
//    return Results.BadRequest("Invalid user credentials");
//}

IResult Create(Movie movie, IMovieService service)
{
    var result = service.Create(movie);
    return Results.Ok(result);
}

IResult Get(int id, IMovieService service)
{
    var movie = service.Get(id);
    if (movie == null) return Results.NotFound("Movie Not Found");
    return Results.Ok(movie);
}

IResult List(IMovieService service)
{
    var movies = service.List();

    return Results.Ok(movies);
}

IResult Update(Movie newMovie, IMovieService service)
{
    var updatedMovie = service.Create(newMovie);
    
    if (updatedMovie == null) return Results.NotFound("Movie Not found");
    
    return Results.Ok(updatedMovie);
}

IResult Delete(int id, IMovieService service)
{
    var result = service.Delete(id);
    if (!result) Results.BadRequest("Something went wrong");
    return Results.Ok(result);
}

app.UseSwaggerUI();

app.Run();
