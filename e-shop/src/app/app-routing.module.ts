import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from 'src/shared/services/auth-guard.service';
import { AdminComponent } from './Admin/admin/admin.component';
import { CouponsComponent } from './Admin/coupons/coupons.component';
import { LoginComponent } from './Admin/login/login.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { ProductsComponent } from './Admin/products/products.component';
import { Page404Component } from './Common/page/page404/page404.component';
import { IndexComponent } from './Portal/index/index.component';
import { PortalComponent } from './Portal/portal/portal.component';

const routes: Routes = [
  // layout 頁面(導至預設頁面)
  { path: 'portal', redirectTo: 'portal/index' },
  { path: 'admin', redirectTo: 'admin/login' },
  // portal 頁面
  {
    path: 'portal', component: PortalComponent, children: [
      { path: 'index', component: IndexComponent },
    ]
  },
  // admin 頁面
  { path: 'admin/login', component: LoginComponent },
  {
    path: 'admin', component: AdminComponent, children: [
      { path: 'products', component: ProductsComponent, canActivate: [AuthGuardService] },
      { path: 'orders', component: OrdersComponent, canActivate: [AuthGuardService] },
      { path: 'coupons', component: CouponsComponent, canActivate: [AuthGuardService] },
    ]
  },
  // 其它頁面
  { path: '404', component: Page404Component },
  { path: '', redirectTo: 'portal/index', pathMatch: 'full' },
  { path: '**', redirectTo: '404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
