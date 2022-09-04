import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from 'src/shared/animations/routerTransition';
import { Client, Pagination, Product, QueryProductsRequest } from 'src/shared/api client/service-proxies';
import { ProductDetailComponent } from './product-detail/product-detail.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  animations: [appModuleAnimation()]
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

  constructor(
    private _apiClient: Client,
    private _modalService: BsModalService
  ) { }

  ngOnInit(): void {
    let token = localStorage.getItem("adminJWT") as string;
    this._apiClient.setAuthToken(token);
    this.getPageData(1);
  }

  getPageData(page: number): void {
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
    });
  }

  create(): void {
    this.showCreateOrEditDialog();
  }

  edit(id: number | undefined): void {
    this.showCreateOrEditDialog(id);
  }

  showCreateOrEditDialog(id?: number): void {
    let createOrEditDialog: BsModalRef;

    if (id !== undefined) {
      createOrEditDialog = this._modalService.show(
        ProductDetailComponent,
        {
          class: 'modal-xl',
          initialState: {
            id: id,
            isEdit: true
          },
        }
      );
    } else {
      createOrEditDialog = this._modalService.show(
        ProductDetailComponent,
        {
          class: 'modal-xl',
          initialState: {
            isEdit: false
          },
        }
      );
    }

    // 儲存完重新載入資料
    createOrEditDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  refresh(): void {
    this.getPageData(1);
  }
}
