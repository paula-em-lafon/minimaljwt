import { Component, Input, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: 'app-movie-page',
  templateUrl: './movie-page.component.html',
  styleUrls: ['./movie-page.component.css']
})
export class MoviePageComponent implements OnInit {
  private cookieJwtVal = '';
  private permissions = 0;

  constructor(private cookieService: CookieService) { }


  ngOnInit(): void {
    this.cookieJwtVal = this.cookieService.get('Authoriztion');
  }
}
