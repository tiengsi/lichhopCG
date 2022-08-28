import {
  BaseResponseModel,
  PageResponseModel,
  PostModel,
  QueryParamsModel,
} from './../../../../../shared/';
import { SubheaderService, PostService, ToastService, LayoutUtilsService } from './../../../../../core';
import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  OnDestroy,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
})
export class PostListComponent implements OnInit, OnDestroy {
  posts: PostModel[];
  totalCount = 0;
  count = 0;
  page = 1;
  pageSize = 10;
  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;
  sortEvent: SortEvent = {
    direction: 'desc',
    column: 'CreatedDate'
  };

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private subheaderService: SubheaderService,
    private postService: PostService,
    private toastService: ToastService,
    private layoutUtilsService: LayoutUtilsService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.initBreadCrumbs();
    this.loadPost();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý tin', page: `admin/post` },
    ]);
  }

  loadPost(): void {
    const filter = {
      filter: this.searchInput.nativeElement.value,
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column,
      this.page,
      this.pageSize
    );

    const subScription = this.postService.getAll(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.posts = response.result.items;
        this.totalCount = response.result.totalCount;
        this.count = response.result.count;
      } else {
        this.toastService.showError(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

  editPost(id: number): void {
    this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
  }

  deletePost(item: PostModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.postService.deletePost(item.postId).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.showSuccess('Xóa thành công!');
            this.loadPost();
          } else {
            this.toastService.showError('Xóa thất bại, xin vui lòng thử lại!');
          }
        });
      }
    });
  }

  onSort(event: SortEvent): void {
    if (!event || !event.column || !event.direction) { return; }

    this.sortEvent = event;
    this.loadPost();
  }
}
