import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Category } from '../model/category.model';
import { Pagination } from '../model/pagination';
import { Recipe } from '../model/recipe.model';
import { RecipeParams } from '../model/recipeParams.model';
import { CategoryService } from '../service/category.service';
import { RecipeService } from '../service/recipe.service';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css']
})
export class RecipesComponent implements OnInit {

  public recipes: Recipe[]=[];
  public category: Observable<Category>;
  public cateogryId: number;
  private paramMapsActiveSub: Subscription[];
  pagination: Pagination;
  recipeParams: RecipeParams=new RecipeParams();
  constructor(private categoryService: CategoryService,private recipeService:RecipeService, private route: ActivatedRoute) {
    this.paramMapsActiveSub = [];
  //  this.recipeParams = this.recipeService.getUserParams();

  }

  ngOnInit(): void {
    this.getRecipes();
    this.category.subscribe(p => this.recipes = p.recipes);
  }
  getRecipes():void {
    this.category = this.route.paramMap.pipe(switchMap(params => {
      this.cateogryId = Number(params.get('categoryId'));
      if(this.cateogryId===0)
      {
        this.recipeParams.getRecentRecipes=true;
        this.recipeService.getSearchedRecipes(this.recipeParams).subscribe(res=>
          {
            this.recipes=res.result;
            this.pagination=res.pagination;
          });
      }
      else return this.categoryService.getCategory(this.cateogryId);
    }));
  }

  OnDestroy() {
    this.paramMapsActiveSub.forEach((sub: Subscription) => {
      sub.unsubscribe();
    });
  }

  searchThis(data) {
    if(data)
    {
      this.recipeParams=data;
      this.recipeService.getSearchedRecipes(this.recipeParams).subscribe(res=>
        {
          this.recipes=res.result;
          this.pagination=res.pagination;
        });
    }
  }
}

