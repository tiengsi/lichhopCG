// Angular
import { Component, Input, OnInit } from '@angular/core';
// RxJS
import { Observable } from 'rxjs';
// NGRX
import { select, Store } from '@ngrx/store';
// State
import { AppState } from '../../../../core/ngrx-store/reducers';
import { currentUser, Logout } from '../../../../core/ngrx-store/auth';
import { AuthModel } from './../../../models/auth.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
})
export class UserProfileComponent implements OnInit {
  // Public properties
  user$: Observable<AuthModel>;

  @Input() userDropdownStyle = 'light';
  @Input() avatar = true;
  @Input() greeting = true;
  @Input() badge: boolean;
  @Input() icon: boolean;

  /**
   * Component constructor
   *
   * @param store: Store<AppState>
   */
  constructor(private store: Store<AppState>, private router: Router) {
  }

  /**
   * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
   */

  /**
   * On init
   */
  ngOnInit(): void {
    this.user$ = this.store.pipe(select(currentUser));
  }

  redirectClick(url: string){
    this.router.navigateByUrl(url);
  }

  /**
   * Log out
   */
  logout() {
    localStorage.clear();
    // sessionStorage.clear();
    // this.router.navigateByUrl('/');
    this.store.dispatch(new Logout());
  }
}
