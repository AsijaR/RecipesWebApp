export interface Member
{    
    username: string;
    firstName: string;
    lastName: string;
    photoUrl:string;
    address?: string;
    email: string;
    city?: string;
    state?: string;
    zip?: string;
    shippingPrice: number;
}