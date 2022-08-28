// Angular
import { Injectable } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { DeleteEntityDialogComponent } from '../../shared/components/content/delete-entity-dialog/delete-entity-dialog.component';
export enum MessageType {
  Create,
  Read,
  Update,
  Delete,
}

@Injectable()
export class LayoutUtilsService {
  protected modalRef: NgbModalRef;
  constructor(protected modalService: NgbModal) {}

  public deleteElement(): NgbModalRef {
    return (this.modalRef = this.modalService.open(
      DeleteEntityDialogComponent,
      { ariaLabelledBy: 'modal-basic-title' }
    ));
  }
}
