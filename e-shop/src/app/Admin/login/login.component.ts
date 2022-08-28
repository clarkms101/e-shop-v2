import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
    private _apiClient: Client,
    private _router: Router
  ) { }

  ngOnInit(): void {

  }

  signin(): void {
    let request = new LoginRequest();
    request.account = this.admin_account;
    request.password = this.admin_password;
    this._apiClient.login(request).subscribe((response) => {
      if (response.success) {
        // 將後端回傳的 JWT 存入 localStorage
        localStorage.setItem("adminJWT", response.token as string);
        this._router.navigate(['admin/products']);
        alert('登入成功!');
      } else {
        // todo 登入錯誤次數過多的鎖定
        console.log(`${request.account} - ${request.password} : ${response.message}`);
      }
    });
  }
}
