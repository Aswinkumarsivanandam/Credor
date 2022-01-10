import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InvestorService {
  APIURL = environment.BASE_URL;

  constructor(private http : HttpClient) { }

  GetInvestor(){
    return this.http.get(this.APIURL + '/Investor/getallinvestoraccounts',{responseType : 'json'});
  }
  GetInvestorById(InvestorId : any){
    return this.http.get(this.APIURL + '/Investor/getuseraccount/' + InvestorId,{responseType : 'json'});
  }
  GetInvestorTag(UserId : any){
    return this.http.get(this.APIURL + '/Investor/getinvestortags/' + UserId,{responseType : 'json'});
  }
  AddInvestorTag(data : any){
    return this.http.post(this.APIURL + '/Investor/addinvestortags',data,{responseType : 'json'});
  }
  UpdateInvestorTag(data : any){
    return this.http.put(this.APIURL + '/Investor/updateinvestortags',data,{responseType : 'json'});
  }
  GetInvestorNotes(InvestorId : any){
    return this.http.get(this.APIURL + '/Investor/getinvestornotes/' + InvestorId,{responseType : 'json'});
  }
  AddInvestorNotes(data : any){
    return this.http.post(this.APIURL + '/Investor/addinvestornotes',data,{responseType : 'json'});
  }
  UpdateInvestorNotes(data : any){
    return this.http.put(this.APIURL + '/Investor/updateinvestornotes',data,{responseType : 'json'});
  }
  DeleteInvestorNotes(UserId : any,InvestorId : any){
    return this.http.delete(this.APIURL + '/Investor/deleteinvestornotes/' + UserId + '/' + InvestorId,{responseType : 'json'});
  }
  ResetPassword(data : any){
    return this.http.put(this.APIURL + '/Investor/resetpassword',data,{responseType : 'json'});
  }
  ResetPasswordLink(data : any){
    return this.http.put(this.APIURL + '/Investor/sendresetpasswordlink',data,{responseType : 'json'});
  }
  VerifyAccreditInvestor(UserId : any,InvestorId : any,Isverify : any){
    return this.http.get(this.APIURL + '/Investor/updateaccreditation/' + UserId + '/' + InvestorId + '/' + Isverify,{responseType : 'json'});
  }
  GetInvestorHeadingSummary(InvestorId : any){
    return this.http.get(this.APIURL + '/Investor/getinvestorsummary/' + InvestorId,{responseType : 'json'});
  }
  GetInvestorsHeadingSummary(){
    return this.http.get(this.APIURL + '/Investor/getinvestorssummary',{responseType : 'json'});
  }
  GetInvestorReservationDetais(InvestorId : any){
    return this.http.get(this.APIURL + '/Investor/getreservations/' + InvestorId,{responseType : 'json'});
  }
  GetInvestorInvestmentDetails(InvestorId : any){
    return this.http.get(this.APIURL + '/Investor/getinvestments/' + InvestorId,{responseType : 'json'});
  }
  CreateReservation(data : any){
    return this.http.post(this.APIURL + '/Investor/addreservation',data,{responseType : 'json'});
  }
  UpdateReservation(data : any){
    return this.http.put(this.APIURL + '/Investor/updatereservation',data,{responseType : 'json'});
  }
  CreateInvestment(data : any){
    return this.http.post(this.APIURL + '/Investor/addinvestment',data,{responseType : 'json'});
  }
  UpdateInvestment(data : any){
    return this.http.put(this.APIURL + '/Investor/updateinvestment',data,{responseType : 'json'});
  }
  UpdateBatch(data : any){
    return this.http.put(this.APIURL + '/Investor/updatebatch',data,{responseType : 'json'});
  }
  DeleteDocument(UserId : any,batchId:any,documentId : any){
    return this.http.delete(this.APIURL + '/Investor/deletedocumentinbatchbyid/' + UserId + '/'+batchId+ '/' + documentId,{responseType : 'json'});
  }
  DeleteDocumentBatch(UserId : any,batchId : any){
    return this.http.delete(this.APIURL + '/Investor/deletedocumentbatch/' + UserId + '/' + batchId,{responseType : 'json'});
  }
  GetReservationList(){
    return this.http.get(this.APIURL + '/Investor/getreservationlist',{responseType : 'json'});
  }
  GetOfferingList(){
    return this.http.get(this.APIURL + '/Investor/getofferinglist',{responseType : 'json'});
  }
  GetInvestorProfiles(InvestorId:any){
    return this.http.get(this.APIURL + '/Investor/getalluserprofile/'+InvestorId,{responseType : 'json'});
  }
  GetAllDocumentBatches(){
    return this.http.get(this.APIURL + '/Investor/getalldocumentbatches',{responseType : 'json'});  
  }
  GetDocumentNameDelimiters(){
    return this.http.get(this.APIURL + '/Investor/getdocumentnamedelimiters',{responseType : 'json'});  
  }
  GetDocumentNamePositions(){
    return this.http.get(this.APIURL + '/Investor/getdocumentnamepositions',{responseType : 'json'});  
  }
  GetDocumentNameSeparators(){
    return this.http.get(this.APIURL + '/Investor/getdocumentnameseparators',{responseType : 'json'});  
  }
  GetDocumentTypes(){
    return this.http.get(this.APIURL + '/Documents/getdocumenttypes',{responseType : 'json'});  
  }
  GetDocumentBatchDetail(batchId : any){
    return this.http.get(this.APIURL + '/Investor/getdocumentbatchdetail/' + batchId,{responseType : 'json'});
  }  
  BulkDocumentUpload(formData : any){
    var headers =  new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    })
    return this.http.post(this.APIURL + '/Investor/bulkdocumentupload',formData,{headers : headers,responseType : 'json'});
  }  
  PublishMatchedDocuments(formData : any){
    var headers =  new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    })
    return this.http.post(this.APIURL + '/Investor/publishmatcheddocuments',formData,{headers : headers,responseType : 'json'});
  }
  PublishMatchedDocument(formData : any){
    var headers =  new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    })
    return this.http.post(this.APIURL + '/Investor/publishmatcheddocument',formData,{headers : headers,responseType : 'json'});
  }
}
