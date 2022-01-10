import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LeadService {
  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient) { }
  GetLead(){
    return this.http.get(this.APIURL + '/Lead/getallleadaccounts',{responseType : 'json'});
  }
  CreateLead(data : any){
    return this.http.post(this.APIURL + '/Lead/addleadaccount',data,{responseType : 'json'});
  }
  UpdateLead(data : any){
    return this.http.put(this.APIURL + '/Lead/updateleadaccount/',data,{responseType : 'json'});
  }
  DeleteLead(data : any){
    return this.http.put(this.APIURL + '/Lead/deleteleads',data,{responseType : 'json'});
  }
  GetLeadSummary(){
    return this.http.get(this.APIURL + '/Lead/getleadsummary',{responseType : 'json'});
  }
  AddNotes(data : any){
    return this.http.post(this.APIURL + '/Lead/addleadnotes',data,{responseType : 'json'});
  }
  UpdateNotes(data : any){
    return this.http.put(this.APIURL + '/Lead/updateleadnotes',data,{responseType : 'json'});
  }
  DeleteNotes(UserId : any,LeadNoteId : any){
    return this.http.delete(this.APIURL + '/Lead/deleteleadnotes/' + UserId + '/' + LeadNoteId,{responseType : 'json'});
  }
  GetLeadNotes(LeadId : any){
    return this.http.get(this.APIURL + '/Lead/getleadnotes/'+ LeadId,{responseType : 'json'});
  }
  MultipleInviteLead(data : any){
    return this.http.put(this.APIURL + '/Lead/resendinvitemultipleleads',data,{responseType : 'json'});
  }
  SingleInviteLead(UserId : any,LeadId : any){
    return this.http.get(this.APIURL + '/Lead/resendinvite/' + UserId + '/' + LeadId,{responseType : 'json'});
  }
  GetLeadTag(UserId : any){
    return this.http.get(this.APIURL + '/Lead/getleadtags/' + UserId,{responseType : 'json'});
  }
  AddLeadTag(data : any){
    return this.http.post(this.APIURL + '/Lead/addleadtags',data,{responseType : 'json'});
  }
  UpdateLeadTag(data : any){
    return this.http.put(this.APIURL + '/Lead/updateleadtags',data,{responseType : 'json'})
  }
  DeleteLeadTag(UserId : any,LeadId : any){
    return this.http.delete(this.APIURL + '/Lead/deleteleadtags/'+ UserId +'/' + LeadId,{responseType : 'json'});
  }
  VerifyAccount(UserId : any,InvestorId : any,Verify : any){
    return this.http.get(this.APIURL + '/Investor/accountverification/' + UserId + '/' + InvestorId + '/' + Verify,{responseType : 'json'});
  }
}
