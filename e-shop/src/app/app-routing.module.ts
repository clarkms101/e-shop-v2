import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Admin/login/login.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { ProductsComponent } from './Admin/products/products.component';
import { IndexComponent } from './Portal/index/index.component';

// 路由守衛
const routes: Routes = [
  { path: '', component: IndexComponent },
  { path: 'admin/login', component: LoginComponent },
  { path: 'admin/products', component: ProductsComponent },
  { path: 'admin/orders', component: OrdersComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
