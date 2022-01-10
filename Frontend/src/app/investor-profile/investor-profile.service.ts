import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InvestorProfileService {

  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient) { }

  GetChooseProfile(){
    return this.http.get(this.APIURL + '/Profile/getuserprofiletypes',{ responseType: 'json' })
  }
  GetDistribution(){
    return this.http.get(this.APIURL + '/Profile/getdistributiontypes',{ responseType: 'json' })
  }
  GetStateorProvince(){
    return this.http.get(this.APIURL + '/Profile/getstateorprovincelist',{ responseType: 'json' })
  }
  CreateProfile(data : any){
    return this.http.post(this.APIURL + '/Profile/createuserprofile',data,{ responseType: 'json' })
  }
  GetProfileById(userid : any){
    return this.http.get(this.APIURL + '/Profile/getalluserprofile/' + userid,{responseType : 'json'})
  }
  UpdateProfile(data : any){
    return this.http.put(this.APIURL + '/Profile/updateuserprofile',data,{responseType : 'json'})
  }
  DeleteProfile(UserId : any,UserProfileId : any){
    return this.http.put(this.APIURL + '/Profile/deleteuserprofile/' + UserId + '/' + UserProfileId,{responseType : 'json'})
  }
  GetBankDetails(userid : any){
    return this.http.get(this.APIURL + '/BankAccount/getallbankaccounts/' + userid,{responseType : 'json'})
  }
  CreateBank(data : any){
    return this.http.post(this.APIURL + '/BankAccount/createbankaccount',data,{responseType : 'json'})
  }
  UpdateBank(data : any){
    return this.http.put(this.APIURL + '/BankAccount/updatebankaccount',data,{responseType : 'json'})
  }
  DeleteBank(UserId : any,BankProfileId : any){
    return this.http.put(this.APIURL + '/BankAccount/deletebankaccount/' + UserId + '/' + BankProfileId,{responseType : 'json'})
  }
  GetUserById(UserId : any){
    return this.http.get(this.APIURL + '/Authentication/getuserbyid/' + UserId,{responseType : 'json'})
  }
}
