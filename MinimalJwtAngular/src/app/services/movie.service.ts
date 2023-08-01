import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { MOVIES } from '../mock-movies'
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
}
