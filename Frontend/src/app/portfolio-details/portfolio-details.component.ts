import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-portfolio-details',
  templateUrl: './portfolio-details.component.html',
  styleUrls: ['./portfolio-details.component.css']
})
export class PortfolioDetailsComponent implements OnInit {
  routeName : any ;
  offeringId : any;
  profileURl : any;
  constructor(private route: ActivatedRoute , private router: Router) { }

  ngOnInit(): void {
    this.routeName = this.route.snapshot.params['name'];
    if (this.routeName == 'offering') {
      this.offeringId = +this.route.snapshot.params['id'];
      this.profileURl =  localStorage.getItem('ImageURL');
      this.getOfferingByStatus(this.offeringId);
    }
  }
  getOfferingByStatus(Id: any) {
      // this.AddOfferingForm.patchValue({
      //   OfferingName: this.ViewDetailsByStaus.name,
      //   OfferingType: this.ViewDetailsByStaus.type,
      //   EntityName: this.ViewDetailsByStaus.entityName,
      //   Status: this.ViewDetailsByStaus.status,
      //   OfferingSize: this.ViewDetailsByStaus.size,
      //   MinimumInvestment: this.ViewDetailsByStaus.minimumInvestment,
      //   Visibility: this.ViewDetailsByStaus.visibility,
      // })
      // this.StatusId = this.ViewDetailsByStaus.status;
      // this.OfferingTypeId = this.ViewDetailsByStaus.type;
      // this.VisibilityId = this.ViewDetailsByStaus.visibility;
  }

  ShowPortfolioList(){
    localStorage.setItem('viewDetails', 'false')
    this.router.navigateByUrl('/portfolio');
  }
  
}
