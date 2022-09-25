import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
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
  queryCategoryName: string = '';
  // page
  pagination: Pagination = new Pagination();
  totalPageArray: number[] = [];
  currentPage: number = 0;
  loading = false;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private _apiClient: Client
  ) {
    this.router.events.subscribe((event: any) => {
      // NavigationStart
      if (event instanceof NavigationStart) {
        // QueryString參數 尚未有值
      }

      // NavigationEnd
      if (event instanceof NavigationEnd) {
        // console.log(event);

        // 取得QueryString參數
        this.activeRoute.queryParams
          .subscribe(params => {
            // console.log(params);

            if (params['category'] !== undefined) {
              this.queryCategoryName = params['category'];
              // console.log(this.queryCategoryName);

            } else {
              let defaultCategory = '金牌';
              this.queryCategoryName = defaultCategory;
            }
          });

        this.getPageData(1);
      }

      // NavigationError
      if (event instanceof NavigationError) {
        // console.log(event.error);
      }
    });
  }

  ngOnInit(): void {

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
