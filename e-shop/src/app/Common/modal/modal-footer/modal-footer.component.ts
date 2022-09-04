import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-modal-footer',
  templateUrl: './modal-footer.component.html',
  styleUrls: ['./modal-footer.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModalFooterComponent implements OnInit {
  @Input() cancelLabel = '取消';
  @Input() cancelDisabled: boolean = false;
  @Input() saveLabel = '儲存';
  @Input() saveDisabled: boolean = false;

  @Output() onCancelClick = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

}
