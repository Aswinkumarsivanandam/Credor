import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent implements OnInit {
  Forgotdata: any;
  result: any;
  UserData: any = [];
  userExists: boolean = false;
  userBool: boolean = false;
  forgotdisabledBool: boolean = false;
  res: any;
  Loader : boolean = false;
  UserId : any;
  constructor(private router: Router,
    private loginService: LoginService,
    private toastr : ToastrService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.GetUser();
  }

  GetResetLink() {
    if (this.Forgotdata == null || this.Forgotdata == '' || this.userExists == true || this.userBool == true) {
      this.userBool = true;
      this.forgotdisabledBool = false;
      if (this.userExists == true) {
        this.userExists = true;
        this.userBool = false
      }
      else if (this.userBool == true) {
        this.userExists = false;
        this.userBool = true
      }
    }
    else {
      this.Loader = true;
      this.res = new Date().getTime();
      var encodedDatetime = btoa(this.res.toString());
      this.forgotdisabledBool = true;
      this.userBool = false;
      let forgot = {
        EmailId: this.Forgotdata,
        EncodedDate: encodedDatetime
      }
      this.loginService.ForgotPassword(forgot).subscribe(data => {
        this.result = data;
        if (this.result != null) {
          if (this.result.data == 1) {
            this.toastr.error("Send Email Failure",'Error!')
            this.Loader = false;
          }
          else if (this.result.data == 2) {
            this.toastr.error("Account Not Exists",'Error!')
            this.Loader = false;
          }
          else {
            var maskid = "";
            var maskidpost = "";
            var myemailId = this.result.data;
            var prefix = myemailId.substring(0, myemailId.lastIndexOf("@"));
            var postfix = myemailId.substring(myemailId.lastIndexOf("@"));
            var postfix1 = postfix.substring(0, postfix.lastIndexOf("."));
            var postfix2 = postfix.substring(postfix.lastIndexOf("."));

            for (var i = 0; i < prefix.length; i++) {
              if (i == 0 || i == prefix.length - 1) {
                maskid = maskid + prefix[i].toString();
              }
              else {
                maskid = maskid + "*";
              }
            }
            for (var i = 0; i < postfix1.length; i++) {
              if (i == 0 || i == 1 || i == postfix1.length - 1) {
                maskidpost = maskidpost + postfix1[i].toString();
              }
              else {
                maskidpost = maskidpost + "*";
              }
            }
            this.Loader = false;
            var mail = maskid + maskidpost + postfix2;
            this.toastr.success("The reset password link has been sent to your registered email ID " + mail,'Success!');
            this.router.navigate(['./../login'], { relativeTo: this.route });

          }
        }
        else {
          this.toastr.error("Invalid credentials",'Error!');
          this.Loader = false;
        }

      })
    }
  }

  goToLogin() {
    this.router.navigate(['./../login'], { relativeTo: this.route });
  }
  GetUser() {
    this.loginService.GetUser().subscribe(data => {
      this.UserData = data;
    })
  }
  onForgot(e: any) {
    if (this.Forgotdata == null || this.Forgotdata == '') {
      this.userBool = true;
      this.userExists = false;
    } else {
      this.userBool = false;
      let x = this.UserData.filter((y: { emailId: any; }) => y.emailId == e.target.value)
      if (x.length > 0) {
        this.userExists = false;
      }
      else {
        this.userExists = true;
      }
    }
  }

}
