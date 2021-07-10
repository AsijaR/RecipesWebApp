import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { Recipe} from '../model/recipe.model';
import { HttpErrorHandler } from '../utils/http-error-handler.model';
import { catchError, map, retry, switchMap} from 'rxjs/operators';
import { Comments } from '../model/comment.model';
import { Order } from '../model/order.model';
import { Ingredient } from '../model/ingredient.model';
import { environment } from 'src/environments/environment';
import { NewRecipe } from '../model/addRecipe.model';
import { RecipeParams } from '../model/recipeParams.model';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

export class JsonIng{
  ing:Ingredient[];
}

@Injectable({
  providedIn: 'root'
})
export class RecipeService{

  //private recipes:Observable<Recipe[]>;
  //private recipe:Observable<Recipe>;
  recipeParams: RecipeParams;
  recipeId:any;
  baseUrl = environment.apiUrl+"recipes/";
  
  constructor(private http: HttpClient) {
   }

  setUserParams(params: RecipeParams) {
    this.recipeParams = params;
  }

  resetUserParams() {
    this.recipeParams = new RecipeParams();
    return this.recipeParams;
  }

  public getSearchedRecipes(recipeParams: RecipeParams) {
    let params = getPaginationHeaders(recipeParams.pageNumber, recipeParams.pageSize);

    if(recipeParams.title!=undefined||null)
      params = params.append('title', recipeParams.title);
    if(recipeParams.ingredient1!=undefined)
      params = params.append('ingredient1', recipeParams.ingredient1);
    if(recipeParams.ingredient2!=undefined)
      params = params.append('ingredient2', recipeParams.ingredient2);
    if(recipeParams.ingredient3!=undefined)
      params = params.append('ingredient3', recipeParams.ingredient3);


    return getPaginatedResult<Recipe[]>(this.baseUrl + 'search-recipes', params, this.http)
      .pipe(
        map(response => {
        return response;
      }),
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err)}));

  }
  public getChefsRecipes(){
    return this.http.get<Recipe[]>(this.baseUrl).pipe(catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  }
  public getRecipeById(id:Number){
    return this.http.get<Recipe>(this.baseUrl+id).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    }));
  }
  public addCommentToRecipe(recipeId:Number,model: any){
    return this.http.post(this.baseUrl+recipeId+"/add-comment",model).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    }));
  }
  public addNewRecipe(model:NewRecipe){
  return this.http.post(this.baseUrl,model).pipe(
    map(r=>this.recipeId=r),
    catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  
}
public editRecipe(recipeId:Number,model:NewRecipe){
  return this.http.put(this.baseUrl+'edit-recipe/'+recipeId,model).pipe(
    catchError(err => {
      console.log('Handling error locally and rethrowing it...', err);
      return throwError(err);
  }));
  
}
  public deleteRecipe(recipeId:number){
    return this.http.delete(this.baseUrl+"?id="+recipeId).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    })).subscribe(
      err => console.log('HTTP Error', err));
  }
}
