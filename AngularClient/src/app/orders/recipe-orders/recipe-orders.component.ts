import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { CustomeRecipeOrders } from 'src/app/model/custome-recipe-orders.model';
import { Order } from 'src/app/model/order.model';

@Component({
  selector: 'app-recipe-orders',
  templateUrl: './recipe-orders.component.html',
  styleUrls: ['./recipe-orders.component.css']
})
export class RecipeOrdersComponent implements OnInit {

  @Input() orders:Order[];
  public options:string[]=["Waiting","Approved","Denied","Completed"];
  @Output() selectedOrderId: number;
  @Output() choosenStatus = new EventEmitter<any[]>();
  public status:string;
  public niz:any[]=[];
  statusFormGroup = new FormGroup({
    statusForm:new FormControl()
   });
   changeStatusForm= new FormGroup({
    status: new FormControl(''),
   // dateForm: new FormControl('')
   });
  //category=new FormControl("");
  constructor() {
   
    
   }

  ngOnInit(): void {
   
  }
  changeStatus() {
    this.niz.push(this.selectedOrderId);
    this.status=this.statusFormGroup.get('statusForm').value;
    this.niz.push(this.changeStatusForm.value);
    this.choosenStatus.emit(this.niz);
    this.niz=[];
  }
}
