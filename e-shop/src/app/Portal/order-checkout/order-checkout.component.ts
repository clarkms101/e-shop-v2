import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Client, OrderInfo } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-order-checkout',
  templateUrl: './order-checkout.component.html',
  styleUrls: ['./order-checkout.component.css']
})
export class OrderCheckoutComponent implements OnInit {
  orderInfo: OrderInfo = new OrderInfo();

  constructor(
    private _router: Router,
    private _activeRoute: ActivatedRoute,
    private _apiClient: Client
  ) { }

  ngOnInit(): void {
    this._activeRoute.queryParams.subscribe(queryParams => {
      if (queryParams['serialNumber'] !== undefined) {
        let serialNumber = queryParams['serialNumber'];
        console.log(serialNumber);
        this._apiClient.orderGET(serialNumber).subscribe((response) => {
          console.log(response);
          if (response.success) {
            this.orderInfo = response.orderInfo as OrderInfo;
          } else {
            this._router.navigate(['portal/products']);
          }
        });
      } else {
        this._router.navigate(['portal/products']);
      }
    });

  }
}
