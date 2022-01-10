import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DocumentService } from './document.service';
import { FileHandle } from './file.directive';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.css']
})
export class DocumentsComponent implements OnInit {
  Selected: any;
  TaxdocumentShow: boolean = false;
  SubscriptionShow: boolean = false;
  AccreditationShow: boolean = false;
  OfferingdocumentShow: boolean = false;
  MiscellaneouShow: boolean = false;
  UpdatesShow: boolean = false;
  DocumentData: any = [];
  TaxDocumentsValue: any = [];
  SubscriptionsValue: any = [];
  AccreditationValue: any = [];
  OfferingDocumentsValue: any = [];
  UpdatesValue: any = [];
  MiscellaneousValue: any = [];
  UserId: any;
  DeleteDocumentPopup: boolean = false;
  Loader: boolean = false;
  DeleteData: any;
  allowedFileExtensions: any = [];
  DocumentFile: any = [];
  filesToUpload: any = [];
  uploadFile: boolean = false;
  files: FileHandle[] = [];
  TypeId: any;
  DocSizeBool: boolean = false;
  DocSizeCount: any = 0;
  ViewFile: any;
  DocumentViewPopup : boolean = false;

  constructor(private documentService: DocumentService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.UserId = Number(localStorage.getItem('UserId'));
    this.allowedFileExtensions = ['txt', 'pdf', 'docx', 'doc', 'xlsx', 'csv', 'pptx', 'jpg', 'jpeg', 'JPEG', 'png', 'PNG', 'gif', 'mp4', 'avi', 'xls'];
    this.selectSubscriptions();
    this.GetDocument();
  }
  selectTaxdocuments() {
    this.Selected = 'Taxdocuments'
    this.TaxdocumentShow = true;
    this.SubscriptionShow = false;
    this.AccreditationShow = false;
    this.OfferingdocumentShow = false;
    this.MiscellaneouShow = false;
    this.UpdatesShow = false;
    this.TypeId = 1;
  }
  selectSubscriptions() {
    this.Selected = 'Subscriptions'
    this.TaxdocumentShow = false;
    this.SubscriptionShow = true;
    this.AccreditationShow = false;
    this.OfferingdocumentShow = false;
    this.MiscellaneouShow = false;
    this.UpdatesShow = false;
    this.TypeId = 2;
  }
  selectAccreditation() {
    this.Selected = 'Accreditation'
    this.TaxdocumentShow = false;
    this.SubscriptionShow = false;
    this.AccreditationShow = true;
    this.OfferingdocumentShow = false;
    this.MiscellaneouShow = false;
    this.UpdatesShow = false;
    this.TypeId = 3;
  }
  selectOfferingdocuments() {
    this.Selected = 'Offeringdocuments'
    this.TaxdocumentShow = false;
    this.SubscriptionShow = false;
    this.AccreditationShow = false;
    this.OfferingdocumentShow = true;
    this.MiscellaneouShow = false;
    this.UpdatesShow = false;
    this.TypeId = 4;
  }
  selectMiscellaneous() {
    this.Selected = 'Miscellaneous'
    this.TaxdocumentShow = false;
    this.SubscriptionShow = false;
    this.AccreditationShow = false;
    this.OfferingdocumentShow = false;
    this.MiscellaneouShow = true;
    this.UpdatesShow = false;
    this.TypeId = 5;
  }
  selectUpdates() {
    this.Selected = 'Updates'
    this.TaxdocumentShow = false;
    this.SubscriptionShow = false;
    this.AccreditationShow = false;
    this.OfferingdocumentShow = false;
    this.MiscellaneouShow = false;
    this.UpdatesShow = true;
    this.TypeId = 6;
  }
  GetDocument() {
    this.documentService.GetAllDocument(this.UserId).subscribe(data => {
      this.DocumentData = data;
      this.TaxDocumentsValue = this.DocumentData.filter((x: { type: number; }) => x.type == 1)
      this.SubscriptionsValue = this.DocumentData.filter((x: { type: number; }) => x.type == 2)
      this.AccreditationValue = this.DocumentData.filter((x: { type: number; }) => x.type == 3)
      this.OfferingDocumentsValue = this.DocumentData.filter((x: { type: number; }) => x.type == 4)
      this.MiscellaneousValue = this.DocumentData.filter((x: { type: number; }) => x.type == 5)
      this.UpdatesValue = this.DocumentData.filter((x: { type: number; }) => x.type == 6)
      this.Loader = false;
      console.log(this.AccreditationValue,'AccreditationValue')
    })
  }
  OnDelete(val: any) {
    this.DeleteData = val;
    this.DeleteDocumentPopup = true;
  }
  DeleteDocument() {
    this.Loader = true;
    this.documentService.DeleteDocumentById(this.UserId, this.DeleteData.id).subscribe(data => {
      if (data == 1) {
        this.DeleteDocumentPopup = false;
        this.GetDocument();
        this.toastr.success("Document deleted successfully", "Success!")
      }
      else {
        this.Loader = false;
        this.DeleteDocumentPopup = false;
        this.toastr.error("Document cannot deleted", "Error!")
      }
    })
  }
  UploadDocument() {
    this.uploadFile = true;
    this.DocumentFile = [];
    this.DocSizeBool = false;
    this.DocSizeCount = 0;
  }
  onFileChange(files: FileHandle[]) {
    for (var i = 0; i < files.length; i++) {
      let ext: any;
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == files[i].file.name.split('.').pop()) {
          this.DocumentFile.push({ Id: this.DocumentFile.length * -1, File: files[i].file });
          this.filesToUpload.push(files[i].file);
        }
      });
    }
    for (let i = 0; i < this.DocumentFile.length; i++) {
      let size = this.DocumentFile[i].File.size / (1024 * 1024)
      if (size < 100) {
        this.DocSizeBool = false;
      }
      else {
        this.DocSizeBool = true;
        this.DocSizeCount = this.DocSizeCount + 1
      }
    }
    if (this.DocSizeCount > 0) {
      this.DocSizeBool = true;
    }
  }
  onFileChange1(event: any) {
    for (var i = 0; i < event.target.files.length; i++) {
      let ext: any;
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == event.target.files[i].name.split('.').pop()) {
          this.filesToUpload.push(event.target.files[i]);
          this.DocumentFile.push({ Id: this.DocumentFile.length * -1, File: event.target.files[i] });
        }
      });
    }
    for (let i = 0; i < this.DocumentFile.length; i++) {
      let size = this.DocumentFile[i].File.size / (1024 * 1024)
      if (size < 100) {
        this.DocSizeBool = false;
      }
      else {
        this.DocSizeBool = true;
        this.DocSizeCount = this.DocSizeCount + 1
      }
    }
    if (this.DocSizeCount > 0) {
      this.DocSizeBool = true;
    }
  }
  OnRemoveDoc(id: any) {
    this.Loader = true;
    this.DocSizeCount = 0;
    this.DocumentFile = this.DocumentFile.filter((x: { Id: any; }) => x.Id != id)
    this.filesToUpload = this.filesToUpload.filter((x: { name: any; }) => x.name != this.DocumentFile[0].File.name)
    this.DocSizeBool = false;
    for (let i = 0; i < this.DocumentFile.length; i++) {
      let size = this.DocumentFile[i].File.size / (1024 * 1024)
      if (size < 100) {
        this.DocSizeBool = false;
      }
      else {
        this.DocSizeBool = true;
        this.DocSizeCount = this.DocSizeCount + 1
      }
    }
    if (this.DocSizeCount > 0) {
      this.DocSizeBool = true;
    }
    else {
      this.DocSizeBool = false;
      this.DocSizeCount = 0;
    }

    this.Loader = false;
  }
  onSaveandUpload() {
    if (this.DocumentFile.length <= 10 && this.DocumentFile.length > 0 && this.DocSizeBool == false) {
      this.Loader = true;
      const UploadDoc = new FormData();
      UploadDoc.append("UserId", this.UserId);
      UploadDoc.append("Type", this.TypeId);
      this.filesToUpload.forEach((item: string | Blob) => {
        UploadDoc.append('Files', item);
      });
      this.documentService.UploadDocument(UploadDoc).subscribe(data => {
        if (data == true) {
          this.uploadFile = false;
          this.GetDocument();
          this.DocumentFile = [];
          this.filesToUpload = [];
        }
      })
    }
  }
  onDownloadFile(value: any) {
    var a = document.createElement('a');
    a.href = value.filePath;
    a.download = value.name;
    a.click();
  }
  onViewFile(item : any){
    this.DocumentViewPopup = true;
    this.ViewFile = item;
  }

}
