import { Component, OnInit } from '@angular/core';
import { InvestService } from '../invest/invest.service';
import { InvestorProfileService } from '../investor-profile/investor-profile.service';
import { DistributionService } from './distribution.service';

@Component({
  selector: 'app-distributions',
  templateUrl: './distributions.component.html',
  styleUrls: ['./distributions.component.css']
})
export class DistributionsComponent implements OnInit {
  UserId: any;
  DistributionData: any = [];
  config: any;
  OfferingId: any = 0;
  DropdownProfile: any = []
  DistributionValue: any = [];
  Filter: any = [];
  Filter1: any = [];
  ProfileId: any = 0;
  DropdownOffering: any = [];
  Loader: boolean = false;
  Marketplace: any = [];

  constructor(private distributionService: DistributionService,
    private investService: InvestService,
    private profileService: InvestorProfileService) {
    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.DistributionData.length
    };
  }

  ngOnInit(): void {
    this.Loader = true;
    this.UserId = localStorage.getItem('UserId');
    this.GetDistribution();
  }

  GetDistribution() {
    this.distributionService.GetDistributionByUserId(this.UserId).subscribe(data => {
      this.DistributionValue = data;
      this.DistributionData = data;
      for (let i = 0; i < this.DistributionData.length; i++) {
        if (this.DistributionData[i].userProfile.name != null) {
          this.DistributionData[i].ProfileName = this.DistributionData[i].userProfile.name
        }
        if (this.DistributionData[i].userProfile.firstName != null) {
          this.DistributionData[i].ProfileName = this.DistributionData[i].userProfile.firstName + this.DistributionData[i].userProfile.lastName
        }
        if (this.DistributionData[i].userProfile.trustName != null) {
          this.DistributionData[i].ProfileName = this.DistributionData[i].userProfile.trustName
        }
        if (this.DistributionData[i].userProfile.retirementPlanName != null) {
          this.DistributionData[i].ProfileName = this.DistributionData[i].userProfile.retirementPlanName
        }
      }
      this.GetMarketplace();
    })
  }
  pageChanged(event: any) {
    this.config.currentPage = event;
  }
  onProfile(e: any) {
    this.ProfileId = +e.target.value;
    if (this.ProfileId == 0) {
      this.DistributionData = this.DistributionValue;
      this.OfferingId = 0;
    }
    else {
      this.Filter = this.DropdownProfile.filter((x: { id: any }) => x.id == this.ProfileId)
      this.DistributionData = this.DistributionValue.filter((x: { ProfileName: any; }) => x.ProfileName == this.Filter[0].name)
      if (this.Filter1.length > 0) {
        this.DistributionData = this.DistributionData.filter((x: { offeringName: any; }) => x.offeringName == this.Filter1[0].name)
      }
    }
  }
  onOffering(e: any) {
    this.OfferingId = +e.target.value;
    if (this.OfferingId == 0) {
      this.DistributionData = this.DistributionValue;
      this.ProfileId = 0;
    }
    else {
      this.Filter1 = this.DropdownOffering.filter((x: { id: any }) => x.id == this.OfferingId)
      this.DistributionData = this.DistributionValue.filter((x: { offeringName: any; }) => x.offeringName == this.Filter1[0].name)
      if (this.Filter.length > 0) {
        this.DistributionData = this.DistributionData.filter((x: { ProfileName: any; }) => x.ProfileName == this.Filter[0].name)
      }
    }
  }

  GetProfile() {
    this.profileService.GetProfileById(this.UserId).subscribe(data => {
      let x: any = data;
      this.DropdownProfile = [];
      this.DropdownProfile.push({ id: 0, name: 'All Profiles' })
      for (let i = 0; i < x.length; i++) {
        if (x[i].name != null) {
          this.DropdownProfile.push({ id: x[i].id, name: x[i].name })
        }
        else if (x[i].firstName != null) {
          this.DropdownProfile.push({ id: x[i].id, name: x[i].firstName })
        }
        else if (x[i].trustName != null) {
          this.DropdownProfile.push({ id: x[i].id, name: x[i].trustName })
        }
        else if (x[i].retirementPlanName != null) {
          this.DropdownProfile.push({ id: x[i].id, name: x[i].retirementPlanName })
        }
      }
      this.Loader = false;
    })
  }
  GetMarketplace() {
    this.investService.GetMarketplace().subscribe(data => {
      this.Marketplace = data
      this.DropdownOffering.push({ id: 0, name: 'All Offering' })
      let x = this.Marketplace.filter((x: { active: boolean; isReservation: boolean }) => x.active == true && x.isReservation == false);
      for (let i = 0; i < x.length; i++) {
        this.DropdownOffering.push({ id: x[i].id, name: x[i].name })
      }
      this.GetProfile();
    });
  }

}
