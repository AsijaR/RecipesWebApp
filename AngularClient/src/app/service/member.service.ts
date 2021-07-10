import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../model/member.model';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  member:Member;
  baseUrl = environment.apiUrl+"user/";
  constructor(private http: HttpClient) { }
  getMembers(){}
  getMember(){
    return this.http.get<Member>(this.baseUrl+'userInfo').pipe(
      map(m=>{this.member=m;return this.member;}),
      catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  }
  updateMemberInfo(data:any){
    return this.http.put(this.baseUrl+'updateProfile',data).pipe(
      catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  }
  updateShippingPrice(data:any){
    return this.http.put(this.baseUrl+'change-order',data).pipe(
      catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  }
  changePassword(data:any){
    return this.http.put(environment.apiUrl+'Account/'+'change-password',data).pipe(
      catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  }
}
