import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../Movie';

@Component({
  selector: 'app-edit-create-form',
  templateUrl: './edit-create-form.component.html',
  styleUrls: ['./edit-create-form.component.css']
})
export class EditCreateFormComponent implements OnInit {
  form: FormGroup;
  id: number;
  isAddMode: boolean;
  loading = false;
  submitted = false;
  toggleOverlay = false;


  constructor(
    private movieService: MovieService,
    public formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    console.log(this.isAddMode)

    this.form = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      rating: ['', Validators.required]
    });

    if (!this.isAddMode) {
      this.movieService.getMovie(this.id)
        .subscribe(x => this.form.patchValue(x));
    }
  }

  get f() { return this.form.controls; }

  public submitMovie() {
    this.toggleOverlay = true;

    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) {
      this.toggleOverlay = false;
      return;
    }

    if (!this.isAddMode) {
      this.editMovie(this.id);
    }
    else {
      this.createMovie();
    }
  }

  private createMovie() {
    this.movieService.createMovie(this.form.value)
      .subscribe({
        next: (responseMovie: Movie) => {
          this.finishCreateOrChange(responseMovie);
        },
        error: error => {
          this.finishCreateOrChange();
        }
      });
  }

  private editMovie(id: number) {
    this.movieService.editMovie(id, this.form.value)
      .subscribe({
        next: (responseMovie: Movie) => {
          this.finishCreateOrChange(responseMovie);
        },
        error: error => {
          this.finishCreateOrChange();
        }
      });
  }

  finishCreateOrChange(responseMovie?: Movie): void {
    this.toggleOverlay = false;
    if (responseMovie !== undefined) {

      this.router.navigate(['/movie/' + responseMovie.id])
    }
    else {
      alert("The movie could not be found")
    }
  }

  onCancel() {
    if (this.isAddMode) {
      this.router.navigate(['/']);
    }
    else {
      this.router.navigate(['/movie/' + this.id]);
    }
  }
}
