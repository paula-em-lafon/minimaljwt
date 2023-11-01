import { Component, Input, OnInit } from '@angular/core';
import { Movie } from '../../Movie';
import { Router } from '@angular/router';

@Component({
  selector: 'app-movie-item',
  templateUrl: './movie-item.component.html',
  styleUrls: ['./movie-item.component.css']
})
export class MovieItemComponent implements OnInit {
  @Input() movie: Movie;
  @Input() isLoggedIn: boolean;
  movieLink: string;
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.movieLink = `/movie/${this.movie.id}`;
  }
}
