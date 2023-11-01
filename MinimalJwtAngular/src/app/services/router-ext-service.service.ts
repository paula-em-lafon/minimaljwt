//import { Injectable } from '@angular/core';
//import { Router, RouterEvent, NavigationEnd, RoutesRecognized } from '@angular/router';
//import { filter, pairwise } from 'rxjs/operators';

///** A router wrapper, adding extra functions. */
//@Injectable({
//  providedIn: 'root'
//})
//export class RouterExtService {

//  private previousUrl: string | null = null;
//  private currentUrl: string | null= null;

//  constructor(private router: Router) {
//    this.currentUrl = this.router.url;
//    router.events.pipe(filter((evt: any) => evt instanceof RoutesRecognized), pairwise())
//      .subscribe((events: RoutesRecognized[]) => {
//        this.previousUrl = events[0].urlAfterRedirects;
//        this.currentUrl = events[1].urlAfterRedirects;
//      });
//  }

//  public getPreviousUrl() {
//    return this.previousUrl;
//  }
//}
