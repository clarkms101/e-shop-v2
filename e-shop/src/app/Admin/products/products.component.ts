import { Component, OnInit } from '@angular/core';
import { Client, Product, QueryProductsRequest } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  keyword: string = '';
  isLoading: boolean = false;
  products: Product[] = [];

  constructor(
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    let request = new QueryProductsRequest();
    request.page = 1;
    request.pageSize = 10;
    this._apiClient.products(request).subscribe((response) => {
      if (response.products !== undefined) {
        this.products = response.products;
      }
    });
  }

  getDataPage(page: number): void {

  }
}
