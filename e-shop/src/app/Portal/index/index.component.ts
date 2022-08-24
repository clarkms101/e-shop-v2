import { Component, OnInit } from '@angular/core';
import { Client } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  Jwt: string = '';
  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
  }
}
