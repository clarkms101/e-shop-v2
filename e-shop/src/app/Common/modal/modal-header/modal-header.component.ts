import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-modal-header',
  templateUrl: './modal-header.component.html',
  styleUrls: ['./modal-header.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModalHeaderComponent implements OnInit {
  @Input() title: string = '請輸入標題';

  @Output() onCloseClick = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

}
