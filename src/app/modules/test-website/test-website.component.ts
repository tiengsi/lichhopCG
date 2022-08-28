import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-test-website',
  templateUrl: './test-website.component.html',
  styleUrls: ['./test-website.component.scss']
})
export class TestWebsiteComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));    
    console.log(window.location.href.includes('scheduler/shared-documents'));
    if(!window.location.href.includes('scheduler/shared-documents'))
    {
      if(userInfo == null || userInfo == undefined || userInfo.userId == undefined){
        this.router.navigate(['/']);
      }
    }
  }

}
