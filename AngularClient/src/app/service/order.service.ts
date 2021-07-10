import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CustomeRecipeOrders } from '../model/custome-recipe-orders.model';
import { Order } from '../model/order.model';
import { OrderParams } from '../model/orderParams.model';
import { Recipe } from '../model/recipe.model';
import { HttpErrorHandler } from '../utils/http-error-handler.model';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  public recipes: Recipe[];
  public orders:Order[];
  orderParams: OrderParams;
  baseUrl = environment.apiUrl+"order/";

  constructor(private http: HttpClient) {
  }
  getOrderParams() {
    return this.orderParams;
  }

  setOrderParams(params: OrderParams) {
    this.orderParams = params;
  }

  public getChefsOrders(orderParams:OrderParams) {
   let params = getPaginationHeaders(orderParams.pageNumber, orderParams.pageSize);

    params = params.append('OrderByStatus', orderParams.orderStatus);
    
    return getPaginatedResult<Order[]>(this.baseUrl, params, this.http).pipe( map(response => {return response;}),
      catchError(err => {console.log('Handling error locally and rethrowing it...', err);
        return throwError(err)}));
   }
 
  public getOrderStatuses() {
    return this.http.get<string[]>(this.baseUrl);
  }
  public makeOrder(model: any) {
    console.log(model);
    return this.http.post(this.baseUrl, model,{responseType: 'text'}).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    }));
  }
  public EditOrder(orderId: number, orderStatus) {
    return this.http.put(this.baseUrl+"change-status/"+orderId,orderStatus).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    }));
  }
}
