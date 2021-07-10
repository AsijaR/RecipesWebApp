import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { CustomeRecipeOrders } from '../model/custome-recipe-orders.model';
import { Order } from '../model/order.model';
import { OrderParams } from '../model/orderParams.model';
import { Pagination } from '../model/pagination';
import { OrderService } from '../service/order.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  public orders:Order[];
 
  sortingForm= new FormGroup({
    statusForm: new FormControl('')
   });
   pagination: Pagination;
   orderParams: OrderParams=new OrderParams();

  constructor(private orderService:OrderService) { 
   
    //this.getChefsOrders(data);
  }

  ngOnInit(): void {
     this.sortingForm.patchValue({
      statusForm:'All',
    });
    this.getChefsOrders(this.sortingForm.value);
  }
  addItem(array: any[]) {
    return this.orderService.EditOrder(array[0],array[1]).subscribe(res=>
      {
        this.orders.find(x=>x.orderId===array[0]).approvalStatus=array[1].status;
      });
  }
  getChefsOrders(data){

    this.orderParams.orderStatus=this.sortingForm.get('statusForm').value;
    return this.orderService.getChefsOrders(this.orderParams).subscribe((res)=>{
      this.orders=res.result;
      this.pagination=res.pagination;
    });
  }
  pageChanged(event: any) {
    this.orderParams.pageNumber = event.page;
    //this.memberService.setUserParams(this.userParams);
    this.getChefsOrders("d");
  }
}
