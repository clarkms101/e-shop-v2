import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from 'src/shared/services/auth-guard.service';
import { CouponsComponent } from './Admin/coupons/coupons.component';
import { LoginComponent } from './Admin/login/login.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { ProductsComponent } from './Admin/products/products.component';
import { Page404Component } from './Common/page/page404/page404.component';
import { ProductComponent } from './Portal/product/product.component';
import { ProductListComponent } from './Portal/product-list/product-list.component';
import { PortalLayoutComponent } from './Portal/layout/portal-layout/portal-layout.component';
import { AdminLayoutComponent } from './Admin/layout/admin-layout/admin-layout.component';
import { ShoppingCartCheckoutComponent } from './Portal/shopping-cart-checkout/shopping-cart-checkout.component';
import { OrderCheckoutComponent } from './Portal/order-checkout/order-checkout.component';

const routes: Routes = [
  // layout 頁面(導至預設頁面)
  { path: 'portal', redirectTo: 'portal/products' },
  { path: 'admin', redirectTo: 'admin/login' },
  // portal 頁面
  {
    path: 'portal', component: PortalLayoutComponent, children: [
      { path: 'products', component: ProductListComponent },
      { path: 'product', component: ProductComponent },
      { path: 'shopping-cart-checkout', component: ShoppingCartCheckoutComponent },
      { path: 'order-checkout', component: OrderCheckoutComponent },
    ]
  },
  // admin 頁面
  { path: 'admin/login', component: LoginComponent },
  {
    path: 'admin', component: AdminLayoutComponent, children: [
      { path: 'products', component: ProductsComponent, canActivate: [AuthGuardService] },
      { path: 'orders', component: OrdersComponent, canActivate: [AuthGuardService] },
      { path: 'coupons', component: CouponsComponent, canActivate: [AuthGuardService] },
    ]
  },
  // 其它頁面
  { path: '404', component: Page404Component },
  { path: '', redirectTo: 'portal/products', pathMatch: 'full' },
  { path: '**', redirectTo: '404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
