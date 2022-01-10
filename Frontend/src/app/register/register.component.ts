import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InvestorProfileService } from '../investor-profile/investor-profile.service';
import { LoginService } from '../login/login.service';
import { RegisterService } from './register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  Residency: any = [];
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
  submitted = false
  registerForm: any;
  ContinueForm: any;
  form1: boolean = true;
  form2: boolean = false;
  residencyvalue: any;
  accreditedInvestor: any;
  lookInvest: any = 0;
  ResidencyId: any = '0';
  LookingInvestId: any = '0';
  AccreditedInvestorId: any = '0';
  FirstNameNull: boolean = false;
  FirstNameInvalid: boolean = false;
  FirstNameSpace: boolean = false;
  LastNameNull: boolean = false;
  LastNameInvalid: boolean = false;
  LastNameSpace: boolean = false;
  EmptyEmail: boolean = false;
  validEmail: boolean = false;
  CompanyUserDetailsbool: boolean = false;
  EmptyPassword: boolean = false;
  ValidPassword: boolean = false;
  ValidLowercase: boolean = false;
  ValidUppercase: boolean = false;
  ValidNumbercase: boolean = false;
  ValidSpecialcase: boolean = false;
  ValidLengthcase: boolean = false;
  EmptyConfirmPassword: boolean = false;
  ValidConfirmPassword: boolean = false;
  lookInvestbool: boolean = false;
  InvestorData: any = 0;
  InvestorDatabool: boolean = false;
  Investorcheck: any;
  Investorcheckbool: boolean = false;
  Phonelength: boolean = false;
  Phonenumberbool: boolean = false;
  Residencybool: boolean = false;
  UserData: any = [];
  registerdisabledBool: boolean = false;
  showPassword: boolean = false;
  CountryNull: boolean = false;
  CountryInvalid: boolean = false;
  CountrySpace: boolean = false;
  CountryStateShow: boolean = false;
  AccredPopup: boolean = false;
  showConfrimPassword: boolean = false;
  profileDropdown: boolean = false;
  UserId: any;
  Loader: boolean = false;
  Aboutusbool: boolean = false;

  constructor(private formbuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private loginService: LoginService,
    private toastr: ToastrService,
    private profileService: InvestorProfileService,
    private registerService: RegisterService) { }

  ngOnInit(): void {
    this.UserId = localStorage.getItem('UserId');
    this.registerForm = this.formbuilder.group({
      Firstname: ['', Validators.required],
      Lastname: ['', Validators.required],
      Email: ['', Validators.required],
      Phonenumber: ['', [Validators.required]],
      Residency: ['', Validators.required],
      Country: [''],
      Password: ['', Validators.required],
      Confirmpassword: ['', Validators.required],
    })
    {
      validator: this.MustMatch('Password', 'ConfirmPassword')
    };
    this.ContinueForm = this.formbuilder.group({
      Invest: ['', Validators.required],
      Creditedinvestor: ['', Validators.required],
      Aboutus: ['', Validators.required],
      Investorterms: ['', Validators.required]

    })
    this.GetUser();
    this.GetStateProvince();
  }

  GetStateProvince() {
    this.profileService.GetStateorProvince().subscribe(data => {
      let x: any = data
      console.log(data, 'province')
      this.Residency.push({ id: 0, name: 'Select', active: true })
      for (let i = 0; i < x.length; i++) {
        this.Residency.push({ id: x[i].id, name: x[i].name, active: x[i].active })
      }
    })
  }

  Continue(registerData: any) {
    if (this.registerForm.invalid || this.FirstNameNull == true || this.FirstNameInvalid == true || this.FirstNameSpace == true
      || this.LastNameNull == true || this.LastNameInvalid == true || this.LastNameSpace == true
      || this.EmptyEmail == true || this.validEmail == true || this.CompanyUserDetailsbool == true
      || this.EmptyPassword == true || this.ValidLowercase == true || this.ValidUppercase == true
      || this.ValidNumbercase == true || this.ValidSpecialcase == true || this.ValidLengthcase == true
      || this.Phonenumberbool == true || this.Phonelength == true || this.Residencybool == true
      || this.CountryNull == true || this.CountryInvalid == true || this.CountrySpace == true
      || (this.ResidencyId == 1 && (this.registerForm.value.Country == ''))
      || this.EmptyConfirmPassword == true || this.ValidConfirmPassword == true) {
      if (this.registerForm.value.Firstname == null || this.registerForm.value.Firstname == '') {
        this.FirstNameNull = true
      }
      if (this.registerForm.value.Lastname == null || this.registerForm.value.Lastname == '') {
        this.LastNameNull = true
      }
      if (this.registerForm.value.Email == null || this.registerForm.value.Email == '') {
        this.EmptyEmail = true
      }
      if (this.registerForm.value.Password == null || this.registerForm.value.Password == '') {
        this.EmptyPassword = true
      }
      if (this.registerForm.value.Confirmpassword == null || this.registerForm.value.Confirmpassword == '') {
        this.EmptyConfirmPassword = true
      }
      if (this.registerForm.value.Phonenumber == null || this.registerForm.value.Phonenumber == '') {
        this.Phonenumberbool = true
      }
      if (this.ResidencyId == null || this.ResidencyId == '' || this.ResidencyId == 0) {
        this.Residencybool = true;
      }
      if (this.ResidencyId == 1) {
        if (this.registerForm.value.Country == null || this.registerForm.value.Country == '') {
          this.CountryNull = true;
        }
      }
      this.submitted = true;
    }
    else {
      this.submitted = false;
      this.form1 = false;
      this.form2 = true;
    }
  }
  get f() { return this.registerForm.controls }
  get c() { return this.ContinueForm.controls }

  onchange(e: any) {
    this.registerForm.get('Residency').setValue(e.target.value);
    this.registerForm.value.Residency = e.target.value;
    this.residencyvalue = +e.target.value
    this.ResidencyId = +e.target.value;
    if (this.ResidencyId == null || this.ResidencyId == '' || this.ResidencyId == 0) {
      this.Residencybool = true
    }
    else {
      this.Residencybool = false
    }
    if (this.ResidencyId == 1) {
      this.CountryStateShow = true;
      this.registerForm.get('Country').setValue();
    }
    else {
      this.registerForm.get('Country').setValue();
      this.CountryStateShow = false;
      this.CountryNull = false;
      this.CountryInvalid = false;
      this.CountrySpace = false;
    }
  }
  oninvestor(e: any) {
    this.ContinueForm.get('Creditedinvestor').setValue(e.target.value);
    this.ContinueForm.value.Creditedinvestor = e.target.value;
    this.InvestorData = e.target.value;
    if (e.target.value == '1' || e.target.value == '2') {
      this.accreditedInvestor = true
      this.InvestorDatabool = false
    }
    else {
      this.accreditedInvestor = false
      this.InvestorDatabool = true
    }
  }
  onlookinvest(e: any) {
    this.ContinueForm.get('Invest').setValue(e.target.value);
    this.ContinueForm.value.Invest = e.target.value;
    this.lookInvest = +e.target.value;
    if (this.lookInvest == null || this.lookInvest == '' || this.lookInvest == 0) {
      this.lookInvestbool = true
    }
    else {
      this.lookInvestbool = false
    }
  }
  checkValue(e: any) {
    this.Investorcheck = e.target.value
    if (this.ContinueForm.value.Investorterms == true) {
      this.Investorcheckbool = false
    }
    else {
      this.Investorcheckbool = true
    }
  }
  onAboutus(){
    if(this.ContinueForm.value.Aboutus == null || this.ContinueForm.value.Aboutus == ''){
      this.Aboutusbool = true;
    }
    else{
      this.Aboutusbool = false;
    }
  }
  Register() {
    if (this.lookInvest == null || this.lookInvest == '' || this.lookInvest == 0
    || this.InvestorData == null || this.InvestorData == '' || this.InvestorData == 0
    || this.ContinueForm.value.Aboutus == null || this.ContinueForm.value.Aboutus == ''
    || this.ContinueForm.value.Investorterms == null || this.ContinueForm.value.Investorterms == '' || this.ContinueForm.value.Investorterms == false){
      if (this.lookInvest == null || this.lookInvest == '' || this.lookInvest == 0) {
        this.lookInvestbool = true
      }
      else {
        this.lookInvestbool = false
      }
      if (this.InvestorData == null || this.InvestorData == '' || this.InvestorData == 0) {
        this.InvestorDatabool = true;
      }
      else {
        this.InvestorDatabool = false;
      }
      if (this.ContinueForm.value.Investorterms == null || this.ContinueForm.value.Investorterms == '' || this.ContinueForm.value.Investorterms == false) {
        this.Investorcheckbool = true;
      }
      else {
        this.Investorcheckbool = false;
      }
      this.onAboutus();
    }
    else {
      this.Loader = true;
      this.registerdisabledBool = true;
      let reg = {
        Id: 0,
        RoleId: 1,
        FirstName: this.registerForm.value.Firstname,
        LastName: this.registerForm.value.Lastname,
        EmailId: this.registerForm.value.Email,
        PhoneNumber: this.registerForm.value.Phonenumber,
        Residency: +this.residencyvalue,
        Country: this.registerForm.value.Country,
        Password: this.registerForm.value.Password,
        ConfirmPassword: this.registerForm.value.Confirmpassword,
        Capacity: this.lookInvest,
        IsAccreditedInvestor: this.accreditedInvestor,
        HeardFrom: this.ContinueForm.value.Aboutus,
        IsTOCApproved: this.ContinueForm.value.Investorterms,
        CreatedBy: 'user',
        Status: 1,
        Active: true,
        ProfileImageUrl : 'https://t3.ftcdn.net/jpg/03/46/83/96/360_F_346839683_6nAPzbhpSkIpb8pmAwufkC7c5eD7wYws.jpg'
      }
      this.registerService.RegisterData(reg).subscribe(data => {
        if (data.statusCode == 1) {
          this.toastr.success(data.message, 'Success!')
          this.router.navigate(['./../login'], { relativeTo: this.route });
        }
        if (data.statusCode == 2) {
          this.toastr.error(data.message, 'Error!')
          this.registerdisabledBool = false;
        }
        if (data.statusCode == 3) {
          this.toastr.error(data.message, 'Error!')
          this.registerdisabledBool = false;
        }
        else {
          return
          this.registerdisabledBool = false;
        }
        this.Loader = false;
      })
    }
  }
  Login() {
    this.router.navigate(['./../login'], { relativeTo: this.route });
  }
  MustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors.MustMatch) {
        // return if another validator has already found an error on the matchingControl
        return;
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ MustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    }
  }

  onFirstname(e: any) {
    const validcompanynameRegEx = /^[a-zA-Z ]*$/;
    const validcompanynamespaceRegEx = /^(\s+\S+\s*)*(?!\s).*$/;
    if (e.target.value == '' || e.target.value == null) {
      this.FirstNameNull = true;
      this.FirstNameInvalid = false;
      this.FirstNameSpace = false;
    }
    else {
      this.FirstNameNull = false;
      if (validcompanynamespaceRegEx.test(e.target.value)) {
        this.FirstNameSpace = false;
        if (validcompanynameRegEx.test(e.target.value)) {
          this.FirstNameInvalid = false;
        } else {
          this.FirstNameInvalid = true;
          this.FirstNameNull = false;
          this.FirstNameSpace = false;
        }
      } else {
        this.FirstNameSpace = true;
        this.FirstNameInvalid = false;
        this.FirstNameNull = false;
      }

    }
  }
  onCountry(e: any) {
    const validcompanynameRegEx = /^[a-zA-Z ]*$/;
    const validcompanynamespaceRegEx = /^(\s+\S+\s*)*(?!\s).*$/;
    if (e.target.value == '' || e.target.value == null) {
      this.CountryNull = true;
      this.CountryInvalid = false;
      this.CountrySpace = false;
    }
    else {
      this.CountryNull = false;
      if (validcompanynamespaceRegEx.test(e.target.value)) {
        this.CountrySpace = false;
        if (validcompanynameRegEx.test(e.target.value)) {
          this.CountryInvalid = false;
        } else {
          this.CountryInvalid = true;
          this.CountryNull = false;
          this.CountrySpace = false;
        }
      } else {
        this.CountrySpace = true;
        this.CountryInvalid = false;
        this.CountryNull = false;
      }

    }
  }
  onLastname(e: any) {
    const validcompanynameRegEx = /^[a-zA-Z ]*$/;
    const validcompanynamespaceRegEx = /^(\s+\S+\s*)*(?!\s).*$/;
    if (e.target.value == '' || e.target.value == null) {
      this.LastNameNull = true;
      this.LastNameInvalid = false;
      this.LastNameSpace = false
    }
    else {
      this.LastNameNull = false;
      if (validcompanynamespaceRegEx.test(e.target.value)) {
        this.LastNameSpace = false;
        if (validcompanynameRegEx.test(e.target.value)) {
          this.LastNameInvalid = false;
        } else {
          this.LastNameInvalid = true;
          this.LastNameSpace = false;
        }
      } else {
        this.LastNameSpace = true;
        this.LastNameInvalid = false;
      }
    }
  }
  onEmail(e: any) {
    const validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (e.target.value == null || e.target.value == ' ' || e.target.value == '') {
      this.EmptyEmail = true;
      this.validEmail = false;
      this.CompanyUserDetailsbool = false;
    }
    else {
      this.EmptyEmail = false;
      if (validEmailRegEx.test(e.target.value)) {
        this.validEmail = false;
        this.CompanyUserDetailsbool = false;
        let x = this.UserData.filter((y: { emailId: any; }) => y.emailId == e.target.value)
        if (x.length > 0) {
          this.CompanyUserDetailsbool = true;
          this.EmptyEmail = false;
          this.validEmail = false;
        }
        else {
          this.CompanyUserDetailsbool = false;
        }
      } else {
        this.validEmail = true;
        this.CompanyUserDetailsbool = false;
      }
    }
  }
  onPassword(e: any) {
    const validPasswordRegEx = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/;
    const validateLowercase = /^(?=.*?[a-z])/;
    const validateUppercase = /^(?=.*[A-Z])/;
    const validateNumbercase = /^(?=.*[0-9])/;
    const validateSpecialcase = /^(?=.*[!@#$%^&*])/;
    const validatelengthcase = /^(?=.{8,})/;
    if (e.target.value == null || e.target.value == ' ' || e.target.value == '') {
      this.EmptyPassword = true;
      this.ValidPassword = false;
    }
    else {
      this.EmptyPassword = false;
      if (validateLowercase.test(e.target.value)) {
        this.ValidLowercase = false;
      }
      else {
        this.ValidLowercase = true;
      }
      if (validateUppercase.test(e.target.value)) {
        this.ValidUppercase = false;
      } else {
        this.ValidUppercase = true;
      }
      if (validateNumbercase.test(e.target.value)) {
        this.ValidNumbercase = false;
      } else {
        this.ValidNumbercase = true;
      }
      if (validateSpecialcase.test(e.target.value)) {
        this.ValidSpecialcase = false;
      } else {
        this.ValidSpecialcase = true;
      }
      if (validatelengthcase.test(e.target.value)) {
        this.ValidLengthcase = false;
      } else {
        this.ValidLengthcase = true;
      }
    }
  }

  onConfirmPassword(event: any) {
    if (event.target.value == '') {
      this.EmptyConfirmPassword = true;
      this.ValidConfirmPassword = false;
    }
    else {
      this.EmptyConfirmPassword = false;
      if (this.registerForm.value.Password == event.target.value) {
        this.ValidConfirmPassword = false;
      }
      else {
        this.ValidConfirmPassword = true;
      }
    }

  }
  onPhoneNumber(e: any) {
    if (e.target.value == '') {
      this.Phonenumberbool = true;
      this.Phonelength = false
    }
    else {
      this.Phonenumberbool = false;
      if (e.target.value.length == 10) {
        this.Phonelength = false
      }
      else {
        this.Phonelength = true
      }
    }
  }

  GetUser() {
    this.loginService.GetUser().subscribe(data => {
      this.UserData = data;
    })
  }
  numberValidation(event: any): Boolean {
    if (event.keyCode >= 48 && event.keyCode <= 57)
      return true;
    else
      return false;
  }
  Password() {
    this.showPassword = !this.showPassword;
  }

  ConfirmPassword() {
    this.showConfrimPassword = !this.showConfrimPassword;
  }
  logout() {
    this.router.navigate(['/login'], { relativeTo: this.route });
    localStorage.clear();
  }
  onPrivacyPolicy(){
    // window.open("http://localhost:4200/privacy-policy")
    window.open("https://credor-app.azurewebsites.net/privacy-policy")
  }
  onTermsandConditions(){
    // window.open("http://localhost:4200/terms-and-conditions")
    window.open("https://credor-app.azurewebsites.net/terms-and-conditions")
  }

}
