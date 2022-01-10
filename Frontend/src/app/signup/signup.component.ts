import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { LoginService } from '../login/login.service';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  UserId: any;
  Loader : boolean = false;
  SignUpForm : any
  EmptyPassword: boolean = false;
  ValidPassword: boolean = false;
  ValidLowercase: boolean = false;
  ValidUppercase: boolean = false;
  ValidNumbercase: boolean = false;
  ValidSpecialcase: boolean = false;
  ValidLengthcase: boolean = false;
  EmptyConfirmPassword: boolean = false;
  ValidConfirmPassword: boolean = false;
  token: any;
  NewUserId: any;
  NewUserDetails: any;

  constructor(private route : ActivatedRoute,
    private loginService : LoginService,
    private formBuilder : FormBuilder) { }

  ngOnInit(): void {
    this.UserId = localStorage.getItem("UserId");
    this.NewUserId = +this.route.snapshot.params['id:token'];
    this.token = this.route.snapshot.queryParams.token;
    console.log(this.NewUserId,this.token,'token')
    this.SignUpForm = this.formBuilder.group({
      Firstname : [''],
      Lastname : [''],
      Email : [''],
      CreatePassword : [''],
      ConfirmPassword : [''],
    })
    this.GetUserId();
  }
  GetUserId(){
    this.loginService.GetUserAccount(this.NewUserId).subscribe(data =>{
      this.NewUserDetails = data;
      console.log(data,'data')
      this.SignUpForm.patchValue({
        Firstname : this.NewUserDetails.firstName,
        Lastname : this.NewUserDetails.lastName,
        Email : this.NewUserDetails.emailId,
      })
    })
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
    }
    else {
      this.EmptyConfirmPassword = false;
      if (this.SignUpForm.value.Password == event.target.value) {
        this.ValidConfirmPassword = false;
      }
      else {
        this.ValidConfirmPassword = true;
      }
    }

  }
  Login(){

  }

}
