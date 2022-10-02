import { environment } from './../environments/environment';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Client } from 'src/shared/api client/service-proxies';
import { API_BASE_URL } from 'src/shared/api client/service-proxies';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Admin/login/login.component';
import { FormsModule } from '@angular/forms';
import { ProductsComponent } from './Admin/products/products.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
// ngrx
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
// ngx-bootstrap
import { defineLocale } from 'ngx-bootstrap/chronos';
import { zhCnLocale } from 'ngx-bootstrap/locale'
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { DatepickerModule, BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { SidebarComponent } from './Admin/layout/sidebar/sidebar.component';
import { NavbarComponent } from './Admin/layout/navbar/navbar.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { ProductDetailComponent } from './Admin/products/product-detail/product-detail.component';
import { ModalHeaderComponent } from './Common/modal/modal-header/modal-header.component';
import { ModalFooterComponent } from './Common/modal/modal-footer/modal-footer.component';
import { AuthGuardService } from 'src/shared/services/auth-guard.service';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from 'ngx-loading';
import { CouponsComponent } from './Admin/coupons/coupons.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { CouponDetailComponent } from './Admin/coupons/coupon-detail/coupon-detail.component';
import { ProductComponent } from './Portal/product/product.component';
import { FooterComponent } from './Portal/layout/footer/footer.component';
import { MenuComponent } from './Portal/layout/menu/menu.component';
import { PortalNavbarComponent } from './Portal/layout/portal-navbar/portal-navbar.component';
import { Page404Component } from './Common/page/page404/page404.component';
import { ProductListComponent } from './Portal/product-list/product-list.component';
import { PortalLayoutComponent } from './Portal/layout/portal-layout/portal-layout.component';
import { AdminLayoutComponent } from './Admin/layout/admin-layout/admin-layout.component';
import { ShoppingCart_RootEffects } from 'src/shared/store/shopping-cart.effects';
import { shoppingCartItemCountReducer } from 'src/shared/store/shopping-cart.reducer';

defineLocale('zh-cn', zhCnLocale);

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProductsComponent,
    SidebarComponent,
    NavbarComponent,
    OrdersComponent,
    ProductDetailComponent,
    ModalHeaderComponent,
    ModalFooterComponent,
    CouponsComponent,
    CouponDetailComponent,
    ProductComponent,
    FooterComponent,
    MenuComponent,
    PortalNavbarComponent,
    Page404Component,
    ProductListComponent,
    PortalLayoutComponent,
    AdminLayoutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientJsonpModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({
      timeOut: 1000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.pulse,
      backdropBackgroundColour: "rgba(255,255,255,0.3)",
      backdropBorderRadius: "10px",
      fullScreenBackdrop: true,
      primaryColour: "#D0D0D0",
      secondaryColour: "#D0D0D0",
      tertiaryColour: "#D0D0D0",
    }),
    SweetAlert2Module.forRoot(),
    // ngx-bootstrap
    ModalModule.forChild(),
    BsDropdownModule,
    CollapseModule,
    TabsModule,
    BsDatepickerModule.forRoot(),
    DatepickerModule.forRoot(),
    // ngrx
    EffectsModule.forRoot([ShoppingCart_RootEffects]),
    StoreModule.forRoot({
      shoppingCartInfo: shoppingCartItemCountReducer
    }),
  ],
  providers: [
    Client,
    { provide: API_BASE_URL, useValue: environment.apiUrl },
    AuthGuardService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private bsLocaleService: BsLocaleService) {
    this.bsLocaleService.use('zh-cn');
  }
}
