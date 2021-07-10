export interface Order {
    orderId: number;
    fullName: string;
    address: string;
    city: string;
    state: string;
    zip: string;
    dateMealShouldBeShipped: Date;
    servingNumber: number;
    noteToChef: string;
    recipeId: number;
    recipeTitle: string;
    shippingPrice: number;
    total: number;
    price: number;
    approvalStatus: string;
}