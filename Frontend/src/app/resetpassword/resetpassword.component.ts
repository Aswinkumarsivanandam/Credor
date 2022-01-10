import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetpasswordComponent implements OnInit {
  resetForm: any;
  submitted = false;
  showPassword: boolean = false;
  showConfrimPassword: boolean = false;
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
  resetDisabled: boolean = false;
  Expiry: any;
  res: any;
  decodedDatetime: any;
  totalhrs: any;
  Loader : boolean = false;
  UserId : any

  constructor(private formbuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private loginService: LoginService) { }

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParams.token;
    var x = this.route.snapshot.queryParams.expiry;
    this.Expiry = x.split('/', 1);
    this.res = new Date().getTime();
    this.decodedDatetime = +atob(this.Expiry[0].toString());
    var hrs = this.res - this.decodedDatetime
    var days = Math.floor(hrs / (60 * 60 * 24 * 1000));
    var hours = +Math.floor(hrs / (60 * 60 * 1000)) - (days * 24);
    var minutes = +Math.floor(hrs / (60 * 1000)) - ((days * 24 * 60) + (hours * 60));
    this.totalhrs = hours + '.' + minutes
    if (this.totalhrs > 1) {
      alert('resetpassword link is expired')
      this.router.navigate(['./../login'], { relativeTo: this.route });
    }
    this.resetForm = this.formbuilder.group({
      Password: ['', Validators.required],
      Confirmpassword: ['', Validators.required]
    })
  }

  ResetPassword() {
    if (this.resetForm.invalid || this.EmptyPassword || this.ValidPassword || this.ValidLowercase || this.ValidUppercase
      || this.ValidNumbercase || this.ValidSpecialcase || this.ValidLengthcase || this.EmptyConfirmPassword
      || this.ValidConfirmPassword) {
      this.submitted = true;
      this.resetDisabled = false;
    }
    else {
      this.Loader = true;
      this.resetDisabled = true;
      let forgot = {
        Token: this.token,
        Password: this.resetForm.value.Password
      }
      this.loginService.ResetPassword(forgot).subscribe(data => {
        this.resetDisabled = false;
        if (data == true) {
          this.Loader = false;
          this.toastr.success('Password reset successfully', 'Success!');
          this.router.navigate(['./../resetlogin'], { relativeTo: this.route });
        }
        else {
          this.Loader = false;
          this.toastr.error('Password reset failed', 'Error!')
        }
      })
    }
  }
  get f() { return this.resetForm.controls }

  Password() {
    this.showPassword = !this.showPassword;
  }

  ConfirmPassword() {
    this.showConfrimPassword = !this.showConfrimPassword;
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
      if (this.resetForm.value.Password == event.target.value) {
        this.ValidConfirmPassword = false;
      }
      else {
        this.ValidConfirmPassword = true;
      }
    }

  }

}
