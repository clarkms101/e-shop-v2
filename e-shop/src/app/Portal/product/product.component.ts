import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { CartDetail, Client, CreateCartDetailRequest, Product } from 'src/shared/api client/service-proxies';
import { CallApiGetShoppingCartInfo } from 'src/shared/store/shopping-cart.action';
import { ShoppingCart_RootState } from 'src/shared/store/shopping-cart.reducer';

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
    private _toastr: ToastrService,
    private _store: Store<{ rootState: ShoppingCart_RootState }>
  ) { }

  ngOnInit(): void {
    this._activeRoute.queryParams.subscribe(queryParams => {
      if (queryParams['productId'] !== undefined) {
        this.productId = queryParams['productId'] as number;
        // 檢查是否為數字
        if (isNaN(this.productId)) {
          this._router.navigate(['portal/products']);
        } else {
          this.getProductData(this.productId);
        }
      } else {
        this._router.navigate(['portal/products']);
      }
    });
  }

  getProductData(productId: number) {
    this.loading = true;
    this._apiClient.productGET(productId).subscribe((response) => {
      if (response.success) {
        this.product = response.product as Product;
        // todo 先固定上限10單位
        // let maxQty = new Array(this.product.num as number);
        let maxQty = new Array(10);
        this.QtyArray = Array.from(maxQty, (x, i) => i + 1)
      } else {
        this._router.navigate(['portal/products']);
        this._toastr.error('查無此商品!');
      }
      this.loading = false;
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
          this._store.dispatch(CallApiGetShoppingCartInfo());
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
