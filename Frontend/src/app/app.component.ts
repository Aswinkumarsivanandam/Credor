import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Credor';

  constructor(private activatedRoute : ActivatedRoute,){
    this.activatedRoute.url.subscribe(url =>{
      let enURL = window.location.href;
      if(enURL.includes('/login')){

      }
      else if(enURL.includes('/register')){

      }
      else if(enURL.includes('/forgot-password')){

      }
      else if(enURL.includes('reset-password/:token')){

      }
    })
    localStorage.setItem('viewDetails', 'false');
    localStorage.removeItem("RouteName");
  }
  ngOnInit(): void {
  }
}
