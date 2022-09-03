import { environment } from './../environments/environment';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Client } from 'src/shared/api client/service-proxies';
import { API_BASE_URL } from 'src/shared/api client/service-proxies';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IndexComponent } from './Portal/index/index.component';
import { LoginComponent } from './Admin/login/login.component';
import { FormsModule } from '@angular/forms';
import { ProductsComponent } from './Admin/products/products.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { SidebarComponent } from './Admin/layout/sidebar/sidebar.component';
import { NavbarComponent } from './Admin/layout/navbar/navbar.component';
import { OrdersComponent } from './Admin/orders/orders.component';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    LoginComponent,
    ProductsComponent,
    SidebarComponent,
    NavbarComponent,
    OrdersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientJsonpModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot(), // ToastrModule added
  ],
  providers: [
    Client,
    { provide: API_BASE_URL, useValue: environment.apiUrl },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
