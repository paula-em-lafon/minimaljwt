import { Component, OnInit } from '@angular/core';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  isAdmin = false;
  isLoggedIn = false;

  constructor(private storageService: StorageService) { }

  ngOnInit() {
    this.storageService.getValueIsAdmin().subscribe(value => {
      this.isAdmin = value;
    });
    this.storageService.getValueLoggedIn().subscribe(value => {
      this.isLoggedIn = value;
    });
  }
}
