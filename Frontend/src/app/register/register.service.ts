import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private router : Router) { }

  RegisterData(data : any):Observable<any>{
    return this.http.post(this.APIURL + '/Authentication/register',data,{ responseType: 'json' })
  }
  Reset(data : any):Observable<any>{
    return this.http.post(this.APIURL + '/Authentication/resetpassword',data,{ responseType: 'json' })
  }
}
