import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',  
})
export class OrganizationComponent implements OnInit {

  userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));     
  isSuperAdmin = this.userInfo.roles.includes('SuperAdmin');
  constructor(private router: Router,) { }

  ngOnInit(): void {
    if(!this.isSuperAdmin){
      //error/404
      this.router.navigate(['/error/404']);
    }
  }

}
