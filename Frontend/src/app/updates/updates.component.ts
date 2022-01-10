import { Component, OnInit } from '@angular/core';
import { UpdatesService } from './updates.service';

@Component({
  selector: 'app-updates',
  templateUrl: './updates.component.html',
  styleUrls: ['./updates.component.css']
})
export class UpdatesComponent implements OnInit {
  UpdateValue: any = [];
  Name: any;
  Subject: any;
  PreviewPopup: boolean = false;
  Date: any;
  UpdateValueId : any = 0;
  DropdownUpdateValue : any = [];
  UpdateData: any = [];
  Filter: any = [];
  config: any;
  UserId: any;
  Loader : boolean = false

  constructor(private updateService: UpdatesService) {
    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.UpdateValue.length
    };
   }

  ngOnInit(): void {
    this.Loader = true;
    this.UserId = localStorage.getItem('UserId');
    this.GetUpdate();
  }
  GetUpdate() {
    this.updateService.GetUpdates(this.UserId).subscribe(data => {
      this.UpdateData = data;
      this.UpdateValue = data;
      this.config.totalItems = this.UpdateValue.length
      this.DropdownUpdateValue.push({ id: 0, name: 'All Updates'})
      for(let i = 0; i< this.UpdateValue.length; i++){
        let x = this.DropdownUpdateValue.filter((x: { name: any; }) => x.name == this.UpdateValue[i].name)
        if(x.length == 0){
          this.DropdownUpdateValue.push({id : this.UpdateValue[i].id, name : this.UpdateValue[i].name})
        }
      }
      this.Loader = false
    })
  }

  onPreview(value: any) {
    this.PreviewPopup = true;
    this.Name = value.name
    this.Subject = value.subject
    this.Date = value.createdOn
  }
  onChange(e : any){
    this.UpdateValueId = +e.target.value;
    console.log(this.UpdateData,'updatedata')
    if(this.UpdateValueId == 0){
      this.UpdateValue = this.UpdateData;
    }
    else{
      this.Filter = this.DropdownUpdateValue.filter((x : {id : any}) => x.id == this.UpdateValueId)
      this.UpdateValue = this.UpdateData.filter((x: { name: any; }) => x.name == this.Filter[0].name)
    }
    this.config.totalItems = this.UpdateValue.length
    console.log(e.target.value,'change')
  }
  pageChanged(event : any) {
    this.config.currentPage = event;
  }
  onChooseDataPerPage(event : any){
    if(+event.target.value == 0){
      this.config.itemsPerPage = 5
    }
    else{
      this.config.itemsPerPage = +event.target.value
    }
  }

}
