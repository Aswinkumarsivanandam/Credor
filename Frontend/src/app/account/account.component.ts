import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HeaderComponent } from '../header/header.component';
import { LoginService } from '../login/login.service';
import { AccountService } from './account.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  @ViewChild('ProfileImg')
  profile!: HeaderComponent;
  Selected: any;
  MyinfoShow: boolean = false;
  SharingShow: boolean = false;
  ActivetimelineShow: boolean = false;
  AccreditedInvestorId: any = '0';
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
  LookingInvestId: any = '0';
  AccountForm: any;
  UpdatePassword: boolean = false;
  CurrentPassword: any;
  NewPassword: any;
  ConfirmNewPassword: any;
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
  EmptyCurrentPassword: boolean = false;
  MismatchCurrentPassword: boolean = false;
  FirstnameError: boolean = false;
  LastnameError: boolean = false;
  PhoneError: boolean = false;
  EmailError: boolean = false;
  UserData: any;
  UserId: any;
  TwoFactor: boolean = false;
  AddNewEmailShow: boolean = false;
  InvestorData: any;
  accreditedInvestor: any = null;
  Loader: boolean = false;
  SecondaryError: boolean = false;
  imageFileToUpload: any = [];
  imagePath: any;
  imageUrl: any;
  allowedFileExtensions: any = [];
  filesToUpload: any = [];
  ProfileImage: any = [];
  TypeId: any;
  OtpPopup: boolean = false;
  OtpEmail: any
  EmailVerify: boolean = false;
  PhoneVerify: boolean = false;
  Getotp: any;
  PhoneOtpPopup: boolean = false;
  OtpPhone: any;
  UserDetails: any = [];
  EmailExists: boolean = false;
  PhoneExists: boolean = false;
  AddUserPopup: boolean = false;
  AddUserForm: any;
  PermissionData: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'View Only' },
    { id: 2, value: 'View and Edit' },
  ];
  PermissionId: any = 0;
  AddUserEmailError: boolean = false;
  AddUserEmailExists: boolean = false;
  UserFirstnameError: boolean = false;
  UserLastnameError: boolean = false;
  AccountAccesstoOther: any = [];
  EditUserId: any;
  ViewBool: boolean = false;
  DeleteUserPopup: boolean = false;
  UserDetailData: any;
  NotificationData: any = [];
  Notification: any = [];
  VerifyPhoneDisabled: boolean = false;
  VerifyEmailDisabled: boolean = false;

  constructor(private formbuilder: FormBuilder,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private loginService: LoginService,
    private accountService: AccountService) { }

  ngOnInit(): void {
    this.Loader = true;
    this.UserId = Number(localStorage.getItem('UserId'));
    this.allowedFileExtensions = ['jpg', 'jpeg', 'png', 'PNG']
    this.AccountForm = this.formbuilder.group({
      FirstName: [''],
      LastName: [''],
      NickName: [''],
      Phone: [''],
      Email: [''],
      Creditedinvestor: [''],
      Invest: [''],
      HowdidYouHear: [''],
      SecondaryEmail: [''],
      ReceiveEmail: [''],
      DOB: [''],
      TwoFactor: [''],
      NewsletterUpdate: [''],
      InvestmentAnnouncements: [''],
    })
    this.AddUserForm = this.formbuilder.group({
      FirstName: [''],
      LastName: [''],
      NickName: [''],
      Title: [''],
      Email: [''],
      Permissions: [''],
      Notification: [''],
    })
    this.selectMyinfo();
    this.GetProfile();
    this.GetAccountAccesstoOthers();
    this.GetNotification();
  }
  selectMyinfo() {
    this.Selected = 'Myinfo'
    this.MyinfoShow = true;
    this.SharingShow = false;
    this.ActivetimelineShow = false;
  }
  selectSharing() {
    this.Selected = 'Sharing'
    this.MyinfoShow = false;
    this.SharingShow = true;
    this.ActivetimelineShow = false;
  }
  selectActivetimeline() {
    this.Selected = 'Activetimeline'
    this.MyinfoShow = false;
    this.SharingShow = false;
    this.ActivetimelineShow = true;
    this.GetNotification();
  }
  uploadFile(event: any) {
    for (var i = 0; i < event.target.files.length; i++) {
      let ext: any;
      this.filesToUpload = [];
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == event.target.files[i].name.split('.').pop()) {
          this.filesToUpload.push(event.target.files[i]);
          if (this.filesToUpload.length > 0) {
            this.UserData.profileImageUrl = ''
          }
          ext = null;
          ext = event.target.files[i].name.split('.').pop();
          const file = event.target.files[i];
          const reader = new FileReader();
          reader.readAsDataURL(file);
          if (event.target.files[i].type == 'image/jpeg' || event.target.files[i].type == 'image/png' || event.target.files[i].type == 'image/PNG' || event.target.files[i].type == 'image/jpg' || event.target.files[i].type == 'image/JPEG') {
            reader.onload = () => {
              this.ProfileImage = [];
              this.ProfileImage.push({ profileImageUrl: reader.result, CoverFileType: 'jpeg' });
            };
          }
        }
      });
      if (ext == null) {
        this.toastr.error(event.target.files[i].name.split('.').pop() + ' files are not accepted.', 'Error');
      }
    }
  }
  oninvestor(e: any) {
    // this.AccountForm.get('Creditedinvestor').setValue(e.target.value);
    // this.AccountForm.value.Creditedinvestor = e.target.value;
    this.InvestorData = e.target.value;
    if (this.InvestorData == '1') {
      this.accreditedInvestor = true;
    }
    else if (this.InvestorData == '2') {
      this.accreditedInvestor = false;
    }
    else {
      this.accreditedInvestor = null
    }
  }
  onlookinvest(e: any) {
    // this.ContinueForm.get('Invest').setValue(e.target.value);
    // this.ContinueForm.value.Invest = e.target.value;
    // this.lookInvest = +e.target.value;
    // if (this.lookInvest == null || this.lookInvest == null || this.lookInvest == 0) {
    //   this.lookInvestbool = true
    // }
    // else {
    //   this.lookInvestbool = false
    // }
  }
  onChangePassword() {
    this.UpdatePassword = true;
    this.CurrentPassword = '';
    this.NewPassword = '';
    this.ConfirmNewPassword = '';
    this.showPassword = false;
    this.showConfrimPassword = false;
    this.EmptyCurrentPassword = false;
    this.EmptyPassword = false;
    this.ValidLowercase = false;
    this.ValidUppercase = false;
    this.ValidNumbercase = false;
    this.ValidSpecialcase = false;
    this.ValidLengthcase = false;
    this.MismatchCurrentPassword = false;
    this.EmptyConfirmPassword = false;
    this.ValidConfirmPassword = false;
  }
  onUpdatePassword() {
    this.Loader = true;
    if (this.CurrentPassword == '' || this.NewPassword == '' || this.ConfirmNewPassword == '' || this.ValidLowercase == true
      || this.ValidUppercase == true || this.ValidNumbercase == true || this.ValidSpecialcase == true || this.ValidLengthcase == true
      || this.MismatchCurrentPassword == true || this.ValidConfirmPassword == true || this.EmptyConfirmPassword == true) {
      this.onCurrentPassword();
      this.onPassword();
      this.onConfirmPassword();
      this.Loader = false;
    }
    else {
      let password = {
        UserId: this.UserId,
        OldPassword: this.CurrentPassword,
        NewPassword: this.NewPassword
      }
      this.accountService.UpdatePassword(password).subscribe(data => {
        if (data == 0) {
          this.UpdatePassword = false;
          this.GetProfile();
          this.toastr.success("Password updated successfully", "Success!");
        }
        else {
          this.Loader = false;
          this.UpdatePassword = false;
          this.toastr.error("Invalid password", "Error!");
        }
      })
    }
  }
  onPassword() {
    const validPasswordRegEx = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/;
    const validateLowercase = /^(?=.*?[a-z])/;
    const validateUppercase = /^(?=.*[A-Z])/;
    const validateNumbercase = /^(?=.*[0-9])/;
    const validateSpecialcase = /^(?=.*[!@#$%^&*])/;
    const validatelengthcase = /^(?=.{12,})/;
    if (this.NewPassword == null || this.NewPassword == ' ' || this.NewPassword == '') {
      this.EmptyPassword = true;
      this.MismatchCurrentPassword = false;
      this.ValidLowercase = false;
      this.ValidUppercase = false;
      this.ValidNumbercase = false;
      this.ValidSpecialcase = false;
      this.ValidLengthcase = false;
    }
    else if (this.NewPassword == this.CurrentPassword) {
      this.MismatchCurrentPassword = true;
      this.EmptyPassword = false;
      this.ValidLowercase = false;
      this.ValidUppercase = false;
      this.ValidNumbercase = false;
      this.ValidSpecialcase = false;
      this.ValidLengthcase = false;
    }
    else {
      this.EmptyPassword = false;
      this.MismatchCurrentPassword = false;
      if (validateLowercase.test(this.NewPassword)) {
        this.ValidLowercase = false;
      }
      else {
        this.ValidLowercase = true;
      }
      if (validateUppercase.test(this.NewPassword)) {
        this.ValidUppercase = false;
      } else {
        this.ValidUppercase = true;
      }
      if (validateNumbercase.test(this.NewPassword)) {
        this.ValidNumbercase = false;
      } else {
        this.ValidNumbercase = true;
      }
      if (validateSpecialcase.test(this.NewPassword)) {
        this.ValidSpecialcase = false;
      } else {
        this.ValidSpecialcase = true;
      }
      if (validatelengthcase.test(this.NewPassword)) {
        this.ValidLengthcase = false;
      } else {
        this.ValidLengthcase = true;
      }
    }
  }
  Password() {
    this.showPassword = !this.showPassword;
  }

  ConfirmPassword() {
    this.showConfrimPassword = !this.showConfrimPassword;
  }
  onConfirmPassword() {
    if (this.ConfirmNewPassword == '') {
      this.EmptyConfirmPassword = true;
    }
    else {
      this.EmptyConfirmPassword = false;
      if (this.NewPassword == this.ConfirmNewPassword) {
        this.ValidConfirmPassword = false;
      }
      else {
        this.ValidConfirmPassword = true;
      }
    }
  }
  onTwoFactor(e: any) {
    if (this.AccountForm.value.TwoFactor == true) {
      this.TwoFactor = true;
    }
    else {
      this.TwoFactor = false;
    }
  }
  onReceiveEmail() {
    if (this.AccountForm.value.ReceiveEmail == true) {
      if (this.AccountForm.value.SecondaryEmail == '' || this.AccountForm.value.SecondaryEmail == null) {
        this.SecondaryError = true;
      }
      else {
        this.SecondaryError = false;
      }
    }
    else {
      this.SecondaryError = false;
    }
  }
  onCurrentPassword() {
    if (this.CurrentPassword == '' || this.CurrentPassword == null) {
      this.EmptyCurrentPassword = true;
    }
    else {
      this.EmptyCurrentPassword = false;
    }
  }
  onFirstName() {
    if (this.AccountForm.value.FirstName == '' || this.AccountForm.value.FirstName == null) {
      this.FirstnameError = true;
    }
    else {
      this.FirstnameError = false;
    }
  }
  onLastName() {
    if (this.AccountForm.value.LastName == '' || this.AccountForm.value.LastName == null) {
      this.LastnameError = true;
    }
    else {
      this.LastnameError = false;
    }
  }
  onPhone() {
    let x = this.UserDetails.filter((x: { phoneNumber: any; }) => x.phoneNumber == this.AccountForm.value.Phone)
    if (this.AccountForm.value.Phone == '' || this.AccountForm.value.Phone == null) {
      this.PhoneError = true;
      this.PhoneVerify = false;
      this.PhoneExists = false;
    }
    else if (this.AccountForm.value.Phone == this.UserData.phoneNumber) {
      this.PhoneVerify = true;
      this.PhoneError = false;
      this.PhoneExists = false;
    }
    else if (x.length > 0) {
      this.PhoneExists = true;
      this.PhoneError = false;
      this.PhoneVerify = false;
    }
    else {
      this.PhoneError = false;
      this.PhoneVerify = false;
      this.PhoneExists = false;
    }
  }
  onEmail() {
    let x = this.UserDetails.filter((x: { emailId: any; }) => x.emailId == this.AccountForm.value.Email)
    if (this.AccountForm.value.Email == '' || this.AccountForm.value.Email == null) {
      this.EmailError = true;
      this.EmailVerify = false;
      this.EmailExists = false;
    }
    else if (this.AccountForm.value.Email == this.UserData.emailId) {
      this.EmailError = false;
      this.EmailVerify = true;
      this.EmailExists = false;
    }
    else if (x.length > 0) {
      this.EmailError = false;
      this.EmailVerify = false;
      this.EmailExists = true;
    }
    else {
      this.EmailError = false;
      this.EmailVerify = false;
      this.EmailExists = false;
    }
  }
  onUserFirstName() {
    if (this.AddUserForm.value.FirstName == '' || this.AddUserForm.value.FirstName == null) {
      this.UserFirstnameError = true;
    }
    else {
      this.UserFirstnameError = false;
    }
  }
  onUserLastName() {
    if (this.AddUserForm.value.LastName == '' || this.AddUserForm.value.LastName == null) {
      this.UserLastnameError = true;
    }
    else {
      this.UserLastnameError = false;
    }
  }
  onAddEmail() {
    let x = this.UserDetails.filter((x: { emailId: any; }) => x.emailId == this.AddUserForm.value.Email)
    if (this.AddUserForm.value.Email == '' || this.AddUserForm.value.Email == null) {
      this.AddUserEmailError = true;
      this.AddUserEmailExists = false;
    }
    else if (this.AddUserForm.value.Email != this.UserDetailData?.emailId) {
      if (x.length > 0) {
        this.AddUserEmailError = false;
        this.AddUserEmailExists = true;
      }
      else {
        this.AddUserEmailError = false;
        this.AddUserEmailExists = false;
      }
    }
    else {
      this.AddUserEmailError = false;
      this.AddUserEmailExists = false;
    }
  }
  SaveProfile() {
    this.Loader = true;
    if (this.AccountForm.value.FirstName == '' || this.AccountForm.value.FirstName == null
      || this.AccountForm.value.LastName == '' || this.AccountForm.value.LastName == null || this.EmailExists == true
      || this.AccountForm.value.Phone == '' || this.AccountForm.value.Phone == null
      || this.AccountForm.value.Email == '' || this.AccountForm.value.Email == null || this.PhoneExists == true) {
      this.onFirstName();
      this.onLastName();
      this.onPhone();
      this.onEmail();
      this.Loader = false;
    }
    else {
      if (this.SecondaryError == true) {
        this.Loader = false;
      }
      else {

        const ProfileImg = new FormData();
        ProfileImg.append("Id", this.UserData.id);
        ProfileImg.append("FirstName", this.AccountForm.value.FirstName);
        ProfileImg.append("LastName", this.AccountForm.value.LastName);
        ProfileImg.append("NickName", this.AccountForm.value.NickName);
        ProfileImg.append("EmailId", this.AccountForm.value.Email);
        ProfileImg.append("SecondaryEmail", this.AccountForm.value.SecondaryEmail);
        if (this.AccountForm.value.ReceiveEmail == null || this.AccountForm.value.ReceiveEmail == '') {
          ProfileImg.append("ReceiveEmailNotifications", 'false');
        }
        else {
          ProfileImg.append("ReceiveEmailNotifications", this.AccountForm.value.ReceiveEmail);
        }
        ProfileImg.append("PhoneNumber", this.AccountForm.value.Phone);
        ProfileImg.append("IsEmailVerified", this.UserData.isEmailVerified);
        ProfileImg.append("IsPhoneVerified", this.UserData.isPhoneVerified);
        ProfileImg.append("DateOfBirth", this.AccountForm.value.DOB);
        ProfileImg.append("Capacity", this.LookingInvestId);
        ProfileImg.append("IsAccreditedInvestor", this.accreditedInvestor);
        ProfileImg.append("HeardFrom", this.AccountForm.value.HowdidYouHear);
        ProfileImg.append("IsTwoFactorAuthEnabled", this.AccountForm.value.TwoFactor);
        if (this.AccountForm.value.NewsletterUpdate == null || this.AccountForm.value.NewsletterUpdate == '') {
          ProfileImg.append("CompanyNewsLetterUpdates", 'false');
        }
        else {
          ProfileImg.append("CompanyNewsLetterUpdates", this.AccountForm.value.NewsletterUpdate);

        }
        if (this.AccountForm.value.InvestmentAnnouncements == null || this.AccountForm.value.InvestmentAnnouncements == '') {
          ProfileImg.append("NewInvestmentAnnouncements", 'false');
        }
        else {
          ProfileImg.append("NewInvestmentAnnouncements", this.AccountForm.value.InvestmentAnnouncements);
        }
        ProfileImg.append("ProfileImageUrl", this.UserData.profileImageUrl);
        this.filesToUpload.forEach((item: string | Blob) => {
          ProfileImg.append('profileImage', item);
        });
        this.accountService.UpdateProfile(ProfileImg).subscribe(data => {
          if (data == 1) {
            this.GetProfile();
            this.toastr.success("Profile updated Successfully", "Success!")
          }
          else{
            this.Loader = false;
          }
        })
      }
    }
  }
  GetProfile() {
    this.accountService.GetProfile(this.UserId).subscribe(data => {
      this.UserData = data;
      console.log(this.UserData, 'userdata')
      localStorage.removeItem("ProfileImg");
      localStorage.setItem('ProfileImg',this.UserData.profileImageUrl)
      this.profile.ProfileImg = this.UserData.profileImageUrl;
      this.AccountForm.patchValue({
        FirstName: this.UserData.firstName,
        LastName: this.UserData.lastName,
        NickName: this.UserData.nickName,
        Phone: this.UserData.phoneNumber,
        Email: this.UserData.emailId,
        HowdidYouHear: this.UserData.heardFrom,
        // SecondaryEmail: this.UserData.secondaryEmail,
        DOB: this.UserData.dateOfBirth,
        TwoFactor: this.UserData.isTwoFactorAuthEnabled,
        ReceiveEmail: this.UserData.receiveEmailNotifications,
        NewsletterUpdate: this.UserData.companyNewsLetterUpdates,
        InvestmentAnnouncements: this.UserData.newInvestmentAnnouncements,
      })
      if (this.UserData.isAccreditedInvestor == true) {
        this.AccreditedInvestorId = 1;
        this.accreditedInvestor = true;
      }
      else if (this.UserData.isAccreditedInvestor == false) {
        this.AccreditedInvestorId = 2;
        this.accreditedInvestor = false;
      }
      else {
        this.AccreditedInvestorId = 0;
        this.accreditedInvestor = null;
      }
      this.LookingInvestId = this.UserData.capacity;
      if (this.UserData.secondaryEmail == null || this.UserData.secondaryEmail == '') {
        this.AddNewEmailShow = false;
      }
      else {
        this.AddNewEmailShow = true;
      }
      this.ProfileImage.push({ profileImageUrl: this.UserData.profileImageUrl });
      if (this.UserData.isEmailVerified == true) {
        this.EmailVerify = true;
      }
      else {
        this.EmailVerify = false
      }
      if (this.UserData.isPhoneVerified == true) {
        this.PhoneVerify = true;
      }
      else {
        this.PhoneVerify = false;
      }
      if (this.UserData.secondaryEmail != null) {
        this.AddNewEmailShow = true;
        this.AccountForm.value.SecondaryEmail = this.UserData.secondaryEmail
      }
      else {
        this.AddNewEmailShow = false;
        this.UserData.secondaryEmail = '';
      }
      this.GetUser();
      this.Loader = false;
    })
  }
  onSecEmail(e: any) {
    const validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (e.target.value != '') {
      if (validEmailRegEx.test(e.target.value)) {
        this.SecondaryError = false;
      } else {
        this.SecondaryError = true;
      }
    }
    if (this.AccountForm.value.ReceiveEmail == true) {
      if (this.AccountForm.value.SecondaryEmail == '' || this.AccountForm.value.SecondaryEmail == null) {
        this.SecondaryError = true;
      }
      else {
        this.SecondaryError = false;
      }
    }
  }
  onCancel() {
    this.Loader = true;
    this.GetProfile();
  }
  onProfileSave() {
    this.Loader = true;
    this.TypeId = 7;
    const ProfileImg = new FormData();
    ProfileImg.append("UserId", this.UserId);
    ProfileImg.append("Type", this.TypeId);
    this.filesToUpload.forEach((item: string | Blob) => {
      ProfileImg.append('Files', item);
    });
    this.accountService.UpdateProfileImage(ProfileImg).subscribe(data => {
      if (data == true) {
        this.GetProfile();
        this.toastr.success("Profile updated successfully", "Success!")
      }
    })
  }
  UpdateAddress() {
    this.router.navigate(['./../investor-profile'], { relativeTo: this.route });
  }
  onVerifyEmail(e: any) {
    this.Loader = true;
    if (e == "Phone") {
      this.Getotp = {
        Id: this.UserId,
        PhoneNumber: this.AccountForm.value.Phone
      }
    }
    else if (e == "Email") {
      this.Getotp = {
        Id: this.UserId,
        EmailId: this.AccountForm.value.Email
      }
    }
    this.accountService.SendOtp(this.Getotp).subscribe(data => {
      console.log(data, 'otp')
      if (data == true) {
        if (e == "Email") {
          this.OtpPopup = true;
          this.toastr.success("OTP send your respective emailid", 'Success!')
        }
        else if (e == "Phone") {
          this.PhoneOtpPopup = true;
          this.toastr.success("OTP send your respective phone number", 'Success!')
        }
        this.Loader = false
      }
      else {
        this.Loader = false;
        if (e == "Phone") {
          this.toastr.info("Otp can't send for this phone number", "Info!")
        }
        if (e == "Email") {
          this.toastr.info("Otp can't send for this email", "Info!")
        }
      }
    })
  }
  ResetOtpEmail(e: any) {
    this.Loader = true;
    if (e == "Phone") {
      this.Getotp = {
        Id: this.UserId,
        PhoneNumber: this.AccountForm.value.Phone
      }
    }
    else if (e == "Email") {
      this.Getotp = {
        Id: this.UserId,
        EmailId: this.AccountForm.value.Email
      }
    }
    this.accountService.ResendOtp(this.Getotp).subscribe(data => {
      console.log(data, 'otp')
      if (data == true) {
        if (e == "Email") {
          this.OtpPopup = true;
        }
        else if (e == "Phone") {
          this.PhoneOtpPopup = true;
        }
        this.toastr.success("OTP resent successfully", "Success!")
        this.Loader = false;
      }
      else {
        this.Loader = false;
        this.toastr.info("Can't resent otp", "Info!")
      }
    })
  }
  ResetOtpPhone() {

  }
  VerifyOtpPhone() {
    this.Loader = true;
    if(this.OtpPhone == null || this.OtpPhone == ''){
      this.toastr.info("Please enter the OTP","Info!");
      this.VerifyPhoneDisabled = true;
      this.Loader = false;
    }
    else if(this.OtpPhone.length != 6){
      this.toastr.info("Enter a valid OTP","Info!");
      this.VerifyPhoneDisabled = true;
      this.Loader = false;
    }
    else{
      let otpPhoneVerify = {
        Id: this.UserId,
        OneTimePassword: this.OtpPhone,
        PhoneNumber: this.AccountForm.value.Phone
      }
      this.accountService.VerifyPhone(otpPhoneVerify).subscribe(data => {
        if (data == true) {
          this.PhoneOtpPopup = false;
          this.GetProfile();
          this.toastr.success("Phone number verified successfully", "Success!")
        }
        else{
          this.Loader = false;
          this.PhoneOtpPopup = false;
          this.toastr.error("Invalid OTP","Error!")
        }
      })
    }
  }
  VerifyOtpEmail() {
    this.Loader = true;
    if(this.OtpEmail == null || this.OtpEmail == ''){
      this.toastr.info("Please enter the OTP","Info!");
      this.VerifyEmailDisabled = true;
      this.Loader = false;
    }
    else if(this.OtpEmail.length != 6){
      this.toastr.info("Enter a valid OTP","Info!");
      this.VerifyEmailDisabled = true;
      this.Loader = false;
    }
    else{
      let otpVerify = {
        Id: this.UserId,
        OneTimePassword: this.OtpEmail,
        EmailId: this.AccountForm.value.Email
      }
      this.accountService.VerifyEmail(otpVerify).subscribe(data => {
        if (data == true) {
          this.OtpPopup = false;
          this.GetProfile();
          this.toastr.success("Email verified successfully", "Success!")
        }
        else{
          this.Loader = false;
          this.OtpPopup = false;
          this.toastr.error("Invalid OTP","Error!")
        }
      })
    }
  }
  onOtpChange(e: any) {
    this.OtpEmail = e;
    if(this.OtpEmail == '' || this.OtpEmail == null){
      this.VerifyEmailDisabled = true;
    }
    else if(this.OtpEmail.length == 6){
      this.VerifyEmailDisabled = false;
    }
    else{
      this.VerifyEmailDisabled = false;
    }
  }
  onOtpPhoneChange(e: any) {
    this.OtpPhone = e;
    if(this.OtpPhone == '' || this.OtpPhone == null){
      this.VerifyPhoneDisabled = true;
    }
    else if(this.OtpPhone.length == 6){
      this.VerifyPhoneDisabled = false;
    }
    else{
      this.VerifyPhoneDisabled = false;
    }
  }
  GetUser() {
    this.loginService.GetUser().subscribe(data => {
      this.UserDetails = data;
      console.log(this.UserDetails, 'userdetails')
    })
  }
  PermissionChange(e: any) {
    this.PermissionId = e.target.value
  }
  onAddUser() {
    this.EditUserId = 0;
    this.PermissionId = 0
    this.AddUserPopup = true;
    this.UserFirstnameError = false;
    this.UserLastnameError = false;
    this.AddUserEmailError = false;
    this.AddUserEmailExists = false
    this.AddUserForm.reset();
    this.AddUserForm.enable();
    this.UserDetailData = '';
  }
  onSendInvite() {
    this.Loader = true;
    if (this.AddUserForm.value.FirstName == '' || this.AddUserForm.value.FirstName == null
      || this.AddUserForm.value.LastName == '' || this.AddUserForm.value.LastName == null
      || this.AddUserForm.value.Email == '' || this.AddUserForm.value.Email == null
      || this.AddUserEmailExists == true) {
      this.onUserFirstName();
      this.onUserLastName();
      this.onAddEmail();
      this.Loader = false;
    }
    else {
      let newuser = {
        Id: this.EditUserId != 0 ? this.EditUserId : 0,
        CurrentUserId: this.UserId,
        FirstName: this.AddUserForm.value.FirstName,
        LastName: this.AddUserForm.value.LastName,
        NickName: this.AddUserForm.value.NickName,
        Title: this.AddUserForm.value.Title,
        EmailId: this.AddUserForm.value.Email,
        Permission: +this.PermissionId,
        IsNotificationEnabled: this.AddUserForm.value.Notification
      }
      if (this.EditUserId == 0) {
        this.accountService.AddNewUser(newuser).subscribe(data => {
          if (data == true) {
            this.AddUserPopup = false;
            this.GetAccountAccesstoOthers()
            this.toastr.success("User added successfully", "Success!")
            this.Loader = false;
          }
        })
      }
      else if (this.EditUserId != 0) {
        this.accountService.UpdateNewUser(newuser).subscribe(data => {
          if (data == true) {
            this.AddUserPopup = false;
            this.GetAccountAccesstoOthers()
            this.toastr.success("User saved successfully", "Success!")
            this.Loader = false;
          }
        })
      }
    }
  }
  GetAccountAccesstoOthers() {
    this.accountService.GetAccountAccesstoOthers(this.UserId).subscribe(data => {
      this.AccountAccesstoOther = data;
      console.log(this.AccountAccesstoOther, 'AccountAccesstoOther')
    })
  }
  onEditUser(val: any, e: any) {
    this.UserDetailData = '';
    this.UserDetailData = val;
    if (e == "View") {
      this.AddUserForm.disable();
      this.ViewBool = true;
    }
    else if (e == "Edit") {
      this.AddUserForm.enable();
      this.ViewBool = false;
    }
    this.EditUserId = val.userId;
    this.AddUserPopup = true;
    this.AddUserForm.patchValue({
      FirstName: val.firstName,
      LastName: val.lastName,
      NickName: val.nickName,
      Title: val.title,
      Email: val.emailId,
      Notification: val.isNotificationEnabled
    })
    this.PermissionId = val.permission
  }

  onDeleteUser(val: any) {
    this.UserDetailData = '';
    this.DeleteUserPopup = true;
    this.UserDetailData = val;
  }
  onDeleteUserConfirmation() {
    this.Loader = true;
    this.accountService.DeleteNewUser(this.UserId, this.UserDetailData.userId).subscribe(data => {
      console.log(data, 'delete')
      if (data == true) {
        this.Loader = false;
        this.GetAccountAccesstoOthers();
        this.DeleteUserPopup = false;
        this.toastr.success("User deleted successfully", "Success!")
      }
      else {
        this.Loader = false;
        this.GetAccountAccesstoOthers();
        this.DeleteUserPopup = false;
        this.toastr.error("User can't be delete", "Error!")
      }
    })
  }
  GetNotification() {
    this.accountService.GetNotification(this.UserId).subscribe(data => {
      this.NotificationData = data;
      this.Notification = [];
      console.log(this.NotificationData, 'NotificationData')
      for (let i = 0; i < 5; i++) {
        this.Notification.push(this.NotificationData[i])
      }
    })
  }
  onLoadMore() {
    let length = this.Notification.length + 5
    for (let i = this.Notification.length; i < length; i++) {
      if (this.NotificationData[i] != null) {
        this.Notification.push(this.NotificationData[i])
      }
    }
    console.log(this.Notification, 'notification')
  }

}
