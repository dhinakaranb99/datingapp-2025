import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  baseUrl = "https://localhost:9001/api/";
  register(creds: RegisterCreds){
    return this.http.post<User>(this.baseUrl + 'account/register', creds).pipe(
      tap(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUser.set(user)
        }
      })
    )
  }

  login(creds :any){
    return this.http.post<User>(this.baseUrl +'account/login',creds).pipe(
      tap(user=>{
        if(user) {
          localStorage.setItem('user',JSON.stringify(user))
          this.currentUser.set(user)
        }
      })
    )
  }
  setCurrentUser(user:User){
   localStorage.setItem('user',JSON.stringify(user))
   this.currentUser.set(user)
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUser.set(null)
  }
}
