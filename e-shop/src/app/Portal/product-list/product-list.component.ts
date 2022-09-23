import { Component, OnInit } from '@angular/core';
import { Client, Pagination, Product, QueryProductsRequest } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  // data
  products: Product[] = [];
  // query
  queryProductName: string = '';
  queryCategoryName: string = ''; // todo 外部傳入產品分類
  // page
  pagination: Pagination = new Pagination();
  totalPageArray: number[] = [];
  currentPage: number = 0;
  loading = false;

  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    this.getPageData(1);
  }

  getPageData(page: number): void {
    this.loading = true;
    let request = new QueryProductsRequest();
    request.page = page;
    request.pageSize = 6;
    request.productName = this.queryProductName;
    request.category = this.queryCategoryName;

    this._apiClient.products(request).subscribe((response) => {
      if (response.products !== undefined && response.pagination !== undefined) {
        this.products = response.products;
        this.pagination = response.pagination;
        this.currentPage = response.pagination.currentPage as number;
        let totalPages = response.pagination.totalPages as number;
        this.totalPageArray = Array.from(new Array(totalPages), (x, i) => i + 1)
      }
      this.loading = false;
    });
  }
}
