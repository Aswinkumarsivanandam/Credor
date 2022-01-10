import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InvestService } from '../invest/invest.service';
import { InvestmentService } from '../investment/investment.service';
import { InvestorProfileService } from '../investor-profile/investor-profile.service';
import * as InlineEditor from '@ckeditor/ckeditor5-build-inline';
import { AfterViewInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-myinvestment',
  templateUrl: './myinvestment.component.html',
  styleUrls: ['./myinvestment.component.css']
})
export class MyinvestmentComponent implements OnInit {
  Editor: any = {};
  Ckbool: boolean = true;
  UserId: any;
  Selected: any;
  ActiveInvestor: any = [];
  PastInvestor: any = [];
  PendingInvestor: any = [];
  ReservationInvestor: any = [];
  ActiveShow: boolean = false;
  PendingShow: boolean = false;
  PastShow: boolean = false;
  ResShow: boolean = false;
  ActiveCount: any;
  PendingCount: any;
  PastCount: any;
  ReserveCount: any;
  InvestAmount: any = 0;
  ViewDetails: boolean = false;
  summaryBool: boolean = false;
  documentBool: boolean = false;
  locationBool: boolean = false;
  ViewDetailsDataValue: any;
  ViewDetailsSummaryValue: any = [];
  ViewDetailsDocValue: any = [];
  ViewDetailsLocationValue: any = [];
  ViewDetailsUpdateValue: any = [];
  Activeofferingtab: boolean = false;
  Reservationtab: boolean = false;
  Pastofferingtab: boolean = false;
  updatenBool: boolean = false;
  EditReservationPopup: boolean = false;
  InvestingProfile: any = 0;
  InvestError: boolean = false;
  InsAmtError: boolean = false;
  MinimumError: boolean = false;
  InvestmentAmount: any = '';
  MinInvestment: any = 0;
  ProfileValue: any = [];
  ProfilePopup: boolean = false;
  ChooseProfileValue: any = [];
  Chooseprofile: any = '1';
  IRAForm: any;
  LLCForm: any;
  IndividualForm: any;
  TrustForm: any;
  JointForm: any;
  RetirementForm: any;
  IRAShow: boolean = false;
  LLCShow: boolean = false;
  Individualhow: boolean = false;
  TrustShow: boolean = false;
  JointShow: boolean = false;
  RetirementShow: boolean = false;
  IranameError: boolean = false;
  LlcnameError: boolean = false;
  StateProvinceId: any = '0'
  StateProvinceId1: any = '0'
  Iraname: any;
  StateorProvince: any;
  CountryStateShow: boolean = false;
  StateError: boolean = false;
  Province: any = [];
  USAddressError: boolean = false;
  CheckProvince: any = [];
  TaxError: boolean = false;
  Llcname: any;
  DisregardedEntity: any = null;
  Disregardedentity: any = '0';
  IRALLC: any = null;
  Irallc: any = '0';
  Firstname: any;
  FirstnameError: boolean = false;
  Lastname: any;
  LastnameError: boolean = false;
  Trustname: any;
  TrustnameError: boolean = false;
  Registrationname: any;
  RegistrationnameError: boolean = false;
  ArrayForm: any = [];
  FirstName: any;
  ArrayFirstnameError: boolean = false;
  LastName: any;
  ArrayLastnameError: boolean = false;
  ArrayEmail: boolean = false;
  ArrayvalidEmail: boolean = false;
  ArrayPhone: any;
  ArrayPhonelength: boolean = false;
  ArrayEmailValue: any;
  ArrayPhoneEmpty: boolean = false;
  Retirementname: any;
  RetirementnameError: boolean = false;
  Titlesignor: any;
  TitlesignorError: boolean = false;
  DistributionMethodId: any;
  Distributionmethod: any;
  BankAccountId: any = 0;
  CheckForm: any;
  OtherDetails: any;
  ACHBool: boolean = false;
  CheckBool: boolean = false;
  OtherBool: boolean = false;
  DistrubutionId: any = '0';
  DistributionValue: any = [];
  BankDetails: any = [];
  BankDetailsData: any = [];
  StreetBool: boolean = false;
  CityBool: boolean = false;
  ZipcodeBool: boolean = false;
  profile: any;
  ProfileId: any = 0;
  BankId: any = 0;
  DeleteBankPopup: boolean = false;
  BankPopup: boolean = false;
  BankTitleShow: boolean = false;
  BanknameError: boolean = false;
  AccounttypeError: boolean = false;
  RoutingnumberError: boolean = false;
  RoutingnumberLength: boolean = false;
  ConfirmroutingnumberError: boolean = false;
  ConfirmRoutingMatch: boolean = false;
  Accountnumber: any;
  AccountnumberError: boolean = false;
  Confirmaccountnumber: any;
  ConfirmaccountnumberError: boolean = false;
  ConfirmAccountMatch: boolean = false;
  BankForm: any;
  Accounttype: any = 0;
  Bankname: any;
  Routingnumber: any;
  Confirmroutingnumber: any;
  showRouting: boolean = false;
  showAccount: boolean = false;
  UserData: any;
  AddEditData: any;
  ReservationData: any = [];
  ExistProfileShow: boolean = false;
  ExistProfileHide: boolean = false;
  ReservationId: any = 0;
  ReservationActive: boolean = true;
  Loader: boolean = false
  HeaderData: any;
  InvestmentData: any = [];
  Markers: any = [];
  lat : any;
  lng : any;
  zoom : any;


  constructor(private investmentService: InvestmentService,
    private router: Router,
    private profileService: InvestorProfileService,
    private investService: InvestService,
    private toastr: ToastrService,
    private formbuilder: FormBuilder,
    private route: ActivatedRoute) {
    this.Editor = InlineEditor;
    this.lat = 0;
    this.lng = 0;
    this.zoom = 2;
    this.Markers = [];
  }

