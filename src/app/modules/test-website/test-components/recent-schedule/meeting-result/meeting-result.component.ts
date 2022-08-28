import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { AuthService, ScheduleService, ToastService } from 'src/app/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthModel } from 'src/app/shared/models/auth.model';
import { ScheduledResultDocumentModel } from '../../../models/resultDocument.model';
import { ScheduledResultReportModel } from '../../../models/resultReport.model';
import { LoadingBarService } from '@ngx-loading-bar/core';
import { ToastrService } from 'ngx-toastr';
import file from 'src/assets/plugins/formvalidation/src/js/validators/file';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-meeting-result',
  templateUrl: './meeting-result.component.html',
  styleUrls: ['./meeting-result.component.scss']
})
export class MeetingResultComponent implements OnInit {

  @Input() meetingInfo: any;
  //meetingInfo = {
  //   title: 'HỘI NGHỊ BAN THƯỜNG VỤ TỈNH ỦY TRIỂN KHAI NHIỆM VỤ CHUYỂN ĐỔI SỐ TỈNH YÊN BÁI NĂM 2022',
  //   timeStart: '07:30 11/03/2022',
  //   timeEnd: '11:30 11/03/2022',
  // }  
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;

  UIPermissions: any = {};  
  documentBtn = true;
  reportBtn = true;

  typeOfUpload = 0;
  docs = [
    {
      name: 'Tài liệu 1',
      modified: new Date(),
      status: 4,   
      path: ''   
    },
    {
      name: 'Tài liệu 2',
      modified: new Date(),
      status: 4,   
      path: ''   
    },
    {
      name: 'Tài liệu 3',
      modified: new Date(),
      status: 4,      
      path: ''
    },
    {
      name: 'Tài liệu 4',
      modified: new Date(),
      status: 4,  
      path: ''    
    },
  ];
  reports = [
    {
      user: {
        name: 'User 1'
      },
      time: new Date(),
      summary: 'Report summary',
      filePath: 'filePath'
    },
    {
      user: {
        name: 'User 2'
      },
      time: new Date(),
      summary: 'Report summary',
      filePath: 'filePath2'
    },
    {
      user: {
        name: 'User 3'
      },
      time: new Date(),
      summary: 'Report summary',
      filePath: 'filePath3'
    },
    {
      user: {
        name: 'User 4'
      },
      time: new Date(),
      summary: 'Report summary',
      filePath: 'filePath4'
    },
  ];

  documentModel: ScheduledResultDocumentModel = new ScheduledResultDocumentModel();
  reportModel: ScheduledResultReportModel = new ScheduledResultReportModel();

  files: File[] = [];
  uploader: FileUploader;
  loader = this.loadingBar.useRef();

  getData(){
    var userInfo = JSON.parse(sessionStorage.getItem('userInfo'));  
    var getDocuments = this.scheduleService.getScheduledResultDocuments(this.meetingInfo.id).toPromise();
    var getReports= this.scheduleService.getScheduledResultReports(this.meetingInfo.id).toPromise();
    Promise.all([getDocuments, getReports]).then((res) =>{
      if(res[0].isSuccess){
        this.docs = [];
        res[0].result.forEach(x => {          
          this.docs.push({
            name: x.title,
            modified: new Date(x.updatedDate),
            status: x.status == true ? 4 : 0,
            path: x.path
          })
        })
        console.log(this.docs);
      }
      else console.log(res[0].message)
      if(res[1].isSuccess){
        this.reports = [];
        res[1].result.forEach(x => {
          this.reports.push({
            user: {
              name: x.user.fullName
            },
            time: new Date(x.reportTime),
            summary: x.reportContent,
            filePath: x.path
          })
        })
        console.log(this.reports);
      }
      else console.log(res[1].message)
    })
  }

  constructor(
    private scheduleService: ScheduleService, 
    private authService: AuthService,
    private toastService: ToastrService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList,
    private loadingBar: LoadingBarService) 
    { }

  hideAndShow(classString){
    var elem = document.querySelectorAll(classString)
    elem.forEach((e: any) => e.style.display = e.style.display != 'none' ? 'none' : 'flex');
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);                
        this.documentBtn = this.UIPermissions[this.permissionList.WEBSITE_SCHEDULE_RESULT_DOCUMENT];          
        this.reportBtn = this.UIPermissions[this.permissionList.WEBSITE_SCHEDULE_RESULT_REPORT];          
      }
      else {
        this.toastService.error(response.message);
      }
    });
    // this.subscriptions.push(subScription);
  }
  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/attachmentFile/v2`,
      authToken: 'Bearer ' + tokenObj.accessToken,
      isHTML5: true,
      allowedFileType: ['image', 'pdf', 'doc', 'docx'],
      removeAfterUpload: false,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024, // 10 MB
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onBuildItemForm = (fileItem, form) => {
      console.log(fileItem.file.rawFile);
      form.append('fileUpload', fileItem.file.rawFile);
      form.append('organizeId', this.organizeId);
      form.append('userId', this.userId);
      form.append('mode', 'Schedule')
    };  

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);        
        if (res.result.getData) {
          // && this.files.find(r => r.name === item?._file?.name)
          if(this.typeOfUpload == 1)
          {
            this.documentModel.path = res.result.getData;
            this.documentModel.title = item?.file.name;
            this.documentModel.status = true;
            this.documentModel.scheduleId = this.meetingInfo.id;          
          }else{
            this.reportModel.path = res.result.getData;            
            this.reportModel.reportContent = item?.file.name;
            this.reportModel.scheduleId = this.meetingInfo.id;
            this.reportModel.reportTime = new Date();            
            this.reportModel.userId = this.userId;
          }
        }

      }
    };

    this.uploader.onCompleteAll = () => {
      if(this.typeOfUpload == 1){
        //console.log(this.documentModel);
        this.scheduleService.createResultDocuments(this.documentModel).subscribe(res =>{
          if(res.isSuccess) {
            console.log('Upload thành công');
            this.files = [];
            this.docs.push({
              name: this.documentModel.title,
              modified: new Date(),
              status: 4,   
              path: this.documentModel.path  
            })
          }
          else console.log('Upload không thành công');
        });
      }else{
        console.log(this.reportModel)
        this.scheduleService.createResultReports(this.reportModel).subscribe(res =>{
          if(res.isSuccess) {
            console.log('Upload thành công');
            this.files = [];
            this.reports.push({
              user: {
                name: JSON.parse(sessionStorage.getItem('userInfo')).name
              },
              time: new Date(),
              summary: this.reportModel.reportContent,
              filePath: this.reportModel.path
            })
          }
          else console.log('Upload không thành công');
        });
      }
      this.loader.complete();            
    };

    this.uploader._onErrorItem = (item, response, status, header) => {
      this.toastService.error('Đã xảy ra lỗi trong quá tình tải file!');
      this.loader.complete();
    };
  }
  onFileSelected(event: EventEmitter<File[]>): any {
    const file: File = event[0];
    this.files.push(file);
  }
  
  upFile(type: number){
    if(this.files.length != 0)
    {
      this.typeOfUpload = type; 
      this.loader.start();
      this.uploader.uploadAll();        
      // this.files = [];
      // this.uploader.cancelAll(); 
      (<HTMLInputElement>document.getElementById('file-upload')).value = '';
    }
  }
  
  ngOnInit(): void {
    this.loadPermissions();
    this.getData();
    this.initializeUploader();    
  }

}
