import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import InlineEditor from '@ckeditor/ckeditor5-build-inline';
import { ToastrService } from 'ngx-toastr';
import { PortfolioService } from './portfolio.service';
import SwiperCore, { Pagination, Navigation } from "swiper/core";
import { FileHandle } from '../documents/file.directive';


SwiperCore.use([Pagination, Navigation]);
@Component({
  selector: 'app-portfolio',
  templateUrl: './portfolio.component.html',
  styleUrls: ['./portfolio.component.css']
})
export class PortfolioComponent implements OnInit {
  Selected: any;
  SelectedDistribution: any;
  PortfolioList: any = [];
  config: any;
  addOfferingShow: boolean = false;
  AddOfferingForm: any;
  AddReservationForm: any;
  AddHistoryForm: any;
  OfferingNameError: boolean = false;
  OfferingTypeId: any = 0;
  StatusId: any = 0;
  VisibilityId: any = 0;
  OfferingStatusError: boolean = false;
  OfferingSizeZeroError: boolean = false;
  OfferingSizeError: boolean = false;
  OfferingSizeValue: any;
  MinimumInvestmentZeroError: any = false;
  MinimumInvestmentError: any = false;
  VisibilityError: any = false;
  submitted: boolean = false;
  EditOfferingData: any;
  OfferingId: any;
  UbdatebtnShow: boolean = false;
  DeleteConfirmationPopup: boolean = false;
  OfferingData: any;
  tabSelectId: any = 1;
  addReservationShow: boolean = false;
  ReservationNameError: boolean = false;
  ReservationSizeValue: any;
  ReservationSizeZeroError: boolean = false;
  ReservationSizeError: boolean = false;
  ReservationId: any;
  EditReservationData: any;
  EditInvestmentData: any;
  ReservationData: any;
  routeName: any;
  offeringId: any;
  ShowOfferingDetails: boolean = false;
  getDetails: any;
  ViewDetailsByStaus: any = {};
  OfferingInformationShow: boolean = false;
  OfferingImagesShow: boolean = false;
  OfferingSummaryShow: boolean = false;
  OfferingDocumentShow: boolean = false;
  OfferingLocationShow: boolean = false;
  OfferingKeyShow: boolean = false;
  OfferingFundShow: boolean = false;
  OfferingName: any;
  EntityName: any;
  OfferingSize: any;
  MinimumInvestment: any;
  reservationId: any;
  ShowReservationDetails: boolean = false;
  ReservationInformationShow: boolean = false;
  GalleryList: any = [];
  SummaryList: any = [];
  DocumentList: any = [];
  Editor: any = {};;
  Markers: any = [];
  lat: any;
  lng: any;
  zoom: any;
  LocationList: any = [];
  keyHignlightList: any = [];
  FundsList: any;
  OfferingDetailsShow: boolean = false;
  SubscriptionsShow: boolean = false;
  ModalName: any;
  Id: any;
  ReservationSize: any;
  ReservationName: any;
  filesToUpload: any = [];
  allowedFileExtensions: any = [];
  allowedFileExtensionsDocument: any = [];
  profileImageUrl: any;
  UploadImage: any = [];
  TypeId: any;
  UserId: any;
  SummaryListValue: any;
  documentUploadShow: boolean = false;
  files: any = [];
  UploadToWelcomeFiles: any = [];
  UploadToOfferingFiles: any = [];
  PortfolioDocument: any;
  DocumentViewPopup: boolean = false;
  ViewFile: any;
  documentValue: any;
  documentType: any;
  DeleteDocumentPopup: boolean = false;
  addkeyShow: boolean = false;
  addKeyName: any;
  addKeyValue: any;
  addKeyVisibility: any;
  SavekeyShow: boolean = false;
  keyDeleteShow: boolean = false;
  KeyValue: any;
  KeyValueId: any;
  wireTransfer: any = 'true';
  byCheck: any;
  custom: any;
  FundAmound: any;
  FundBankName: any;
  FundBankAddress: any;
  FundBeneficiaryAccountName: any;
  FundRoutingNumber: any;
  FundAccountNumber: any;
  FundAccountType: any;
  FundBeneficiaryAddress: any;
  FundReference: any;
  FundOtherInstructions: any;
  wireTransferReadOnly: boolean = false;
  byCheckReadOnly: boolean = false;
  byCustomReadOnly: boolean = false;
  byCheckShow: boolean = false;
  FundMailingAddress: any;
  FundMailBeneficiary: any;
  FundMailMemo: any;
  FundMailOtherInstructions: any;
  customShow: boolean = false;
  customMailValue: any;
  FundsId: any;
  Loader: boolean = false;
  SummaryBtnShow: boolean = true;
  AddOfferAndReservationBtn: boolean = true;
  InvestorTabShow: boolean = false;
  InvestorSummary: any = {};
  addInvestmentShow: boolean = false;
  InvestmentOfferingId: any = 0;
  InvestmentUserId: any = 0;
  InvestmentProfileId: any = 0;
  InvestmentStatusId: any = 0;
  FundReceivedDate: any;
  DocsSignedDate: any;
  AddInvestmentForm: any;
  AddNewReservationForm: any;
  UserTypeList: any = [];
  profileList: any = [];
  statusList: any = [];
  InvestmentId: any;
  InvestmentDeleteShow: boolean = false;
  InvestmentDelValue: any;
  AddInvestmentBtn: boolean = false;
  AddReservationBtn: boolean = false;
  addNewReservationShow: boolean = false;
  OfferingReservationTabShow: boolean = false;
  offeringReservationSummary: any;
  ReservationUserId: any = 0;
  ReservationUserError: boolean = false;
  ReservationProfileId: any = 0;
  ReservationAmountError: boolean = false;
  ReservationAmountZeroError: boolean = false;
  ReservationLevelId: any = 0;
  ReservationLevelError: boolean = false;
  ReservationProfileError: boolean = false;
  ShowReservationDetailsView: boolean = false;
  ShowNewReservationList: boolean = false;
  ReservationDeleteShow: boolean = false;
  ReservationValue: any;
  ReservationSummary: any;
  ResertvationId: any;
  NewReservationValue: any;
  OfferingDistributionShow: boolean = false;
  captablevalues: any;
  distributionShow: boolean = false;
  OwnershipValue: any;
  DistributionId: any;
  distributionHistoryDeleteShow: boolean = false;
  Historyvalue: any;
  viewDistributionShow: boolean = false;
  viewHistoryValue: any;
  editDistributionHistoryShow: boolean = false;
  HistoryId: any = 0;
  historyTypeList: any = [];
  HistoryTypeError: boolean = false;
  HistoryMemoError: boolean = false;
  HistoryAmountError: boolean = false;
  HistoryMethodId: any = 0;
  HistoryMethodError: boolean = false;
  HistoryStartDateError: boolean = false;
  HistoryEndDateError: boolean = false;
  HistoryPaymentDateError: boolean = false;
  updateHistoryValue: any;
  InvestorType :any = "1";
  DistributionList: any = [];
  CalculationId: any = 0;
  CalculationTypeError: boolean = false;
  DistributionAmount: any;
  DistributionAmountError: boolean = false;
  DistributionAmountZeroError: boolean = false;
  DistributionStartDate: any;
  DistributionStartDateError: boolean = false;
  DistributionEndDateError: boolean = false;
  DistributioEndDate: any;
  DistributionPaymentDateError: boolean = false;
  DistributionPaymentDate: any;
  DistributionMemoError: boolean = false;
  DistributionMemo: any;
  capValueList: any = [];
  CalculateValueList: any = [];
  addDistributionId: any;
  AllInvestorShow : boolean = false;
  SingleInvestorShow : boolean = false;
  InvestorNameList : any = [];
  InvestorId : any = 0;
  InvestorNameError : boolean = false;
  PaymentBtnShow : boolean = true;
  TotalAmount : any ;
  OfferingTypeList: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Investment' },
    { id: 2, value: 'Commitment' },
  ];
  OfferingStatusList: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Draft' },
    { id: 2, value: 'Open' },
    { id: 3, value: 'Manage' },
    { id: 4, value: 'Past' },
  ]
  OfferingVisibilityList: any = [
    { id: 0, value: 'Select Visibility' },
    { id: 1, value: 'No Users' },
    { id: 2, value: 'All Users' },
    { id: 3, value: 'Verified Users' },
    { id: 4, value: 'Accredited Only' },
    { id: 5, value: 'Real Estate Offering name (will contain investors who participated in it)' },
    { id: 6, value: 'Test (IT and Admin)' },
  ]
  ReservationStatusList: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Draft' },
    { id: 2, value: 'Active-Accepting Reservations' },
    { id: 3, value: 'Reservations Closed' },
  ]
  ReservationVisibilityList: any = [
    { id: 0, value: 'Select Visibility' },
    { id: 1, value: 'No Users' },
    { id: 2, value: 'All Users' },
    { id: 3, value: 'Verified Users' },
    { id: 4, value: 'Accredited Only' },
    { id: 5, value: 'Specific Investors who have invested in individual Offerings' },
    { id: 6, value: 'Test (IT and Admin)' },
  ]
  AccountTypeList: any = [
    { id: 1, value: 'Checking' },
    { id: 2, value: 'Savings' }
  ]

  ConfidenceLevelList: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'Very Likely' },
    { id: 2, value: 'Likely' },
    { id: 3, value: 'Unlikely' }
  ]

  historyMethodList: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: 'ACH' },
    { id: 2, value: 'Check' },
    { id: 3, value: 'Others' }
  ]

  calculationTypeList: any = [
    { id: 0, value: 'Select' },
    { id: 1, value: '% Funded' },
    { id: 2, value: '% Ownership' },
  ]
  constructor(private _portfolio: PortfolioService, private router: Router, private route: ActivatedRoute, private formBuilder: FormBuilder, private toastr: ToastrService,) {
    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.PortfolioList.length
    };
    this.Editor = InlineEditor;
    this.lat = 0;
    this.lng = 0;
    this.zoom = 2;
    this.Markers = [];
  }

  ngOnInit(): void {
    this.routeName = this.route.snapshot.params['name'];
    if (this.routeName == 'offering') {
      localStorage.setItem('RouteName', this.routeName);
      this.offeringId = +this.route.snapshot.params['id'];
      this.AddInvestmentBtn = true;
      this.AddOfferAndReservationBtn = false;
      this.AddReservationBtn = false;
      this.ShowOfferingDetails = true;
      this.getOfferingByStatus(this.offeringId);
      this.OfferingInformation();
      this.getInvestmentStatusList();
    }
    else if (this.routeName == 'reservation') {
      localStorage.setItem('RouteName', this.routeName);
      this.reservationId = +this.route.snapshot.params['id'];
      this.AddReservationBtn = true;
      this.AddOfferAndReservationBtn = false;
      this.AddInvestmentBtn = false;
      this.ShowReservationDetails = true;
      this.getReservationByStatus(this.reservationId);
      this.ReservationInformation();
    }
    else {
      this.ShowOfferingDetails = false;
      this.ShowReservationDetails = false;
      this.AddOfferAndReservationBtn = true;
      if (localStorage.getItem('RouteName') == 'offering') {
        this.tabSelectId = 1;
        this.Selected = 'Offerings';
        this.getOfferings();
      }
      else if (localStorage.getItem('RouteName') == 'reservation') {
        this.tabSelectId = 2;
        this.Selected = 'Reservations';
        this.getReservation();
      }
      else {
        this.Selected = 'Offerings';
        this.getOfferings();
      }
    }
    this.getInvestmentUserList();
    this.allowedFileExtensions = ['jpg', 'jpeg', 'png', 'PNG'];
    this.allowedFileExtensionsDocument = ['pdf', 'PDF'];
    this.AddOfferingForm = this.formBuilder.group({
      OfferingName: ['', Validators.required],
      OfferingType: [''],
      EntityName: [''],
      Status: ['', Validators.required],
      OfferingSize: ['', Validators.required],
      MinimumInvestment: ['', Validators.required],
      Visibility: ['', Validators.required],
    })
    this.AddReservationForm = this.formBuilder.group({
      ReservationName: ['', Validators.required],
      EntityName: [''],
      Status: [''],
      Visibility: ['', Validators.required],
      ReservationSize: ['', Validators.required],
    })
    this.AddInvestmentForm = this.formBuilder.group({
      InvestmentOfferingType: [''],
      InvestmentUser: [''],
      InvestmentAmount: [''],
      InvestmentProfileType: [''],
      InvestmentStatus: [''],
      InvestmentFundReceived: [''],
      InvestmentDocsSigned: [''],
    })
    this.AddNewReservationForm = this.formBuilder.group({
      NewReservationName: [''],
      ReservationUser: ['', Validators.required],
      ReservationProfileType: ['', Validators.required],
      ReservationAmount: ['', Validators.required],
      ReservationLevel: ['', Validators.required],
    })
    this.AddHistoryForm = this.formBuilder.group({
      HistoryType: ['', Validators.required],
      HistoryStartDate: ['', Validators.required],
      HistoryEndDate: ['', Validators.required],
      HistoryPaymentDate: ['', Validators.required],
      HistoryMemo: ['', Validators.required],
      Amount: [''],
      DistributionAmount: ['']
      // HistoryInvestor: ['', Validators.required],
      // HistoryAmount: ['', Validators.required],
      // HistoryMethod: ['', Validators.required]
    })

  }

  pageChanged(event: any) {
    this.config.currentPage = event;
  }

  ViewDetailsOffering(e: any) {
    this.getDetails = e;
    console.log('getDetails', this.getDetails)
    this.router.navigate(['./../portfolio' + '/' + 'offering' + '/' + this.getDetails.id], { relativeTo: this.route });
  }

  ViewReservationDetails(e: any) {
    this.getDetails = e;
    this.router.navigate(['./../portfolio' + '/' + 'reservation' + '/' + this.getDetails.id], { relativeTo: this.route });
  }

  getOfferingByStatus(Id: any) {
    this.Loader = true;
    this.ViewDetailsByStaus = {};
    this.GalleryList = [];
    this.SummaryList = [];
    this.DocumentList = [];
    this.LocationList = [];
    this.keyHignlightList = [];
    this.FundsList = {};
    this.Markers = [];
    this.SummaryListValue = '';
    this._portfolio.getPortfoliobyOfferingStatus(Id).subscribe(data => {
      if (data != null) {
        this.ViewDetailsByStaus = data;
        this.selectOfferingDetails();
        this.Loader = false;
        this.Id = this.ViewDetailsByStaus.id;
        this.OfferingName = this.ViewDetailsByStaus.name,
          this.AddInvestmentForm.patchValue({
            InvestmentOfferingType: this.ViewDetailsByStaus.name,
          })
        this.OfferingTypeId = this.ViewDetailsByStaus.type;
        this.EntityName = this.ViewDetailsByStaus.entityName,
          this.StatusId = this.ViewDetailsByStaus.status;
        this.OfferingSize = this.ViewDetailsByStaus.size;
        this.MinimumInvestment = this.ViewDetailsByStaus.minimumInvestment;
        this.VisibilityId = this.ViewDetailsByStaus.visibility;
        this.GalleryList = this.ViewDetailsByStaus.galleries;
        console.log('GalleryList', this.GalleryList);
        this.SummaryList = this.ViewDetailsByStaus.summary;
        if (this.SummaryList.length > 0) {
          this.SummaryListValue = this.ViewDetailsByStaus.summary[0].summary;
        }
        this.DocumentList = this.ViewDetailsByStaus.documents;
        console.log('DocumentList', this.DocumentList);
        this.LocationList = this.ViewDetailsByStaus.locations;
        this.keyHignlightList = this.ViewDetailsByStaus.keyHighlights;
        console.log('keyHignlightList', this.keyHignlightList);
        this.FundsList = this.ViewDetailsByStaus.funds;
        if (this.FundsList != null) {
          this.getFundsDetails();
        }
        this.Markers.push({
          position: {
            lat: this.LocationList[0].latitude,
            lng: this.LocationList[0].longitude
          },
        });
      }
      else {
        this.Loader = false;
      }
    })
  }

  getFundsDetails() {
    this.FundsId = this.FundsList.id;
    this.FundBankName = this.FundsList.receivingBank;
    this.FundBankAddress = this.FundsList.bankAddress;
    this.FundRoutingNumber = this.FundsList.routingNumber;
    this.FundAccountNumber = this.FundsList.accountNumber;
    this.FundAccountType = this.FundsList.accountType;
    console.log('FundAccountType', this.FundAccountType)
    this.FundBeneficiaryAccountName = this.FundsList.beneficiary;
    this.FundBeneficiaryAddress = this.FundsList.beneficiaryAddress;
    this.FundReference = this.FundsList.reference;
    this.FundOtherInstructions = this.FundsList.otherInstructions;
    this.FundMailingAddress = this.FundsList.mailingAddress;
    this.FundMailBeneficiary = this.FundsList.checkBenificiary;
    this.FundMailMemo = this.FundsList.memo;
    this.FundMailOtherInstructions = this.FundsList.checkOtherInstructions;
    this.customMailValue = this.FundsList.custom;
  }

  getReservationByStatus(Id: any) {
    this.Loader = true;
    this.ViewDetailsByStaus = {};
    this.GalleryList = [];
    this.SummaryList = [];
    this.DocumentList = [];
    this.LocationList = [];
    this.keyHignlightList = [];
    this.FundsList = [];
    this.Markers = [];
    this.SummaryListValue = '';
    this._portfolio.getPortfoliobyReservationStatus(Id).subscribe(data => {
      this.ViewDetailsByStaus = data;
      this.selectReservationDetails();
      this.Loader = false;
      this.Id = this.ViewDetailsByStaus.id;
      this.ReservationName = this.ViewDetailsByStaus.name,
        this.AddNewReservationForm.patchValue({
          NewReservationName: this.ViewDetailsByStaus.name,
        })
      this.EntityName = this.ViewDetailsByStaus.entityName,
        this.StatusId = this.ViewDetailsByStaus.status;
      this.ReservationSize = this.ViewDetailsByStaus.size;
      this.VisibilityId = this.ViewDetailsByStaus.visibility;
      this.GalleryList = this.ViewDetailsByStaus.galleries;
      this.SummaryList = this.ViewDetailsByStaus.summary;
      if (this.SummaryList.length > 0) {
        this.SummaryListValue = this.ViewDetailsByStaus.summary[0].summary;
      }
      this.DocumentList = this.ViewDetailsByStaus.documents;
      this.LocationList = this.ViewDetailsByStaus.locations;
      this.keyHignlightList = this.ViewDetailsByStaus.keyHighlights;
      this.FundsList = this.ViewDetailsByStaus.funds;
      this.Markers.push({
        position: {
          lat: this.LocationList[0].latitude,
          lng: this.LocationList[0].longitude
        },
      });
    })
  }
  ShowPortfolioList() {
    localStorage.setItem('RouteName', 'offering');
    this.router.navigateByUrl('/portfolio');
  }

  ShowPortfolioListReservation() {
    localStorage.setItem('RouteName', 'reservation');
    this.router.navigateByUrl('/portfolio');
  }

  // Offering
  selectOfferings(Id: any) {
    this.tabSelectId = Id;
    this.Selected = 'Offerings';
    this.getOfferings();
  }

  getOfferings() {
    this.Loader = true;
    this.PortfolioList = [];
    this._portfolio.GetPortfolioOfferings().subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('PortfolioList', data);
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  get O() { return this.AddOfferingForm.controls }

  onAddPortfolioOffering() {
    this.ModalName = 'Add';
    this.addOfferingShow = true;
    this.submitted = false;
    this.UbdatebtnShow = false;
    this.AddOfferingForm.reset();
    this.OfferingNameError = false;
    this.OfferingStatusError = false;
    this.OfferingSizeZeroError = false;
    this.OfferingSizeError = false;
    this.VisibilityError = false;
    this.MinimumInvestmentZeroError = false;
    this.MinimumInvestmentError = false;
    this.OfferingTypeId = 0;
    this.StatusId = 0;
    this.VisibilityId = 0;
  }

  onOfferingName() {
    if (this.AddOfferingForm.value.OfferingName == '' || this.AddOfferingForm.value.OfferingName == null) {
      this.OfferingNameError = true;
    }
    else {
      this.OfferingNameError = false;
    }
  }

  onchangeOfferingType(e: any) {
    this.AddOfferingForm.get('OfferingType').setValue(e.target.value);
  }

  onchangeOfferingStatus(e: any) {
    this.AddOfferingForm.get('Status').setValue(e.target.value);
    this.AddOfferingForm.value.Status = e.target.value;
    this.StatusId = +e.target.value;
    if (this.StatusId == 0) {
      this.OfferingStatusError = true;
    }
    else {
      this.OfferingStatusError = false;
    }
  }

  onchangeVisibility(e: any) {
    this.AddOfferingForm.get('Visibility').setValue(e.target.value);
    this.AddOfferingForm.value.Visibility = e.target.value;
    this.VisibilityId = +e.target.value;
    if (this.VisibilityId == 0) {
      this.VisibilityError = true;
    }
    else {
      this.VisibilityError = false;
    }
  }

  numberValidation(event: any): Boolean {
    if (event.keyCode >= 48 && event.keyCode <= 57)
      return true;
    else
      return false;
  }

  onOfferingSize(e: any) {
    this.AddOfferingForm.get('OfferingSize').setValue(e.target.value);
    this.OfferingSizeValue = e.target.value;
    if (e.target.value == 0 && e.target.value != '') {
      this.OfferingSizeZeroError = true;
    }
    else {
      this.OfferingSizeZeroError = false;
    }
    if (this.AddOfferingForm.value.OfferingSize == '' || this.AddOfferingForm.value.OfferingSize == null) {
      this.OfferingSizeError = true;
    }
    else {
      this.OfferingSizeError = false;
    }
  }

  onMinimumInvestment(e: any) {
    this.AddOfferingForm.get('MinimumInvestment').setValue(e.target.value);
    if (this.OfferingSizeValue < e.target.value) {
      this.MinimumInvestmentZeroError = true;
    }
    else {
      this.MinimumInvestmentZeroError = false;
    }
    if (this.AddOfferingForm.value.MinimumInvestment == '' || this.AddOfferingForm.value.MinimumInvestment == null) {
      this.MinimumInvestmentError = true;
    }
    else {
      this.MinimumInvestmentError = false;
    }
  }

  onSubmitOfferingForm() {
    this.Loader = true;
    this.submitted = true;
    if (this.AddOfferingForm.value.Status == 0) {
      this.OfferingStatusError = true;
      this.Loader = false;
    }
    if (this.AddOfferingForm.value.Visibility == 0) {
      this.VisibilityError = true;
      this.Loader = false;
    }
    else if (this.AddOfferingForm.invalid) {
      this.Loader = false;
      return
    }
    else if (this.OfferingNameError == true || this.OfferingStatusError == true || this.OfferingSizeZeroError == true
      || this.OfferingSizeError == true || this.VisibilityError == true || this.MinimumInvestmentZeroError == true || this.MinimumInvestmentError == true) {
      this.Loader = false;
      return
    }
    else {
      let portfolioOfferingModel: any = {};
      portfolioOfferingModel.Name = this.AddOfferingForm.value.OfferingName;
      portfolioOfferingModel.Type = +this.OfferingTypeId;
      portfolioOfferingModel.EntityName = this.AddOfferingForm.value.EntityName;
      portfolioOfferingModel.Status = +this.StatusId;
      portfolioOfferingModel.Size = +this.AddOfferingForm.value.OfferingSize;
      portfolioOfferingModel.MinimumInvestment = +this.AddOfferingForm.value.MinimumInvestment;
      portfolioOfferingModel.Visibility = +this.VisibilityId;
      if (this.UbdatebtnShow == false) {
        this._portfolio.SavePortfolioOffering(portfolioOfferingModel).subscribe(data => {
          console.log('SavePortfolioOffering', data)
          if (data) {
            this.toastr.success("Offering added successfully", "Success");
            this.addOfferingShow = false;
            this.AddOfferingForm.reset();
            this.OfferingTypeId = 0;
            this.StatusId = 0;
            this.VisibilityId = 0;
            this.getOfferings();
            this.router.navigate(['./../portfolio' + '/' + 'offering' + '/' + data], { relativeTo: this.route });
          }
          else {
            this.Loader = false;
            this.toastr.error("Offering can't be added", "Error");
          }
        })
      }
      else {
        portfolioOfferingModel.Id = this.OfferingId;
        this._portfolio.UpdatePortfolioOffering(portfolioOfferingModel).subscribe(data => {
          console.log('UpdatePortfolioOffering', data)
          if (data) {
            this.toastr.success("Offering updated successfully", "Success");
            this.addOfferingShow = false;
            this.AddOfferingForm.reset();
            this.OfferingTypeId = 0;
            this.StatusId = 0;
            this.VisibilityId = 0;
            this.getOfferings();
            this.router.navigate(['./../portfolio' + '/' + 'offering' + '/' + data], { relativeTo: this.route });
          }
          else {
            this.Loader = false;
            this.toastr.error("Offering can't be updated", "Error");
          }
        })
      }
    }
  }

  SaveOfferingDetails(id: any) {
    this.Loader = true;
    if (this.StatusId == 0) {
      this.OfferingStatusError = true;
      this.Loader = false;
    }
    if (this.VisibilityId == 0) {
      this.VisibilityError = true;
      this.Loader = false;
    }
    else if (this.OfferingNameError == true || this.OfferingStatusError == true || this.OfferingSizeZeroError == true
      || this.OfferingSizeError == true || this.VisibilityError == true || this.MinimumInvestmentZeroError == true || this.MinimumInvestmentError == true) {
      this.Loader = false;
      return
    }
    else {
      let portfolioOfferingModel: any = {};
      portfolioOfferingModel.Name = this.OfferingName;
      portfolioOfferingModel.Type = +this.OfferingTypeId;
      portfolioOfferingModel.EntityName = this.EntityName;
      portfolioOfferingModel.Status = +this.StatusId;
      portfolioOfferingModel.Size = +this.OfferingSize;
      portfolioOfferingModel.MinimumInvestment = +this.MinimumInvestment;
      portfolioOfferingModel.Visibility = +this.VisibilityId;
      portfolioOfferingModel.Id = id;
      this._portfolio.UpdatePortfolioOffering(portfolioOfferingModel).subscribe(data => {
        console.log('UpdatePortfolioOffering', data)
        if (data) {
          this.Loader = false;
          this.toastr.success("Offering updated successfully", "Success");
        }
        else {
          this.Loader = false;
          this.toastr.error("Offering can't be updated", "Error");
        }
      })
    }
  }

  addOfferingCancel() {
    this.addOfferingShow = false;
    this.AddOfferingForm.reset();
    this.submitted = false;
    this.OfferingNameError = false;
    this.OfferingStatusError = false;
    this.OfferingSizeZeroError = false;
    this.OfferingSizeError = false;
    this.VisibilityError = false;
    this.MinimumInvestmentZeroError = false;
    this.MinimumInvestmentError = false;
    this.OfferingTypeId = 0;
    this.StatusId = 0;
    this.VisibilityId = 0;
    this.UbdatebtnShow = false;
  }

  EditOffering(val: any) {
    this.ModalName = 'Update';
    this.OfferingId = val.id;
    this.UbdatebtnShow = true;
    this.EditOfferingData = val;
    this.addOfferingShow = true;

    this.AddOfferingForm.patchValue({
      OfferingName: this.EditOfferingData.name,
      OfferingType: this.EditOfferingData.type,
      EntityName: this.EditOfferingData.entityName,
      Status: this.EditOfferingData.status,
      OfferingSize: this.EditOfferingData.size,
      MinimumInvestment: this.EditOfferingData.minimumInvestment,
      Visibility: this.EditOfferingData.visibility,
    })
    this.StatusId = this.EditOfferingData.status;
    this.OfferingTypeId = this.EditOfferingData.type;
    this.VisibilityId = this.EditOfferingData.visibility;
  }

  DeleteOffering(val: any) {
    this.OfferingData = '';
    this.DeleteConfirmationPopup = true;
    this.OfferingData = val;
  }

  onDeleteConfirmation() {
    if (this.tabSelectId == 1) {
      this._portfolio.deletePortfolioOffering(this.OfferingData.adminUserId, this.OfferingData.id).subscribe(data => {
        if (data) {
          this.getOfferings();
          this.toastr.success("Offering deleted successfully", "Success!");
          this.DeleteConfirmationPopup = false;

        }
        else {
          this.toastr.error("Offering can't be delete", "Error!");
          this.DeleteConfirmationPopup = false;
        }
      })
    }
    else if (this.tabSelectId == 2) {
      this._portfolio.deletePortfolioReservation(this.ReservationData.adminUserId, this.ReservationData.id).subscribe(data => {
        if (data) {
          this.getReservation();
          this.toastr.success("Reservation deleted successfully", "Success!");
          this.DeleteConfirmationPopup = false;

        }
        else {
          this.toastr.error("Reservation can't be delete", "Error!");
          this.DeleteConfirmationPopup = false;
        }
      })
    }
  }

  // Reservation

  selectReservations(Id: any) {
    this.tabSelectId = Id
    this.Selected = 'Reservations';
    this.getReservation();
  }

  get R() { return this.AddReservationForm.controls }

  getReservation() {
    this.Loader = true;
    this.PortfolioList = [];
    this._portfolio.GetPortfolioReservation().subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('ReservationsList', this.PortfolioList)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  onchangeReservationStatus(e: any) {
    this.AddReservationForm.get('Status').setValue(e.target.value);
  }

  onAddPortfolioReservation() {
    this.ModalName = 'Add';
    this.addReservationShow = true;
    this.submitted = false;
    this.UbdatebtnShow = false;
    this.AddReservationForm.reset();
    this.StatusId = 0;
    this.VisibilityId = 0;
    this.ReservationNameError = false;
    this.ReservationSizeZeroError = false;
    this.VisibilityError = false;
    this.ReservationSizeError = false;
  }

  addReservationCancel() {
    this.addReservationShow = false;
    this.submitted = false;
    this.UbdatebtnShow = false;
    this.AddReservationForm.reset();
    this.StatusId = 0;
    this.VisibilityId = 0;
    this.ReservationNameError = false;
    this.ReservationSizeZeroError = false;
    this.VisibilityError = false;
    this.ReservationSizeError = false;
  }

  onReservationName() {
    if (this.AddReservationForm.value.ReservationName == '' || this.AddReservationForm.value.ReservationName == null) {
      this.ReservationNameError = true;
    }
    else {
      this.ReservationNameError = false;
    }
  }

  onchangeReservationVisibility(e: any) {
    this.AddReservationForm.get('Visibility').setValue(e.target.value);
    this.AddReservationForm.value.Visibility = e.target.value;
    this.VisibilityId = +e.target.value;
    if (this.VisibilityId == 0) {
      this.VisibilityError = true;
    }
    else {
      this.VisibilityError = false;
    }
  }

  onReservationSize(e: any) {
    this.AddReservationForm.get('ReservationSize').setValue(e.target.value);
    this.ReservationSizeValue = e.target.value;
    if (e.target.value == 0 && e.target.value != '') {
      this.ReservationSizeZeroError = true;
    }
    else {
      this.ReservationSizeZeroError = false;
    }
    if (this.AddReservationForm.value.ReservationSize == '' || this.AddReservationForm.value.ReservationSize == null) {
      this.ReservationSizeError = true;
    }
    else {
      this.ReservationSizeError = false;
    }
  }

  onSubmitReservationForm() {
    this.Loader = true;
    this.submitted = true;
    if (this.AddReservationForm.value.Visibility == 0) {
      this.VisibilityError = true;
      this.Loader = false;
    }
    if (this.AddReservationForm.invalid) {
      this.Loader = false;
      return
    }
    else if (this.ReservationNameError == true || this.ReservationSizeZeroError == true || this.VisibilityError == true || this.ReservationSizeError == true) {
      this.Loader = false;
      return
    }
    else {
      let portfolioReservationModel: any = {};
      portfolioReservationModel.Name = this.AddReservationForm.value.ReservationName;
      portfolioReservationModel.EntityName = this.AddReservationForm.value.EntityName;
      portfolioReservationModel.Status = +this.StatusId;
      portfolioReservationModel.Visibility = +this.VisibilityId;
      portfolioReservationModel.ReservationSize = +this.AddReservationForm.value.ReservationSize;
      if (this.UbdatebtnShow == false) {
        this._portfolio.SavePortfolioReservation(portfolioReservationModel).subscribe(data => {
          console.log('portfolioReservationModel', data)
          if (data) {
            this.toastr.success("Reservation added successfully", "Success");
            this.addReservationShow = false;
            this.AddReservationForm.reset();
            this.StatusId = 0;
            this.VisibilityId = 0;
            this.getReservation();
            this.router.navigate(['./../portfolio' + '/' + 'reservation' + '/' + data], { relativeTo: this.route });
          }
          else {
            this.Loader = false;
            this.toastr.error("Reservation can't be added", "Error");
          }
        })
      }
      else {
        portfolioReservationModel.Id = this.ReservationId;
        this._portfolio.UpdatePortfolioOffering(portfolioReservationModel).subscribe(data => {
          console.log('UpdatePortfolioOffering', data)
          if (data) {
            this.toastr.success("Reservation updated successfully", "Success");
            this.addReservationShow = false;
            this.AddReservationForm.reset();
            this.StatusId = 0;
            this.VisibilityId = 0;
            this.getReservation();
            this.router.navigate(['./../portfolio' + '/' + 'reservation' + '/' + data], { relativeTo: this.route });
          }
          else {
            this.Loader = false;
            this.toastr.error("Reservation can't be updated", "Error");
          }
        })
      }
    }
  }

  SaveReservationDetails(id: any) {
    this.Loader = true;
    if (this.VisibilityId == 0) {
      this.VisibilityError = true;
      this.Loader = false;
    }
    else if (this.ReservationNameError == true || this.ReservationSizeZeroError == true || this.VisibilityError == true || this.ReservationSizeError == true) {
      this.Loader = false;
      return
    }
    else {
      let portfolioReservationModel: any = {};
      portfolioReservationModel.Name = this.ReservationName;
      portfolioReservationModel.EntityName = this.EntityName;
      portfolioReservationModel.Status = +this.StatusId;
      portfolioReservationModel.Visibility = +this.VisibilityId;
      portfolioReservationModel.ReservationSize = +this.ReservationSize;
      portfolioReservationModel.Id = id;
      this._portfolio.UpdatePortfolioOffering(portfolioReservationModel).subscribe(data => {
        console.log('UpdatePortfolioOffering', data)
        if (data) {
          this.Loader = false;
          this.toastr.success("Reservation updated successfully", "Success");
        }
        else {
          this.Loader = false;
          this.toastr.error("Reservation can't be updated", "Error");
        }
      })
    }
  }

  EditReservation(val: any) {
    this.ModalName = 'Update';
    this.ReservationId = val.id;
    this.UbdatebtnShow = true;
    this.EditReservationData = val;
    this.addReservationShow = true;

    this.AddReservationForm.patchValue({
      ReservationName: this.EditReservationData.name,
      EntityName: this.EditReservationData.entityName,
      Status: this.EditReservationData.status,
      ReservationSize: this.EditReservationData.size,
      Visibility: this.EditReservationData.visibility,
    })
    this.StatusId = this.EditReservationData.status;
    this.VisibilityId = this.EditReservationData.visibility;
  }

  DeleteReservation(val: any) {
    this.ReservationData = '';
    this.DeleteConfirmationPopup = true;
    this.ReservationData = val;
  }

  //Archives

  selectArchives(Id: any) {
    this.tabSelectId = Id
    this.Selected = 'Archives';
    this.getArchives();
  }

  getArchives() {
    this.Loader = true;
    this.PortfolioList = [];
    this._portfolio.getportfolioArchives().subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('getArchives', this.PortfolioList)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  RestoreArchives(val: any) {
    let RestoreValue = val;
    if (RestoreValue.isReservation == true) {
      this._portfolio.RestoreArchivesReservation(RestoreValue.adminUserId, RestoreValue.id).subscribe(data => {
        if (data) {
          this.toastr.success("Archives Restored successfully", "Success");
          this.getArchives();
        }
        else {
          this.toastr.error("Archives can't be Restored", "Error");
        }
      })
    }
    else {
      this._portfolio.RestoreArchivesOffering(RestoreValue.adminUserId, RestoreValue.id).subscribe(data => {
        if (data) {
          this.toastr.success("Archives Restored successfully", "Success");
          this.getArchives();
        }
        else {
          this.toastr.error("Archives can't be Restored", "Error");
        }
      })
    }

  }

  // Offering View by status
  selectInvestors() {
    this.Selected = 'Investors';
    this.InvestorTabShow = true;
    this.OfferingDetailsShow = false;
    this.SubscriptionsShow = false;
    this.OfferingReservationTabShow = false;
    this.OfferingDistributionShow = false;
    this.getInvestorListSummary();
    this.getInvestorList();
  }

  selectOfferingDetails() {
    this.Selected = 'OfferingDetails'
    this.OfferingDetailsShow = true;
    this.SubscriptionsShow = false;
    this.OfferingDistributionShow = false;
    this.OfferingReservationTabShow = false;
    this.InvestorTabShow = false;
  }

  selecteSignTemplates() {
    this.Selected = 'eSignTemplates'
    this.OfferingDetailsShow = false;
    this.SubscriptionsShow = false;
  }

  selectSubscriptions() {
    this.SubscriptionsShow = true;
    this.Selected = 'Subscriptions'
    this.InvestorTabShow = false;
    this.OfferingDetailsShow = false;
    this.OfferingDistributionShow = false;
    this.OfferingReservationTabShow = false;
    this.getSubscriptionsList();
  }

  selectAccreditations() {
    this.OfferingDetailsShow = false;
    this.Selected = 'Accreditations'
    this.SubscriptionsShow = false;
  }

  selectOfferingreservations() {
    this.OfferingDetailsShow = false;
    this.Selected = 'Reservations'
    this.SubscriptionsShow = false;
    this.InvestorTabShow = false;
    this.OfferingDistributionShow = false;
    this.OfferingReservationTabShow = true;
    this.getOfferingReservationSummary();
    this.getofferingReservationList();
  }

  selectCapitalCalls() {
    this.OfferingDetailsShow = false;
    this.Selected = 'CapitalCalls'
    this.SubscriptionsShow = false;
  }

  selectDistributions() {
    this.OfferingDetailsShow = false;
    this.Selected = 'Distributions'
    this.SubscriptionsShow = false;
    this.OfferingDistributionShow = true;
    this.OfferingReservationTabShow = false;
    this.InvestorTabShow = false;
    this.SelectedDistribution = 'CapTable';
    this.getCapTables();
    this.getHistoryType();
  }

  selectUpdates() {
    this.OfferingDetailsShow = false;
    this.Selected = 'Updates'
    this.SubscriptionsShow = false;
  }

  OfferingInformation() {
    //this.getOfferingByStatus(this.offeringId);
    this.OfferingInformationShow = !this.OfferingInformationShow;
  }

  OfferingImages() {
    this.OfferingImagesShow = !this.OfferingImagesShow;
  }

  OfferingSummary() {
    this.OfferingSummaryShow = !this.OfferingSummaryShow;
  }

  OfferingDocuments() {
    this.OfferingDocumentShow = !this.OfferingDocumentShow
  }

  OfferingLocation() {
    this.OfferingLocationShow = !this.OfferingLocationShow
  }

  OfferingKey() {
    this.OfferingKeyShow = !this.OfferingKeyShow
  }

  OfferingFund() {
    this.OfferingFundShow = !this.OfferingFundShow
  }

  getInvestorList() {
    this.Loader = true;
    this.PortfolioList = [];
    let offeringId = this.offeringId;
    this._portfolio.getInvestor(offeringId).subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('getInvestorList', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  getInvestorListSummary() {
    this.Loader = true;
    this.InvestorSummary = {};
    let offeringId = this.offeringId;
    this._portfolio.getInvestorSummary(offeringId).subscribe(data => {
      if (data != null) {
        this.InvestorSummary = data;
        console.log('getInvestorSummary', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  // Offering Distribution Tabs Methods

  //*************Distribution Cap Table*******/
  getCapTables() {
    this.Loader = true;
    this.PortfolioList = [];
    let offeringId = this.offeringId;
    this._portfolio.getCapTable(offeringId).subscribe(data => {
      if (data != null) {
        this.captablevalues = data;
        this.PortfolioList = this.captablevalues.capTableInvestments;
        this.capValueList = this.captablevalues.capTableInvestments;
        console.log('getCapTable', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  selectDistributionCapTableview() {
    this.SelectedDistribution = 'CapTable';
    this.getCapTables();
  }

  editDistribution(value: any) {
    console.log('editDistribution', value)
    this.distributionShow = true;
    this.OwnershipValue = value.ownershipPercentage;
    this.DistributionId = value.id;
  }

  UpdateOwnership() {
    this.Loader = true;
    let CapTable: any = {};
    CapTable.Id = this.DistributionId;
    CapTable.OfferingId = this.offeringId;
    CapTable.AdminUserId = Number(localStorage.getItem('UserId'));
    CapTable.FundedPercentage = Number(this.OwnershipValue);
    this._portfolio.updateOwnership(CapTable).subscribe(data => {
      if (data) {
        this.distributionShow = false;
        this.getCapTables();
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  //*************Distribution History*******/
  selectHistoryview() {
    this.SelectedDistribution = 'History';
    this.getHistory();
  }

  getHistory() {
    this.Loader = true;
    this.PortfolioList = [];
    let Id = this.offeringId;
    this._portfolio.GetDistributionHistory(Id).subscribe(data => {
      if (data) {
        this.PortfolioList = data;
        console.log('GetDistributionHistory', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  DeleteDistributionHistory(value: any) {
    this.distributionHistoryDeleteShow = true;
    this.Historyvalue = value;
  }

  DeletedistributionHistoryItems() {
    this.Loader = true;
    let UserId = Number(localStorage.getItem('UserId'));
    this._portfolio.deleteHistory(this.Historyvalue.id, UserId).subscribe(data => {
      if (data) {
        this.distributionHistoryDeleteShow = false;
        this.getHistory();
        this.toastr.success("History deleted successfully", "Success");
        this.Loader = false;
      }
      else {
        this.Loader = false;
        this.toastr.error("History can't be deleted", "Error");
      }
    })
  }

  viewHistory(value: any) {
    this.Loader = true;
    this.viewDistributionShow = true;
    this.viewHistoryValue = {};
    this.PortfolioList = [];
    this._portfolio.getViewHistoryList(value.id).subscribe(data => {
      if (data) {
        let a: any = {}
        a = data;
        this.viewHistoryValue = a;
        this.PortfolioList = a.distributions;
        console.log('viewHistoryValue', a)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  viewDistributionCancel() {
    this.viewDistributionShow = false;
    this.getHistory();
  }

  editHistory(value: any) {
    console.log('editHistory', value)
    this.editDistributionHistoryShow = true;
    this.UbdatebtnShow = true;
    this.DistributionList = [];
    this.updateHistoryValue = value;
    this.DistributionList = value.distributions;

    this.AddHistoryForm.patchValue({
      HistoryStartDate: (new Date(value.startDate)).toISOString().substring(0, 10),
      HistoryEndDate:(new Date(value.endDate)).toISOString().substring(0, 10), 
      HistoryPaymentDate: (new Date(value.paymentDate)).toISOString().substring(0, 10), 
      HistoryMemo: value.memo,
    })
    this.HistoryId = value.type;
  }

  editHistoryCancel() {
    this.editDistributionHistoryShow = false;
  }

  get H() { return this.AddHistoryForm.controls }

  getHistoryType() {
    this._portfolio.getHistoryType().subscribe(data => {
      if (data) {
        this.historyTypeList = data;
        this.historyTypeList.push({
          'id': 0,
          'name': 'Select Type'
        })
        console.log('historyTypeList', data)
      }
    })
  }



  onchangeHistoryType(e: any) {
    // this.AddHistoryForm.get('HistoryType').setValue(e.target.value);
    // this.AddHistoryForm.value.HistoryType = e.target.value;
    this.HistoryId = +e.target.value;
    if (this.HistoryId == 0) {
      this.HistoryTypeError = true;
    }
    else {
      this.HistoryTypeError = false;
    }
  }

  onHistoryMemo() {
    if (this.AddHistoryForm.value.HistoryMemo == '' || this.AddHistoryForm.value.HistoryMemo == null) {
      this.HistoryMemoError = true;
    }
    else {
      this.HistoryMemoError = false;
    }
  }

  onHistoryAmount(e: any) {
    console.log('onHistoryAmount', e)
    if (e.target.value == '' || e.target.value == null) {
      this.HistoryAmountError = true;
    }
    else {
      this.HistoryAmountError = false;
    }
  }

  HistoryStartDatechange() {
    if (this.AddHistoryForm.value.HistoryStartDate == '' || this.AddHistoryForm.value.HistoryStartDate == null) {
      this.HistoryStartDateError = true;
    }
    else {
      this.HistoryStartDateError = false;
    }
  }

  HistoryEndDatechange() {
    if (this.AddHistoryForm.value.HistoryEndDate == '' || this.AddHistoryForm.value.HistoryEndDate == null) {
      this.HistoryEndDateError = true;
    }
    else {
      this.HistoryEndDateError = false;
    }
  }

  HistoryPaymentDatechange() {
    if (this.AddHistoryForm.value.HistoryPaymentDate == '' || this.AddHistoryForm.value.HistoryPaymentDate == null) {
      this.HistoryPaymentDateError = true;
    }
    else {
      this.HistoryPaymentDateError = false;
    }
  }

  onchangeHistoryMethod(e: any, index: any) {
    this.HistoryMethodId = parseInt(e.target.value, 10);
    if (this.HistoryMethodId == 0) {
      this.HistoryMethodError = true;
    }
    else {
      this.HistoryMethodError = false;
    }
    this.DistributionList[index].paymentMethod = +e.target.value;
  }

  onSubmitHistoryForm() {
    this.Loader = true;
    this.submitted = true;
    if (this.AddHistoryForm.value.HistoryType == 0) {
      this.HistoryTypeError = true;
      this.Loader = false;
    }
    if (this.AddHistoryForm.value.HistoryMethod == 0) {
      this.HistoryMethodError = true;
      this.Loader = false;
    }
    else if (this.AddHistoryForm.invalid) {
      this.Loader = false;
      return
    }
    else if (this.HistoryTypeError == true || this.HistoryStartDateError == true || this.HistoryEndDateError == true
      || this.HistoryPaymentDateError == true || this.HistoryMemoError == true || this.HistoryAmountError == true || this.HistoryMethodError == true) {
      this.Loader = false;
      return
    }
    else {
      let HistoryModel: any = {};
      HistoryModel.AdminUserId = Number(localStorage.getItem('UserId'));
      HistoryModel.OfferingId = this.offeringId;
      HistoryModel.Type = +this.HistoryId;
      HistoryModel.StartDate = this.AddHistoryForm.value.HistoryStartDate;
      HistoryModel.EndDate = this.AddHistoryForm.value.HistoryEndDate;
      HistoryModel.PaymentDate = this.AddHistoryForm.value.HistoryPaymentDate;
      HistoryModel.Memo = this.AddHistoryForm.value.HistoryMemo;
      HistoryModel.CalculationMethod = this.updateHistoryValue.calculationMethod;
      HistoryModel.Distributions = this.DistributionList;
      console.log('Distributions', this.DistributionList)
      if (this.UbdatebtnShow == false) {
        this._portfolio.SaveNewReservation(HistoryModel).subscribe(data => {
          console.log('SaveNewReservation', data)
          if (data) {
            this.toastr.success("Reservation added successfully", "Success");
            this.editDistributionHistoryShow = false;
            this.AddHistoryForm.reset();
            this.HistoryId = 0;
            this.ReservationProfileId = 0;
            this.ReservationLevelId = 0;
            this.getNewReservationSummary();
            this.getNewReservationList();
          }
          else {
            this.Loader = false;
            this.toastr.error("Reservation can't be added", "Error");
          }
        })
      }
      else {
        HistoryModel.Id = this.updateHistoryValue.id;
        this._portfolio.UpdateHistory(HistoryModel).subscribe(data => {
          console.log('HistoryModel', data)
          if (data) {
            this.toastr.success("History updated successfully", "Success");
            this.editDistributionHistoryShow = false;
            this.AddHistoryForm.reset();
            this.HistoryStartDateError = false;
            this.HistoryEndDateError = false;
            this.HistoryPaymentDateError = false;
            this.getHistory();
          }
          else {
            this.Loader = false;
            this.toastr.error("History can't be updated", "Error");
          }
        })
      }
    }
  }

  //************* Distribution Add Details *******/
  selectAddDistribution() {
    this.PortfolioList = [];
    this.SelectedDistribution = 'AddDistribution';
    this.getInvestorName();
    this.InvestorTypechange('InitialInvestor', '1')
  }

  getInvestorName(){
    this._portfolio.getInvestorNames(this.offeringId).subscribe(data => {
      if(data){
        this.InvestorNameList = data;
        this.InvestorNameList.push({
          'userId': 0,
          'lastName': 'Select User'
        })
        console.log('InvestorNameList', this.InvestorNameList)
      }
    })
  }

  InvestorTypechange(e: any, Id: any) {
    if(e == 'InitialInvestor') {
      this.AllInvestorShow = true;
      this.SingleInvestorShow = false;
      this.InvestorNameError == false;
      this.getcapvalues()
    }
    else {
      if (e.target.value == 1) {
        this.AllInvestorShow = true;
        this.SingleInvestorShow = false;
        this.InvestorNameError == false;
        this.getcapvalues()
      }
      else {
        this.SingleInvestorShow = true;
        this.AllInvestorShow = false;
        this.CalculationTypeError == false;
        this.PortfolioList = []
      }
    }
  }

  getcapvalues() {
    this.PortfolioList = [];
    let offeringId = this.offeringId;
    this._portfolio.getCapTable(offeringId).subscribe(data => {
      if (data != null) {
        this.captablevalues = data;
        this.capValueList = this.captablevalues.capTableInvestments;
      }
    })
  }

  onchangeInvestorName(e: any) {
    this.InvestorId = +e.target.value;
    if (this.InvestorId == 0) {
      this.InvestorNameError = true;
    }
    else {
      this.InvestorNameError = false;
    }
  }

  onchangeCalculationType(e: any) {
    this.CalculationId = +e.target.value;
    if (this.CalculationId == 0) {
      this.CalculationTypeError = true;
    }
    else {
      this.CalculationTypeError = false;
    }
  }

  onDistributionAmount(e: any) {
    if (e.target.value == 0 && e.target.value != '') {
      this.DistributionAmountZeroError = true;
    }
    else {
      this.DistributionAmountZeroError = false;
    }
    if (this.DistributionAmount == '' || this.DistributionAmount == null) {
      this.DistributionAmountError = true;
    }
    else {
      this.DistributionAmountError = false;
    }
  }

  DistributionStartDatechange() {
    if (this.DistributionStartDate == '' || this.DistributionStartDate == null) {
      this.DistributionStartDateError = true;
    }
    else {
      this.DistributionStartDateError = false;
    }
  }

  DistributionEndDatechange() {
    if (this.DistributioEndDate == '' || this.DistributioEndDate == null) {
      this.DistributionEndDateError = true;
    }
    else {
      this.DistributionEndDateError = false;
    }
  }

  DistributionPaymentDatechange() {
    if (this.DistributionPaymentDate == '' || this.DistributionPaymentDate == null) {
      this.DistributionPaymentDateError = true;
    }
    else {
      this.DistributionPaymentDateError = false;
    }
  }

  onDistributionMemo() {
    if (this.DistributionMemo == '' || this.DistributionMemo == null) {
      this.DistributionMemoError = true;
    }
    else {
      this.DistributionMemoError = false;
    }
  }

  CalculateDistribution() {
    this.TotalAmount = 0
    if (this.HistoryId == 0) {
      this.HistoryTypeError = true;
    }
    else {
      this.HistoryTypeError = false;
    }
    if(this.AllInvestorShow == true){
      if (this.CalculationId == 0) {
        this.CalculationTypeError = true;
      }
      else {
        this.CalculationTypeError = false;
      }
    }
    else{
      if (this.InvestorId == 0) {
        this.InvestorNameError = true;
      }
      else {
        this.InvestorNameError = false;
      }
    }
    if (this.DistributionStartDate == '' || this.DistributionStartDate == null) {
      this.DistributionStartDateError = true;
    }
    else {
      this.DistributionStartDateError = false;
    }
    if (this.DistributioEndDate == '' || this.DistributioEndDate == null) {
      this.DistributionEndDateError = true;
    }
    else {
      this.DistributionEndDateError = false;
    }
    if (this.DistributionPaymentDate == '' || this.DistributionPaymentDate == null) {
      this.DistributionPaymentDateError = true;
    }
    else {
      this.DistributionPaymentDateError = false;
    }
    if (this.DistributionMemo == '' || this.DistributionMemo == null) {
      this.DistributionMemoError = true;
    }
    else {
      this.DistributionMemoError = false;
    }
    if (this.DistributionAmount == '' || this.DistributionAmount == null) {
      this.DistributionAmountError = true;
    }
    else {
      this.DistributionAmountError = false;
    }
    if (this.HistoryTypeError == false && this.CalculationTypeError == false &&  this.InvestorNameError == false&& this.DistributionStartDateError == false && this.DistributionEndDateError == false &&
      this.DistributionPaymentDateError == false && this.DistributionMemoError == false && this.DistributionAmountError == false) {
      console.log('capValueList', this.capValueList)
      this.CalculateValueList = [];
      if(this.SingleInvestorShow == true){
        this.PortfolioList = [];
        var distributionTypeName;
        if (this.HistoryId == 1) {
          distributionTypeName = 'Operating Income';
        }
        else if (this.HistoryId == 2) {
          distributionTypeName = 'Retained Earning';
        }
        else if (this.HistoryId == 3) {
          distributionTypeName = 'Return of Capital';
        }
        else if (this.HistoryId == 4) {
          distributionTypeName = 'Gain from Sale';
        }
        else if (this.HistoryId == 5) {
          distributionTypeName = 'Proceeds from Refi';
        }
        else if (this.HistoryId == 6) {
          distributionTypeName = 'Preferred Return';
        }
        else if (this.HistoryId == 7) {
          distributionTypeName = 'Interest';
        }
        const Investorname = this.InvestorNameList.filter((x : any ) => x.userId == this.InvestorId);
        console.log('Investorname', Investorname);
        this.PortfolioList.push({
          'EndDate' : new Date(this.DistributioEndDate),
          'Memo' : this.DistributionMemo,
          'PaymentAmount' : this.DistributionAmount,
          'PaymentDate' : new Date(this.DistributionPaymentDate),
          'StartDate' : new Date(this.DistributionStartDate),
          'Type' : +this.HistoryId,
          'distributionAmount' : Number(this.DistributionAmount),
          'distributionTypeName' : distributionTypeName,
          'investerName' : Investorname[0].lastName + ' ' + Investorname[0].firstName,
          'offeringId' : this.offeringId,
          'profileTypeName' : 'Individual',
          'fundedPercentage' : '100',
          'userId' : Number(this.InvestorId),
          'paymentMethod' : Investorname[0].distributionMethod
        })
      }
      else{
        for (var i = 0; i < this.capValueList.length; i++) {
          this.capValueList[i].PaymentAmount = Number(((this.DistributionAmount * this.capValueList[i].fundedPercentage) / 100).toFixed(2))
          this.capValueList[i].Type = this.HistoryId;
          if (this.HistoryId == 1) {
            this.capValueList[i].distributionTypeName = 'Operating Income';
          }
          else if (this.HistoryId == 2) {
            this.capValueList[i].distributionTypeName = 'Retained Earning';
          }
          else if (this.HistoryId == 3) {
            this.capValueList[i].distributionTypeName = 'Return of Capital';
          }
          else if (this.HistoryId == 4) {
            this.capValueList[i].distributionTypeName = 'Gain from Sale';
          }
          else if (this.HistoryId == 5) {
            this.capValueList[i].distributionTypeName = 'Proceeds from Refi';
          }
          else if (this.HistoryId == 6) {
            this.capValueList[i].distributionTypeName = 'Preferred Return';
          }
          else if (this.HistoryId == 7) {
            this.capValueList[i].distributionTypeName = 'Interest';
          }
          this.capValueList[i].StartDate = new Date(this.DistributionStartDate);
          this.capValueList[i].EndDate = new Date(this.DistributioEndDate);
          this.capValueList[i].PaymentDate = new Date(this.DistributionPaymentDate);
          this.capValueList[i].Memo = this.DistributionMemo;
          this.capValueList[i].distributionAmount = Number(this.DistributionAmount);
          this.PortfolioList.push(this.capValueList[i]);
          for (var i = 0; i < this.PortfolioList.length; i++){
              this.TotalAmount += this.PortfolioList[i].PaymentAmount
          }
        }
        console.log('sum', this.TotalAmount)
      }
      console.log('CalculateValueList', this.PortfolioList)
    }

  }

  editAddDistribution(value: any) {
    console.log('editAddDistribution', value);
    this.addDistributionId = value.id;
    this.editDistributionHistoryShow = true;
    this.AddHistoryForm.patchValue({
      HistoryStartDate: (new Date(value.StartDate)).toISOString().substring(0, 10), 
      HistoryEndDate: (new Date(value.EndDate)).toISOString().substring(0, 10), 
      HistoryPaymentDate: (new Date(value.PaymentDate)).toISOString().substring(0, 10),
      HistoryMemo: value.Memo,
      DistributionAmount: value.distributionAmount,
      Amount: value.PaymentAmount
    })
    this.HistoryId = value.Type;
  }

  onUpdateDistributionForm(Id: any) {
    for (var i = 0; i < this.PortfolioList.length; i++) {
      if (this.PortfolioList[i].id == Id) {
        this.PortfolioList[i].Type = this.HistoryId;
        if (this.HistoryId == 1) {
          this.PortfolioList[i].distributionTypeName = 'Operating Income';
        }
        else if (this.HistoryId == 2) {
          this.PortfolioList[i].distributionTypeName = 'Retained Earning';
        }
        else if (this.HistoryId == 3) {
          this.PortfolioList[i].distributionTypeName = 'Return of Capital';
        }
        else if (this.HistoryId == 4) {
          this.PortfolioList[i].distributionTypeName = 'Gain from Sale';
        }
        else if (this.HistoryId == 5) {
          this.PortfolioList[i].distributionTypeName = 'Proceeds from Refi';
        }
        else if (this.HistoryId == 6) {
          this.PortfolioList[i].distributionTypeName = 'Preferred Return';
        }
        else if (this.HistoryId == 7) {
          this.PortfolioList[i].distributionTypeName = 'Interest';
        }
        this.PortfolioList[i].StartDate = new Date(this.AddHistoryForm.value.HistoryStartDate);
        this.PortfolioList[i].EndDate = new Date(this.AddHistoryForm.value.HistoryEndDate);
        this.PortfolioList[i].PaymentDate = new Date(this.AddHistoryForm.value.HistoryPaymentDate);
        this.PortfolioList[i].Memo = this.AddHistoryForm.value.HistoryMemo;
        this.PortfolioList[i].PaymentAmount = Number(this.AddHistoryForm.value.Amount);
      }
      console.log('onUpdateDistributionForm', this.PortfolioList)
      this.editDistributionHistoryShow = false;
    }
  }

  SaveDistribution() {
    let addDistribution: any = {};
    addDistribution.AdminUserId = Number(localStorage.getItem('UserId'));
    addDistribution.OfferingId = this.offeringId;
    addDistribution.Type = +this.HistoryId;
    if(this.AllInvestorShow == true){
      addDistribution.CalculationMethod = +this.CalculationId;
    }
    else{
      addDistribution.InvesterId = +this.InvestorId;
    }
    addDistribution.Amount = +this.DistributionAmount;
    addDistribution.StartDate = new Date(this.DistributionStartDate);
    addDistribution.EndDate = new Date(this.DistributioEndDate);
    addDistribution.PaymentDate = new Date(this.DistributionPaymentDate);
    addDistribution.Memo = this.DistributionMemo;
    addDistribution.Distributions = this.PortfolioList;
    addDistribution.TotalDistributions = Number(this.TotalAmount);
    this._portfolio.addDistribution(addDistribution).subscribe(data => {
      if (data) {
        console.log('addDistribution', data);
        this.PaymentBtnShow = false;
      }
    })
  }

  ProceedPaymentsDistribution(){
    alert('Payment Process')
  }

  //Reservation View by Status
  selectReservationsview() {
    this.Selected = 'Reservationsview';
    this.ShowReservationDetailsView = false;
    this.ShowNewReservationList = true;
    this.getNewReservationSummary();
    this.getNewReservationList();
  }

  selectReservationDetails() {
    this.Selected = 'ReservationDetails';
    this.ShowReservationDetailsView = true;
    this.ShowNewReservationList = false;
  }

  selectUpdatesview() {
    this.Selected = 'Updates';
  }

  ReservationInformation() {
    this.ReservationInformationShow = !this.ReservationInformationShow
  }

  getSubscriptionsList() {
    this.Loader = true;
    this.PortfolioList = [];
    this._portfolio.getSubscription().subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('Subscription', this.PortfolioList)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  // Offering details and Reservation gallery upload

  uploadFile(event: any) {
    this.UploadImage = [];
    this.filesToUpload = [];
    for (var i = 0; i < event.target.files.length; i++) {
      let ext: any;
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == event.target.files[i].name.split('.').pop()) {
          this.filesToUpload.push(event.target.files[i]);
          if (this.filesToUpload.length > 0) {
            this.profileImageUrl = ''
          }
          ext = null;
          ext = event.target.files[i].name.split('.').pop();
          const file = event.target.files[i];
          const reader = new FileReader();
          reader.readAsDataURL(file);
          if (event.target.files[i].type == 'image/jpeg' || event.target.files[i].type == 'image/png' || event.target.files[i].type == 'image/PNG' || event.target.files[i].type == 'image/jpg' || event.target.files[i].type == 'image/JPEG') {
            reader.onload = () => {
              this.UploadImage.push({ profileImageUrl: reader.result, CoverFileType: 'jpeg' });
              console.log('UploadImage', this.UploadImage)
            };
          }
        }
      });
      if (ext == null) {
        this.toastr.error(event.target.files[i].name.split('.').pop() + ' files are not accepted.', 'Error');
      }
    }
  }

  SetDefaultImage(value: any) {
    console.log('SetDefaultImage', value);
    let ImageSet: any = {};
    ImageSet.Id = value.id;
    ImageSet.AdminUserId = value.adminUserId;
    ImageSet.IsDefaultImage = true;
    ImageSet.Active = true;
    this._portfolio.SetDefaultImageAndRemoveImage(ImageSet).subscribe(data => {
      let a: any = {};
      a = data;
      if (a.status == true) {
        console.log('data', data)
        this.UploadImage = [];
        this.toastr.success("Default Image updated", "Success!");
        this.GalleryList = a.gallery;
      }
      else {
        this.UploadImage = [];
        this.toastr.error("Default Image updated failed", "Failed!");
      }
    })
  }

  RemoveImage(value: any) {
    console.log('SetDefaultImage', value);
    let ImageSet: any = {};
    ImageSet.Id = value.id;
    ImageSet.AdminUserId = value.adminUserId;
    ImageSet.IsDefaultImage = false;
    ImageSet.Active = false;
    this._portfolio.SetDefaultImageAndRemoveImage(ImageSet).subscribe(data => {
      let a: any = {};
      a = data;
      if (a.status == true) {
        console.log('data', data)
        this.toastr.success("Image deleted successfully", "Success!");
        this.GalleryList = a.gallery;
      }
      else {
        this.toastr.error("Image deleted failed", "Failed!");
      }
    })
  }

  // Offering Details and Reservation Gallery Expand Save methods

  SaveGallery(Id: any) {
    this.Loader = true;
    if (Id == 1) {
      this.TypeId = this.offeringId;
    }
    else {
      this.TypeId = this.reservationId;
    }
    this.UserId = localStorage.getItem('UserId');
    const GalleryImg = new FormData();
    GalleryImg.append("AdminUserId", this.UserId);
    GalleryImg.append("OfferingId", this.TypeId);
    this.filesToUpload.forEach((item: string | Blob) => {
      GalleryImg.append('Images', item);
    });
    this._portfolio.UpdateGalleryImage(GalleryImg).subscribe(data => {
      let a: any = {};
      a = data;
      if (a.status == true) {
        console.log('data', data)
        this.UploadImage = [];
        this.toastr.success("Image updated successfully", "Success!");
        this.Loader = false;
        this.GalleryList = a.gallery;
      }
      else {
        this.UploadImage = [];
        this.Loader = false;
        this.toastr.error("Image updated failed", "Failed!");
      }
    })
  }

  CancelGallery() {
    this.UploadImage = [];
  }



  // Offering Details and Reservation Summary Expand Save methods
  SaveSummary(Id: any, Type: any) {
    this.Loader = true;
    let SummaryValue: any = {}
    let a: any = {};
    let UserId = localStorage.getItem('UserId');
    SummaryValue.AdminUserId = Number(UserId);
    if (Type == 'offering') {
      SummaryValue.OfferingId = this.offeringId;
    }
    else {
      SummaryValue.OfferingId = this.reservationId;
    }
    SummaryValue.Summary = this.SummaryListValue;
    SummaryValue.Active = true;
    SummaryValue.Status = 1;
    if (Id == 1) {
      this._portfolio.SaveSummary(SummaryValue).subscribe(data => {
        a = data;
        if (a.status == true) {
          this.SummaryList = [];
          this.toastr.success("Summary added successfully", "Success!");
          this.SummaryBtnShow = true;
          this.Loader = false;
          this.SummaryListValue = a.summary;
          this.SummaryList = a.summary;
        }
        else {
          this.Loader = false;
          this.toastr.error("Summary added failed", "Failed!");
        }
      })
    }
    else {
      SummaryValue.Id = this.SummaryList[0].id;
      this._portfolio.UpdateSummary(SummaryValue).subscribe(data => {
        let a: any = {};
        a = data;
        if (a.status == true) {
          this.Loader = false;
          this.toastr.success("Summary updated successfully", "Success!");
          this.SummaryBtnShow = true;
          this.SummaryListValue = a.summary;
        }
        else {
          this.Loader = false;
          this.toastr.error("Summary updated failed", "Failed!");
        }
      })
    }
  }

  onSummaryChange(value: any) {
    if (value.editor.sourceElement.textContent != null || value.editor.sourceElement.textContent != "") {
      this.SummaryBtnShow = false;
    }
  }

  // Offering Details and Reservation Document Expand Save methods

  UploadDocument(Type: any) {
    this.PortfolioDocument = Type;
    this.documentUploadShow = true;
    this.files = [];
    this.filesToUpload = [];
    this.UploadToOfferingFiles = [];
    this.UploadToWelcomeFiles = [];
  }

  uploadFileDocument(event: any, documentType: any) {
    for (var i = 0; i < event.target.files.length; i++) {
      let ext: any;
      this.allowedFileExtensionsDocument.forEach((element: any) => {
        if (element == event.target.files[i].name.split('.').pop()) {
          this.filesToUpload.push(event.target.files[i]);
          ext = null;
          var temp: any = {};
          ext = event.target.files[i].name.split('.').pop();
          const file = event.target.files[i];
          const reader = new FileReader();
          reader.readAsDataURL(file);
          if (documentType == 'Offering') {
            if (this.UploadToOfferingFiles.length >= 0 && this.UploadToOfferingFiles.length <= 9) {
              this.UploadToOfferingFiles.push(event.target.files[i]);
              temp.documentType = documentType;
              temp.name = event.target.files[i].name;
              temp.size = event.target.files[i].size;
              temp.type = event.target.files[i].type;
              this.files.push(temp);
              console.log('UploadToOfferingFiles', this.UploadToOfferingFiles)
            }
          }
          else {
            this.UploadToWelcomeFiles = [];
            this.UploadToWelcomeFiles.push(event.target.files[i]);
            let x = this.files.filter((x: { documentType: any; }) => x.documentType != 'Welcome');
            this.files = x;
            temp.documentType = documentType;
            temp.name = event.target.files[i].name;
            temp.size = event.target.files[i].size;
            temp.type = event.target.files[i].type;
            this.files.push(temp);
            console.log('UploadToWelcomeFiles', this.UploadToWelcomeFiles)
          }
        }
      });
      if (ext == null) {
        this.toastr.error(event.target.files[i].name.split('.').pop() + ' files are not accepted.', 'Error');
      }
    }
  }

  removeDocument(file: any, index: any) {
    var temp: any = Array.from(this.filesToUpload);
    temp = temp.filter((x: any) => x != this.filesToUpload[index]);
    this.filesToUpload = temp;
    this.files = this.files.filter((x: any) => x != file);
  }

  SaveDocuments() {
    this.Loader = true;
    let a: any = {};
    this.UserId = localStorage.getItem('UserId');
    const Document = new FormData();
    Document.append("AdminUserId", this.UserId);
    Document.append("UserId", this.UserId);
    if (this.PortfolioDocument == 'Offering') {
      Document.append("OfferingId", this.offeringId);
    }
    else {
      Document.append("OfferingId", this.reservationId);
    }
    this.filesToUpload.forEach((item: any) => {
      Document.append('Files', item);
    });
    this.UploadToWelcomeFiles.forEach((item: any) => {
      Document.append('WelcomeDocuments', item);
    });
    this.UploadToOfferingFiles.forEach((item: any) => {
      Document.append('OfferingDocuments', item);
    });
    this._portfolio.AddDocument(Document).subscribe(data => {
      a = data;
      if (a.status == true) {
        this.documentUploadShow = false;
        this.DocumentList = [];
        this.toastr.success("Document added successfully", "Success!");
        this.Loader = false;
        this.DocumentList = a.offeringDocuments;
      }
      else {
        this.Loader = false;
        this.toastr.error("Document added failed", "Failed!");
      }
    })
  }

  onDownloadFile(value: any) {
    var a = document.createElement('a');
    a.href = value.filePath;
    a.download = value.name;
    a.click();
  }

  onViewFile(item: any) {
    this.DocumentViewPopup = true;
    this.ViewFile = item;
  }

  onRemoveDocumentShow(value: any, type: any) {
    this.DeleteDocumentPopup = true;
    this.documentValue = value;
    this.documentType = type;
  }

  onRemoveDocument() {
    this.Loader = true;
    let adminuserid = Number(localStorage.getItem('UserId'));
    if (this.documentType == 'offering') {
      var Id = this.offeringId;
    }
    else {
      var Id = this.reservationId;
    }
    let documentid = this.documentValue.id;
    this._portfolio.RemoveDocument(adminuserid, Id, documentid).subscribe(data => {
      let a: any = {};
      a = data;
      if (a.status == true) {
        this.DeleteDocumentPopup = false;
        this.DocumentList = [];
        this.toastr.success("Document deleted successfully", "Success!");
        this.Loader = false;
        this.DocumentList = a.offeringDocuments;
      }
      else {
        this.DeleteDocumentPopup = false;
        this.Loader = false;
        this.toastr.error("Document deleted failed", "Failed!");
      }
    })
  }

  // Add keys for both Offering details and Reservation

  Addkeys() {
    this.addKeyName = '';
    this.addKeyValue = '';
    this.addKeyVisibility = ''
    this.addkeyShow = true;
    this.SavekeyShow = true;
  }

  Addkeyhighlight(value: any) {
    if (value == 'offering') {
      this.keyHignlightList.push({
        'id': 0,
        'name': this.addKeyName,
        'field': this.addKeyName,
        'offeringId': this.offeringId,
        'value': this.addKeyValue,
        'visibility': this.addKeyVisibility ? this.addKeyVisibility : false,
        'active': true
      })
    }
    else {
      this.keyHignlightList.push({
        'id': 0,
        'name': this.addKeyName,
        'field': this.addKeyName,
        'offeringId': this.reservationId,
        'value': this.addKeyValue,
        'visibility': this.addKeyVisibility ? this.addKeyVisibility : false,
        'active': true
      })
    }

    this.addkeyShow = false;
    this.SavekeyShow = false;
    console.log('Savekeys', this.keyHignlightList)
  }

  SaveKeyHighlights(value: any) {
    this.Loader = true;
    let keys: any = {}
    keys.AdminUserId = Number(localStorage.getItem('UserId'));
    if (value == 'offering') {
      keys.OfferingId = this.offeringId;
    }
    else {
      keys.OfferingId = this.reservationId;
    }
    keys.PortfolioKeyHighlights = this.keyHignlightList;
    this._portfolio.UpdateKeys(keys).subscribe(data => {
      let a: any = {};
      a = data;
      console.log('UpdateKeys', data)
      if (a.status == true) {
        this.keyHignlightList = [];
        this.toastr.success("Keys Added successfully", "Success!");
        this.Loader = false;
        this.keyHignlightList = a.portfolioKeyHighlights;
      }
      else {
        this.Loader = false;
        this.toastr.error("Keys Added failed", "Failed!");
      }
    })
  }

  KeyDelete(value: any) {
    this.KeyValueId = value.id;
    this.KeyValue = value.field;
    this.keyDeleteShow = true;
  }

  onRemoveKeys() {
    if (this.keyHignlightList.length > 0) {
      if (this.KeyValueId == 0) {
        this.keyHignlightList = this.keyHignlightList.filter((x: { field: any; }) => x.field != this.KeyValue);
      }
      else {
        for (var i = 0; i < this.keyHignlightList.length; i++) {
          if (this.keyHignlightList[i].id == this.KeyValueId) {
            this.keyHignlightList[i].active = false;
          }
        }
      }
    }
    this.keyDeleteShow = false;
  }

  // Funds Instructions for offering details 

  onWireTransfer(e: any) {
    console.log('onWireTransfer', this.wireTransfer);
    if (e.target.checked == false || this.wireTransfer == false) {
      this.wireTransferReadOnly = true;
    }
    else {
      this.wireTransferReadOnly = false;
    }

  }

  onCheck(e: any) {
    this.byCheckShow = true;
    if (e.target.checked == false || this.byCheck == false) {
      this.byCheckReadOnly = true;
    }
    else {
      this.byCheckReadOnly = false;
    }
  }

  onCustom(e: any) {
    this.customShow = true;
    if (e.target.checked == false || this.custom == false) {
      this.byCustomReadOnly = true;
    }
    else {
      this.byCustomReadOnly = false;
    }
  }

  onSaveFundsInstructions(Id: any) {
    this.Loader = true;
    let funds: any = {};
    funds.Id = Id;
    funds.OfferingId = this.offeringId;
    funds.AdminUserId = Number(localStorage.getItem('UserId'));
    funds.ReceivingBank = this.FundBankName;
    funds.BankAddress = this.FundBankAddress;
    funds.RoutingNumber = this.FundRoutingNumber;
    funds.AccountNumber = this.FundAccountNumber;
    funds.AccountType = Number(this.FundAccountType);
    funds.Beneficiary = this.FundBeneficiaryAccountName;
    funds.BeneficiaryAddress = this.FundBeneficiaryAddress;
    funds.Reference = this.FundReference;
    funds.OtherInstructions = this.FundOtherInstructions;
    funds.MailingAddress = this.FundMailingAddress;
    funds.CheckBenifiary = this.FundMailBeneficiary;
    funds.Memo = this.FundMailMemo;
    funds.CheckOtherInstructions = this.FundMailOtherInstructions;
    funds.Custom = this.customMailValue;
    if (Id == 0) {
      this._portfolio.SaveFunds(funds).subscribe(data => {
        if (data != null) {
          this.FundsList = data;
          if (this.FundsList != null) {
            this.getFundsDetails();
          }
          this.toastr.success("Funds Added successfully", "Success!");
          this.Loader = false;
        }
        else {
          this.toastr.error("Funds Added failed", "Failed!");
          this.Loader = false;
        }
      })
    }
    else {
      this._portfolio.UpdateFunds(funds).subscribe(data => {
        if (data != null) {
          this.FundsList = data;
          if (this.FundsList != null) {
            this.getFundsDetails();
          }
          this.toastr.success("Funds updated successfully", "Success!");
          this.Loader = false;
        }
        else {
          this.toastr.error("Funds updated failed", "Failed!");
          this.Loader = false;
        }
      })
    }

  }

  // Offering Details Add Investment and get Investment List
  onAddInvestment() {
    this.ModalName = 'Add';
    this.addInvestmentShow = true;
    this.UbdatebtnShow = false;
  }

  addInvestmentCancel() {
    this.addInvestmentShow = false;
  }

  addNotes(value: any) {
    console.log('addNotes', value)
    for (var i = 0; i < this.PortfolioList.length; i++) {
      if (this.PortfolioList[i].id == value.id) {
        this.PortfolioList[i].isNotes = true;
      }
    }
  }

  saveNotes(value: any) {
    let notes: any = {}
    notes.AdminUserId = Number(localStorage.getItem('UserId'));
    notes.InvestmentId = value.id;
    notes.Notes = value.notes
    this._portfolio.saveNotes(notes).subscribe(data => {
      if (data) {
        this.getInvestorList();
        this.cancelNotes(value);
      }
    })
  }

  cancelNotes(value: any) {
    for (var i = 0; i < this.PortfolioList.length; i++) {
      if (this.PortfolioList[i].id == value.id) {
        this.PortfolioList[i].isNotes = false;
      }
    }
  }

  onSubmitInvestmentForm() {
    this.Loader = true;
    const InvestmentModel = new FormData();
    this.UserId = Number(localStorage.getItem('UserId'));
    InvestmentModel.append("AdminUserId", this.UserId);
    InvestmentModel.append("UserId", this.InvestmentUserId);
    InvestmentModel.append("OfferingId", this.offeringId);
    InvestmentModel.append("Amount", this.AddInvestmentForm.value.InvestmentAmount);
    InvestmentModel.append("ProfileId", this.InvestmentProfileId);
    InvestmentModel.append("Status", this.InvestmentStatusId);
    InvestmentModel.append("FundsReceivedDate", this.AddInvestmentForm.value.InvestmentFundReceived);
    InvestmentModel.append("DocumenteSignedDate", this.AddInvestmentForm.value.InvestmentDocsSigned);
    this.filesToUpload.forEach((item: any) => {
      InvestmentModel.append('eSignedDocument', item);
    });
    if (this.UbdatebtnShow == false) {
      this._portfolio.SaveInvestment(InvestmentModel).subscribe(data => {
        console.log('SaveInvestment', data)
        if (data) {
          this.toastr.success("Investment added successfully", "Success");
          this.addInvestmentShow = false;
          this.AddInvestmentForm.reset();
          this.InvestmentUserId = 0;
          this.InvestmentProfileId = 0;
          this.InvestmentStatusId = 0;
          this.getInvestorList();
          this.filesToUpload = [];
        }
        else {
          this.Loader = false;
          this.toastr.error("Investment can't be added", "Error");
        }
      })
    }
    else {
      if (this.filesToUpload.length == 0) {
        InvestmentModel.append("eSignedDocumentPath", this.EditInvestmentData.eSignedDocumentPath);
      }
      InvestmentModel.append("Id", this.InvestmentId);
      this._portfolio.UpdateInvestment(InvestmentModel).subscribe(data => {
        console.log('UpdateInvestment', data)
        if (data) {
          this.toastr.success("Investment updated successfully", "Success");
          this.addInvestmentShow = false;
          this.AddInvestmentForm.reset();
          this.InvestmentUserId = 0;
          this.InvestmentProfileId = 0;
          this.InvestmentStatusId = 0;
          this.getInvestorList();
          this.filesToUpload = [];
        }
        else {
          this.Loader = false;
          this.toastr.error("Investment can't be updated", "Error");
        }
      })
    }
  }

  EditInvestment(value: any) {
    this.ModalName = 'Update';
    this.addInvestmentShow = true;
    this.UbdatebtnShow = true;
    this.EditInvestmentData = value;
    this.InvestmentId = value.id;
    this.AddInvestmentForm.patchValue({
      InvestmentAmount: this.EditInvestmentData.amount,
      InvestmentFundReceived: (new Date(this.EditInvestmentData.fundsReceivedDate)).toISOString().substring(0, 10),
      InvestmentDocsSigned: (new Date(this.EditInvestmentData.documenteSignedDate)).toISOString().substring(0, 10),
    })
    this.InvestmentUserId = this.EditInvestmentData.userId;
    if (this.InvestmentUserId != null) {
      this.onchangeInvestmentUser(this.InvestmentUserId);
    }
    this.InvestmentStatusId = this.EditInvestmentData.status;
    console.log('EditInvestment', value)
  }

  DeleteInvestmentConformation(value: any) {
    this.InvestmentDeleteShow = true;
    this.InvestmentDelValue = value;
  }

  DeleteInvestment() {
    let adminUserId = localStorage.getItem('UserId');
    this._portfolio.deleteInvestment(adminUserId, this.InvestmentDelValue.id).subscribe(data => {
      if (data) {
        this.getInvestorList();
        this.toastr.success("Investment deleted successfully", "Success!");
        this.InvestmentDeleteShow = false;

      }
      else {
        this.toastr.error("Investment can't be delete", "Error!");
        this.InvestmentDeleteShow = false;
      }
    })
  }

  getInvestmentUserList() {
    this._portfolio.getUserList().subscribe(data => {
      if (data) {
        this.UserTypeList = data;
        this.UserTypeList.push({
          'id': 0,
          'fullName': 'Select User'
        })
        console.log('UserTypeList', data)
      }
    })
  }

  onchangeInvestmentUser(value: any) {
    let Investorid = Number(value);
    this.profileList = [];
    this._portfolio.getProfileType(Investorid).subscribe(data => {
      if (data) {
        this.profileList = data;
        this.profileList.push({
          'id': 0,
          'name': 'Select Profile'
        })
        console.log('profileList', data)
      }
      if (this.UbdatebtnShow == true) {
        this.InvestmentProfileId = this.EditInvestmentData.profileId;
      }
    })
  }

  getInvestmentStatusList() {
    this._portfolio.getStatusType().subscribe(data => {
      if (data) {
        this.statusList = data;
        this.statusList.push({
          'id': 0,
          'name': 'Select'
        })
        console.log('statusList', data)
      }
    })
  }

  onFileSelectDrag(files: FileHandle[]) {
    this.filesToUpload = [];
    this.files = [];
    for (var i = 0; i < files.length; i++) {
      let ext: any;
      this.allowedFileExtensions.forEach((element: any) => {
        if (element == files[i].file.name.split('.').pop()) {
          this.filesToUpload.push(files[i].file);
          // ext = null;
          // var temp: any = {};
          // ext = files[i].name.split('.').pop();
          // const file = files[i];
          // const reader = new FileReader();
          // reader.readAsDataURL(file);
          //     temp.name = files[i].name;
          //     temp.size = files[i].size;
          //     temp.type = files[i].type;
          //     this.files.push(temp);
        }

      });
    }
  }

  onFilesSelect(event: any) {
    this.filesToUpload = [];
    this.files = [];
    for (var i = 0; i < event.target.files.length; i++) {
      let ext: any;
      this.allowedFileExtensionsDocument.forEach((element: any) => {
        if (element == event.target.files[i].name.split('.').pop()) {
          this.filesToUpload.push(event.target.files[i]);
          ext = null;
          var temp: any = {};
          ext = event.target.files[i].name.split('.').pop();
          const file = event.target.files[i];
          const reader = new FileReader();
          reader.readAsDataURL(file);
          temp.name = event.target.files[i].name;
          temp.size = event.target.files[i].size;
          temp.type = event.target.files[i].type;
          this.files.push(temp);
        }
      });
      if (ext == null) {
        this.toastr.error(event.target.files[i].name.split('.').pop() + ' files are not accepted.', 'Error');
      }
    }
  }

  // Offering Reservation summary and List Get
  getOfferingReservationSummary() {
    this.Loader = true;
    let offeringId = this.offeringId;
    this._portfolio.getOfferingReservationSummary(offeringId).subscribe(data => {
      if (data != null) {
        this.offeringReservationSummary = data;
        console.log('offeringReservationSummary', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  getofferingReservationList() {
    this.Loader = true;
    this.PortfolioList = [];
    let offeringId = this.offeringId;
    this._portfolio.getOfferingReservationList(offeringId).subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('getOfferingReservationList', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  // Reservation Details for Add Reservation 
  onAddReservation() {
    this.ModalName = 'Add';
    this.UbdatebtnShow = false;
    this.addNewReservationShow = true;
  }

  get NR() { return this.AddNewReservationForm.controls }

  addNewReservationCancel() {
    this.addNewReservationShow = false;
  }

  onchangeReservationUser(e: any, Id: any) {
    this.AddNewReservationForm.get('ReservationUser').setValue(e.target.value);
    this.AddNewReservationForm.value.ReservationUser = e.target.value;
    let ReservationUserId = +e.target.value;
    if (ReservationUserId == 0) {
      this.ReservationUserError = true;
    }
    else {
      this.ReservationUserError = false;
      this.onchangeReservationtUser(Id);
    }
  }

  onchangeReservationtUser(value: any) {
    this.profileList = [];
    let Userid = Number(value);
    this._portfolio.getProfileType(Userid).subscribe(data => {
      if (data) {
        this.profileList = data;
        this.profileList.push({
          'id': 0,
          'name': 'Select Profile'
        })
        console.log('profileList', data)
      }
      if (this.UbdatebtnShow == true) {
        this.ReservationProfileId = this.NewReservationValue.profileId;
      }
    })
  }

  onchangeProfileType(e: any) {
    this.AddNewReservationForm.get('ReservationProfileType').setValue(e.target.value);
    this.AddNewReservationForm.value.ReservationProfileType = e.target.value;
    let ReservationProfileId = +e.target.value;
    if (ReservationProfileId == 0) {
      this.ReservationProfileError = true;
    }
    else {
      this.ReservationProfileError = false;
    }
  }

  onReservationAmount(e: any) {
    this.AddNewReservationForm.get('ReservationAmount').setValue(e.target.value);
    if (e.target.value == 0) {
      this.ReservationAmountZeroError = true;
    }
    else {
      this.ReservationAmountZeroError = false;
    }
    if (this.AddNewReservationForm.value.ReservationAmount == '' || this.AddNewReservationForm.value.ReservationAmount == null) {
      this.ReservationAmountError = true;
    }
    else {
      this.ReservationAmountError = false;
    }
  }

  onchangeReservationLevel(e: any) {
    this.AddNewReservationForm.get('ReservationLevel').setValue(e.target.value);
    this.AddNewReservationForm.value.ReservationLevel = e.target.value;
    let ReservationLevelId = +e.target.value;
    if (ReservationLevelId == 0) {
      this.ReservationLevelError = true;
    }
    else {
      this.ReservationLevelError = false;
    }
  }

  onSubmitNewReservationForm() {
    this.Loader = true;
    this.submitted = true;
    if (this.AddNewReservationForm.value.ReservationUser == 0) {
      this.ReservationUserError = true;
      this.Loader = false;
    }
    if (this.AddNewReservationForm.value.ReservationProfileType == 0) {
      this.ReservationProfileError = true;
      this.Loader = false;
    }
    if (this.AddNewReservationForm.value.ReservationLevel == 0) {
      this.ReservationLevelError = true;
      this.Loader = false;
    }
    else if (this.AddNewReservationForm.invalid) {
      this.Loader = false;
      return
    }
    else if (this.ReservationUserError == true || this.ReservationProfileError == true || this.ReservationAmountError == true
      || this.ReservationAmountZeroError == true || this.ReservationLevelError == true) {
      this.Loader = false;
      return
    }
    else {
      let ReservationModel: any = {};
      ReservationModel.AdminUserId = Number(localStorage.getItem('UserId'));
      ReservationModel.ReservationId = this.reservationId;
      ReservationModel.ReservationName = this.AddNewReservationForm.value.NewReservationName;
      ReservationModel.UserId = +this.ReservationUserId;
      ReservationModel.ProfileId = +this.ReservationProfileId;
      ReservationModel.Amount = +this.AddNewReservationForm.value.ReservationAmount;
      ReservationModel.ConfidenceLevel = +this.ReservationLevelId;
      if (this.UbdatebtnShow == false) {
        this._portfolio.SaveNewReservation(ReservationModel).subscribe(data => {
          console.log('SaveNewReservation', data)
          if (data) {
            this.toastr.success("Reservation added successfully", "Success");
            this.addNewReservationShow = false;
            this.AddNewReservationForm.reset();
            this.ReservationUserId = 0;
            this.ReservationProfileId = 0;
            this.ReservationLevelId = 0;
            this.getNewReservationSummary();
            this.getNewReservationList();
          }
          else {
            this.Loader = false;
            this.toastr.error("Reservation can't be added", "Error");
          }
        })
      }
      else {
        ReservationModel.Id = this.ResertvationId;
        this._portfolio.UpdateNewReservation(ReservationModel).subscribe(data => {
          console.log('UpdatePortfolioOffering', data)
          if (data) {
            this.toastr.success("Reservation updated successfully", "Success");
            this.addNewReservationShow = false;
            this.AddNewReservationForm.reset();
            this.ReservationUserId = 0;
            this.ReservationProfileId = 0;
            this.ReservationLevelId = 0;
            this.getNewReservationSummary();
            this.getNewReservationList();
          }
          else {
            this.Loader = false;
            this.toastr.error("Reservation can't be updated", "Error");
          }
        })
      }
    }
  }

  EditNewReservation(value: any) {
    console.log('EditNewReservation', value)
    this.ResertvationId = value.id;
    this.NewReservationValue = value;
    this.UbdatebtnShow = true;
    this.addNewReservationShow = true;
    this.AddNewReservationForm.patchValue({
      NewReservationName: value.investorName,
      ReservationAmount: value.amount,
    })
    this.ReservationUserId = value.userId;
    if (value.userId != null) {
      this.onchangeReservationtUser(this.ReservationUserId);
    }
    this.ReservationProfileId = value.profileId;
    this.ReservationLevelId = value.confidenceLevel;
  }

  getNewReservationSummary() {
    this.Loader = true;
    let reservationId = this.reservationId;
    this._portfolio.getReservationSummary(reservationId).subscribe(data => {
      if (data != null) {
        this.ReservationSummary = data;
        console.log('ReservationSummary', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  getNewReservationList() {
    this.Loader = true;
    this.PortfolioList = [];
    let reservationId = this.reservationId;
    this._portfolio.getReservationList(reservationId).subscribe(data => {
      if (data != null) {
        this.PortfolioList = data;
        console.log('getReservationList', data)
        this.Loader = false;
      }
      else {
        this.Loader = false;
      }
    })
  }

  DeleteConformationReservation(value: any) {
    this.ReservationDeleteShow = true;
    this.ReservationValue = value;
  }

  DeleteReservationItems() {
    let adminUserId = localStorage.getItem('UserId');
    this._portfolio.deleteReservation(adminUserId, this.ReservationValue.id).subscribe(data => {
      if (data) {
        this.getNewReservationSummary();
        this.getNewReservationList();
        this.toastr.success("Reservation deleted successfully", "Success!");
        this.ReservationDeleteShow = false;

      }
      else {
        this.toastr.error("Reservation can't be delete", "Error!");
        this.ReservationDeleteShow = false;
      }
    })
  }

  saveNewReservationNotes(value: any) {
    let notes: any = {}
    notes.AdminUserId = Number(localStorage.getItem('UserId'));
    notes.ReservationId = value.id;
    notes.Notes = value.notes
    this._portfolio.saveNewReservationNotes(notes).subscribe(data => {
      if (data) {
        this.getNewReservationList();
        this.cancelNewReservationNotes(value);
      }
    })
  }

  cancelNewReservationNotes(value: any) {
    for (var i = 0; i < this.PortfolioList.length; i++) {
      if (this.PortfolioList[i].id == value.id) {
        this.PortfolioList[i].isNotes = false;
      }
    }
  }
}
