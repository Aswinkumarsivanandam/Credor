import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  APIURL = environment.BASE_URL

  constructor(private http: HttpClient, private router: Router) { }

  GetProfile(UserId: any) {
    return this.http.get(this.APIURL + '/Account/getuseraccount/' + UserId, { responseType: 'json' });
  }
  UpdateProfile(data: any) {
    return this.http.put(this.APIURL + '/Account/updateuseraccount/', data, { responseType: 'json' });
  }
  UpdatePassword(data : any){
    return this.http.put(this.APIURL + '/Account/updatepassword',data,{responseType : 'json'});
  }
  UpdateProfileImage(data : any){
    var headers =  new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    })
    return this.http.put(this.APIURL + '/Account/uploadprofileimage/',data,{headers : headers,responseType : 'json'});
  }
  SendOtp(data : any){
    return this.http.put(this.APIURL + '/Account/sendonetimepassword',data,{responseType : 'json'})
  }
  VerifyPhone(data : any){
    return this.http.put(this.APIURL + '/Account/verifyuserphonenumber',data,{responseType : 'json'})
  }
  VerifyEmail(data : any){
    return this.http.put(this.APIURL + '/Account/verifyuseremailid',data,{responseType : 'json'})
  }
  ResendOtp(data : any){
    return this.http.put(this.APIURL + '/Account/resendonetimepassword',data,{responseType : 'json'});
  }
  AddNewUser(data : any){
    return this.http.post(this.APIURL + '/Account/addnewuseraccount',data,{responseType : 'json'});
  }
  UpdateNewUser(data : any){
    return this.http.put(this.APIURL + '/Account/updatenewuseraccount/',data,{responseType : 'json'});
  }
  GetAccountAccesstoOthers(UserId : any){
    return this.http.get(this.APIURL + '/Account/getaccessgrantedtoothers/' + UserId,{responseType : 'json'});
  }
  DeleteNewUser(UserId : any,NewId : any){
    return this.http.delete(this.APIURL + '/Account/deletenewuseraccount/' + UserId + '/' + NewId,{responseType : 'json'});
  }
  GetNotification(UserId : any){
    return this.http.get(this.APIURL + '/Notification/getusernotifications/' + UserId,{responseType : 'json'});
  }
  UpdateNotification(data : any){
    return this.http.post(this.APIURL + '/Notification/updatenotification',data,{responseType : 'json'})
  }
  ClearAllNotification(UserId : any){
    return this.http.delete(this.APIURL + '/Notification/clearallnotifications/' + UserId,{responseType : 'json'});
  }
}
