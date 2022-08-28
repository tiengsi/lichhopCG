import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  links = [
    {value: 'http://www.quan1.hochiminhcity.gov.vn/', text: 'Quận 1'},
    {value: 'http://www.quan2.hochiminhcity.gov.vn/', text: 'Quận 2'},
    {value: 'http://www.quan3.hochiminhcity.gov.vn/', text: 'Quận 3'},
    {value: 'http://www.quan4.hochiminhcity.gov.vn/', text: 'Quận 4'},
    {value: 'http://www.quan5.hochiminhcity.gov.vn/', text: 'Quận 5'},
    {value: 'http://www.quan6.hochiminhcity.gov.vn/', text: 'Quận 6'},
    {value: 'http://www.quan7.hochiminhcity.gov.vn/', text: 'Quận 7'},
    {value: 'http://www.quan8.hochiminhcity.gov.vn/', text: 'Quận 8'},
    {value: 'http://www.quan9.hochiminhcity.gov.vn/', text: 'Quận 9'},
    {value: 'http://www.quan10.hochiminhcity.gov.vn/', text: 'Quận 10'},
    {value: 'http://www.quan11.hochiminhcity.gov.vn/', text: 'Quận 11'},
    {value: 'http://www.quan12.hochiminhcity.gov.vn/', text: 'Quận 12'},
    {value: 'http://www.quan13.hochiminhcity.gov.vn/', text: 'Quận 13'},
    {value: 'www.binhtan.hochiminhcity.gov.vn', text: 'Quận Bình Tân'},
    {value: 'http://www.binhthanh.hochiminhcity.gov.vn/binhthanh/trang-chu', text: 'Quận Bình Thạnh'},
    {value: 'http://www.binhthanh.hochiminhcity.gov.vn/binhthanh/trang-chu', text: 'Quận Bình Thạnh'},
    {value: 'www.govap.hochiminhcity.gov.vn', text: 'Quận Gò Vấp'},
    {value: 'http://www.phunhuan.hochiminhcity.gov.vn/Pages/default.aspx', text: 'Quận Phú Nhuận'},
    {value: 'www.tanbinh.hochiminhcity.gov.vn', text: 'Quận Tân Bình'},
    {value: 'http://www.tanphu.hochiminhcity.gov.vn/Default.aspx', text: 'Quận Tân Phú'},
    {value: 'https://tpthuduc.hochiminhcity.gov.vn/', text: 'Thành Phố Thủ Đức'},
    {value: 'http://binhchanh.hochiminhcity.gov.vn/Pages/default.aspx', text: 'Huyện Bình Chánh'},
    {value: 'http://www.cangio.hochiminhcity.gov.vn/', text: 'Huyện Cần Giờ'},
    {value: 'http://www.cuchi.hochiminhcity.gov.vn/default.aspx', text: 'Huyện Củ Chi'},
    {value: 'http://www.hocmon.hochiminhcity.gov.vn/', text: 'Huyện Hóc Môn'},
    {value: 'http://www.nhabe.hochiminhcity.gov.vn/default.aspx', text: 'Huyện Nhà Bè'},
  ];

  constructor() { }

  goToLink(){
    try {      
      var link = <HTMLSelectElement>document.getElementById('ddlLinks');
      if(link.value != '-1'){
        window.open(link.value, '_blank');
      }      
    } catch (error) {
            
    }
  }
  ngOnInit(): void {
  }

}
