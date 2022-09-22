import { DateHelper } from 'src/shared/helpers/DateHelper';
import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from 'src/shared/animations/routerTransition';
import { Client, OrderInfo, Pagination, QueryOrdersRequest, SelectionItem } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  // animations: [appModuleAnimation()]
})
export class OrdersComponent implements OnInit {
  // data
  orders: OrderInfo[] = [];
  paymentMethodList: SelectionItem[] = [];
  // query
  query_startDate: string = '';
  query_endDate: string = '';
  query_paymentMethod: number | null | undefined = null;
  // page
  pagination: Pagination = new Pagination();
  totalPageArray: number[] = [];
  currentPage: number = 0;
  loading = false;

  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    let token = localStorage.getItem("adminJWT") as string;
    this._apiClient.setAuthToken(token);

    this._apiClient.paymentMethod().subscribe((response) => {
      this.paymentMethodList = response.items as SelectionItem[];
    });

    this.getPageData(1);
  }

  getPageData(page: number): void {
    this.loading = true;
    let request = new QueryOrdersRequest();
    request.page = page;
    request.pageSize = 10;
    if (this.query_paymentMethod !== null && this.query_paymentMethod !== undefined) {
      request.paymentMethod = this.query_paymentMethod;
    }
    if (this.query_startDate !== '') {
      // yyyy-MM-dd
      request.startDate = DateHelper.getDateString(new Date(this.query_startDate));
    }
    if (this.query_endDate !== '') {
      // yyyy-MM-dd
      request.endDate = DateHelper.getDateString(new Date(this.query_endDate));
    }

    this._apiClient.orders(request).subscribe((response) => {
      if (response.orderInfos !== undefined && response.pagination !== undefined) {
        this.orders = response.orderInfos;
        this.pagination = response.pagination;
        this.currentPage = response.pagination.currentPage as number;
        let totalPages = response.pagination.totalPages as number;
        this.totalPageArray = Array.from(new Array(totalPages), (x, i) => i + 1)
      }
      this.loading = false;
    });
  }
}
