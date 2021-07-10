import { Order } from "./order.model";

export class RecipeOrders {
    public orderId:number;
    public order:Order;
    public userId:number;
    public chefId:number;
    approvalStatus:string;
}
