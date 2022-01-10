import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: any;
  submitted = false;
  logindisabledBool: boolean =  false;
  LoginValue: any;
  Resetlogin: boolean = false;
  routeUrl: string | undefined;
  profileDropdown: boolean = false;
  UserId: any;
  Loader : boolean = false;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private loginService: LoginService,
    private toastr : ToastrService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.UserId =  localStorage.getItem('UserId');
    this.routeUrl = this.route.snapshot.routeConfig?.path;
    if (this.routeUrl == 'resetlogin') {
      this.Resetlogin = true
    }

    this.loginForm = this.formBuilder.group({
      Email: ['', Validators.required],
      Password: ['', Validators.required]
    })
  }
  get f() { return this.loginForm.controls }

  onSubmit(logindata: any) {
    if (this.loginForm.invalid) {
      this.submitted = true;
      this.logindisabledBool = false;
    }
    else {
      this.Loader = true;
      this.logindisabledBool = true;
      this.submitted = false;
      let loginData = {
        EmailId: this.loginForm.value.Email,
        Password: this.loginForm.value.Password
      }
      this.loginService.LoginData(loginData).subscribe(data => {
        this.LoginValue = data;
        if (data != null) {
          this.Loader = false;
          this.logindisabledBool = false;
          if (data.statusCode == 0) {
            this.toastr.info('Invalid credentials','Info!')
            this.submitted = false;
            this.loginForm.reset();
          }
          else {
            localStorage.setItem("UserId", this.LoginValue.data.id)
            localStorage.setItem("RoleId", this.LoginValue.data.roleId)
            localStorage.setItem("Token", this.LoginValue.accessToken)
            localStorage.setItem("ProfileImg", this.LoginValue.data.profileImageUrl)
            this.toastr.success('Login Successfully','Success!');
            if(this.LoginValue.data.roleId == 1){
              this.router.navigate(['./../invest'], { relativeTo: this.route });
            }
            else if(this.LoginValue.data.roleId == 3){
              this.router.navigate(['./../lead'], { relativeTo: this.route });
            }
            else{
              this.router.navigate(['./../invest'], { relativeTo: this.route });
            }
            this.Loader = false;
          }
        }
      })
    }
  }

  goToForgetPassword() {
    this.router.navigate(['./../forgot-password'], { relativeTo: this.route });
  }

  register() {
    this.router.navigate(['./../register'], { relativeTo: this.route });
  }

  Onexplore(){
    this.router.navigate(['./../invest'], { relativeTo: this.route });
  }
  logout() {
    this.router.navigate(['/login'], { relativeTo: this.route });
    localStorage.clear();
  }

}
