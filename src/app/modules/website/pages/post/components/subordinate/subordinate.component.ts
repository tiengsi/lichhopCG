import { Component, ContentChild, EventEmitter, HostListener, Input, Output, TemplateRef } from '@angular/core';
import { chain } from 'lodash';
import { ITreeDepartmentOfficer } from 'src/app/shared/models/TreeDepartmentOfficer';

@Component({
  selector: 'app-list-subordinate',
  templateUrl: './subordinate.component.html',
  styleUrls: ['./subordinate.component.scss']
})
export class SubordinateComponent {
  @ContentChild(TemplateRef) templateRef: TemplateRef<any>;
  @Input() departments: ITreeDepartmentOfficer[] = [];
  showList: {
    [key: number]: boolean;
  } = {};
  expandedSubordinate: {
    [key: number]: boolean;
  } = {};
  d = '';

  constructor() { }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(): void {
    this.drawSvg();
  }

  toggleDisplay(departmentId: number): void {
    this.showList[departmentId] = !this.showList[departmentId];
    setTimeout(() => this.drawSvg(), 200);
  }

  toggleSubordinate(departmentId: number, value: boolean): void {
    this.expandedSubordinate[departmentId] = value;
    setTimeout(() => this.drawSvg(), 200);
  }

  drawSvg(): void {
    const length = document.getElementsByClassName('svg-mark-point').length;
    let listCoordinates: any[] = [];
    for (let index = 0; index < length; index++) {
      const y = document.getElementsByClassName('svg-mark-point')[index].getBoundingClientRect().top;
      const x = document.getElementsByClassName('svg-mark-point')[index].getBoundingClientRect().left;
      listCoordinates.push({ x, y });
    }
    listCoordinates = chain(listCoordinates.sort((a, b) => a.x - b.x))
      .groupBy('x')
      .map((coords, x) => ({ coords, x }))
      .value();
    listCoordinates.forEach(({ x, coords }, index) => {
      if (index === 0) {
        this.d = `M ${+x + 10} ${coords[0].y + 20} `;
        return;
      }
      const prevX = +listCoordinates[index - 1].x;
      this.d += `L ${prevX + 10} ${coords[0].y + 10} M ${prevX + 10} ${coords[0].y + 10} L ${+x} ${coords[0].y + 10} `;
      coords.forEach(({ y }: { y: number }, i: number) => {
        if (i === 0) {
          this.d += `M ${+x + 10} ${y + 20} `;
          return;
        }
        this.d += `L ${+x + 10} ${y} `;
        if (i <= coords.length - 2) {
          this.d += `M ${+x + 10} ${y + 20}`;
        }
      });
    });
  }
}
