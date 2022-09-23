import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from 'src/shared/services/auth-guard.service';
import { CouponsComponent } from './Admin/coupons/coupons.component';
import { LoginComponent } from './Admin/login/login.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { ProductsComponent } from './Admin/products/products.component';
import { Page404Component } from './Common/page/page404/page404.component';
import { IndexComponent } from './Portal/index/index.component';

const routes: Routes = [
  { path: '', redirectTo: 'index', pathMatch: 'full' },
  { path: '**', redirectTo: '404' },
  { path: '404', component: Page404Component },
  { path: 'index', component: IndexComponent },
  { path: 'admin/login', component: LoginComponent },
  { path: 'admin/products', component: ProductsComponent, canActivate: [AuthGuardService] },
  { path: 'admin/orders', component: OrdersComponent, canActivate: [AuthGuardService] },
  { path: 'admin/coupons', component: CouponsComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
