import { flatten, ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FileHandle } from '../documents/file.directive';
import { InvestorProfileService } from '../investor-profile/investor-profile.service';
import { LeadService } from './lead.service';

@Component({
  selector: 'app-lead',
  templateUrl: './lead.component.html',
  styleUrls: ['./lead.component.css']
})
export class LeadComponent implements OnInit {
  LeadDetails: any = [];
  LeadDetailsData: any = [];
  AddLeadPopup: boolean = false;
  AddLeadForm: any;
  LeadFirstnameError: boolean = false;
  LeadLastnameError: boolean = false;
  AddLeadEmailError: boolean = false;
  validEmail: boolean = false;
  Phonenumberbool: boolean = false;
  Phonelength: boolean = false;
  Residency: any = [];
  ResidencyId: any = 0;
  LookingInvest: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Less than $10,000' },
    { id: 2, value: '$10,000 to $50,000' },
    { id: 3, value: '$50,000 to $100,000' },
    { id: 4, value: '$100,000 to $250,000' },
    { id: 5, value: 'More than $250,000' },
  ];
  AccreditedInvestor: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Yes' },
    { id: 2, value: 'No' },
  ];
  RegType: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Signed Up' },
    { id: 2, value: 'Invited' },
  ];
  TagNameArray: any = [
    { id: 0, value: 'Edge' },
    { id: 1, value: 'Firefox' },
    { id: 2, value: 'Chrome' },
    { id: 2, value: 'Opera' },
    { id: 2, value: 'Safari' },
  ];
  LookingInvestId: any = 0;
  lookInvest: any;
  lookInvestbool: boolean = false;
  InvestorData: any;
  InvestorDatabool: any = null;
  AccreditedInvestorId: any = 0;
  EmailConfirmAccountError: boolean = false;
  EmailConfirmAccount1: any;
  AddLeadShow: boolean = false;
  AddLeadsShow: boolean = false;
  config: any;
  allowedFileExtensions: any = [];
  DocumentFile: any = [];
  filesToUpload: any = [];
  DocumentValidCheck = "Maximum File Size 15MB | File Format CSV | Character Encoding UTF-8 {unicode} | 500 Leads per import";
  DocSizeBool: boolean = false;
  DocSizeCount: any;
  UserId: any;
  RoleId: any;
  CountryShow: boolean = false;
  EditLeadData: any;
  EditDetailsShow: boolean = false;
  AddLeadDetailsShow: boolean = false;
  LeadEditId: any;
  Loader: boolean = false;
  LeadSummary: any;
  InvestCapId: any = 0;
  ResidencyFilterId: any = 0;
  SelfAccFilterId: any = 0;
  RegTypeFilterId: any = 0;
  VerifyFilterId: any = 0;
  TagDetailsId: any = 0;
  SelfAccData: any;
  VerifyData: any;
  DeleteArray: any = [];
  DeleteList: any = [];
  NotePopup: boolean = false;
  WriteNoteBool: boolean = false;
  TableView: boolean = false;
  SelectAllCheckbox: boolean = false;
  EditBool: boolean = false;
  Notes: any
  NoteUserId: any;
  LeadData: any = [];
  EditNoteId: any;
  RegTypeName: any;
  ResendInvitePopup: boolean = false
  AddTagPopup: boolean = false;
  TagName: any;
  TagDetailsList: any = [];
  TagDetails: any = [];
  TagId: any = 0;
  ChooseBool: boolean = false;
  ResendInviteArray: any = [];
  ResendList: any = [];
  InvestorEmpty : boolean = false;
  VerifyAccountData: any;
  VerifyAccountPopup : boolean = false;
  verifyuser : boolean = false;
  VerifyAccountBool : boolean = false;

  constructor(private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private leadService: LeadService,
    private profileService: InvestorProfileService) {
    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.LeadDetails.length
    };
  }

  ngOnInit(): void {
    this.Loader = true;
    this.UserId = Number(localStorage.getItem("UserId"));
    this.RoleId = Number(localStorage.getItem("RoleId"));
    this.allowedFileExtensions = ['CSV', 'csv'];
    this.AddLeadForm = this.formBuilder.group({
      FirstName: [''],
      LastName: [''],
      NickName: [''],
      Email: [''],
      Phonenumber: [''],
      Residency: [''],
      Country: [''],
      Invest: [''],
      Creditedinvestor: [''],
      HowdidYouHear: [''],
      VerifyAccount: [''],
      NewletterUpdate: [''],
      InvestmentAnnoucements: [''],
      EmailConfirmAccount: [''],
    })
    this.GetStateProvince();
    this.GetLeadSummary();
    this.GetLead();
    this.GetTag();
  }
  onAddLead() {
    this.AddLeadPopup = true;
    this.AddLeadShow = true;
    this.AddLeadsShow = true;
    this.AddLeadDetailsShow = true;

    this.LeadFirstnameError = false;
    this.LeadLastnameError = false;
    this.AddLeadEmailError = false;
    this.validEmail = false;
    this.DocSizeBool = false;
    this.filesToUpload = [];
    this.DocumentFile = [];
    this.LeadEditId = 0;
    this.AddLeadForm.reset();
    this.ResidencyId = 0;
    this.LookingInvestId = 0;
    this.AccreditedInvestorId = 0;
  }

  onLeadFirstName() {
    if (this.AddLeadForm.value.FirstName == '' || this.AddLeadForm.value.FirstName == null) {
      this.LeadFirstnameError = true;
    }
    else {
      this.LeadFirstnameError = false;
    }
  }
  onLeadLastName() {
    if (this.AddLeadForm.value.LastName == '' || this.AddLeadForm.value.LastName == null) {
      this.LeadLastnameError = true;
    }
    else {
      this.LeadLastnameError = false;
    }
  }
  onLeadEmail() {
    const validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (this.AddLeadForm.value.Email == null || this.AddLeadForm.value.Email == ' ' || this.AddLeadForm.value.Email == '') {
      this.AddLeadEmailError = true;
      this.validEmail = false;
    }
    else {
      this.AddLeadEmailError = false;
      if (validEmailRegEx.test(this.AddLeadForm.value.Email)) {
        this.validEmail = false;
      } else {
        this.validEmail = true;
      }
    }
  }
  onPhoneNumber(e: any) {
    if (this.AddLeadForm.value.Phonenumber == '') {
      this.Phonenumberbool = true;
      this.Phonelength = false
    }
    else {
      this.Phonenumberbool = false;
      if (this.AddLeadForm.value.Phonenumber.length == 10) {
        this.Phonelength = false
      }
      else {
        this.Phonelength = true
      }
    }
  }
  numberValidation(event: any): Boolean {
    if (event.keyCode >= 48 && event.keyCode <= 57)
      return true;
    else
      return false;
  }
  GetStateProvince() {
    this.profileService.GetStateorProvince().subscribe(data => {
      let x: any = data
      this.Residency.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < x.length; i++) {
        this.Residency.push({ id: x[i].id, name: x[i].name, active: x[i].active })
      }
    })
  }
  onchange(e: any) {
    this.AddLeadForm.get('Residency').setValue(e.target.value);
    this.AddLeadForm.value.Residency = e.target.value;
    this.ResidencyId = +e.target.value;
    if (this.ResidencyId == 1) {
      this.CountryShow = true;
    }
    else {
      this.CountryShow = false;
    }
  }
  oninvestor(e: any) {
    this.InvestorData = e.target.value;
    if (e.target.value == '1') {
      this.InvestorDatabool = true;
    }
    else if (e.target.value == '2') {
      this.InvestorDatabool = false;
    }
    else {
      this.InvestorDatabool = null;
    }
  }
  onEmailConfirmAccount() {
    if (this.AddLeadForm.value.ReceiveEmail == true) {
      if (this.AddLeadForm.value.EmailConfirmAccount == '' || this.AddLeadForm.value.EmailConfirmAccount == null) {
        this.EmailConfirmAccountError = true;
      }
      else {
        this.EmailConfirmAccountError = false;
      }
    }
    else {
      this.EmailConfirmAccountError = false;
    }
  }
  onAddLeadSave() {
    this.Loader = true;
    if (this.AddLeadForm.value.FirstName == null || this.AddLeadForm.value.FirstName == ''
      || this.AddLeadForm.value.LastName == null || this.AddLeadForm.value.LastName == ''
      || this.AddLeadForm.value.Email == null || this.AddLeadForm.value.Email == '') {
      this.onLeadFirstName();
      this.onLeadLastName();
      this.onLeadEmail();
      this.Loader = false;
    }
    else {
      let lead = {
        Id: this.LeadEditId != 0 ? this.LeadEditId : 0,
        AdminUserId: this.UserId,
        FirstName: this.AddLeadForm.value.FirstName,
        LastName: this.AddLeadForm.value.LastName,
        NickName: this.AddLeadForm.value.NickName,
        EmailId: this.AddLeadForm.value.Email,
        PhoneNumber: this.AddLeadForm.value.Phonenumber,
        Residency: +this.ResidencyId,
        Country: this.AddLeadForm.value.Country,
        Capacity: +this.LookingInvestId,
        IsAccreditedInvestor: this.InvestorDatabool,
        HeardFrom: this.AddLeadForm.value.HowdidYouHear,
        VerifyAccount: this.AddLeadForm.value.VerifyAccount,
        CompanyNewsLetterUpdates: this.AddLeadForm.value.NewletterUpdate,
        NewInvestmentAnnouncements: this.AddLeadForm.value.InvestmentAnnoucements,
        EmailConfirmAccount: this.AddLeadForm.value.EmailConfirmAccount,
        Active: true,
        Status: 1,

      }
      if (this.LeadEditId == 0) {
        this.leadService.CreateLead(lead).subscribe(data => {
          console.log(data, 'leaddata')
          if (data == true) {
            this.GetLead();
            this.CancelLead();
            this.AddLeadPopup = false;
            this.toastr.success("Lead added successfully", "Success")

          }
          else {
            this.AddLeadPopup = false;
            this.toastr.error("Lead can't be added", "Error");
            this.Loader = false;
          }
        })
      }
      else {
        this.leadService.UpdateLead(lead).subscribe(data => {
          console.log(data, 'leaddata')
          if (data == 1) {
            this.GetLead();
            this.CancelLead();
            this.AddLeadPopup = false;
            this.toastr.success("Updated successfully", "Success")

          }
          else {
            this.AddLeadPopup = false;
            this.toastr.error("Can't updated", "Error");
            this.Loader = false;
          }
        })
      }

    }
  }
  onChooseDataPerPage(event: any) {
    if (+event.target.value == 0) {
      this.config.itemsPerPage = 5
    }
    else {
      this.config.itemsPerPage = +event.target.value
    }
  }
  onFileChange(files: FileHandle[]) {
    this.filesToUpload = [];
    this.DocumentFile = [];
    for (var i = 0; i < files.length; i++) {
      let ext: any;
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == files[i].file.name.split('.').pop()) {
          ext = null;
          ext = files[i].file.name.split('.').pop();
          this.DocumentFile.push({ Id: this.DocumentFile.length * -1, File: files[i].file });
          this.filesToUpload.push(files[i].file);
        }
      });
      if (ext == null) {
        this.toastr.error(files[i].file.name.split('.').pop() + ' files are not accepted.', 'Error');
      }
    }
    for (let i = 0; i < this.DocumentFile.length; i++) {
      let size = this.DocumentFile[i].File.size / (1024 * 1024)
      if (size < 15) {
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
    this.filesToUpload = [];
    this.DocumentFile = [];
    for (var i = 0; i < event.target.files.length; i++) {
      let ext: any;
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == event.target.files[i].name.split('.').pop()) {
          ext = null;
          ext = event.target.files[i].name.split('.').pop();
          this.filesToUpload.push(event.target.files[i]);
          this.DocumentFile.push({ Id: this.DocumentFile.length * -1, File: event.target.files[i] });
        }
      });
      if (ext == null) {
        this.toastr.error(event.target.files[i].name.split('.').pop() + ' files are not accepted.', 'Error');
      }
    }
    for (let i = 0; i < this.DocumentFile.length; i++) {
      let size = this.DocumentFile[i].File.size / (1024 * 1024)
      if (size < 15) {
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
  removeCSVFile() {
    this.filesToUpload = [];
    this.DocumentFile = [];
    this.DocSizeBool = false;
  }
  GetLead() {
    this.leadService.GetLead().subscribe(data => {
      this.LeadDetailsData = data;
      for (let i = 0; i < this.LeadDetailsData.length; i++) {
        if (this.LeadDetailsData[i].lastLogin == null) {
          this.LeadDetailsData[i].lastLogin1 = "Invited"
        }
        else {
          this.LeadDetailsData[i].lastLogin1 = "Signed Up"
        }
      }
      this.LeadDetails = this.LeadDetailsData,
        console.log(this.LeadDetails, 'leaddetails')
      this.Loader = false;
    })
  }
  CancelLead() {
    this.AddLeadPopup = false;
    this.AddLeadShow = false;
    this.AddLeadsShow = false;
    this.EditDetailsShow = false;
    this.AddLeadDetailsShow = false;
  }
  EditLead(val: any) {
    this.EditLeadData = val;
    this.LeadEditId = this.EditLeadData.id
    this.EditDetailsShow = true;
    this.AddLeadPopup = true;
    this.AddLeadShow = true;
    this.AddLeadDetailsShow = false;
    this.AddLeadsShow = false;

    this.AddLeadForm.patchValue({
      FirstName: this.EditLeadData.firstName,
      LastName: this.EditLeadData.lastName,
      NickName: this.EditLeadData.nickName,
      Email: this.EditLeadData.emailId,
      Phonenumber: this.EditLeadData.phoneNumber,
      Residency: this.EditLeadData.residency,
      Country: this.EditLeadData.country,
      Invest: this.EditLeadData.capacity,
      Creditedinvestor: this.EditLeadData.isAccreditedInvestor,
      HowdidYouHear: this.EditLeadData.heardFrom,
      VerifyAccount: this.EditLeadData.verifyAccount,
      NewletterUpdate: this.EditLeadData.companyNewsLetterUpdates,
      InvestmentAnnoucements: this.EditLeadData.newInvestmentAnnouncements,
    })
    this.ResidencyId = this.EditLeadData.residency;
    this.LookingInvestId = this.EditLeadData.capacity;
    if (this.EditLeadData.residency == 1) {
      this.CountryShow = true;
    }
    else {
      this.CountryShow = false;
    }
    if (this.EditLeadData.isAccreditedInvestor == true) {
      this.AccreditedInvestorId = 1
      this.InvestorDatabool = true;
    }
    else if (this.EditLeadData.isAccreditedInvestor == false) {
      this.AccreditedInvestorId = 2;
      this.InvestorDatabool = false;
    }
    else {
      this.AccreditedInvestorId = 0
      this.InvestorDatabool = null;
    }
  }
  GetLeadSummary() {
    this.leadService.GetLeadSummary().subscribe(data => {
      this.LeadSummary = data,
        console.log(this.LeadSummary, 'leadsumary')
    })
  }
  pageChanged(event: any) {
    this.config.currentPage = event;
  }
  onInvestCapFilter(e: any) {
    this.InvestCapId = +e.target.value
    console.log(e.target.value, 'e')
    console.log(this.LeadDetails, 'leaddetails')
    this.LeadDetails = [];
    if (this.InvestCapId == 0) {
      this.LeadDetails = this.LeadDetailsData;
    }
    else {
      this.LeadDetails = this.LeadDetailsData.filter((x: { capacity: any; }) => x.capacity == this.InvestCapId);
    }
    if (this.ResidencyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { residency: any; }) => x.residency == this.ResidencyFilterId);
    }
    if (this.SelfAccFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
    }
    if (this.VerifyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData)
    }
    if (this.RegTypeFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { lastLogin1: any; }) => x.lastLogin1 == this.RegTypeName)
    }
  }
  onResidencyFilter(e: any) {
    this.ResidencyFilterId = +e.target.value;
    this.LeadDetails = [];
    if (this.ResidencyFilterId == 0) {
      this.LeadDetails = this.LeadDetailsData;
    }
    else {
      this.LeadDetails = this.LeadDetailsData.filter((x: { residency: any; }) => x.residency == this.ResidencyFilterId);
    }
    if (this.InvestCapId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { capacity: any; }) => x.capacity == this.InvestCapId);
    }
    if (this.SelfAccFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
    }
    if (this.VerifyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData)
    }
    if (this.RegTypeFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { lastLogin1: any; }) => x.lastLogin1 == this.RegTypeName)
    }
  }
  onSelfAccFilter(e: any) {
    this.SelfAccFilterId = e.target.value
    this.LeadDetails = [];
    if (this.SelfAccFilterId == 0) {
      this.LeadDetails = this.LeadDetailsData;
    }
    else {
      if (this.SelfAccFilterId == 1) {
        this.SelfAccData = true
        this.LeadDetails = this.LeadDetailsData.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
      }
      else if (this.SelfAccFilterId == 2) {
        this.SelfAccData = false
        this.LeadDetails = this.LeadDetailsData.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
      }
      else {
        this.SelfAccData = null
        this.LeadDetails = this.LeadDetailsData.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
      }
    }
    if (this.InvestCapId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { capacity: any; }) => x.capacity == this.InvestCapId);
    }
    if (this.ResidencyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { residency: any; }) => x.residency == this.ResidencyFilterId);
    }
    if (this.VerifyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData)
    }
    if (this.RegTypeFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { lastLogin1: any; }) => x.lastLogin1 == this.RegTypeName)
    }
  }
  onRegTypeFilter(e: any) {
    this.RegTypeFilterId = e.target.value;
    this.LeadDetails = [];
    if (this.RegTypeFilterId == 0) {
      this.LeadDetails = this.LeadDetailsData;
    }
    else {
      if (this.RegTypeFilterId == 1) {
        this.RegTypeName = 'Signed Up'
      }
      else if (this.RegTypeFilterId == 2) {
        this.RegTypeName = 'Invited'
      }
      this.LeadDetails = this.LeadDetailsData.filter((x: { lastLogin1: any; }) => x.lastLogin1 == this.RegTypeName)
    }
    if (this.InvestCapId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { capacity: any; }) => x.capacity == this.InvestCapId);
    }
    if (this.ResidencyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { residency: any; }) => x.residency == this.ResidencyFilterId);
    }
    if (this.SelfAccFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
    }
    if (this.VerifyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData)
    }
  }
  onVerifyFilter(e: any) {
    this.VerifyFilterId = +e.target.value
    this.LeadDetails = [];
    if (this.VerifyFilterId == 0) {
      this.LeadDetails = this.LeadDetailsData;
    }
    else {
      if (this.VerifyFilterId == 1) {
        this.VerifyData = true
        this.LeadDetails = this.LeadDetailsData.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData)
      }
      else if (this.VerifyFilterId == 2) {
        this.VerifyData = false
        this.LeadDetails = this.LeadDetailsData.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData || x.verifyAccount == null)
      }
      else {
        this.VerifyData = null
        this.LeadDetails = this.LeadDetailsData.filter((x: { verifyAccount: any; }) => x.verifyAccount == this.VerifyData)
      }
    }
    if (this.InvestCapId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { capacity: any; }) => x.capacity == this.InvestCapId);
    }
    if (this.ResidencyFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { residency: any; }) => x.residency == this.ResidencyFilterId);
    }
    if (this.SelfAccFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { isAccreditedInvestor: any; }) => x.isAccreditedInvestor == this.SelfAccData)
    }
    if (this.RegTypeFilterId != 0) {
      this.LeadDetails = this.LeadDetails.filter((x: { lastLogin1: any; }) => x.lastLogin1 == this.RegTypeName)
    }
  }
  onResetAll() {
    this.InvestCapId = 0;
    this.ResidencyFilterId = 0;
    this.SelfAccFilterId = 0;
    this.VerifyFilterId = 0;
    this.LeadDetails = this.LeadDetailsData;
  }
  FilterLead(e: any) {
    let x = e.target.value
    this.LeadDetails = [];
    if (x == null || x == '') {
      this.LeadDetails = this.LeadDetailsData;
    } else {
      this.LeadDetailsData.forEach((element: { fullName: string; emailId: string; phoneNumber: string; }) => {
        if (element.fullName.toLowerCase().includes(x.toLowerCase())
          || element.emailId.toLowerCase().includes(x.toLowerCase())
          || element.phoneNumber.toLowerCase().includes(x.toLowerCase())) {
          this.LeadDetails.push(element);
        }
      });
    }
    // if (this.selectedIndustryItem != null) {
    //   this.dealsListPublished = this.dealsListPublished.filter(x => x.industry == this.selectedIndustryItem)
    // }
  }

  SelectAll(event: any) {
    const checked = event.target.checked;
    if (event.target.checked) {
      this.LeadDetails.forEach((item: { selected: any; }) => item.selected = checked);
      for (let i = 0; i < this.LeadDetails.length; i++) {
        this.DeleteArray.push({ Id: this.LeadDetails[i].id })
        this.TagDetailsList.push({ Id: 0, TagId: 0, UserId: this.LeadDetails[i].id, Active: true })
        this.ResendInviteArray.push(this.LeadDetails[i])
      }
    }
    else {
      this.LeadDetails.forEach((item: { selected: any; }) => item.selected = checked);
      for (let i = 0; i < this.DeleteArray.length; i++) {
        this.DeleteArray = this.DeleteArray.filter((x: { id: any; }) => !this.DeleteArray[i])
        this.TagDetailsList = this.TagDetailsList.filter((x: { id: any; }) => !this.TagDetailsList[i])
        this.ResendInviteArray = [];
      }
    }
    if(this.DeleteArray.length == 0 || this.TagDetailsList.length == 0){
      this.ChooseBool = true;
    }
    else {
      this.ChooseBool = false;
    }
    console.log(this.DeleteArray, 'deletearray')
    console.log(this.TagDetailsList, 'TagDetailsList')
    console.log(this.ResendInviteArray, 'ResendInviteArray')
  }
  Select(e: any, e1: any) {
    if (e1.target.checked) {
      this.DeleteArray.push({ Id: e.id })
      this.TagDetailsList.push({ Id: 0, TagId: 0, UserId: e.id, Active: true })
      this.ResendInviteArray.push(e)
    }
    else {
      this.DeleteArray = this.DeleteArray.filter((x: { Id: any; }) => x.Id != e.id)
      this.TagDetailsList = this.TagDetailsList.filter((x: { UserId: any; }) => x.UserId != e.id)
      this.ResendInviteArray = this.ResendInviteArray.filter((x: { id: any; }) => x.id != e.id)
    }
    if(this.DeleteArray.length == 0 || this.TagDetailsList.length == 0){
      this.ChooseBool = true;
    }
    else {
      this.ChooseBool = false;
    }
    console.log(this.DeleteArray, 'deletearray')
    console.log(this.TagDetailsList, 'TagDetailsList')
    console.log(this.ResendInviteArray, 'ResendInviteArray')
  }

  DeleteUser() {
    if(this.DeleteArray.length == 0){
      this.toastr.info("Please select any row from the table","Info!")
      this.ChooseBool = true;
    }
    else{
      this.Loader = true;
      for (let i = 0; i < this.DeleteArray.length; i++) {
        this.DeleteList.push(this.DeleteArray[i].Id)
      }
      let DeleteUser = {
        AdminUserId: this.UserId,
        Ids: this.DeleteList
      }
      this.leadService.DeleteLead(DeleteUser).subscribe(data => {
        console.log(data, 'delete')
        if (data == 1) {
          this.GetLead();
          this.SelectAllCheckbox = false;
          this.TagDetailsId = 0;
        }
        else {
          this.Loader = false;
        }
      })
    }
  }
  onNotes(e: any) {
    this.NotePopup = true;
    this.EditBool = false;
    this.NoteUserId = e.id;
    this.EditNoteId = 0;
    this.WriteNoteBool = false;
    this.GetLeadId();
  }
  GetLeadId() {
    this.leadService.GetLeadNotes(this.NoteUserId).subscribe(data => {
      console.log(data, 'leaddata')
      this.LeadData = data;
      if (this.LeadData.length > 0) {
        this.TableView = true;
        this.Notes = this.LeadData[0].notes
      }
      else {
        this.TableView = false;
        this.InvestorEmpty = true;
      }
      this.Loader = false;
    })
  }
  onWriteANote() {
    this.InvestorEmpty = false;
    this.WriteNoteBool = true;
    this.EditBool = false;
    this.Notes = '';
  }
  NoteSave() {
    this.Loader = true;
    let notes = {
      Id: this.EditNoteId != 0 ? this.EditNoteId : 0,
      AdminUserId: this.UserId,
      UserId: this.NoteUserId,
      Notes: this.Notes,
      Active: true
    }
    if (this.EditNoteId == 0) {
      this.leadService.AddNotes(notes).subscribe(data => {
        if (data == true) {
          this.WriteNoteBool = false;
          this.GetLeadId();
        }
        else{
          this.Loader = false;
        }
      })
    }
    else {
      this.leadService.UpdateNotes(notes).subscribe(data => {
        if (data == true) {
          this.WriteNoteBool = false;
          this.GetLeadId();
        }
        else{
          this.Loader = false;
        }
      })
    }
  }
  EditNote(e: any) {
    this.WriteNoteBool = true;
    this.TableView = false;
    this.EditBool = true;
    this.Notes = e.notes;
    this.EditNoteId = e.id
  }
  DeleteNote(e: any) {
    this.Loader = true;
    this.leadService.DeleteNotes(this.UserId, e.id).subscribe(data => {
      if (data == true) {
        this.WriteNoteBool = false;
        this.GetLeadId();
      }
      else{
        this.Loader = false;
      }
    })
  }
  OnCancel() {
    this.WriteNoteBool = false;
    if (this.LeadData.length > 0) {
      this.TableView = true;
      this.Notes = this.LeadData[0].notes
    }
    else{
      this.InvestorEmpty = true;
    }
  }
  ResendInvite() {
    this.Loader = true;
    if(this.ResendInviteArray.length == 0){
      this.toastr.info("Please select any row from the table","Info!")
      this.ChooseBool = true;
      this.Loader = false
    }
    else{
      for (let i = 0; i < this.ResendInviteArray.length; i++) {
        this.ResendList.push(this.ResendInviteArray[i].id)
      }
      let ResendUser = {
        AdminUserId: this.UserId,
        Ids: this.ResendList
      }
      this.leadService.MultipleInviteLead(ResendUser).subscribe(data=>{
        if (data == true) {
          this.Loader = false;
          this.ResendInvitePopup = false;
          this.LeadDetails.forEach((item: { selected: any; }) => item.selected = false);
          this.toastr.success("Resend invite sent successfully for respective lead user", "Successfully!")
        }
        else {
          this.Loader = false;
          this.ResendInvitePopup = false;
          this.toastr.error("Resend invite failed", "Error!")
        }
      })
    }
  }
  onSingleResendInvite(e: any) {
    console.log(e, 'singleinvite')
    this.Loader = true;
    this.leadService.SingleInviteLead(this.UserId, e.id).subscribe(data => {
      if (data == true) {
        this.Loader = false;
        this.toastr.success("Resend invite sent successfully", "Successfully!")
      }
      else {
        this.Loader = false;
        this.toastr.error("Resend invite failed", "Error!")
      }
    })
  }
  onAddTag() {
    if(this.DeleteArray.length == 0){
      this.toastr.info("Please select any row from the table","Info!")
      this.ChooseBool = true;
    }
    else{
      this.AddTagPopup = true;
      this.TagDetailsId = 0;
      this.TagName = ''
    }
  }
  GetTag() {
    this.leadService.GetLeadTag(this.UserId).subscribe(data => {
      let x = { id: 0, name: 'Select Tag', active: true }
      this.TagDetails = data;
      this.TagDetails.unshift(x);
      console.log(this.TagDetails, 'tagdetails')
    })
  }
  onSaveTag() {
    this.Loader = true;
    if (this.TagName == null || this.TagName == '') {
      let x = this.TagDetails.filter((x: { name: any; }) => x.name == this.TagName)
      this.Loader = false;
    }
    else {
      let x = this.TagDetails.filter((x: { name: any; }) => x.name == this.TagName)
      if(x.length > 0){
        this.TagId = x[0].id;
      }
      else{
        this.TagId = 0;
      }
      let Tag = {
        Id: this.TagId != 0 ? this.TagId : 0,
        AdminUserId: this.UserId,
        Name: this.TagName,
        Active: true,
        tagDetails: this.TagDetailsList
      }
      if(this.TagId == 0){
        this.leadService.AddLeadTag(Tag).subscribe(data => {
          console.log(data, 'addlead')
          if (data == true) {
            this.AddTagPopup = false;
            this.TagDetailsList = [];
            this.DeleteArray = [];
            this.LeadDetails.forEach((item: { selected: any; }) => item.selected = false);
            this.TagDetailsId = 0;
            this.GetTag();
            this.GetLead();
            this.TagDetailsId = 0;
          }
          else {
            this.AddTagPopup = false;
            this.TagDetailsList = [];
            this.DeleteArray = [];
            this.LeadDetails.forEach((item: { selected: any; }) => item.selected = false);
            this.TagDetailsId = 0;
            this.Loader = false;
          }
        })
      }
      else if(this.TagId != 0){
        this.leadService.UpdateLeadTag(Tag).subscribe(data =>{
          if(data == true){
            this.AddTagPopup = false;
            this.TagDetailsList = [];
            this.DeleteArray = [];
            this.TagDetailsId = 0;
            this.LeadDetails.forEach((item: { selected: any; }) => item.selected = false);
            this.TagDetailsId = 0;
            this.GetTag();
            this.GetLead();
          }
          else {
            this.AddTagPopup = false;
            this.TagDetailsList = [];
            this.DeleteArray = [];
            this.TagDetailsId = 0;
            this.LeadDetails.forEach((item: { selected: any; }) => item.selected = false);
            this.Loader = false;
          }
        })
      }
    }

  }
  onChooseTags(e: any) {
    let a = +e.target.value
    if (a == 0) {
      this.LeadDetails = this.LeadDetailsData;
    }
    else {
      console.log(e.target.value, 'filtertag')
      let x = this.TagDetails.filter((x: { id: any; }) => x.id == a)
      console.log(x, 'x')
      let b = x[0].tagDetails;
      this.LeadDetails = [];
      console.log(this.LeadDetailsData, 'leaddata')
      for (let i = 0; i < b.length; i++) {
        let c = this.LeadDetailsData.filter((x: { id: any; }) => x.id == b[i].userId)
        if (c.length > 0) {
          this.LeadDetails.push(c[0])
        }
      }
    }
  }
  MultipleResendInvite(){
    if(this.ResendInviteArray.length == 0){
      this.toastr.info("Please select any row from the table","Info!")
      this.ChooseBool = true;
    }
    else{
      this.ChooseBool = false;
      this.ResendInvitePopup = true;
    }
  }
  RemoveResend(e : any){
    let x = this.LeadDetails.filter((x: { id: any; }) => x.id == e.id)
    x[0].selected = false;
    this.ResendInviteArray = this.ResendInviteArray.filter((x: { id: any; }) => x.id != e.id)
  }
  onVerifyAccount(val : any,e : any){
    console.log(e,'ver')
    this.VerifyAccountData = val;
    this.VerifyAccountBool = e;
    console.log(this.VerifyAccountData,'VerifyAccountData')
    this.VerifyAccountPopup = true;
    if(this.VerifyAccountBool == true){
      this.verifyuser = true;
    }
    else{
      this.verifyuser = false;
    }
  }
  CancelVerifyAccount(){
    this.Loader = true;
    this.VerifyAccountPopup = false;
    this.GetLead();
  }
  onSaveVerifyAccount(){
    this.Loader = true;
    this.leadService.VerifyAccount(this.UserId,this.VerifyAccountData.id,this.VerifyAccountBool).subscribe(data =>{
      if(data == true){
        this.VerifyAccountPopup = false;
        this.GetLead();
      }
      else{
        this.VerifyAccountPopup = false;
        this.Loader = false;
      }
    })
  }
}
