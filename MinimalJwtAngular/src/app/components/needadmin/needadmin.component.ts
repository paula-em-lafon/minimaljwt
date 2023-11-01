import { Component, OnInit } from '@angular/core';
import { StorageService } from '../../services/storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-needadmin',
  templateUrl: './needadmin.component.html',
  styleUrls: ['./needadmin.component.css']
})
export class NeedadminComponent implements OnInit {
  private LastPageThis: string | null = null;

  constructor(private storageService: StorageService, public router: Router) { }

  ngOnInit() {
    this.storageService.getValueLastPage().subscribe(value => {
      this.LastPageThis = value;
    });
  }
}
