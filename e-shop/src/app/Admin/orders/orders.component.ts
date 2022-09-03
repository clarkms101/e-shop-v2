import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from 'src/shared/animations/routerTransition';
import { Client, OrderInfo, Pagination, QueryOrdersRequest } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  animations: [appModuleAnimation()]
})
export class OrdersComponent implements OnInit {
  // data
  orders: OrderInfo[] = [];
  // page
  pagination: Pagination = new Pagination();
  totalPageArray: number[] = [];
  currentPage: number = 0;

  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    let token = localStorage.getItem("adminJWT") as string;
    this._apiClient.setAuthToken(token);
    this.getPageData(1);
  }

  getPageData(page: number): void {
    let request = new QueryOrdersRequest();
    request.page = page;
    request.pageSize = 10;

    this._apiClient.orders(request).subscribe((response) => {
      if (response.orderInfos !== undefined && response.pagination !== undefined) {
        this.orders = response.orderInfos;
        this.pagination = response.pagination;
        this.currentPage = response.pagination.currentPage as number;
        let totalPages = response.pagination.totalPages as number;
        this.totalPageArray = Array.from(new Array(totalPages), (x, i) => i + 1)
      }
    });
  }
}
