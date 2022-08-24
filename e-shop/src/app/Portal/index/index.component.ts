import { Component, OnInit } from '@angular/core';
import { Client, LoginRequest } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  Jwt: string | undefined = '';
  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    let request = new LoginRequest();
    request.account = '';
    request.password = '';
    this._apiClient.login(request).subscribe((response) => {
      if (response.success) {
        this.Jwt = response.token;
        console.log(this.Jwt);
      } else {
        console.log(response.message);
      }
    });
  }
}
