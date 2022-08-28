import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from 'src/app/core';
import { CategoryModel, ECategoryType } from 'src/app/shared';

@Component({
  selector: 'app-menu-top',
  templateUrl: './menu-top.component.html',
  styleUrls: ['./menu-top.component.scss'],
})
export class MenuTopComponent implements OnInit, OnDestroy {
  categories: CategoryModel[];
  categoryCodeFromUrl: string;
  // Subscriptions
  private subscriptions: Subscription[] = [];
  constructor(
    private categoryService: CategoryService,
    private activatedRoute: ActivatedRoute) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      this.categoryCodeFromUrl = params.categoryCode;
      this.loadCategories();
    });
    this.subscriptions.push(routeSubscription);
  }

  loadCategories(): void {
    const loadSub = this.categoryService.getAllByMenu('menu-top').subscribe((response) => {
      if (response.isSuccess) {
        this.categories = response.result;

        // tslint:disable-next-line:prefer-for-of
        for (let index = 0; index < this.categories.length; index++) {
          const element = this.categories[index];
          if (element.categoryCode === this.categoryCodeFromUrl) {
            element.isActive = true;
          } else {
            element.isActive = false;
          }
        }
      }
    });
    this.subscriptions.push(loadSub);
  }

  generateCategoryLink(category: CategoryModel): string {
    let cateUrl = null;
    switch (category.typeCode) {
      case ECategoryType.Article:
        cateUrl = `/bai-viet/${category.categoryCode}`;
        break;
      case ECategoryType.Post:
        cateUrl = `/tin/danh-sach/${category.categoryCode}`;
        break;
      case ECategoryType.Schedule:
        cateUrl = `/lich-hop/${category.categoryCode}`;
        break;
      default:
        cateUrl = category.link;
    }

    return cateUrl;
  }
}
