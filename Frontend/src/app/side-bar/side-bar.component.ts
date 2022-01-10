import { Component, OnInit } from '@angular/core';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {
  RoleId: any;
  RoleMapping: any = [];

  constructor(private loginService : LoginService) { }

  ngOnInit(): void {
    this.RoleId = Number(localStorage.getItem("RoleId"))
    this.loginService.GetRoleMapping(this.RoleId).subscribe(data =>{
      this.RoleMapping = data;
    })
  }


}
