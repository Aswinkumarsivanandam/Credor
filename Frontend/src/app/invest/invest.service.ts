import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InvestService {

  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private router : Router) { }

  GetMarketplace(){
    return this.http.get(this.APIURL + '/Investment/GetOfferingsAndReservations',{ responseType: 'json' })
  }
  GetMarketplaceById(reservationId : any){
    return this.http.get(this.APIURL + '/Investment/getOfferingDetailById/'+ reservationId,{ responseType: 'json' })
  }
  CreateReservation(data : any){
    return this.http.post(this.APIURL + '/Investment/addreservation',data,{responseType : 'json'});
  }
  GetReservationById(UserId : any){
    return this.http.get(this.APIURL + '/Investment/getreservationdetailbyid/'+UserId,{responseType : 'json'})
  }
  UpdateReservation(data : any){
    return this.http.put(this.APIURL + '/Investment/updatereservation',data,{responseType : 'json'});
  }
  GetHeaderDataByMyInvestment(UserId : any){
    return this.http.get(this.APIURL + '/MyInvestment/getheaderelements/' + UserId,{responseType : 'json'});
  }
}
