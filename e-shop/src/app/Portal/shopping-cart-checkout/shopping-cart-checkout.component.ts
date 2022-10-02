import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Cart, Client, QueryCartResponse, ShoppingProduct } from 'src/shared/api client/service-proxies';
import { CallApiGetShoppingCartInfo } from 'src/shared/store/shopping-cart.action';

@Component({
  selector: 'app-shopping-cart-checkout',
  templateUrl: './shopping-cart-checkout.component.html',
  styleUrls: ['./shopping-cart-checkout.component.css']
})
export class ShoppingCartCheckoutComponent implements OnInit {
  isCollapsed = true;
  carts: Cart[] = [];
  totalAmount: number = 0;
  finalTotalAmount: number = 0;

  constructor(
    private _toastr: ToastrService,
    private _apiClient: Client,
    private _store: Store<{
      shoppingCartInfo: QueryCartResponse
    }>
  ) { }

  ngOnInit(): void {
    this._store.select('shoppingCartInfo').subscribe(data => {
      if (data.success) {
        this.carts = data.carts as Cart[];
        this.totalAmount = data.totalAmount as number;
        this.finalTotalAmount = data.finalTotalAmount as number;
      } else {
        this.carts = [];
        this.totalAmount = 0;
        this.finalTotalAmount = 0;
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
      this._store.dispatch(CallApiGetShoppingCartInfo());
    });
  }

  getProduct(product: any): ShoppingProduct {
    return product as ShoppingProduct;
  }

  getShoppingItemPrice(cartItem: Cart): number {
    let product = cartItem.product as ShoppingProduct;
    let itemPrice = (cartItem.qty as number) * (product.price as number)
    return itemPrice;
  }
}
