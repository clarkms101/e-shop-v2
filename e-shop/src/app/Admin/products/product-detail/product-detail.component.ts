import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Client, CreateProductRequest, Product, UpdateProductRequest } from 'src/shared/api client/service-proxies';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  // 外部注入值進來
  id: number = 0;
  isEdit: boolean = false;
  product: Product = new Product();
  // 內部使用
  saving = false;
  // 元件資料傳遞
  @Output() onSave = new EventEmitter<any>();

  constructor(
    private _apiClient: Client,
    public bsModalRef: BsModalRef,
    private _toastr: ToastrService
  ) { }

  ngOnInit(): void {
  }

  save(): void {
    this.saving = true;

    // 編輯
    if (this.isEdit) {
      let editData = new UpdateProductRequest();
      editData.product = this.product;
      this._apiClient.productPUT(editData).subscribe((response) => {
        this._toastr.success(`${response.message}`);
        this.bsModalRef.hide();
        this.onSave.emit();
      }, () => {
        this.saving = false;
      });
    }
    // 新增
    else {
      let createData = new CreateProductRequest();
      createData.product = this.product;
      this._apiClient.productPOST(createData).subscribe((response) => {
        this._toastr.success(`${response.message}`);
        this.bsModalRef.hide();
        this.onSave.emit();
      }, () => {
        this.saving = false;
      });
    }
  }
}
