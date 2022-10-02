import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Cart, Client, QueryCartResponse, ShoppingProduct } from 'src/shared/api client/service-proxies';
import { CallApiGetShoppingCarInfo } from 'src/shared/store/shopping-car.action';

@Component({
  selector: 'app-portal-navbar',
  templateUrl: './portal-navbar.component.html',
  styleUrls: ['./portal-navbar.component.css']
})
export class PortalNavbarComponent implements OnInit {
  shoppingCarItemInfoList: Cart[] = [];
  shoppingCarItemCount: number = 0

  constructor(
    private _toastr: ToastrService,
    private _apiClient: Client,
    private _store: Store<{
      shoppingCarInfo: QueryCartResponse
    }>
  ) { }

  ngOnInit(): void {
    this._store.select('shoppingCarInfo').subscribe(data => {
      if (data.success) {
        this.shoppingCarItemCount = data.carts?.length as number;
        this.shoppingCarItemInfoList = data.carts as Cart[];
      } else {
        this.shoppingCarItemCount = 0;
        this.shoppingCarItemInfoList = [];
      }
    });
  }

  removeFromCart(cartDetailId: string | undefined): void {
    this._apiClient.cartDELETE(cartDetailId as string).subscribe((response) => {
      if (response.success) {
        this._toastr.success('商品已從購物車移除!');
      } else {
        this._toastr.error('移除商品失敗!');
      }
      this._store.dispatch(CallApiGetShoppingCarInfo());
    });
  }

  getProduct(product: any): ShoppingProduct {
    return product as ShoppingProduct;
  }

  getShoppingItemPrice(carItem: Cart): number {
    let product = carItem.product as ShoppingProduct;
    let itemPrice = (carItem.qty as number) * (product.price as number)
    return itemPrice;
  }
}
