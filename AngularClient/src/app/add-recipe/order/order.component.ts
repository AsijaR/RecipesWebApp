import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

  @Input() formGroupName: string;
  @Input() submitted:boolean;

  //order=false;
  order = false;
  orderForm: FormGroup;
  constructor(private rootFormGroup: FormGroupDirective) { }

  ngOnInit(): void {
    this.orderForm = this.rootFormGroup.control.get(this.formGroupName) as FormGroup;
  }
  eventHandler(event){
    if(event.target.value.length == 2 && (event.code == "Backspace" || event.code == "Delete")){
      return false;
    }
    return true;
   }
}
