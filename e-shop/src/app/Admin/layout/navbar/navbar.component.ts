import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Client, LogoutRequest } from 'src/shared/api client/service-proxies';
import { JwtHelper } from 'src/shared/helpers/JwtHelper';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(
    private _toastr: ToastrService,
    private _router: Router,
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    let token = localStorage.getItem("adminJWT") as string;
    this._apiClient.setAuthToken(token);
  }

  signout(): void {
    // 取得登入時的 api access key
    let apiAccessKey = JwtHelper.parseJwt().JwtKeyApiAccessKey;
    let request = new LogoutRequest();
    request.apiAccessKey = apiAccessKey;
    this._apiClient.logout(request).subscribe((response) => {
      if (response.success) {
        this._toastr.success(`${response.message}`);
        this._router.navigate(['admin/login']);
      } else {
        this._toastr.warning(`${response.message}`);
        this._router.navigate(['admin/login']);
      }
    });
  }
}
