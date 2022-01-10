import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DistributionService {
  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private route : Router) { }

  GetDistributionByUserId(UserId : any){
    return this.http.get(this.APIURL + '/Distributions/getalldistributions/' + UserId,{responseType : 'json'});
  }
}
