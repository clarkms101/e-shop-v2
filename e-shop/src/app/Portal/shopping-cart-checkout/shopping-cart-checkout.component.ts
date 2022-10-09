import { OrderForm } from './../../../shared/api client/service-proxies';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { add } from 'ngx-bootstrap/chronos';
import { ToastrService } from 'ngx-toastr';
import { Cart, CartCoupon, Client, CreateOrderRequest, QueryCartResponse, SelectionItem, ShoppingProduct, UpdateCartCouponRequest } from 'src/shared/api client/service-proxies';
import { CallApiGetShoppingCartInfo } from 'src/shared/store/shopping-cart.action';
import { Router } from '@angular/router';
import { JwtHelper } from 'src/shared/helpers/JwtHelper';

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
  couponCode: string = '';
  // 下拉
  countryList: SelectionItem[] = [];
  cityList: SelectionItem[] = [];
  // 表單資料
  userInfo: UserInfo = new UserInfo;
  creditCardInfo: CreditCardInfo = new CreditCardInfo;
  // step
  currentStep: Step = Step.UserInfo;
  // enum
  Step = Step;
  PaymentType = PaymentType;

  constructor(
    private _router: Router,
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
    this.resetCreditCardInfo();
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

  resetCreditCardInfo(): void {
    this.creditCardInfo.CardUserName = 'Clark';
    this.creditCardInfo.CardNumber = '1234 1234 1234 1234';
    this.creditCardInfo.CardCvc = '12 / 27';
    this.creditCardInfo.CardExpiration = '678';
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

  useCouponCode(): void {
    let request = new UpdateCartCouponRequest();
    let coupon = new CartCoupon();
    coupon.couponCode = this.couponCode;
    request.coupon = coupon;

    if (this.couponCode === '') {
      this._toastr.warning('請輸入優惠碼!');
      return;
    }

    this._apiClient.useCoupon(request).subscribe((response) => {
      if (response.success) {
        this._toastr.success(response.message);
        this._store.dispatch(CallApiGetShoppingCartInfo());
        this.couponCode = '';
      }
      else {
        this._toastr.warning(response.message);
      }
    });
  }

  toUserInfoStep(): void {
    this.scrollToTop();
    this.currentStep = Step.UserInfo;
  }

  toPaymentStep(): void {
    this.scrollToTop();
    this.currentStep = Step.Payment;
  }

  // 送出訂單
  createOrder(paymentType: PaymentType): void {
    let systemUserId = JwtHelper.parseJwt().JwtKeyAdminSystemUserId as number;

    let request = new CreateOrderRequest();
    request.orderForm = new OrderForm();
    let address = this.getUserFullAddress();
    request.systemUserId = systemUserId;
    request.orderForm.userName = this.userInfo.UserName as string;
    request.orderForm.address = address;
    request.orderForm.email = this.userInfo.Email as string;
    request.orderForm.tel = this.userInfo.Tel as string;
    request.orderForm.message = this.userInfo.Message as string;
    request.orderForm.paymentMethod = paymentType as string;

    this._apiClient.orderPOST(request).subscribe((response) => {
      if (response.success) {
        this._toastr.success(response.message);
        this._store.dispatch(CallApiGetShoppingCartInfo());
        this._router.navigate(['portal/order-checkout'], { queryParams: { serialNumber: response.serialNumber } });
      }
      else {
        this._toastr.error(response.message);
      }
    });
  }

  scrollToTop(): void {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'auto'
    });
  }

  getCountryName(countryValue: number | null | undefined): string {
    var countryItem = this.countryList.find(function (item, index, array) {
      return item.value === countryValue;
    });
    return countryItem?.text as string;
  }

  getCityName(cityValue: number | null | undefined): string {
    var cityItem = this.cityList.find(function (item, index, array) {
      return item.value === cityValue;
    });
    return cityItem?.text as string;
  }

  getUserFullAddress(): string {
    let country = this.getCountryName(this.userInfo.Country);
    let city = this.getCityName(this.userInfo.City);
    let zipCode = this.userInfo.ZipCode;
    let address = this.userInfo.Address;
    return `${country} ${city} ${zipCode} ${address}`;
  }
}

class UserInfo {
  UserName: string | null | undefined;
  Email: string | null | undefined;
  Country: number | null | undefined;
  City: number | null | undefined;
  ZipCode: string | null | undefined;
  Address: string | null | undefined;
  Tel: string | null | undefined;
  Message: string | null | undefined;
}

class CreditCardInfo {
  CardUserName: string | null | undefined;
  CardNumber: string | null | undefined;
  CardExpiration: string | null | undefined;
  CardCvc: string | null | undefined;
}

export enum Step {
  UserInfo = 'UserInfo',
  Payment = 'Payment',
}

export enum PaymentType {
  CashOnDelivery = 'CashOnDelivery',
  CreditCardPayment = 'CreditCardPayment'
}
