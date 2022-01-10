// import { CurrencyPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InvestService } from '../invest/invest.service';
import { InvestorProfileService } from '../investor-profile/investor-profile.service';
import { InvestmentService } from './investment.service';

@Component({
  selector: 'app-investment',
  templateUrl: './investment.component.html',
  styleUrls: ['./investment.component.css'],
  // providers : [CurrencyPipe]
})
export class InvestmentComponent implements OnInit {
  token: any;
  UserId: any;
  InvestmentData: any;
  KeyHighlights: any = [];
  DiligenceShow: boolean = false;
  InvestShow: boolean = false;
  ESignShow: boolean = false;
  FundInvestmentShow: boolean = false;
  InvestorTerms: any;
  InvestorTermsDisabled: boolean = false;
  EsignCheckDisabled: boolean = false;
  ProfileValue: any = [];
  InvestingProfile: any = 0;
  MinInvestment: any;
  InvestmentAmount: any;
  InvestError: boolean = false;
  InsAmtError: boolean = false;
  MinimumError: boolean = false;
  Offeringtoken: any;
  InvestmentTab: any;
  OfferingtokenName: any;
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
  Loader : boolean = false;
  diligenceshow : boolean = false;
  diligencedone : boolean = false
  investshow : boolean = false;
  investdone : boolean = false
  esignshow : boolean = false;
  esigndone : boolean = false
  fundshow : boolean = false;
  funddone : boolean = false
  EsignCheck: any;
  InvestmentId: any;
  WireTransferDate : any;
  StepNavShow : boolean = true;
  DocumentViewPopup: boolean = false;
  ViewFile: any;

  constructor(private router: Router,
    private investService: InvestService,
    private profileService: InvestorProfileService,
    private investmentService: InvestmentService,
    private formbuilder : FormBuilder,
    private toastr : ToastrService,
    // private currencyPipe : CurrencyPipe,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.Loader = true;
    this.token = +this.route.snapshot.params['id'];
    this.Offeringtoken = +this.route.snapshot.params['offId']
    this.OfferingtokenName = this.route.snapshot.params['name']
    this.UserId = Number(localStorage.getItem('UserId'))
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
    this.DiligenceShow = true;
    this.diligenceshow = true
    this.GetMarketplaceById();
    this.GetProfile();

    this.GetStateProvince();
    this.GetDistribution();
    this.GetBankDetails();
    this.GetChooseProfile();
    this.GetUserById();
    if(this.OfferingtokenName == null || this.OfferingtokenName == 'undefined'){
      this.Loader = false;
    }
    else{
      this.GetInvestmentByTab();
    }
  }

