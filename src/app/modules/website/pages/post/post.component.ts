import { PostService } from './../../../../core/services/post.service';
import { PostModel } from './../../../../shared/models/post.model';
import { QueryParamsModel } from './../../../../shared/models/query-params.model';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
})
export class PostComponent implements OnInit, OnDestroy {
  totalCount = 0;
  count = 0;
  page = 1;
  pageSize = 10;
  posts: PostModel[] = [];
  categoryName = 'Tin';
  categoryCode: string;
  // Subscriptions
  private subscriptions: Subscription[] = [];
  constructor(
    private activatedRoute: ActivatedRoute,
    private postService: PostService
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadPostByCategory();
  }

  loadPostByCategory(): void {
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      this.categoryCode = params.categoryCode;
      const filter = {
        categoryCode: params.categoryCode,
      };
      const queryParams = new QueryParamsModel(
        filter,
        'Asc',
        'UserName',
        this.page,
        this.pageSize
      );

      this.postService.getAll(queryParams).subscribe((response) => {
        if (response.isSuccess) {
          this.posts = response.result.items;
          this.totalCount = response.result.totalCount;
          this.count = response.result.count;
          if (this.posts.length > 0) {
            this.categoryName = this.posts[0].categoryName;
          }
        }
      });
    });
    this.subscriptions.push(routeSubscription);
  }
}
