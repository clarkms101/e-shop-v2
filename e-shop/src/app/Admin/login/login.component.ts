import { Component, OnInit } from '@angular/core';
import { Client, LoginRequest } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  admin_account: string = '';
  admin_password: string = '';

  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {

  }

  signin(): void {
    let request = new LoginRequest();
    request.account = '';
    request.password = '';
    this._apiClient.login(request).subscribe((response) => {
      if (response.success) {
        // 將後端回傳的 JWT 存入 localStorage
        localStorage.setItem("adminJWT", response.token as string);
        // todo 登入完成並導頁
      } else {
        console.log(response.message);
      }
    });
  }
}
