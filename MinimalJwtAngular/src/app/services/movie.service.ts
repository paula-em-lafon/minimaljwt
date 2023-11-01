import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Movie } from '../Movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private movieApiUrl = 'http://localhost:5224';

  constructor(private http: HttpClient) { }

  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.movieApiUrl + "/list", { withCredentials: true});
  };

  getMovie(id: number): Observable<Movie> {
    return this.http.get<Movie>(this.movieApiUrl + "/get?id=" + id, { withCredentials: true })
  }

  deleteMovie(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.movieApiUrl + "/delete?id=" + id, { withCredentials: true })
  }

  createMovie(values: any): Observable<Movie> {
    return this.http.post<Movie>(this.movieApiUrl + "/create", values, { withCredentials: true })
  }

  editMovie(id: number, values: any): Observable<Movie> {
    return this.http.put<Movie>(this.movieApiUrl + "/update?id=" + id, values, { withCredentials: true })
  }
}
