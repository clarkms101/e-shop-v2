import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Cart, Client, QueryCartResponse, SelectionItem, ShoppingProduct } from 'src/shared/api client/service-proxies';
import { CallApiGetShoppingCartInfo } from 'src/shared/store/shopping-cart.action';

@Component({
  selector: 'app-shopping-cart-checkout',
  templateUrl: './shopping-cart-checkout.component.html',
  styleUrls: ['./shopping-cart-checkout.component.css']
})
export class ShoppingCartCheckoutComponent implements OnInit {
  isCollapsed = false;
  carts: Cart[] = [];
  totalAmount: number = 0;
  finalTotalAmount: number = 0;
  // 下拉
  countryList: SelectionItem[] = [];
  cityList: SelectionItem[] = [];
  // 表單資料
  userInfo: UserInfo = new UserInfo;

  constructor(
    private _toastr: ToastrService,
    private _apiClient: Client,
    private _store: Store<{
      shoppingCartInfo: QueryCartResponse
    }>
  ) { }

  ngOnInit(): void {
    // 訂閱購物車資訊
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

    // 下拉資料取得
    this._apiClient.country().subscribe((response) => {
      if (response.success) {
        this.countryList = response.items as SelectionItem[];
      }
    });

    this.resetUserInfo();
  }

  resetUserInfo(): void {
    let defaultCountryId = 1;
    this.userInfo.UserName = '';
    this.userInfo.Email = '';
    this.userInfo.Country = defaultCountryId;
    this.userInfo.City = null;
    this.userInfo.ZipCode = '';
    this.userInfo.Address = '';
    this.setCityListData(defaultCountryId);
  }

  setCityListData(countryId: number): void {
    this._apiClient.city(countryId).subscribe((response) => {
      if (response.success) {
        this.cityList = response.items as SelectionItem[];
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

  nextStep(): void {
    // todo
  }
}

class UserInfo {
  UserName: string | null | undefined;
  Email: string | null | undefined;
  Country: number | null | undefined;
  City: number | null | undefined;
  ZipCode: string | null | undefined;
  Address: string | null | undefined;
}
