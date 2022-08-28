import { Injectable, TemplateRef } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  toasts: any[] = [];

  show(textOrTpl: string | TemplateRef<any>, type: string, options: any = {}): void {
    this.toasts.push({ textOrTpl, type, ...options });
  }

  showSuccess(textOrTpl: string | TemplateRef<any>, options: any = {}): void {
    options = { classname: 'bg-success text-light', delay: 4000 };
    this.show(textOrTpl, 'success', options);
  }

  showError(textOrTpl: string | TemplateRef<any>, options: any = {}): void {
    options = { classname: 'bg-danger text-light', delay: 4000 };
    this.show(textOrTpl, 'error', options);
  }

  showInfo(textOrTpl: string | TemplateRef<any>, options: any = {}): void {
    options = { classname: 'bg-info text-light', delay: 4000 };
    this.show(textOrTpl, 'info', options);
  }

  showWarning(textOrTpl: string | TemplateRef<any>, options: any = {}): void {
    options = { classname: 'bg-warning text-light', delay: 4000 };
    this.show(textOrTpl, 'warning', options);
  }

  remove(toast): void {
    this.toasts = this.toasts.filter((t) => t !== toast);
  }
}
