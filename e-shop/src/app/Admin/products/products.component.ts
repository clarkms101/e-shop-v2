import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { appModuleAnimation } from 'src/shared/animations/routerTransition';
import { Client, Pagination, Product, QueryProductsRequest } from 'src/shared/api client/service-proxies';
import { JwtHelper } from 'src/shared/helpers/JwtHelper';
import { ProductDetailComponent } from './product-detail/product-detail.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  // animations: [appModuleAnimation()]
})
export class ProductsComponent implements OnInit {
  // data
  products: Product[] = [];
  // query
  queryProductName: string = '';
  // page
  pagination: Pagination = new Pagination();
  totalPageArray: number[] = [];
  currentPage: number = 0;
  loading = false;
  permission: string = '';

  constructor(
    private _apiClient: Client,
    private _modalService: BsModalService,
    private _toastr: ToastrService
  ) { }

  ngOnInit(): void {
    let token = localStorage.getItem("adminJWT") as string;
    this._apiClient.setAuthToken(token);
    this.permission = JwtHelper.parseJwt().JwtKeyAdminPermission as string;

    this.getPageData(1);
  }

  getPageData(page: number): void {
    this.loading = true;
    let request = new QueryProductsRequest();
    request.page = page;
    request.pageSize = 10;
    request.productName = this.queryProductName;

    this._apiClient.products(request).subscribe((response) => {
      if (response.products !== undefined && response.pagination !== undefined) {
        this.products = response.products;
        this.pagination = response.pagination;
        this.currentPage = response.pagination.currentPage as number;
        let totalPages = response.pagination.totalPages as number;
        this.totalPageArray = Array.from(new Array(totalPages), (x, i) => i + 1)
      }
      this.loading = false;
    });
  }

  create(): void {
    this.showCreateOrEditDialog();
  }

  edit(product: Product): void {
    this.showCreateOrEditDialog(product);
  }

  remove(id: number | undefined): void {
    if (id !== undefined) {
      this._apiClient.productDELETE(id).subscribe((response) => {
        if (response.success) {
          this._toastr.success(`${response.message}`);
        } else {
          this._toastr.warning(`${response.message}`);
        }
        this.getPageData(1);
      });
    }
  }

  showCreateOrEditDialog(product?: Product): void {
    let createOrEditDialog: BsModalRef;

    // Edit
    if (product !== undefined) {
      createOrEditDialog = this._modalService.show(
        ProductDetailComponent,
        {
          class: 'modal-lg',
          initialState: {
            product: product,
            isEdit: true
          },
        }
      );
    }
    // Create
    else {
      createOrEditDialog = this._modalService.show(
        ProductDetailComponent,
        {
          class: 'modal-lg',
          initialState: {
            isEdit: false
          },
        }
      );
    }

    // 儲存完重新載入資料
    createOrEditDialog.content.onSave.subscribe(() => {
      this.getPageData(1);
    });
  }
}
