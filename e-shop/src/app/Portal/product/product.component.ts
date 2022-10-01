import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Client, Product } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  product: Product = new Product;
  productId: number | undefined;
  loading = false;
  selectQty: number = 0;
  QtyArray: number[] = [];

  constructor(
    private _router: Router,
    private _activeRoute: ActivatedRoute,
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    this._activeRoute.queryParams.subscribe(queryParams => {
      if (queryParams['productId'] !== undefined) {
        this.productId = queryParams['productId'] as number;
        this.getProductData(this.productId);
      } else {
        this._router.navigate(['portal/products']);
      }
    });
  }

  getProductData(productId: number) {
    this.loading = true;
    this._apiClient.productGET(productId).subscribe((response) => {
      this.product = response.product as Product;
      this.loading = false;

      // todo 先固定上限10單位
      // let maxQty = new Array(this.product.num as number);
      let maxQty = new Array(10);
      this.QtyArray = Array.from(maxQty, (x, i) => i + 1)
    });
  }
}
