import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Client, Coupon, CreateCouponRequest, UpdateCouponRequest } from 'src/shared/api client/service-proxies';
import { DateHelper } from 'src/shared/helpers/DateHelper';
import { JwtHelper } from 'src/shared/helpers/JwtHelper';

@Component({
  selector: 'app-coupon-detail',
  templateUrl: './coupon-detail.component.html',
  styleUrls: ['./coupon-detail.component.css']
})
export class CouponDetailComponent implements OnInit {
  // 外部注入值進來
  id: number = 0;
  isEdit: boolean = false;
  coupon: Coupon = new Coupon();
  // 內部使用
  saving = false;
  // 日期
  dueDateTimeString: any = "";
  // 元件資料傳遞
  @Output() onSave = new EventEmitter<any>();

  constructor(
    private _apiClient: Client,
    public bsModalRef: BsModalRef,
    private _toastr: ToastrService
  ) { }

  ngOnInit(): void {
    if (this.isEdit && this.coupon.dueDateTimeStamp != undefined) {
      var date = new Date(this.coupon.dueDateTimeStamp);
      this.dueDateTimeString = date;
    }
  }

  save(): void {
    this.saving = true;
    let dueDateTime = new Date(this.dueDateTimeString);
    let systemUserId = JwtHelper.parseJwt().JwtKeyAdminSystemUserId as number;

    // 編輯
    if (this.isEdit) {
      let editData = new UpdateCouponRequest();
      editData.systemUserId = systemUserId;
      editData.coupon = this.coupon;
      editData.coupon.dueDateTimeStamp = DateHelper.getTimestampSeconds(dueDateTime);

      this._apiClient.couponPUT(editData).subscribe((response) => {
        this._toastr.success(`${response.message}`);
        this.bsModalRef.hide();
        this.onSave.emit();
      }, () => {
        this.saving = false;
      });
    }
    // 新增
    else {
      let createData = new CreateCouponRequest();
      createData.systemUserId = systemUserId;
      createData.coupon = this.coupon;
      createData.coupon.dueDateTimeStamp = DateHelper.getTimestampSeconds(dueDateTime);

      this._apiClient.couponPOST(createData).subscribe((response) => {
        this._toastr.success(`${response.message}`);
        this.bsModalRef.hide();
        this.onSave.emit();
      }, () => {
        this.saving = false;
      });
    }
  }
}
