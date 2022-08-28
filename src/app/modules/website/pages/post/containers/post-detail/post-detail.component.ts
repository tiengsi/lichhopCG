import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { PostModel } from '../../../../../../shared/models/post.model';
import { PostService } from '../../../../../../core/services/post.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.scss'],
})
export class PostDetailComponent implements OnInit, OnDestroy {
  post: PostModel;
  // Subscriptions
  private subscriptions: Subscription[] = [];
  constructor(
    private postService: PostService,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadPostDefault();
  }

  loadPostDefault(): void {
    const sbPost = this.activatedRoute.params.subscribe((params) => {
      this.postService.getPostById(params.id).subscribe((response) => {
        if (response.isSuccess) {
          this.post = response.result;
        }
      });
    });

    this.subscriptions.push(sbPost);
  }
}
