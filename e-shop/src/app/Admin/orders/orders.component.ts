import { DateHelper } from 'src/shared/helpers/DateHelper';
import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from 'src/shared/animations/routerTransition';
import { Client, OrderInfo, OrderStatus, Pagination, QueryOrdersRequest, SelectionItem, UpdateOrderRequest } from 'src/shared/api client/service-proxies';
import { JwtHelper } from 'src/shared/helpers/JwtHelper';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { OrderDetailComponent } from './order-detail/order-detail.component';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  // animations: [appModuleAnimation()]
})
export class OrdersComponent implements OnInit {
  // data
  orders: OrderInfo[] = [];
  paymentMethodList: SelectionItem[] = [];
  // query
  query_startDate: string = '';
  query_endDate: string = '';
  query_paymentMethod: number | null | undefined = null;
  // page
  pagination: Pagination = new Pagination();
  totalPageArray: number[] = [];
  currentPage: number = 0;
  loading = false;
  permission: string = '';

  constructor(
    private _apiClient: Client,
    private _toastr: ToastrService,
    private _modalService: BsModalService,
  ) { }

  ngOnInit(): void {
    let token = localStorage.getItem("adminJWT") as string;
    this._apiClient.setAuthToken(token);
    this.permission = JwtHelper.parseJwt().JwtKeyAdminPermission as string;

    this._apiClient.paymentMethod().subscribe((response) => {
      if (response.success) {
        this.paymentMethodList = response.items as SelectionItem[];
      }
    });

    this.getPageData(1);
  }

  getPageData(page: number): void {
    this.loading = true;
    let request = new QueryOrdersRequest();
    request.page = page;
    request.pageSize = 10;
    if (this.query_paymentMethod !== null && this.query_paymentMethod !== undefined) {
      request.paymentMethod = this.query_paymentMethod;
    }
    if (this.query_startDate !== '') {
      // yyyy-MM-dd
      request.startDate = DateHelper.getDateString(new Date(this.query_startDate));
    }
    if (this.query_endDate !== '') {
      // yyyy-MM-dd
      request.endDate = DateHelper.getDateString(new Date(this.query_endDate));
    }

    this._apiClient.orders(request).subscribe((response) => {
      if (response.orderInfos !== undefined && response.pagination !== undefined) {
        this.orders = response.orderInfos;
        this.pagination = response.pagination;
        this.currentPage = response.pagination.currentPage as number;
        let totalPages = response.pagination.totalPages as number;
        this.totalPageArray = Array.from(new Array(totalPages), (x, i) => i + 1)
      }
      this.loading = false;
    });
  }

  showOrderDetail(serialNumber: string | undefined): void {
    let viewDialog: BsModalRef;

    viewDialog = this._modalService.show(
      OrderDetailComponent,
      {
        class: 'modal-lg',
        initialState: {
          serialNumber: serialNumber as string
        },
      }
    );
  }

  updateOrder(serialNumber: string | undefined, orderStatus: string) {
    let systemUserId = JwtHelper.parseJwt().JwtKeyAdminSystemUserId as number;

    let request = new UpdateOrderRequest();
    request.systemUserId = systemUserId;
    request.serialNumber = serialNumber;
    switch (orderStatus) {
      case 'Finished':
        request.orderStatus = OrderStatus._1; // Finished 對應後端的Enum
        break;
      case 'Cancel':
        request.orderStatus = OrderStatus._2;
        break;
      case 'Refund':
        request.orderStatus = OrderStatus._3;
        break;
    }
    this._apiClient.orderPUT(request).subscribe((response) => {
      if (response.success) {
        this._toastr.success(`${response.message}`);
        this.getPageData(1);
      }
      else {
        this._toastr.warning(`${response.message}`);
      }
    });
  }
}
