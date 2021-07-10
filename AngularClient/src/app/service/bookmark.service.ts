import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Identifiers } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Bookmark } from '../model/bookmark.model';
import { Recipe } from '../model/recipe.model';
import { User } from '../model/user.model';
import { HttpErrorHandler } from '../utils/http-error-handler.model';
import { AccountService } from './account.service';
import { getPaginatedResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService  {
  baseUrl = environment.apiUrl+'bookmark/';
  public userBookmark: Bookmark;
  bookmarkCache = new Map();
  user: User;
  public odg:string;
  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    })
  }
  getBookmarks() {
    return this.http.get<Bookmark>(this.baseUrl).pipe(map(res=>{
      this.bookmarkCache.set(Object.values(res),res.recipes);
      return res;
    }));
  }
  public addToBookmark(recipeId: Number){
    return this.http.put(this.baseUrl+"add-to-bookmark/"+recipeId,recipeId,  {responseType: 'text'}).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    })); 
  }
  public removeRecipe(recipeId: Number){
    return this.http.put(this.baseUrl+"remove-from-bookmark/"+recipeId,recipeId,{responseType: 'text'}).pipe(
      catchError(err => {
        console.log('Handling error locally and rethrowing it...', err);
        return throwError(err);
    })); 
   }
}