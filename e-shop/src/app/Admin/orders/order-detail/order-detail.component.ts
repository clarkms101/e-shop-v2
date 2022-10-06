import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Client, OrderInfo } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent implements OnInit {
  // 外部傳入
  serialNumber: string = '';
  // 內部資料
  orderInfo: OrderInfo = new OrderInfo();

  constructor(
    private _apiClient: Client,
    public bsModalRef: BsModalRef,
  ) { }

  ngOnInit(): void {
    this._apiClient.orderGET(this.serialNumber).subscribe((response) => {
      console.log(response);
      if (response.success) {
        this.orderInfo = response.orderInfo as OrderInfo;
      }
    });
  }
}
