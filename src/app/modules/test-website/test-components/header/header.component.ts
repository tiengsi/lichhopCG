import { Component, OnChanges, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  
  imgPath = 'assets/img/logo_yenbai.jpeg';
  navBarText = 'Quản lý cuộc họp';

  isHidden = true;
  displayName = '';
  isAdmin = true;
  loadBlock = false;
  checkLogin(){

    try{
      var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));    
      if(userInfo != null){
        this.isHidden = false;
        this.displayName = userInfo.displayName;
      }     
      var user = JSON.parse(localStorage.getItem('app-schedule-token'));
      //console.log(user.roles.filter(x => ['Admin', 'SuperAdmin', 'Normal-Admin'].includes(x)).length);
      if(user.roles.filter(x => ['Admin', 'SuperAdmin', 'Scheduler'].includes(x)).length == 0){
        this.isAdmin = false;      
      }
    }
    catch{
      this.isAdmin = false;
    }
    
    this.loadBlock = true;
  }
  toggleBox(){
    var t = document.getElementById('logout');
    t.style.display = t.style.display == 'none' ? 'block' : 'none';
  }
  logOut(){
    localStorage.clear();
    sessionStorage.clear();
    this.router.navigateByUrl('/');
  }

  goToAdminPage(){
    this.router.navigateByUrl('/admin');
  }

  changePass(){
    this.router.navigateByUrl('/scheduler/change-password');
  }

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    
   }  

  ngOnInit(): void {
    this.checkLogin();
  }

}
