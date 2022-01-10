import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account/account.service';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  profileDropdown: boolean = false;
  NotificationDropdown: boolean = false;
  UserId: any;
  UserData: any;
  thisUser: any;
  NotificationData: any = [];
  Notification: any = [];
  RoleId: any;
  // ProfileImg: any;
  @Input() ProfileImg: any;

  constructor(private router: Router,
    private accountService : AccountService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.UserId = Number(localStorage.getItem('UserId'));
    this.RoleId = Number(localStorage.getItem("RoleId"));
    this.ProfileImg = localStorage.getItem("ProfileImg");
    this.GetNotification();


    const connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Information)
    .withUrl(environment.NOTIFICATION_URL + "/notificationHub")
    .build();

    connection.start().then(function () {
      console.log('Connected!');
    }).catch(function (err) {
      return console.error(err);
    });
    connection.onclose(() => {
      setTimeout(function () {
        connection.start().then(function () {
          console.log('Reconnected!');
        }).catch(function (err) {
          console.error(err)
        });
      }, 3000);
    });
    connection.on("ForceLogout", (data: any) => {
      console.clear();
    });
    connection.on("Push_Notification", (data: any) => {
      this.thisUser = [];
      let UserId = Number(localStorage.getItem('UserId'));
      if(data.userId == UserId){
        this.NotificationData.unshift(data);
      }
    });
  }
  logout() {
    this.router.navigate(['/login'], { relativeTo: this.route });
    localStorage.clear();
  }
  Account() {
    this.router.navigate(['/account'], { relativeTo: this.route });
  }
  GetNotification(){
    this.accountService.GetNotification(this.UserId).subscribe(data =>{
      this.NotificationData = data;
      this.NotificationData = this.NotificationData.filter((x: { status: number; }) => x.status == 1)
      for(let i = 0; i< 5;i++){
        this.Notification.push(this.NotificationData[i])
      }
    })
  }
  NotificationClick(){
    this.NotificationDropdown = !this.NotificationDropdown
  }
  Read(val : any){
    this.NotificationDropdown = true;
    this.Notification = this.Notification.filter((x: { id: any; }) => x.id != val.id);
    let notification = {
      Id : val.id,
      UserId : this.UserId,
      Status : 2
    }
    this.accountService.UpdateNotification(notification).subscribe(data =>{
      if(data == true){
        this.NotificationDropdown = true;
      }
    })
  }
  ClearAll(){
    this.Notification = [];
    this.accountService.ClearAllNotification(this.UserId).subscribe(data =>{

    })
  }
  onLoadMore(){
    let length = this.Notification.length + 5
    for(let i = this.Notification.length; i< length; i++){
      if(this.NotificationData[i] != null){
        this.Notification.push(this.NotificationData[i])
      }
    }
  }
  onLogo(){
    if(this.RoleId == 1){
      this.router.navigate(['/invest'], { relativeTo: this.route });
    }
    else if(this.RoleId == 2){
      this.router.navigate(['/invest'], { relativeTo: this.route });
    }
    else if(this.RoleId == 3){
      this.router.navigate(['/admin-dashboard'], { relativeTo: this.route });
    }
  }
}
