import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { concatWith, lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
 
  private http = inject(HttpClient);
  protected  title = 'Dating app';
  protected members = signal<any>([])
  
  async ngOnInit() {
   this.members.set(await this.GetMembers())
  }

  async GetMembers(){
    try{
      return lastValueFrom(this.http.get('https://localhost:9001/api/members'));
    }
    catch(error){
      console.log(error);
      throw error;
    }
  }

}
