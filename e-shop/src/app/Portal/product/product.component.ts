import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationError, Router } from '@angular/router';
import { finalize } from 'rxjs';
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
    private activeRoute: ActivatedRoute,
    private _apiClient: Client
  ) {
    this._router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        this.activeRoute.queryParams
          .subscribe(params => {
            if (params['productId'] !== undefined) {
              this.productId = params['productId'] as number;
              this.getProductData(this.productId);
            } else {
              this._router.navigate(['portal/products']);
            }
          });
      }
      if (event instanceof NavigationError) {
        console.log(event.error);
      }
    });
  }

  ngOnInit(): void {
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
