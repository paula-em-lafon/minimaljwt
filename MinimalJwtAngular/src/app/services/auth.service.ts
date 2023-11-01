import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { StorageService } from './storage.service';
import { User } from '../User';


@Injectable({
  providedIn: 'root'
})
export class AuthService {


  private userApiUrl = 'http://localhost:5296';

  constructor(private http: HttpClient, private storageService: StorageService) { }

  async tryRefreshingTokens(): Promise<boolean> {

    const token: any = this.storageService.getToken();
    const refreshToken: any = this.storageService.getRefreshToken();
    if (!token || !refreshToken) {
      return false;
    }

    const credentials = JSON.stringify({ refreshToken: refreshToken });
    let isRefreshSuccess: boolean;
    
    const refreshRes = await new Promise<any>((resolve, reject) => {
      this.http.post<any>(this.userApiUrl +"/refreshToken", credentials, {
        withCredentials: true,
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe({
        next: (res: any) => resolve(res),
        error: (_) => { reject; isRefreshSuccess = false; }
      });
    });
    this.storageService.saveUser(refreshRes);
    isRefreshSuccess = true;
    return isRefreshSuccess;
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post(
      this.userApiUrl + '/login',
      {
        username,
        password
      },
      
    )
  }

  create(user: User): Observable<User> {
    return this.http.post<User>(this.userApiUrl + "/create", user)
  }

  getUser(): Observable<User> {
    return this.http.get<User>(this.userApiUrl + "/getUser", { withCredentials: true })
  }

  updateUser(user: User): Observable<User> {
    return this.http.put<User>(this.userApiUrl + "/update", user, { withCredentials: true })
  }

  deleteUser(): Observable<boolean> {
    return this.http.delete<boolean>(this.userApiUrl + "/delete", { withCredentials: true })
  }
}
