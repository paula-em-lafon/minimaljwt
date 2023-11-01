import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/movie.service'
import { Movie } from '../../Movie';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit {
  movies: Movie[] = [];
  isLoggedIn = false;

  constructor(private movieService: MovieService, private storageService: StorageService) { }

  ngOnInit(): void {
    this.movieService.getMovies().subscribe((movies) => this.movies = movies);
    this.storageService.getValueLoggedIn().subscribe(value => {
      this.isLoggedIn = value;
    });
  }
}
