import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Recipe } from '../model/recipe.model';
import { User } from '../model/user.model';
import { getPaginatedResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl+"admin/";

  constructor(private http: HttpClient) { }

  getAllUsers() {
    return this.http.get<User[]>(this.baseUrl+"get-all-users").pipe(
      map(response => {
        return response;
      }),
    catchError(err => {console.log('Handling error locally and rethrowing it...', err);
      return throwError(err)}));
  }

  deleteUser(userId: number) {
    return this.http.delete(this.baseUrl + 'delete-user/' + userId).pipe(
      catchError(err => {console.log('Handling error locally and rethrowing it...', err);
      return throwError(err)})
    );
  }
  getRecipes()
  {
    return getPaginatedResult<Recipe[]>(this.baseUrl+"get-all-recipes","",this.http).pipe(
      map(response => {
        return response;
      }),
    catchError(err => {console.log('Handling error locally and rethrowing it...', err);
      return throwError(err)}));
  }
  deleteRecipe(recipeId: number) {
    return this.http.delete(this.baseUrl + 'delete-recipe/' + recipeId).pipe(
      catchError(err => {console.log('Handling error locally and rethrowing it...', err);
      return throwError(err)})
    );
  }
}