import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from 'src/shared/animations/routerTransition';
import { Client, Coupon, Pagination } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-coupons',
  templateUrl: './coupons.component.html',
  styleUrls: ['./coupons.component.css'],
  // animations: [appModuleAnimation()]
})
export class CouponsComponent implements OnInit {
  // data
  coupons: Coupon[] = [];
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

    this.getPageData(1);
  }

  getPageData(page: number): void {
    this.loading = true;
    let pageSize = 10;

    this._apiClient.coupons(page, pageSize).subscribe((response) => {
      if (response.coupons !== undefined && response.pagination !== undefined) {
        this.coupons = response.coupons;
        this.pagination = response.pagination;
        this.currentPage = response.pagination.currentPage as number;
        let totalPages = response.pagination.totalPages as number;
        this.totalPageArray = Array.from(new Array(totalPages), (x, i) => i + 1)
      }
      this.loading = false;
    });
  }

  create(): void {
    // todo
  }

  edit(): void {
    // todo
  }
}