  ngOnInit(): void {
    this.Loader = true;
    this.UserId = localStorage.getItem('UserId');
    this.IRAForm = this.formbuilder.group({
      Iraname: ['', Validators.required],
      Incare: ['',],
      Streetaddress1: ['',],
      Streetaddress2: ['',],
      City: ['',],
      Province: [''],
      Country: [''],
      State: [''],
      Zipcode: [''],
      Taxid: [''],
    })
    this.CheckForm = this.formbuilder.group({
      Incare: ['',],
      Streetaddress1: ['',],
      Streetaddress2: ['',],
      City: ['',],
      Province: [''],
      Zipcode: [''],
    })
    this.LLCForm = this.formbuilder.group({
      Llcname: ['', Validators.required],
      Incare: ['',],
      Streetaddress1: ['',],
      Streetaddress2: ['',],
      City: ['',],
      Province: [''],
      Country: [''],
      State: [''],
      Zipcode: [''],
      Taxid: [''],
      Disregardedentity: [''],
      Irallc: [''],
    })
    this.IndividualForm = this.formbuilder.group({
      Firstname: ['', Validators.required],
      Lastname: ['', Validators.required],
      Streetaddress1: [''],
      Streetaddress2: [''],
      City: [''],
      Province: [''],
      Country: [''],
      State: [''],
      Zipcode: [''],
      Taxid: [''],
    })
    this.TrustForm = this.formbuilder.group({
      Trustname: ['', Validators.required],
      Incare: [''],
      Streetaddress1: [''],
      Streetaddress2: [''],
      City: [''],
      Province: [''],
      Country: [''],
      State: [''],
      Zipcode: [''],
      Taxid: [''],
    })
    this.JointForm = this.formbuilder.group({
      Registrationname: ['', Validators.required],
      Incare: [''],
      Streetaddress1: [''],
      Streetaddress2: [''],
      City: [''],
      Province: [''],
      Country: [''],
      State: [''],
      Zipcode: [''],
      Taxid: [''],
    })
    this.RetirementForm = this.formbuilder.group({
      Retirementname: ['', Validators.required],
      Titlesignor: ['', Validators.required],
      Streetaddress1: [''],
      Streetaddress2: [''],
      City: [''],
      Province: [''],
      Country: [''],
      State: [''],
      Zipcode: [''],
      Taxid: [''],
    })
    this.BankForm = this.formbuilder.group({
      Bankname: ['', Validators.required],
      Accounttype: ['', Validators.required],
      Routingnumber: ['', Validators.required],
      Confirmroutingnumber: ['', Validators.required],
      Accountnumber: ['', Validators.required],
      Confirmaccountnumber: ['', Validators.required],
    })
    this.Selected = 'ActiveInvestment';
    this.ActiveShow = true;
    this.GetHeaderData();
    this.GetInvestmentByUserId();
    this.GetProfile();

    this.GetStateProvince();
    this.GetDistribution();
    this.GetBankDetails();
    this.GetChooseProfile();
    this.GetUserById();
  }
  // mapOptions: google.maps.MapOptions = {
  //   center: this.coordinates,
  //   zoom: 8
  // };
  // marker = new google.maps.Marker({
  //   position: this.coordinates,
  //   // map: this.map,
  // });
  // ngAfterViewInit() {
  //   this.mapInitializer();
  // }
  // mapInitializer() {
  //   this.map = new google.maps.Map(this.gmap.nativeElement,
  //     this.mapOptions);
  //   this.marker.setMap(this.map);
  // }

