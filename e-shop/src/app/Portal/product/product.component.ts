import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CartDetail, Client, CreateCartDetailRequest, Product } from 'src/shared/api client/service-proxies';

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
    private _apiClient: Client,
    private _toastr: ToastrService
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

  addToCart() {
    if (this.selectQty !== 0) {
      let request = new CreateCartDetailRequest();
      request.cartDetail = new CartDetail();
      request.cartDetail.productId = this.product.productId;
      request.cartDetail.qty = this.selectQty;

      this._apiClient.cartPOST(request).subscribe((response) => {
        if (response.success) {
          this._toastr.success(`${this.product.title} x ${this.selectQty} ${this.product.unit} 已加入購物車!`);
          // todo 更新navbar購物車的數量
        } else {
          this._toastr.error(response.message);
        }
      });
    }
    else {
      this._toastr.warning('請選擇購買數量!');
    }
  }
}
