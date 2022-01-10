import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UpdatesService {
  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private router : Router) { }

  GetUpdates(UserId : any){
    return this.http.get(this.APIURL + '/Updates/getallportfolioupdates/'+ UserId,{responseType : 'json'});
  }
}
