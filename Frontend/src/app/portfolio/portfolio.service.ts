import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PortfolioService {
  APIURL = environment.BASE_URL;
  constructor(private http : HttpClient,private route : Router) { }

  GetPortfolioOfferings(){
    return this.http.get(this.APIURL + '/portfolio/getportfolioofferings' ,{responseType : 'json'});
  }

  SavePortfolioOffering(data : any){
    return this.http.post(this.APIURL + '/portfolio/addportfoliooffering', data,{responseType : 'json'});
  }

  UpdatePortfolioOffering(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateportfoliooffering', data,{responseType : 'json'});
  }

  deletePortfolioOffering(AdminId : any , Id : any){
    return this.http.delete(this.APIURL + '/portfolio/deleteportfoliooffering' + '/' + AdminId + '/' + Id ,{responseType : 'json'});
  }

  GetPortfolioReservation(){
    return this.http.get(this.APIURL + '/portfolio/getportfolioreservations' ,{responseType : 'json'});
  }

  SavePortfolioReservation(data : any){
    return this.http.post(this.APIURL + '/portfolio/addportfolioreservation', data,{responseType : 'json'});
  }
  
  UpdatePortfolioReservation(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateportfolioreservation', data,{responseType : 'json'});
  }

  deletePortfolioReservation(AdminId : any , Id : any){
    return this.http.delete(this.APIURL + '/portfolio/deleteportfolioreservation' + '/' + AdminId + '/' + Id ,{responseType : 'json'});
  }

  getPortfoliobyOfferingStatus(Id: any){
    return this.http.get(this.APIURL + '/portfolio/getportfoliooffering' + '/' + Id ,{responseType : 'json'});
  }

  getPortfoliobyReservationStatus(Id: any){
    return this.http.get(this.APIURL + '/portfolio/getportfolioreservation' + '/' + Id ,{responseType : 'json'});
  }

  getportfolioArchives(){
    return this.http.get(this.APIURL + '/portfolio/getarchivedportfolioofferingandreservations' ,{responseType : 'json'});
  }

  RestoreArchivesReservation(AdminId : any , Id : any){
    return this.http.put(this.APIURL + '/portfolio/restoreportfolioreservation' + '/' + AdminId + '/' + Id ,{responseType : 'json'});
  }

  RestoreArchivesOffering(AdminId : any , Id : any){
    return this.http.put(this.APIURL + '/portfolio/restoreportfoliooffering' + '/' + AdminId + '/' + Id ,{responseType : 'json'});
  }

  getSubscription(){
    return this.http.get(this.APIURL + '/Documents/getallsubscriptiondocuments' ,{responseType : 'json'});
  }

  UpdateGalleryImage(data : any){
    var headers =  new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    })
    return this.http.post(this.APIURL + '/portfolio/uploadportfoliogallery', data,{headers : headers,responseType : 'json'});
  }

  SaveSummary(data : any){
    return this.http.post(this.APIURL + '/portfolio/addportfoliosummary', data,{responseType : 'json'});
  }

  UpdateSummary(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateportfoliosummary', data,{responseType : 'json'});
  }

  AddDocument(data : any){
    return this.http.post(this.APIURL + '/portfolio/uploadofferingdocuments', data,{responseType : 'json'});
  }

  SetDefaultImageAndRemoveImage(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateportfoliogallery', data,{responseType : 'json'});
  }

  RemoveDocument(adminId : any , Id : any, documentId : any){
    return this.http.delete(this.APIURL + '/portfolio/deleteofferingdocument' + '/' + adminId + '/' + Id + '/' + documentId ,{responseType : 'json'});
  }

  UpdateKeys(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateportfoliokeyhightlight', data,{responseType : 'json'});
  }

  SaveFunds(data : any){
    return this.http.post(this.APIURL + '/portfolio/addportfoliofundinginstructions', data,{responseType : 'json'});
  }

  UpdateFunds(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateportfoliofundinginstructions', data,{responseType : 'json'});
  }

  getInvestor(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getportfolioofferinginvestments' + '/' + Id ,{responseType : 'json'});
  }

  getInvestorSummary(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getportfolioinvestorssummary' + '/' + Id ,{responseType : 'json'});
  }

  getUserList(){
    return this.http.get(this.APIURL + '/Investor/getallinvestoraccounts',{responseType : 'json'});
  }

  getProfileType(investorid:any){
    return this.http.get(this.APIURL + '/Investor/getalluserprofile' + '/' + investorid,{responseType : 'json'});
  }

  getStatusType(){
    return this.http.get(this.APIURL + '/portfolio/getinvestmentstatuses', {responseType : 'json'});
  }

  SaveInvestment(data : any){
    var headers =  new HttpHeaders({
      'contentType':'application/json'
    })
    return this.http.post(this.APIURL + '/portfolio/addinvestment', data, {headers : headers,responseType : 'json'});
  }

  UpdateInvestment(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateinvestment', data, {responseType : 'json'});
  }

  saveNotes(data : any){
    return this.http.put(this.APIURL + '/portfolio/addinvestmentnotes', data,{responseType : 'json'});
  }

  deleteInvestment(adminId : any , Id : any){
    return this.http.delete(this.APIURL + '/portfolio/deleteinvestment' + '/' + adminId + '/' + Id ,{responseType : 'json'});
  }

  getOfferingReservationSummary(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getportfolioreservationssummary' + '/' + Id ,{responseType : 'json'});
  }

  getOfferingReservationList(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getportfolioofferingReservations' + '/' + Id ,{responseType : 'json'});
  }

  getReservationSummary(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getreservationssummary' + '/' + Id ,{responseType : 'json'});
  }
  
  getReservationList(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getreservationslist' + '/' + Id ,{responseType : 'json'});
  }
 
  SaveNewReservation(data : any){
    return this.http.post(this.APIURL + '/portfolio/addreservation', data,{responseType : 'json'});
  }

  deleteReservation(adminId : any , Id : any){
    return this.http.delete(this.APIURL + '/portfolio/deletereservation' + '/' + adminId + '/' + Id ,{responseType : 'json'});
  }

  UpdateNewReservation(data : any){
    return this.http.put(this.APIURL + '/portfolio/updatereservation', data,{responseType : 'json'});
  }
  
  saveNewReservationNotes(data : any){
    return this.http.put(this.APIURL + '/portfolio/addreservationnotes', data,{responseType : 'json'});
  }

  getCapTable(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getportfolioofferingcaptable' + '/' + Id ,{responseType : 'json'});
  }
  
  updateOwnership(data : any){
    return this.http.put(this.APIURL + '/portfolio/updatecaptable', data,{responseType : 'json'});
  }
  
  GetDistributionHistory(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getofferingdistributions' + '/' + Id ,{responseType : 'json'});
  }
 
  deleteHistory(Id : any , adminId : any){
    return this.http.delete(this.APIURL + '/portfolio/deleteofferingdistribution' + '/' + Id + '/' + adminId ,{responseType : 'json'});
  }

  getViewHistoryList(Id : any){
    return this.http.get(this.APIURL + '/portfolio/getofferingdistributiondetail' + '/' + Id ,{responseType : 'json'});
  }
  
  getHistoryType(){
    return this.http.get(this.APIURL + '/portfolio/getportfoliodistributiontypes' ,{responseType : 'json'});
  }

  UpdateHistory(data : any){
    return this.http.put(this.APIURL + '/portfolio/updateofferingdistribution', data,{responseType : 'json'});
  }

  addDistribution(data : any){
    return this.http.post(this.APIURL + '/portfolio/addofferingdistributions', data,{responseType : 'json'});
  }
  
  getInvestorNames(Id : any){
    return this.http.get(this.APIURL + '/portfolio/GetInvestors' + '/' + Id ,{responseType : 'json'});
  }
}
