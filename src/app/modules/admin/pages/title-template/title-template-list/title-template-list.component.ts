import { ITitleTemplate } from './../../../../../shared/models/title-template.model';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LayoutUtilsService, SubheaderService } from 'src/app/core';
import { TitleTemplateService } from 'src/app/core/services/title-template.service';
import { QueryParamsModel } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddEditTitleTemplateComponent } from '../../../components/add-edit-title-template/add-edit-title-template.component';

@Component({
  selector: 'app-title-template-list',
  templateUrl: './title-template-list.component.html',
})
export class TitleTemplateListComponent implements OnInit {
  items: ITitleTemplate[] = [];
  totalCount = 0;
  count = 0;
  page = 1;
  pageSize = 10;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;

  constructor(
    private toastService: ToastrService,
    private subheaderService: SubheaderService,
    private titleTemplateService: TitleTemplateService,
    private modalService: NgbModal,
    private layoutUtilsService: LayoutUtilsService,
  ) { }

  ngOnInit(): void {
    this.initBreadCrumbs();
    this.loadTitleTemplate();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý mẫu tiêu đề', page: `admin/title-template` },
    ]);
  }

  loadTitleTemplate(): void {
    const queryParams = new QueryParamsModel(
      {},
      '',
      '',
      this.page,
      this.pageSize
    );

    this.titleTemplateService.getAllScheduleTitleTemplateByOrganizeId(queryParams, this.organizeId).subscribe((response) => {
      if (response.isSuccess) {
        this.items = response.result.items;
        this.totalCount = response.result.totalCount;
        this.count = response.result.count;
      }
      else {
        this.toastService.error(response.message);
      }
    });
  }

  addEditModal(item?: ITitleTemplate): void {
    const modalRef = this.modalService.open(AddEditTitleTemplateComponent);
    modalRef.componentInstance.titleTemplate = item;
    modalRef.result.then((result) => {
      if (result.submit) {
        this.loadTitleTemplate();
      }
    });
  }

  delete(item: ITitleTemplate): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.titleTemplateService.delete(item.id).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.success('Xóa mẫu tiêu đề thành công!');
            this.loadTitleTemplate();
          } else {
            this.toastService.error('Xóa thất bại, xin vui lòng thử lại!');
          }
        });
      }
    });
  }
}
