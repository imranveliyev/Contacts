import { Component, Input, Output, EventEmitter } from '@angular/core';

declare var $: any;

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {

  @Input() title: string;
  @Input() content: string;
  @Input() set opened(value: boolean) {
    if(value) {
      $('#modal').modal('show');
    } else {
      $('#modal').modal('hide');
    }
  };

  @Output() close: EventEmitter<boolean> = new EventEmitter<boolean>();

  onOkClick() {
    this.close.emit(true);
  }

  onCancelClick() {
    this.close.emit(false);
  }
}