  GetMarketplaceById() {
    this.investService.GetMarketplaceById(this.token).subscribe(data => {
      console.log(data,'market')
      this.InvestmentData = data;
      console.log(this.InvestmentData.name,'market')
      for (let i = 0; i < this.InvestmentData.keyHighlights.length; i++) {
        if (i == 0 || i == 1 || i == 2) {
          this.KeyHighlights.push(this.InvestmentData.keyHighlights[i])
        }
      }
      let x = this.KeyHighlights.filter((x: { name: string; }) => x.name == 'Minimum Investment')
      this.MinInvestment = x[0].value;
      this.StepNavShow = true;
      console.log(x, 'x')
      console.log(this.KeyHighlights, 'KeyHighlights')
    })
  }
  onBack() {
    if(this.OfferingtokenName != null){
      this.router.navigate(['./../../../../myinvestment'], { relativeTo: this.route });
    }
    else{
      this.router.navigate(['./../../invest'], { relativeTo: this.route });
    }
  }
  onInvestorTermsChanges(e: any) {
    if (this.InvestorTerms == true) {
      this.InvestorTermsDisabled = true;
    }
    else {
      this.InvestorTermsDisabled = false;
    }
  }
  DiligenceNext() {
    this.DiligenceShow = false;
    this.InvestShow = true;
    this.ESignShow = false;
    this.FundInvestmentShow = false;
    this.diligencedone = true;
    this.investshow = true;
    this.investdone = false;
    this.InvestmentId = 0;
  }
  GetProfile() {
    this.profileService.GetProfileById(this.UserId).subscribe(data => {
      let x: any = data;
      this.ProfileValue = [];
      this.ProfileValue.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < x.length; i++) {
        if(x[i].name != null){
          this.ProfileValue.push({ id: x[i].id, name: x[i].name, active: x[i].active })
        }
        else if(x[i].firstName != null){
          this.ProfileValue.push({ id: x[i].id, name: x[i].firstName, active: x[i].active })
        }
        else if(x[i].trustName != null){
          this.ProfileValue.push({ id: x[i].id, name: x[i].trustName, active: x[i].active })
        }
        else if(x[i].retirementPlanName != null){
          this.ProfileValue.push({ id: x[i].id, name: x[i].retirementPlanName, active: x[i].active })
        }
      }
      console.log(this.ProfileValue, 'profilevalue')
    })
  }
  onChooseProfileChange(e: any) {
    this.InvestingProfile = +e.target.value;
    if (e.target.value == 0) {
      this.InvestError = true;
    }
    else {
      this.InvestError = false;
    }
  }
  onInsAmt(e: any) {
    this.InvestmentAmount = e.target.value
    // this.InvestmentAmount = this.currencyPipe.transform(e.target.value)
    if (this.InvestmentAmount == null || this.InvestmentAmount == '') {
      this.InsAmtError = true;
      this.MinimumError = false;
    }
    else if (+this.InvestmentAmount > +this.MinInvestment) {
      this.MinimumError = true;
      this.InsAmtError = false;
    }
    else {
      this.InsAmtError = false;
      this.MinimumError = false;
    }
  }
  InvestBack(e: any) {
    if (e == 1) {
      this.DiligenceShow = true;
      this.InvestShow = false;
      this.ESignShow = false;
      this.FundInvestmentShow = false;
    }
    if (e == 2) {
      this.DiligenceShow = false;
      this.InvestShow = true;
      this.ESignShow = false;
      this.FundInvestmentShow = false;
    }
    if (e == 3) {
      this.DiligenceShow = false;
      this.InvestShow = false;
      this.ESignShow = true;
      this.FundInvestmentShow = false;
    }
  }
  InvestNext() {
    if (this.InvestingProfile == 0 || this.InvestmentAmount == null || this.InvestmentAmount == '' || +this.InvestmentAmount > +this.MinInvestment) {
      if (this.InvestingProfile == 0) {
        this.InvestError = true;
      }
      else {
        this.InvestError = false;
      }
      if (this.InvestmentAmount == null || this.InvestmentAmount == '') {
        this.InsAmtError = true;
        this.MinimumError = false;
      }
      else {
        this.InsAmtError = false;
      }
      if (+this.InvestmentAmount > +this.MinInvestment) {
        this.MinimumError = true;
        this.InsAmtError = false;
      }
      else {
        this.MinimumError = false;
      }
    }
    else {
      let invest = {
        Id: this.InvestmentId != 0? this.InvestmentId : 0,
        UserId: this.UserId,
        UserProfileId: this.InvestingProfile,
        OfferingId: this.token,
        Amount: +this.InvestmentAmount,
        Status: 3,
        IsConfirmed: this.InvestorTerms
      }
      if(this.InvestmentId == 0){
        this.investmentService.CreateInvestment(invest).subscribe(data => {
          console.log(data, '1next')
          let x: any = data;
          if (x.statusCode == 1) {
            this.DiligenceShow = false;
            this.InvestShow = false;
            this.ESignShow = true;
            this.FundInvestmentShow = false;
            this.investdone = true;
            this.esignshow = true;
          }
        })
      }
      else{
        this.investmentService.UpdateInvestment(invest).subscribe(data => {
          console.log(data, '1next')
          let x: any = data;
          if (x.statusCode == 1) {
            this.DiligenceShow = false;
            this.InvestShow = false;
            this.ESignShow = true;
            this.FundInvestmentShow = false;
            this.investdone = true;
            this.esignshow = true;
          }
        })
      }
    }
  }
  EsignNext(){
    let invest = {
      Id: this.InvestmentId,
      UserId: this.UserId,
      OfferingId: this.token,
      Status: 3,
      IsConfirmed: this.InvestorTerms,
      IseSignCompleted : this.EsignCheckDisabled
    }
    this.investmentService.UpdateInvestment(invest).subscribe(data => {
      console.log(data, '1next')
      let x: any = data;
      if (x.statusCode == 1) {
        this.EsignSkip();
      }
    })
  }

  GetInvestmentByTab() {
    this.investmentService.GetInvestmentDetailsById(this.UserId, this.Offeringtoken).subscribe(data => {
      console.log(data, 'getinvestment')
      this.InvestmentTab = data;
      this.InvestorTerms = this.InvestmentTab.isConfirmed
      this.InvestingProfile = this.InvestmentTab.userProfileId
      this.InvestmentAmount = this.InvestmentTab.amount
      this.InvestmentId = this.InvestmentTab.id
      this.StepNavShow =  true;
      if (this.InvestmentTab.isConfirmed == false) {
        this.DiligenceShow = true;
        this.InvestShow = false;
        this.ESignShow = false;
        this.FundInvestmentShow = false;
        this.Loader = false;
      }
      else if (this.InvestmentTab.iseSignCompleted == false && this.OfferingtokenName == 'esign') {
        this.DiligenceShow = false;
        this.InvestShow = false;
        this.ESignShow = true;
        this.FundInvestmentShow = false;
        this.diligencedone = true;
        this.investdone = true;
        this.esignshow = true;
        this.Loader = false;
      }
      else if (this.InvestmentTab.wireTransferDate == null && this.OfferingtokenName == 'fund') {
        this.DiligenceShow = false;
        this.InvestShow = false;
        this.ESignShow = false;
        this.FundInvestmentShow = true;
        this.diligencedone = true;
        this.investdone = true;
        this.esigndone = true;
        this.fundshow = true;
        this.Loader = false;
      }
    })
  }
  EsigncheckValue(e : any){
    // this.EsignCheck = e.target.value
    // console.log(this.EsignCheck,'esigncheck')
    if (this.EsignCheck == true) {
      this.EsignCheckDisabled = true;
    }
    else {
      this.EsignCheckDisabled = false;
    }
  }
  EsignSkip(){
    this.DiligenceShow = false;
    this.InvestShow = false;
    this.ESignShow = false;
    this.FundInvestmentShow = true;
    this.esigndone = true;
  }
  onFundInvestmentSubmit(){
    let invest = {
      Id: this.InvestmentId,
      UserId: this.UserId,
      OfferingId: this.token,
      Status: 1,
      IsConfirmed: this.InvestorTerms,
      WireTransferDate : this.WireTransferDate
    }
    this.investmentService.UpdateInvestment(invest).subscribe(data => {
      console.log(data, 'submit')
      let x: any = data;
      if (x.statusCode == 1) {
        this.funddone = true;
        this.router.navigate(['./../../../../myinvestment'], { relativeTo: this.route });
      }
    })
  }
  onAddNewProfile(){
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
      IsOwner : false,
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
            BankAccountId: this.BankAccountId == 0? null : this.BankAccountId,
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
            BankAccountId: this.BankAccountId == 0? null : this.BankAccountId,
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
          BankAccountId: this.BankAccountId == 0? null : this.BankAccountId,
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
            BankAccountId: this.BankAccountId == 0? null : this.BankAccountId,
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
            BankAccountId: this.BankAccountId == 0? null : this.BankAccountId,
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
          BankAccountId: this.BankAccountId == 0? null : this.BankAccountId,
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
      if(this.ArrayForm.length == 0){
        this.ArrayForm.push({
          Id: 0,
          FirstName: this.UserData.firstName,
          LastName: this.UserData.lastName,
          EmailId: this.UserData.emailId,
          Phone: this.UserData.phoneNumber,
          IsOwner : true
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
  GetUserById(){
    this.profileService.GetUserById(this.UserId).subscribe(data =>{
      this.UserData = data;
    })
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
