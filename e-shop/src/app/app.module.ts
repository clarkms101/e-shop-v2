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
// ngx-bootstrap
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { SidebarComponent } from './Admin/layout/sidebar/sidebar.component';
import { NavbarComponent } from './Admin/layout/navbar/navbar.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { ProductDetailComponent } from './Admin/products/product-detail/product-detail.component';
import { ModalHeaderComponent } from './Common/modal/modal-header/modal-header.component';
import { ModalFooterComponent } from './Common/modal/modal-footer/modal-footer.component';
import { AuthGuardService } from 'src/shared/services/auth-guard.service';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from 'ngx-loading';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    LoginComponent,
    ProductsComponent,
    SidebarComponent,
    NavbarComponent,
    OrdersComponent,
    ProductDetailComponent,
    ModalHeaderComponent,
    ModalFooterComponent
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
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.wanderingCubes,
      backdropBackgroundColour: "rgba(0,0,0,0.1)",
      backdropBorderRadius: "4px",
      primaryColour: "#ffffff",
      secondaryColour: "#ffffff",
      tertiaryColour: "#ffffff",
    }),
    // ngx-bootstrap
    ModalModule.forChild(),
    BsDropdownModule,
    CollapseModule,
    TabsModule,
  ],
  providers: [
    Client,
    { provide: API_BASE_URL, useValue: environment.apiUrl },
    AuthGuardService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
