import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users:any;
  
  // Dependency injection like c#
  constructor( private http:HttpClient) {  }

  ngOnInit() {
    this.http.get("https://localhost:7180/api/users")
    .subscribe(response=>{
      this.users = response;
      console.log(this.users);
    },error=>{
      console.log(error);
    })
  }

}
