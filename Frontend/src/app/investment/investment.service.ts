import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InvestmentService {
  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private router : Router) { }

  CreateInvestment(data : any){
    return this.http.post(this.APIURL + '/Investment/addinvestment',data,{responseType : 'json'});
  }
  UpdateInvestment(data : any){
    return this.http.put(this.APIURL + '/Investment/updateinvestment',data,{responseType : 'json'});
  }
  GetInvestmentByUser(UserId : any){
    return this.http.get(this.APIURL + '/MyInvestment/getinvestmentandreservationbyuserid/' + UserId,{responseType : 'json'});
  }
  GetInvestmentDetailsById(UserId : any,OfferingId : any){
    return this.http.get(this.APIURL + '/MyInvestment/getinvestmentdetailbyid/' + UserId + '/' + OfferingId,{responseType : 'json'});
  }
}
