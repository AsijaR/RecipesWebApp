import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Category } from '../model/category.model';
import { HttpErrorHandler } from '../utils/http-error-handler.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  baseUrl = environment.apiUrl + 'categories/';
  public categories: Category[];
  public category: Category;
  constructor(private http: HttpClient) {
  }
  public getCategories() {
    return this.http.get<Category[]>(this.baseUrl).
      pipe(map((res) => {
        this.categories = res;
        return this.categories;
      }));
  }
  public getCategory(id: number) {
    return this.http.get<Category>(this.baseUrl + id).
      pipe(map((res) => {
        this.category = res;
        return this.category;
      })
      );
  }
}
