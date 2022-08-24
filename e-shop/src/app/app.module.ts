import { environment } from './../environments/environment.prod';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Client } from 'src/shared/api client/service-proxies';
import { API_BASE_URL } from 'src/shared/api client/service-proxies';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IndexComponent } from './Portal/index/index.component';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientJsonpModule,
  ],
  providers: [
    Client,
    // todo 從設定檔注入api網址
    { provide: API_BASE_URL, useValue: 'https://localhost:44331' },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
