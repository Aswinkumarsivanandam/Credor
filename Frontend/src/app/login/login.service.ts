import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private router : Router) { }

  LoginData(data : any):Observable<any>{
    return this.http.post(this.APIURL + '/Authentication/login',data,{ responseType: 'json' })
  }
  ForgotPassword(data : any){
    return this.http.put(this.APIURL + '/Authentication/forgotpassword',data,{ responseType: 'json' })
  }
  GetUser(){
    return this.http.get(this.APIURL + '/Authentication/getallusers',{ responseType: 'json' })
  }
  ResetPassword(data : any){
    return this.http.put(this.APIURL + '/Authentication/resetpassword',data,{ responseType: 'json' })
  }
  GetUserById(UserId : any){
    return this.http.get(this.APIURL + '/Authentication/getuserbyid/' + UserId,{responseType : 'json'})
  }
  GetUserAccount(UserId :any)  {
    return this.http.get(this.APIURL + '/Account/getuseraccount/' + UserId,{responseType : 'json'})
  }
  GetRoleMapping(RoleId : any){
    return this.http.get(this.APIURL + '/Authentication/getrolefeaturemapping/' + RoleId,{responseType : 'json'});
  }
}
