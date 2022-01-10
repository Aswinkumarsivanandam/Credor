import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { HttpClientModule } from 'ngx-http-client';
import { InvestComponent } from './invest/invest.component';
import { InvestmentComponent } from './investment/investment.component';
import { HeaderComponent } from './header/header.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InvestorProfileComponent } from './investor-profile/investor-profile.component';
import { MyinvestmentComponent } from './myinvestment/myinvestment.component';
import { UpdatesComponent } from './updates/updates.component';
import { DistributionsComponent } from './distributions/distributions.component';
import { SideBarComponent } from './side-bar/side-bar.component';
import { DocumentsComponent } from './documents/documents.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SwiperModule } from "swiper/angular";
import SwiperCore, { Pagination, Navigation } from "swiper/core";
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { AccountComponent } from './account/account.component';
import { DragDirective } from './documents/file.directive';
import { NgOtpInputModule } from 'ng-otp-input';
import { SignupComponent } from './signup/signup.component';
import { OtpVerificationComponent } from './otp-verification/otp-verification.component';
import { LeadComponent } from './lead/lead.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { InvestorComponent } from './investor/investor.component';
import { PortfolioComponent } from './portfolio/portfolio.component';
import { EmailComponent } from './email/email.component';
import { ReportsComponent } from './reports/reports.component';
import { PaymentComponent } from './payment/payment.component';
import { ServicesComponent } from './services/services.component';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { AgmCoreModule } from '@agm/core';
import { TermsandconditionsComponent } from './termsandconditions/termsandconditions.component';
import { PrivacypolicyComponent } from './privacypolicy/privacypolicy.component';
import { PortfolioDetailsComponent } from './portfolio-details/portfolio-details.component';


SwiperCore.use([Pagination, Navigation]);
const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'resetlogin', component: LoginComponent },
  { path: 'forgot-password', component: ForgotpasswordComponent },
  { path: 'reset-password', component: ResetpasswordComponent },
  { path: 'reset-password/:token', component: ResetpasswordComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'invest', component: InvestComponent },
  { path: 'investment', component: InvestmentComponent },
  { path: 'investment/:id', component: InvestmentComponent },
  { path: 'investment/:id/:offId/:name', component: InvestmentComponent },
  {path : 'investor-profile',component : InvestorProfileComponent},
  {path : 'myinvestment', component : MyinvestmentComponent},
  {path : 'update',component : UpdatesComponent},
  {path : 'distribution',component : DistributionsComponent},
  {path : 'document',component : DocumentsComponent},
  {path : 'account',component : AccountComponent},
  {path : 'signup/:id:token',component : SignupComponent},
  {path : 'otp-verification', component : OtpVerificationComponent},
  {path : 'admin-dashboard',component : AdminDashboardComponent},
  {path : 'lead', component : LeadComponent},
  {path : 'investor',component : InvestorComponent},
  {path : 'portfolio',component : PortfolioComponent},
  {path : 'portfolio/:name/:id', component: PortfolioComponent },
  {path : 'portfolio-details/:name/:id', component: PortfolioDetailsComponent },
  {path : 'email',component : EmailComponent},
  {path : 'report',component : ReportsComponent},
  {path : 'payment',component : PaymentComponent},
  {path : 'service',component : ServicesComponent},
  {path : 'privacy-policy',component : PrivacypolicyComponent},
  {path : 'terms-and-conditions',component : TermsandconditionsComponent}
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    ForgotpasswordComponent,
    ResetpasswordComponent,
    InvestmentComponent,
    InvestComponent,
    HeaderComponent,
    InvestorProfileComponent,
    MyinvestmentComponent,
    UpdatesComponent,
    DistributionsComponent,
    SideBarComponent,
    DocumentsComponent,
    AccountComponent,
    DragDirective,
    SignupComponent,
    OtpVerificationComponent,
    LeadComponent,
    AdminDashboardComponent,
    InvestorComponent,
    PortfolioComponent,
    EmailComponent,
    ReportsComponent,
    PaymentComponent,
    ServicesComponent,
    TermsandconditionsComponent,
    PrivacypolicyComponent,
    PortfolioDetailsComponent,
   
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    NgxPaginationModule,
    SwiperModule,
    CKEditorModule,
    NgOtpInputModule,
    NgxDocViewerModule,
    AgmCoreModule.forRoot({
      apiKey: "AIzaSyAjeJEPREBQFvAIqDSZliF0WjQrCld-Mh0"
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
