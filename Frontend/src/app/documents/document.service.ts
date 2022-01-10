import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
APIURL = environment.BASE_URL;

  constructor(private http : HttpClient,private route : Router) { }

  GetAllDocument(UserId : any){
    return this.http.get(this.APIURL + '/Documents/getalldocuments/' + UserId,{responseType : 'json'});
  }
  DeleteDocumentById(UserId : any,DocId : any){
    return this.http.delete(this.APIURL + '/Documents/deletedocument/' + UserId + '/' + DocId,{responseType : 'json'});
  }
  UploadDocument(formData : any){
    var headers =  new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    })
    return this.http.post(this.APIURL + '/Documents/uploaddocuments',formData,{headers : headers,responseType : 'json'});
  }
}
