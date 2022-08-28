import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BrandnameRoutingModule } from './brandname-routing.module';
import { ViettelComponent } from './viettel/viettel.component';
import { VnptComponent } from './vnpt/vnpt.component';
import { VnptAddComponent } from './vnpt-add/vnpt-add.component';
import { ViettelAddComponent } from './viettel-add/viettel-add.component';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrandnameRoutingModule
  ]
})
export class BrandnameModule { }
