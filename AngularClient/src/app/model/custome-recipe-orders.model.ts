import { Order } from "./order.model";

export class CustomeRecipeOrders {

    public orderId:number;
    public title:string="";
    public price:number=0;
    public order:Order;
    public approvalStatus:string;
}
