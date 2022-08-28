import { DepartmentService } from 'src/app/core/services/department.service';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { UserService } from './../../../../core/services/user.service';
import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { UserForSelectModel } from 'src/app/shared';

@Component({
  selector: 'app-representative-modal',
  templateUrl: './representative-modal.component.html',
})
export class RepresentativeModalComponent implements OnInit {
  @Input() department: IDepartmentModel;
  users: UserForSelectModel[] = [
    {
      id: 0,
      fullName: '---- Chọn cán bộ làm đại diện ----'
    }
  ];
  userSelected = this.users[0].id;
  departmentName: string;

  constructor(
    private toastr: ToastrService,
    public activeModal: NgbActiveModal,
    private userService: UserService,
    private departmentService: DepartmentService
  ) {
  }

  ngOnInit(): void {
    this.loadUser();
    this.departmentName = this.department.name.replace('--- ', '');
  }

  loadUser(): void {
    this.userService.getUserForSelect(-1, this.department.id).subscribe(response => {
      if (response.isSuccess) {
        this.users = this.users.concat(response.result);
        const foundUser = this.users.filter((user) => {
          return user.id === this.department.representativeId;
        });
        this.userSelected = foundUser.length === 0 ? this.users[0].id : foundUser[0].id;
      } else {
        this.toastr.error(response.message);
      }
    });
  }

  closeModal(isSubmit: boolean): void {
    this.activeModal.close({
      submit: isSubmit,
    });
  }

  onSubmit(): void {
    if (this.userSelected === 0) {
      this.toastr.warning('Bạn phải chọn ít nhất một người đại diện!');
      return;
    }

    const payload = {
      departmentId: this.department.id,
      representativeId: this.userSelected
    };

    this.departmentService
    .updateRepresentative(payload)
    .subscribe((response) => {
      if (response.isSuccess) {
        this.toastr.success('Bạn đã cập nhật người đại diện thành công!');
        this.closeModal(true);
      } else {
        this.toastr.error(
          'Cập nhật thất bại, vui lòng thử lại!'
        );
      }
    });
  }
}
