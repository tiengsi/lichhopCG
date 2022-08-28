import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { SubheaderService } from 'src/app/core';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { BrandNameModel, ViettelBrandNameModel, VNPTBrandNameModel } from 'src/app/shared/models/brand-name.model';

@Component({
  selector: 'app-brandname',
  templateUrl: './brandname.component.html',  
})
export class BrandnameComponent implements OnInit, OnDestroy {

  viettelBrandNames: ViettelBrandNameModel[];
  vnptBrandNames: VNPTBrandNameModel[];
  addItem: BrandNameModel = new BrandNameModel();
  type = '';
  afterType = '';
  isAddViettel = true;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : -1;
  renderable = [false, false];

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;
  private subscriptions: Subscription[] = [];

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '15rem',
    minHeight: '5rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [[]],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText',
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
  };
  constructor(
    private subheaderService: SubheaderService,
    private brandnameService: BrandNameService,
    private toastService: ToastrService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }
  
  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];        
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {  
    // this.loadPermissions();      
    // this.initBreadCrumbs();
    // this.loadViettel();
    // this.loadVNPT();    
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Cấu hình BrandName', page: `admin/brandname` },
    ]);
  }
  
  loadViettel(): void {
    const loadViettelBrandName = this.brandnameService.GetViettelBrandName().subscribe(response => {
      if (response.isSuccess) {
        this.viettelBrandNames = response.result as ViettelBrandNameModel[];
        try{
          if(this.viettelBrandNames.length == 0) this.type = 'vnpt'
          else {
            this.type = 'viettel';
            this.afterType = 'viettel';    
          }
        }
        catch{

        }
        this.renderable[0] = true;         
      }
    });    
    this.subscriptions.push(loadViettelBrandName);    
  }

  loadVNPT():void {
    const loadVNPTBrandName = this.brandnameService.GetVNPTBrandName().subscribe(response => {
      if (response.isSuccess) {
        this.vnptBrandNames = response.result as VNPTBrandNameModel[];
        try{
          if(this.vnptBrandNames.length == 0) this.type = 'viettel'
          else {
            this.type = 'vnpt';
            this.afterType = 'vnpt';
          }
        }
        catch{}
        this.renderable[1] = true;        
      }
    });
    this.subscriptions.push(loadVNPTBrandName);    
  }

  onChangeBrand(e: any): void {
    this.afterType = e.target?.value;
    if(e.target?.value == 'viettel') this.isAddViettel = true;
    else this.isAddViettel = false;
    console.log([this.type, this.afterType]);
  }

  updateVnpt(): void{
    const updateSub = this.brandnameService.UpdateVNPTBrandName(this.vnptBrandNames[0]).subscribe(response => {
      if (response.isSuccess) {
        this.toastService.success('Cập nhật thành công!');
      } else {
        this.toastService.error('Cập nhật thất bại!');
      }
    });
    this.subscriptions.push(updateSub);
  }
  updateViettel(): void{
    const updateSub = this.brandnameService.UpdateViettelBrandName(this.viettelBrandNames[0]).subscribe(response => {
      if (response.isSuccess) {
        this.toastService.success('Cập nhật thành công!');
      } else {
        this.toastService.error('Cập nhật thất bại!');
      }
    });
    this.subscriptions.push(updateSub);
  }

  vnptToViettel(): void{
    var viettelToAdd = new ViettelBrandNameModel();
    viettelToAdd.brandNameId = 0;
    viettelToAdd.branchName = this.vnptBrandNames[0].branchName;
    viettelToAdd.apiUser = this.vnptBrandNames[0].apiUser;
    viettelToAdd.contractType = this.vnptBrandNames[0].contractType;
    viettelToAdd.apiPass = this.vnptBrandNames[0].apiPass;
    viettelToAdd.cpCode = (<HTMLInputElement>document.getElementById('vnptCode')).value;
    viettelToAdd.isActive = true;
    viettelToAdd.userName = 'a';
    viettelToAdd.apiLink = 'a';
    viettelToAdd.organizeId = this.organizeId;
    const updateSub = this.brandnameService.CreateViettelBrandName(viettelToAdd).subscribe(response => {
      if (response.isSuccess) {
        this.toastService.success('Cập nhật thành công!');
      } else {
        this.toastService.error('Cập nhật thất bại!');
      }
    });
    this.subscriptions.push(updateSub);
  }

  viettelToVNPT(): void{
    var vnptToAdd = new VNPTBrandNameModel();
    vnptToAdd.brandNameId = 0;
    vnptToAdd.branchName = this.viettelBrandNames[0].branchName;
    vnptToAdd.apiUser = this.viettelBrandNames[0].apiUser;
    vnptToAdd.apiPass = this.viettelBrandNames[0].apiPass;
    vnptToAdd.contractType = this.viettelBrandNames[0].contractType;
    vnptToAdd.phoneNumber = (<HTMLInputElement>document.getElementById('viettelPhone')).value;
    vnptToAdd.isActive = true;
    vnptToAdd.userName = 'a'; 
    vnptToAdd.apiLink = 'a';
    vnptToAdd.organizeId = this.organizeId;
    const updateSub = this.brandnameService.CreateVNPTBrandName(vnptToAdd).subscribe(response => {
      if (response.isSuccess) {
        this.toastService.success('Cập nhật thành công!');
      } else {
        this.toastService.error('Cập nhật thất bại!');
      }
    });
    this.subscriptions.push(updateSub);
  }

  onSubmit(): void{   
    console.log(this.type) ;
    if(this.type == 'vnpt'){
      if(this.afterType == 'vnpt'){
        this.updateVnpt();
      } else{
        this.vnptToViettel();
      }
    }else if(this.type == 'viettel'){
      if(this.afterType == 'vnpt'){
        this.viettelToVNPT();
      } else{
        this.updateViettel();
      }
    }   
    else if(this.type==''){
      if(this.isAddViettel){
        var viettelToAdd = new ViettelBrandNameModel();
        viettelToAdd.brandNameId = 0;
        viettelToAdd.branchName = this.addItem.branchName;
        viettelToAdd.apiUser = this.addItem.apiUser;
        viettelToAdd.contractType = this.addItem.contractType;
        viettelToAdd.apiPass = this.addItem.apiPass;
        viettelToAdd.cpCode = (<HTMLInputElement>document.getElementById('addCode')).value;
        viettelToAdd.isActive = true;
        viettelToAdd.userName = 'a';
        viettelToAdd.apiLink = 'a';
        viettelToAdd.organizeId = this.organizeId;
        const updateSub = this.brandnameService.CreateViettelBrandName(viettelToAdd).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.success('Cập nhật thành công!');
          } else {
            this.toastService.error('Cập nhật thất bại!');
          }
        });
        this.subscriptions.push(updateSub);
      }
    }else{
      var vnptToAdd = new VNPTBrandNameModel();
      vnptToAdd.brandNameId = 0;
      vnptToAdd.branchName = this.addItem.branchName;
      vnptToAdd.apiUser = this.addItem.apiUser;
      vnptToAdd.apiPass = this.addItem.apiPass;
      vnptToAdd.contractType = this.addItem.contractType;
      vnptToAdd.phoneNumber = (<HTMLInputElement>document.getElementById('addPhone')).value;
      vnptToAdd.isActive = true;
      vnptToAdd.userName = 'a'; 
      vnptToAdd.apiLink = 'a';
      vnptToAdd.organizeId = this.organizeId;
      const updateSub = this.brandnameService.CreateVNPTBrandName(vnptToAdd).subscribe(response => {
        if (response.isSuccess) {
          this.toastService.success('Cập nhật thành công!');
        } else {
          this.toastService.error('Cập nhật thất bại!');
        }
      });
      this.subscriptions.push(updateSub);
    } 
  }
  
}