  GetInvestmentByUserId() {
    this.investmentService.GetInvestmentByUser(this.UserId).subscribe(data => {
      this.InvestmentData = data;
      this.ActiveInvestor = this.InvestmentData.filter((x: { status: number; }) => x.status == 1);
      this.PendingInvestor = this.InvestmentData.filter((x: { status: number; }) => x.status == 3);
      this.PastInvestor = this.InvestmentData.filter((x: { status: number; }) => x.status == 0);
      this.ReservationInvestor = this.InvestmentData.filter((x: { isReservation: boolean; }) => x.isReservation == true);
      this.ActiveCount = this.ActiveInvestor.length;
      this.PendingCount = this.PendingInvestor.length;
      this.PastCount = this.PastInvestor.length;
      this.ReserveCount = this.ReservationInvestor.length;
      if (this.ActiveInvestor.length != 0) {
        for (let i = 0; i < this.ActiveInvestor.length; i++) {
          this.InvestAmount = this.InvestAmount + this.ActiveInvestor[i].amount;
        }
      }
      this.Loader = false;
    })
  }
  ActiveInvest() {
    this.Selected = 'ActiveInvestment';
    this.ActiveShow = true;
    this.PendingShow = false;
    this.PastShow = false;
    this.ResShow = false;
  }
  PendingInvest() {
    this.Selected = 'PendingInvestment';
    this.ActiveShow = false;
    this.PendingShow = true;
    this.PastShow = false;
    this.ResShow = false;
  }
  PastInvest() {
    this.Selected = 'PastInvestment';
    this.ActiveShow = false;
    this.PendingShow = false;
    this.PastShow = true;
    this.ResShow = false;
  }
  Reservation() {
    this.Selected = 'Reservation';
    this.ActiveShow = false;
    this.PendingShow = false;
    this.PastShow = false;
    this.ResShow = true;
  }
  onFundingInstruction(val: any) {
    this.router.navigate(['./../investment/' + val.offeringId + '/' + val.id + '/fund'], { relativeTo: this.route });
  }
  onCompleteEsign(val: any) {
    this.router.navigate(['./../investment/' + val.offeringId + '/' + val.id + '/esign'], { relativeTo: this.route });
  }
  onViewDetails(value: any) {
    this.ViewDetails = true;
    this.ViewDetailsDataValue = value;
    this.ViewDetailsSummaryValue = [];
    this.ViewDetailsDocValue = [];
    this.ViewDetailsLocationValue = [];
    this.ViewDetailsUpdateValue = []
    this.ViewDetailsSummaryValue = value.summary;
    this.ViewDetailsDocValue = value.documents;
    this.ViewDetailsLocationValue = value.locations;
    this.ViewDetailsUpdateValue = value.updates
    this.Selected = 'Summary';
    this.summaryBool = true;
    this.Markers.push({
      position: {
        lat: this.ViewDetailsLocationValue[0].latitude,
        lng:this.ViewDetailsLocationValue[0].longitude
      },
      // label: {
      //   color: 'black',
      //   text: 'Madrid',
      // },
    });
  }
  onViewAvalibleOfferings() {
    this.router.navigate(['./invest'])
  }
  selectSummary() {
    this.Selected = 'Summary';
    this.summaryBool = true;
    this.documentBool = false;
    this.locationBool = false;
    this.updatenBool = false;
  }
  selectDocument() {
    this.Selected = 'Documents';
    this.summaryBool = false;
    this.documentBool = true;
    this.locationBool = false;
    this.updatenBool = false;
  }
  selectLocation() {
    this.Selected = 'Location';
    this.summaryBool = false;
    this.documentBool = false;
    this.locationBool = true;
    this.updatenBool = false;
  }
  selectUpdate() {
    this.Selected = 'Update';
    this.summaryBool = false;
    this.documentBool = false;
    this.locationBool = false;
    this.updatenBool = true;
  }
  CloseViewDetails(val: any) {
    this.ViewDetails = false;
    if (val.active == true && val.isReservation == false) {
      this.selectActiveOffering();
    }
    else if (val.active == false && val.isReservation == false) {
      this.selectPastOffering();
    }
    else if (val.active == true && val.isReservation == true) {
      this.selectReservation();
    }
  }
  selectActiveOffering() {
    this.Selected = 'ActiveOffering';
    this.Activeofferingtab = true;
    this.Reservationtab = false;
    this.Pastofferingtab = false;
    this.ViewDetails = false;
  }
  selectReservation() {
    this.Selected = 'Reservation';
    this.Activeofferingtab = false;
    this.Reservationtab = true;
    this.Pastofferingtab = false;
    this.ViewDetails = false;
  }
  selectPastOffering() {
    this.Selected = 'PastOffering';
    this.Activeofferingtab = false;
    this.Reservationtab = false;
    this.Pastofferingtab = true;
    this.ViewDetails = false;
  }
  onEditReservation(val: any) {
    this.EditReservationPopup = true;
    this.InsAmtError = false;
    this.MinimumError = false;
    this.InvestError = false;
    this.AddEditData = val;
    this.InvestingProfile = +this.AddEditData.userProfileId
    this.InvestmentAmount = +this.AddEditData.amount
    this.ReservationId = +this.AddEditData.id
  }
  onChooseProfileChange(e: any) {
    this.InvestingProfile = +e.target.value;
    if (e.target.value == 0) {
      this.InvestError = true;
    }
    else {
      this.InvestError = false;
    }
    let x = this.ReservationData.filter((x: { userProfileId: any; }) => x.userProfileId == this.InvestingProfile)
    if (x.length != 0) {
      this.ReservationId = x[0].id
      this.InvestmentAmount = x[0].amount
    }
    else {
      this.InvestmentAmount = ''
    }
  }
  GetProfile() {
    this.profileService.GetProfileById(this.UserId).subscribe(data => {
      let x: any = data;
      this.ProfileValue = [];
      this.ProfileValue.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < x.length; i++) {
        if (x[i].name != null) {
          this.ProfileValue.push({ id: x[i].id, name: x[i].name, active: x[i].active })
        }
        else if (x[i].firstName != null) {
          this.ProfileValue.push({ id: x[i].id, name: x[i].firstName, active: x[i].active })
        }
        else if (x[i].trustName != null) {
          this.ProfileValue.push({ id: x[i].id, name: x[i].trustName, active: x[i].active })
        }
        else if (x[i].retirementPlanName != null) {
          this.ProfileValue.push({ id: x[i].id, name: x[i].retirementPlanName, active: x[i].active })
        }
      }
    })
  }
  onInsAmt(e: any) {
    let x = this.AddEditData.keyHighlights.filter((x: { name: string; }) => x.name == 'Reservation Size')
    let y = x[0].value
    this.InvestmentAmount = e.target.value
    if (this.InvestmentAmount == null || this.InvestmentAmount == '') {
      this.InsAmtError = true;
      this.MinimumError = false;
    }
    else if (+this.InvestmentAmount > +y) {
      this.MinimumError = true;
      this.InsAmtError = false;
    }
    else {
      this.InsAmtError = false;
      this.MinimumError = false;
    }
  }
  onReserve(val: any) {
    if (val == "withdraw") {
      this.ReservationActive = false
    }
    else {
      this.ReservationActive = true;
    }
    if (this.InvestingProfile == 0 || this.InvestmentAmount == null) {
      if (this.InvestingProfile == 0) {
        this.InvestError = true;
      }
      else {
        this.InvestError = false;
      }
    }
    if (val != "withdraw") {
      if (this.InvestmentAmount == '' || +this.InvestmentAmount < +this.MinInvestment) {
        if (this.InvestmentAmount == null || this.InvestmentAmount == '') {
          this.InsAmtError = true;
          this.MinimumError = false;
        }
        else if (+this.InvestmentAmount < +this.MinInvestment) {
          this.MinimumError = true;
          this.InsAmtError = false;
        }
        else {
          this.InsAmtError = false;
          this.MinimumError = false;
        }
      }
    }
    if (this.InvestError == false && this.InsAmtError == false && this.MinimumError == false) {
      let reserve = {
        Id: this.ReservationId != 0 ? this.ReservationId : 0,
        UserId: +this.UserId,
        UserProfileId: this.InvestingProfile,
        OfferingId: this.AddEditData.id,
        Amount: this.InvestmentAmount == '' ? 0 : +this.InvestmentAmount,
        IsReservation: true,
        Status: 2,
        Active: this.ReservationActive
      }
      if (this.ReservationId != 0) {
        this.Loader = true;
        this.investService.UpdateReservation(reserve).subscribe(data => {
          let x: any = data
          if (x.statusCode == 1) {
            this.GetInvestmentByUserId();
            this.EditReservationPopup = false;
            this.toastr.success('Reservation updated successfully', 'Success!')
          }
        })
      }
    }
  }
  onAddNewProfile() {
    this.ProfilePopup = true;
    this.ProfilePopup = true;
    this.IRAShow = true;
    this.LLCShow = false;
    this.Individualhow = false;
    this.TrustShow = false;
    this.JointShow = false;
    this.RetirementShow = false;
    this.Chooseprofile = 1
    this.IRAForm.reset();
    this.ArrayForm = [];
    this.StateProvinceId = 0;
    this.Distributionmethod = 0;
    this.IranameError = false;
    this.LlcnameError = false;
    this.FirstnameError = false;
    this.LastnameError = false;
    this.TrustnameError = false;
    this.RegistrationnameError = false;
    this.RetirementnameError = false;
    this.TitlesignorError = false;
    this.TaxError = false;
    this.ProfileId = 0
    this.IRAForm.enable();
    this.LLCForm.enable();
    this.IndividualForm.enable();
    this.TrustForm.enable();
    this.JointForm.enable();
    this.RetirementForm.enable();
    this.ACHBool = false;
    this.CheckBool = false;
    this.OtherBool = false
  }
  onIRAName(e: any) {
    this.Iraname = e.target.value
    if (this.Iraname == '' || this.Iraname == null) {
      this.IranameError = true;
    }
    else {
      this.IranameError = false;
    }
  }
  onProvinceChange(e: any) {
    this.StateorProvince = +e.target.value;
    this.StateProvinceId = +e.target.value;
    if (this.StateorProvince == 1) {
      this.CountryStateShow = true
    }
    else {
      this.CountryStateShow = false
    }
  }
  onProvinceChange1(e: any) {
    this.StateProvinceId1 = +e.target.value;
    if (this.StateProvinceId1 == 1) {
      this.CountryStateShow = true
      this.StateError = false
      this.USAddressError = true;
    }
    else if (this.StateProvinceId1 == 0) {
      this.StateError = true
      this.CountryStateShow = false
      this.USAddressError = false;
    }
    else {
      this.StateError = false
      this.CountryStateShow = false
      this.USAddressError = false;
    }
  }
  GetStateProvince() {
    this.profileService.GetStateorProvince().subscribe(data => {
      let x: any = data
      this.Province.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < x.length; i++) {
        this.Province.push({ id: x[i].id, name: x[i].name, active: x[i].active })
      }
      this.CheckProvince = this.Province;
    })
  }
  onTaxid(e: any) {
    if (e.target.value == '' || e.target.value == null) {
      this.TaxError = false;
    }
    else {
      const validtaxRegEx1 = /^\d{3}-?\d{2}-?\d{4}$/;
      const validtaxRegEx2 = /^\d{3}-?\d{3}-?\d{3}$/;
      const validtaxRegEx3 = /^\d{2}-?\d{7}$/;
      if (validtaxRegEx1.test(e.target.value) || validtaxRegEx2.test(e.target.value) || validtaxRegEx3.test(e.target.value)) {
        this.TaxError = false;
      }
      else {
        this.TaxError = true;
      }
    }
  }
  onLLCNameChange(e: any) {
    this.Llcname = e.target.value
    if (this.Llcname == '' || this.Llcname == null) {
      this.LlcnameError = true;
    }
    else {
      this.LlcnameError = false;
    }
  }
  onDisregardedEntityChange(e: any) {
    if (e.target.value == '' || e.target.value == null) {
      this.DisregardedEntity = null
    }
    else if (e.target.value == 1) {
      this.DisregardedEntity = true;
    }
    else if (e.target.value == 2) {
      this.DisregardedEntity = false;
    }
  }
  onIRALLCChange(e: any) {
    if (e.target.value == '' || e.target.value == null) {
      this.IRALLC = null
    }
    else if (e.target.value == 1) {
      this.IRALLC = true;
    }
    else if (e.target.value == 2) {
      this.IRALLC = false;
    }
  }
  onFirstname(e: any) {
    this.Firstname = e.target.value
    if (this.Firstname == '' || this.Firstname == null) {
      this.FirstnameError = true;
    }
    else {
      this.FirstnameError = false;
    }
  }
  onLastname(e: any) {
    this.Lastname = e.target.value
    if (this.Lastname == '' || this.Lastname == null) {
      this.LastnameError = true;
    }
    else {
      this.LastnameError = false;
    }
  }
  onTrustname(e: any) {
    this.Trustname = e.target.value
    if (this.Trustname == '' || this.Trustname == null) {
      this.TrustnameError = true;
    }
    else {
      this.TrustnameError = false;
    }
  }
  onArrayFirstname(e: any) {
    this.FirstName = e.target.value
    if (this.FirstName == '' || this.FirstName == null) {
      this.ArrayFirstnameError = true;
    }
    else {
      this.ArrayFirstnameError = false;
    }
  }
  onArrayLastname(e: any) {
    this.LastName = e.target.value
    if (this.LastName == '' || this.LastName == null) {
      this.ArrayLastnameError = true;
    }
    else {
      this.ArrayLastnameError = false;
    }
  }
  onArrayEmail(e: any) {
    const validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    this.ArrayEmailValue = e.target.value;
    if (this.ArrayEmailValue == null || this.ArrayEmailValue == ' ' || this.ArrayEmailValue == '') {
      this.ArrayEmail = true;
      this.ArrayvalidEmail = false;
    }
    else {
      this.ArrayEmail = false;
      if (validEmailRegEx.test(this.ArrayEmailValue)) {
        this.ArrayvalidEmail = false;
      } else {
        this.ArrayvalidEmail = true;
      }
    }
  }
  onArrayPhone(e: any) {
    this.ArrayPhone = e.target.value;
    if (this.ArrayPhone == '') {
      this.ArrayPhoneEmpty = true;
      this.ArrayPhonelength = false
    }
    else {
      this.ArrayPhoneEmpty = false;
      if (e.target.value.length == 10) {
        this.ArrayPhonelength = false
      }
      else {
        this.ArrayPhonelength = true
      }
    }
  }
  numberValidation(event: any): Boolean {
    if (event.keyCode >= 48 && event.keyCode <= 57)
      return true;
    else
      return false;
  }
  onAddInvestor() {
    let arr = {
      id: 0,
      FirstName: '',
      LastName: '',
      EmailId: '',
      Phone: '',
      Active: true,
      Status: 1,
      IsOwner: false,
    }
    this.ArrayForm.push(arr);
  }
  onRegistrationnameChange(e: any) {
    this.Registrationname = e.target.value
    if (this.Registrationname == '' || this.Registrationname == null) {
      this.RegistrationnameError = true;
    }
    else {
      this.RegistrationnameError = false;
    }
  }
  onRetirementnameChanges(e: any) {
    this.Retirementname = e.target.value
    if (this.Retirementname == '' || this.Retirementname == null) {
      this.RetirementnameError = true;
    }
    else {
      this.RetirementnameError = false;
    }
  }
  onTitlesignorChanges(e: any) {
    this.Titlesignor = e.target.value
    if (this.Titlesignor == '' || this.Titlesignor == null) {
      this.TitlesignorError = true;
    }
    else {
      this.TitlesignorError = false;
    }
  }
  onDistrubutionMethodChange(e: any) {
    this.DistributionMethodId = +e.target.value;
    this.Distributionmethod = +e.target.value;
    this.BankAccountId = 0;
    this.CheckForm.reset();
    this.StateProvinceId1 = 0;
    this.OtherDetails = null
    this.CheckForm.enable();
    if (e.target.value == 1) {
      this.ACHBool = true;
      this.CheckBool = false;
      this.OtherBool = false;
    }
    else if (e.target.value == 2) {
      this.BankAccountId = null;
      this.ACHBool = false;
      this.CheckBool = true;
      this.OtherBool = false;
    }
    else if (e.target.value == 3) {
      this.BankAccountId = null;
      this.ACHBool = false;
      this.CheckBool = false;
      this.OtherBool = true;
    }
    else {
      this.BankAccountId = null;
      this.ACHBool = false;
      this.CheckBool = false;
      this.OtherBool = false;
    }
  }
  GetDistribution() {
    this.profileService.GetDistribution().subscribe(data => {
      let x: any = data;
      this.DistributionValue.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < x.length; i++) {
        this.DistributionValue.push({ id: x[i].id, name: x[i].name, active: x[i].active })
      }
    })
  }
  onBankAccountChange(e: any) {
    this.BankAccountId = +e.target.value
  }
  GetBankDetails() {
    this.profileService.GetBankDetails(this.UserId).subscribe(data => {
      this.BankDetails = data;
      this.BankDetailsData = [];
      this.BankDetailsData.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < this.BankDetails.length; i++) {
        this.BankDetailsData.push({ id: this.BankDetails[i].id, name: this.BankDetails[i].bankName + '(' + this.BankDetails[i].accountNumber + ')', active: this.BankDetails[i].active })
      }
    })
  }
  onStreetChanges(e: any) {
    if (e.target.value == null || e.target.value == '' || this.CheckForm.value.Streetaddress1 == null || this.CheckForm.value.Streetaddress1 == '') {
      this.StreetBool = true;
    }
    else {
      this.StreetBool = false;
    }
  }
  onCityChanges(e: any) {
    if (e.target.value == null || e.target.value == '' || this.CheckForm.value.City == null || this.CheckForm.value.City == '') {
      this.CityBool = true;
    }
    else {
      this.CityBool = false;
    }
  }
  onZipcodeChanges(e: any) {
    if (e.target.value == null || e.target.value == '' || this.CheckForm.value.Zipcode == null || this.CheckForm.value.Zipcode == '') {
      this.ZipcodeBool = true;
    }
    else {
      this.ZipcodeBool = false;
    }
  }
  onBankAccount() {
    this.BankId = 0;
    this.BankPopup = true;
    this.BankTitleShow = false;
    this.BanknameError = false;
    this.AccounttypeError = false;
    this.RoutingnumberError = false;
    this.RoutingnumberLength = false;
    this.ConfirmroutingnumberError = false;
    this.ConfirmRoutingMatch = false;
    this.AccountnumberError = false;
    this.ConfirmaccountnumberError = false;
    this.ConfirmAccountMatch = false;
    this.BankForm.reset();
    this.Accounttype = 0;
  }
  onSaveProfile() {
    this.DistributionMethod();
    if (this.Chooseprofile == 1) {
      if (this.IRAForm.value.Iraname == '' || this.IRAForm.value.Iraname == null) {
        this.IranameError = true
      }
      else if (this.StreetBool == true || this.CityBool == true || this.ZipcodeBool == true || this.USAddressError == true || this.StateError == true) {
        this.DistributionMethod();
      }
      else {
        this.IranameError = false;
        if (this.TaxError == true) {
          this.TaxError = true;
        }
        else {
          this.profile = {
            Id: this.ProfileId != 0 ? this.ProfileId : 0,
            UserId: +this.UserId,
            Type: this.Chooseprofile,
            Name: this.IRAForm.value.Iraname,
            InCareOf: this.IRAForm.value.Incare,
            StreetAddress1: this.IRAForm.value.Streetaddress1,
            StreetAddress2: this.IRAForm.value.Streetaddress2,
            City: this.IRAForm.value.City,
            StateOrProvinceId: this.StateProvinceId,
            Country: this.IRAForm.value.Country,
            State: this.IRAForm.value.State,
            ZipCode: this.IRAForm.value.Zipcode,
            TaxId: this.IRAForm.value.Taxid,
            DistributionTypeId: this.Distributionmethod,
            BankAccountId: this.BankAccountId == 0 ? null : this.BankAccountId,
            CheckInCareOf: this.CheckForm.value.Incare,
            CheckAddressLine1: this.CheckForm.value.Streetaddress1,
            CheckAddressLine2: this.CheckForm.value.Streetaddress2,
            CheckCity: this.CheckForm.value.City,
            CheckStateId: +this.StateProvinceId1,
            CheckZip: this.CheckForm.value.Zipcode,
            DistributionDetail: this.OtherDetails
          }
          this.CreateUpdateProfile();
        }
      }
    }
    else if (this.Chooseprofile == 2) {
      if (this.LLCForm.value.Llcname == null || this.LLCForm.value.Llcname == '') {
        this.LlcnameError = true;
      }
      else if (this.StreetBool == true || this.CityBool == true || this.ZipcodeBool == true || this.USAddressError == true || this.StateError == true) {
        this.DistributionMethod();
      }
      else {
        this.LlcnameError = false;
        if (this.TaxError == true) {
          this.TaxError = true;
        }
        else {
          if (this.DisregardedEntity == 1 || this.DisregardedEntity == true) {
            this.DisregardedEntity = true;
          }
          else if (this.DisregardedEntity == 2 || this.DisregardedEntity == false) {
            this.DisregardedEntity = false
          }
          else if (this.DisregardedEntity == 0 || this.DisregardedEntity == null) {
            this.DisregardedEntity = null
          }
          if (this.IRALLC == 1 || this.IRALLC == true) {
            this.IRALLC = true;
          }
          else if (this.IRALLC == 2 || this.IRALLC == false) {
            this.IRALLC = false
          }
          else if (this.IRALLC == 0 || this.IRALLC == null) {
            this.IRALLC = null
          }
          this.profile = {
            Id: this.ProfileId != 0 ? this.ProfileId : 0,
            UserId: +this.UserId,
            Type: this.Chooseprofile,
            Name: this.LLCForm.value.Llcname,
            InCareOf: this.LLCForm.value.Incare,
            StreetAddress1: this.LLCForm.value.Streetaddress1,
            StreetAddress2: this.LLCForm.value.Streetaddress2,
            City: this.LLCForm.value.City,
            StateOrProvinceId: this.StateProvinceId,
            Country: this.LLCForm.value.Country,
            State: this.LLCForm.value.State,
            ZipCode: this.LLCForm.value.Zipcode,
            TaxId: this.LLCForm.value.Taxid,
            IsDisregardedEntity: this.DisregardedEntity,
            IsIRALLC: this.IRALLC,
            DistributionTypeId: this.Distributionmethod,
            BankAccountId: this.BankAccountId == 0 ? null : this.BankAccountId,
            CheckInCareOf: this.CheckForm.value.Incare,
            CheckAddressLine1: this.CheckForm.value.Streetaddress1,
            CheckAddressLine2: this.CheckForm.value.Streetaddress2,
            CheckCity: this.CheckForm.value.City,
            CheckStateId: +this.StateProvinceId1,
            CheckZip: this.CheckForm.value.Zipcode,
            DistributionDetail: this.OtherDetails
          }
          this.CreateUpdateProfile();
        }
      }
    }
    else if (this.Chooseprofile == 3) {
      if (this.TaxError == true || this.IndividualForm.value.Firstname == null || this.IndividualForm.value.Firstname == ''
        || this.IndividualForm.value.Lastname == null || this.IndividualForm.value.Lastname == '') {
        if (this.IndividualForm.value.Firstname == null || this.IndividualForm.value.Firstname == '') {
          this.FirstnameError = true;
        }
        else {
          this.FirstnameError = false;
        }
        if (this.IndividualForm.value.Lastname == null || this.IndividualForm.value.Lastname == '') {
          this.LastnameError = true;
        }
        else {
          this.LastnameError = false;
        }
        if (this.TaxError == true) {
          this.TaxError = true;
        }
      }
      else if (this.StreetBool == true || this.CityBool == true || this.ZipcodeBool == true || this.USAddressError == true || this.StateError == true) {
        this.DistributionMethod();
      }
      else {
        this.profile = {
          Id: this.ProfileId != 0 ? this.ProfileId : 0,
          UserId: +this.UserId,
          Type: this.Chooseprofile,
          FirstName: this.IndividualForm.value.Firstname,
          LastName: this.IndividualForm.value.Lastname,
          StreetAddress1: this.IndividualForm.value.Streetaddress1,
          StreetAddress2: this.IndividualForm.value.Streetaddress2,
          City: this.IndividualForm.value.City,
          StateOrProvinceId: this.StateProvinceId,
          Country: this.IndividualForm.value.Country,
          State: this.IndividualForm.value.State,
          ZipCode: this.IndividualForm.value.Zipcode,
          TaxId: this.IndividualForm.value.Taxid,
          DistributionTypeId: this.Distributionmethod,
          BankAccountId: this.BankAccountId == 0 ? null : this.BankAccountId,
          CheckInCareOf: this.CheckForm.value.Incare,
          CheckAddressLine1: this.CheckForm.value.Streetaddress1,
          CheckAddressLine2: this.CheckForm.value.Streetaddress2,
          CheckCity: this.CheckForm.value.City,
          CheckStateId: +this.StateProvinceId1,
          CheckZip: this.CheckForm.value.Zipcode,
          DistributionDetail: this.OtherDetails
        }
        this.CreateUpdateProfile();
      }
    }
    else if (this.Chooseprofile == 4) {
      if (this.TrustForm.value.Trustname == null || this.TrustForm.value.Trustname == '') {
        this.TrustnameError = true;
      }
      else if (this.StreetBool == true || this.CityBool == true || this.ZipcodeBool == true || this.USAddressError == true || this.StateError == true) {
        this.DistributionMethod();
      }
      else {
        this.TrustnameError = false;
        if (this.TaxError == true) {
          this.TaxError = true;
        }
        else {
          this.profile = {
            Id: this.ProfileId != 0 ? this.ProfileId : 0,
            UserId: +this.UserId,
            Type: this.Chooseprofile,
            TrustName: this.TrustForm.value.Trustname,
            InCareOf: this.TrustForm.value.Incare,
            StreetAddress1: this.TrustForm.value.Streetaddress1,
            StreetAddress2: this.TrustForm.value.Streetaddress2,
            City: this.TrustForm.value.City,
            StateOrProvinceId: this.StateProvinceId,
            Country: this.TrustForm.value.Country,
            State: this.TrustForm.value.State,
            ZipCode: this.TrustForm.value.Zipcode,
            TaxId: this.TrustForm.value.Taxid,
            DistributionTypeId: this.Distributionmethod,
            BankAccountId: this.BankAccountId == 0 ? null : this.BankAccountId,
            CheckInCareOf: this.CheckForm.value.Incare,
            CheckAddressLine1: this.CheckForm.value.Streetaddress1,
            CheckAddressLine2: this.CheckForm.value.Streetaddress2,
            CheckCity: this.CheckForm.value.City,
            CheckStateId: +this.StateProvinceId1,
            CheckZip: this.CheckForm.value.Zipcode,
            DistributionDetail: this.OtherDetails
          }
          this.CreateUpdateProfile();
        }
      }
    }
    else if (this.Chooseprofile == 5) {
      if (this.JointForm.value.Registrationname == null || this.JointForm.value.Registrationname == '') {
        this.RegistrationnameError = true;
      }
      else {
        this.RegistrationnameError = false;
      }
      this.AddInvestorCheck();
      if (this.TaxError == true) {
        this.TaxError = true;
      }
      else if (this.StreetBool == true || this.CityBool == true || this.ZipcodeBool == true || this.USAddressError == true || this.StateError == true) {
        this.DistributionMethod();
      }
      else {
        this.TaxError = false;
        if (this.RegistrationnameError == false && this.ArrayFirstnameError == false && this.ArrayLastnameError == false
          && this.ArrayvalidEmail == false && this.ArrayEmail == false && this.ArrayPhoneEmpty == false) {
          this.profile = {
            Id: this.ProfileId != 0 ? this.ProfileId : 0,
            UserId: +this.UserId,
            Type: this.Chooseprofile,
            Name: this.JointForm.value.Registrationname,
            InCareOf: this.JointForm.value.Incare,
            StreetAddress1: this.JointForm.value.Streetaddress1,
            StreetAddress2: this.JointForm.value.Streetaddress2,
            City: this.JointForm.value.City,
            StateOrProvinceId: this.StateProvinceId,
            Country: this.JointForm.value.Country,
            State: this.JointForm.value.State,
            ZipCode: this.JointForm.value.Zipcode,
            TaxId: this.JointForm.value.Taxid,
            DistributionTypeId: this.Distributionmethod,
            Investers: this.ArrayForm,
            BankAccountId: this.BankAccountId == 0 ? null : this.BankAccountId,
            CheckInCareOf: this.CheckForm.value.Incare,
            CheckAddressLine1: this.CheckForm.value.Streetaddress1,
            CheckAddressLine2: this.CheckForm.value.Streetaddress2,
            CheckCity: this.CheckForm.value.City,
            CheckStateId: +this.StateProvinceId1,
            CheckZip: this.CheckForm.value.Zipcode,
            DistributionDetail: this.OtherDetails
          }
          this.CreateUpdateProfile();
        }
      }
    }
    else if (this.Chooseprofile == 6) {
      if (this.TaxError == true
        || this.RetirementForm.value.Retirementname == null || this.RetirementForm.value.Retirementname == ''
        || this.RetirementForm.value.Titlesignor == null || this.RetirementForm.value.Titlesignor == ''
      ) {
        if (this.RetirementForm.value.Retirementname == null || this.RetirementForm.value.Retirementname == '') {
          this.RetirementnameError = true;
        }
        else {
          this.RetirementnameError = false;
        }
        if (this.RetirementForm.value.Titlesignor == null || this.RetirementForm.value.Titlesignor == '') {
          this.TitlesignorError = true;
        }
        else {
          this.TitlesignorError = false;
        }
        if (this.TaxError == true) {
          this.TaxError = true;
        }
      }
      else if (this.StreetBool == true || this.CityBool == true || this.ZipcodeBool == true || this.USAddressError == true || this.StateError == true) {
        this.DistributionMethod();
      }
      else {
        this.profile = {
          Id: this.ProfileId != 0 ? this.ProfileId : 0,
          UserId: +this.UserId,
          Type: this.Chooseprofile,
          RetirementPlanName: this.RetirementForm.value.Retirementname,
          SignorName: this.RetirementForm.value.Titlesignor,
          StreetAddress1: this.RetirementForm.value.Streetaddress1,
          StreetAddress2: this.RetirementForm.value.Streetaddress2,
          City: this.RetirementForm.value.City,
          StateOrProvinceId: this.StateProvinceId,
          Country: this.RetirementForm.value.Country,
          State: this.RetirementForm.value.State,
          ZipCode: this.RetirementForm.value.Zipcode,
          TaxId: this.RetirementForm.value.Taxid,
          DistributionTypeId: this.Distributionmethod,
          BankAccountId: this.BankAccountId == 0 ? null : this.BankAccountId,
          CheckInCareOf: this.CheckForm.value.Incare,
          CheckAddressLine1: this.CheckForm.value.Streetaddress1,
          CheckAddressLine2: this.CheckForm.value.Streetaddress2,
          CheckCity: this.CheckForm.value.City,
          CheckStateId: +this.StateProvinceId1,
          CheckZip: this.CheckForm.value.Zipcode,
          DistributionDetail: this.OtherDetails
        }
        this.CreateUpdateProfile();
      }
    }
  }
  DistributionMethod() {
    if (this.Distributionmethod == 2) {
      if (this.CheckForm.value.Streetaddress1 == null || this.CheckForm.value.Streetaddress1 == '') {
        this.StreetBool = true;
      }
      else {
        this.StreetBool = false;
      }
      if (this.CheckForm.value.City == null || this.CheckForm.value.City == '') {
        this.CityBool = true;
      }
      else {
        this.CityBool = false;
      }
      if (this.CheckForm.value.Zipcode == null || this.CheckForm.value.Zipcode == '') {
        this.ZipcodeBool = true;
      }
      else {
        this.ZipcodeBool = false;
      }
      if (this.StateProvinceId1 == 1) {
        this.CountryStateShow = true
        this.StateError = false
      }
      else if (this.StateProvinceId1 == 0) {
        this.StateError = true
        this.CountryStateShow = false
      }
      else {
        this.StateError = false
        this.CountryStateShow = false
      }
    }
  }
  private CreateUpdateProfile() {
    if (this.ProfileId == 0) {
      this.profileService.CreateProfile(this.profile).subscribe(data => {
        if (data == 1) {
          this.ProfilePopup = false;
          this.GetProfile();
        }
        else {
          this.toastr.error('Data not saved');
        }
      });
    }
    else {
      this.profileService.UpdateProfile(this.profile).subscribe(data => {
        if (data == 1) {
          this.ProfilePopup = false;
          this.GetProfile();
        }
        else {
          this.toastr.error('Data not saved');
        }
      });
    }
  }
  private AddInvestorCheck() {
    const validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    for (let i = 0; i < this.ArrayForm.length; i++) {
      if (this.ArrayForm[i].FirstName == '' || this.ArrayForm[i].FirstName == null) {
        this.ArrayFirstnameError = true;
      }
      else {
        this.ArrayFirstnameError = false;
      }
      if (this.ArrayForm[i].LastName == '' || this.ArrayForm[i].LastName == null) {
        this.ArrayLastnameError = true;
      }
      else {
        this.ArrayLastnameError = false;
      }
      if (this.ArrayForm[i].EmailId == null || this.ArrayForm[i].EmailId == ' ' || this.ArrayForm[i].EmailId == '') {
        this.ArrayEmail = true;
        this.ArrayvalidEmail = false;
      }
      else {
        this.ArrayEmail = false;
        if (validEmailRegEx.test(this.ArrayForm[i].EmailId)) {
          this.ArrayvalidEmail = false;
        } else {
          this.ArrayvalidEmail = true;
        }
      }
      if (this.ArrayForm[i].Phone == '') {
        this.ArrayPhoneEmpty = true;
      }
      else {
        this.ArrayPhoneEmpty = false;
      }
    }
  }
  onBanknameChanges(e: any) {
    this.Bankname = e.target.value
    if (this.Bankname == '' || this.Bankname == null) {
      this.BanknameError = true;
    }
    else {
      this.BanknameError = false;
    }
  }
  onAccounttypeChanges(e: any) {
    this.Accounttype = +e.target.value
    if (this.Accounttype == 0) {
      this.AccounttypeError = true;
    }
    else {
      this.AccounttypeError = false;
    }
  }
  onRoutingnumChanges(e: any) {
    this.Routingnumber = e.target.value
    if (this.Routingnumber == '' || this.Routingnumber == null) {
      this.RoutingnumberError = true;
      this.RoutingnumberLength = false;
    }
    else {
      this.RoutingnumberError = false;
      if (this.Routingnumber.length < 9) {
        this.RoutingnumberLength = true;
      }
      else {
        this.RoutingnumberLength = false;
      }
    }
  }
  onConRoutingnumChanges(e: any) {
    this.Confirmroutingnumber = e.target.value
    if (this.Confirmroutingnumber == '' || this.Confirmroutingnumber == null) {
      this.ConfirmroutingnumberError = true;
      this.ConfirmRoutingMatch = false
    }
    else {
      this.ConfirmroutingnumberError = false;
      if (this.Routingnumber == this.Confirmroutingnumber || this.BankForm.value.Routingnumber == this.BankForm.value.Confirmroutingnumber) {
        this.ConfirmRoutingMatch = false
      }
      else {
        this.ConfirmRoutingMatch = true;
      }
    }
  }
  onAccountnumberChanges(e: any) {
    this.Accountnumber = e.target.value
    if (this.Accountnumber == '' || this.Accountnumber == null) {
      this.AccountnumberError = true;
    }
    else {
      this.AccountnumberError = false;
    }
  }
  onConfirmaccountnumChanges(e: any) {
    this.Confirmaccountnumber = e.target.value;
    if (this.Confirmaccountnumber == '' || this.Confirmaccountnumber == null) {
      this.ConfirmaccountnumberError = true;
      this.ConfirmAccountMatch = false
    }
    else {
      this.ConfirmaccountnumberError = false;
      if (this.Accountnumber == this.Confirmaccountnumber || this.BankForm.value.Accountnumber == this.BankForm.value.Confirmaccountnumber) {
        this.ConfirmAccountMatch = false
      }
      else {
        this.ConfirmAccountMatch = true;
      }
    }
  }
  Routing() {
    this.showRouting = !this.showRouting;
  }
  Account() {
    this.showAccount = !this.showAccount;
  }
  onBankSave() {
    if (this.BankForm.value.Bankname == null || this.BankForm.value.Bankname == ''
      || this.BankForm.value.Accounttype == 0
      || this.BankForm.value.Routingnumber == null || this.BankForm.value.Routingnumber == '' || this.BankForm.value.Routingnumber.length < 9
      || this.BankForm.value.Confirmroutingnumber == null || this.BankForm.value.Confirmroutingnumber == '' || this.BankForm.value.Routingnumber != this.BankForm.value.Confirmroutingnumber
      || this.BankForm.value.Accountnumber == null || this.BankForm.value.Accountnumber == ''
      || this.BankForm.value.Confirmaccountnumber == null || this.BankForm.value.Confirmaccountnumber == '' || this.BankForm.value.Accountnumber != this.BankForm.value.Confirmaccountnumber) {
      if (this.BankForm.value.Bankname == '' || this.BankForm.value.Bankname == null) {
        this.BanknameError = true;
      }
      else {
        this.BanknameError = false;
      }
      if (this.Accounttype == 0) {
        this.AccounttypeError = true;
      }
      else {
        this.AccounttypeError = false;
      }
      if (this.BankForm.value.Routingnumber == '' || this.BankForm.value.Routingnumber == null) {
        this.RoutingnumberError = true;
        this.RoutingnumberLength = false;
      }
      else {
        this.RoutingnumberError = false;
        if (this.BankForm.value.Routingnumber.length < 9) {
          this.RoutingnumberLength = true;
        }
        else {
          this.RoutingnumberLength = false;
        }
      }
      if (this.BankForm.value.Confirmroutingnumber == '' || this.BankForm.value.Confirmroutingnumber == null) {
        this.ConfirmroutingnumberError = true;
        this.ConfirmRoutingMatch = false
      }
      else {
        this.ConfirmroutingnumberError = false;
        if (this.BankForm.value.Routingnumber == this.BankForm.value.Confirmroutingnumber) {
          this.ConfirmRoutingMatch = false
        }
        else {
          this.ConfirmRoutingMatch = true;
        }
      }
      if (this.BankForm.value.Accountnumber == '' || this.BankForm.value.Accountnumber == null) {
        this.AccountnumberError = true;
      }
      else {
        this.AccountnumberError = false;
      }
      if (this.BankForm.value.Confirmaccountnumber == '' || this.BankForm.value.Confirmaccountnumber == null) {
        this.ConfirmaccountnumberError = true;
        this.ConfirmAccountMatch = false
      }
      else {
        this.ConfirmaccountnumberError = false;
        if (this.BankForm.value.Accountnumber == this.BankForm.value.Confirmaccountnumber) {
          this.ConfirmAccountMatch = false
        }
        else {
          this.ConfirmAccountMatch = true;
        }
      }
    }
    else {
      let bank = {
        Id: this.BankId != 0 ? this.BankId : 0,
        UserId: +this.UserId,
        BankName: this.BankForm.value.Bankname,
        AccountType: this.Accounttype,
        RoutingNumber: this.BankForm.value.Routingnumber,
        AccountNumber: this.BankForm.value.Accountnumber,
      }
      if (this.BankId == 0) {
        this.profileService.CreateBank(bank).subscribe(data => {
          if (data == 1) {
            this.GetBankDetails();
            this.BankPopup = false
          }
          else {
            this.toastr.error("Data not saved", 'Error!')
          }
        })
      }
      else if (this.BankId != 0) {
        this.profileService.UpdateBank(bank).subscribe(data => {
          if (data == 1) {
            this.BankPopup = false;
            this.GetBankDetails();
          }
          else {
            this.toastr.error("Data not saved", 'Error!')
          }
        })
      }
    }
  }
  GetChooseProfile() {
    this.profileService.GetChooseProfile().subscribe(data => {
      this.ChooseProfileValue = data
    })
  }
  onChooseProfileChange1(e: any) {
    this.IranameError = false;
    this.LlcnameError = false;
    this.FirstnameError = false;
    this.LastnameError = false;
    this.TrustnameError = false;
    this.RegistrationnameError = false;
    this.RetirementnameError = false;
    this.TitlesignorError = false;
    this.TaxError = false;
    this.Chooseprofile = +e.target.value;
    this.IRAForm.reset();
    this.LLCForm.reset();
    this.IndividualForm.reset();
    this.TrustForm.reset();
    this.JointForm.reset();
    this.RetirementForm.reset();
    this.StateProvinceId = 0;
    this.Distributionmethod = 0;
    if (this.Chooseprofile == 1) {
      this.IRAShow = true;
      this.LLCShow = false;
      this.Individualhow = false;
      this.TrustShow = false;
      this.JointShow = false;
      this.RetirementShow = false;
    }
    else if (this.Chooseprofile == 2) {
      this.IRAShow = false;
      this.LLCShow = true;
      this.Individualhow = false;
      this.TrustShow = false;
      this.JointShow = false;
      this.RetirementShow = false;
    }
    else if (this.Chooseprofile == 3) {
      this.IRAShow = false;
      this.LLCShow = false;
      this.Individualhow = true;
      this.TrustShow = false;
      this.JointShow = false;
      this.RetirementShow = false;
    }
    else if (this.Chooseprofile == 4) {
      this.IRAShow = false;
      this.LLCShow = false;
      this.Individualhow = false;
      this.TrustShow = true;
      this.JointShow = false;
      this.RetirementShow = false;
    }
    else if (this.Chooseprofile == 5) {
      if (this.ArrayForm.length == 0) {
        this.ArrayForm.push({
          Id: 0,
          FirstName: this.UserData.firstName,
          LastName: this.UserData.lastName,
          EmailId: this.UserData.emailId,
          Phone: this.UserData.phoneNumber,
          IsOwner: true
        })
      }
      this.IRAShow = false;
      this.LLCShow = false;
      this.Individualhow = false;
      this.TrustShow = false;
      this.JointShow = true;
      this.RetirementShow = false;
    }
    else if (this.Chooseprofile == 6) {
      this.IRAShow = false;
      this.LLCShow = false;
      this.Individualhow = false;
      this.TrustShow = false;
      this.JointShow = false;
      this.RetirementShow = true;
    }
  }
  GetUserById() {
    this.profileService.GetUserById(this.UserId).subscribe(data => {
      this.UserData = data;
    })
  }
  GetReservationById() {
    this.investService.GetReservationById(this.UserId).subscribe(data => {
      this.ReservationData = data;
    })
  }
  GetHeaderData() {
    this.investService.GetHeaderDataByMyInvestment(this.UserId).subscribe(data => {
      this.HeaderData = data;
    })
  }
  GoogleMap(){
    this.Markers.push({
      position: {
        lat: 51.673858,
        lng: 7.815982,
      },
      label: {
        color: 'black',
        text: 'Madrid',
      },
    });
  }

}
