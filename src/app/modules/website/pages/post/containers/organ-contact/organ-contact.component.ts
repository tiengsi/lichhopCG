import { Component, OnDestroy, OnInit } from '@angular/core';
import { DepartmentService } from 'src/app/core/services/department.service';
import { ITreeDepartmentOfficer } from 'src/app/shared/models/TreeDepartmentOfficer';
import { UserService } from '../../../../../../core/services/user.service';
import { IUserForListModel } from '../../../../../../shared/models/user-for-list.model';
import { GetOfficerRequest } from '../../../../../../shared/models/user.model';

@Component({
  selector: 'app-organ-contact',
  templateUrl: './organ-contact.component.html',
  styleUrls: ['./organ-contact.component.scss']
})
export class OrganContactComponent implements OnInit, OnDestroy {
  keyFilter = '';
  departmentOfficers: ITreeDepartmentOfficer[] = [];
  isLoading = true;
  sortField = 'Name';

  constructor(private departmentService: DepartmentService
  ) { }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.getAllOfficer();
  }

  getAllOfficer(): void {
    const payLoad: GetOfficerRequest = {
      filter: this.keyFilter,
      sortField: this.sortField
    };

    this.departmentService.getDepartmentOfficer(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.departmentOfficers = res.result;
        this.isLoading = false;
      }
    });
  }
}
