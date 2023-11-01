import { Component, Input, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { Movie } from '../../Movie';
import { MovieService } from '../../services/movie.service';
import { ActivatedRoute, Router } from '@angular/router';
import { StorageService } from '../../services/storage.service';



@Component({
  selector: 'app-movie-page',
  templateUrl: './movie-page.component.html',
  styleUrls: ['./movie-page.component.css']
})
export class MoviePageComponent implements OnInit {
  id: number;
  movie: Movie | null = null;
  deleted: boolean = false;
  isAdmin = false;
  toggleOverlay = false;
  editLink: string;

  private sub: any;

  constructor(private storageService: StorageService, private movieService: MovieService, private route: ActivatedRoute, public router: Router) { }


  ngOnInit(): void {
    this.storageService.getValueIsAdmin().subscribe(value => {
      this.isAdmin = value;
    });
    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id']; // (+) converts string 'id' to a number

      this.movieService.getMovie(this.id).subscribe((movie) => this.movie = movie);
    });

    this.editLink = `/movie/edit/${this.id}`;
  }



  delete(): void {
    this.toggleOverlay = true;
    this.movieService.deleteMovie(this.id).subscribe({
      next: (value) => {
        this.deleted = value;
        this.finishDelete()
      },
      error: (error) => {
        this.finishDelete()
      }
  })
  }

  finishDelete(): void {
    this.toggleOverlay = false;
    if (this.deleted == true) {
      this.router.navigate(['/'])
    }
    else {
      alert("The page could not be deleted")
    }
  }


  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
