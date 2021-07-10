import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { delay } from 'rxjs/operators';
import { Bookmark } from '../model/bookmark.model';
import { Recipe } from '../model/recipe.model';
import { BookmarkService } from '../service/bookmark.service';
import { RecipeService } from '../service/recipe.service';
import { AddCommentComponent } from './recipe-comments/add-comment/add-comment.component';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css']
})
export class RecipeComponent implements OnInit {

  public recipe: Recipe;
  //public recipe: Recipe2;
  public recipeId: number;
  public serverResponse:string;
  public deleted: boolean = false;
  public canOrder: boolean;
  public recipeDescription: string[] = [];
  public notification: string = "";
  public headerPhoto:string="";
  private paramMapsActiveSub: Subscription[];
  @ViewChild('addComment') comment:ElementRef;
  public r: boolean;
  constructor(private route: ActivatedRoute,
    private recipeService: RecipeService
    ) {
    this.paramMapsActiveSub = [];
  }

  ngOnInit(): void {
    delay(1000);
    this.getRecipeById();
  }
  getRecipeById(): void {
    this.recipeId = Number(this.route.snapshot.paramMap.get('recipeId'));
    this.recipeService.getRecipeById(this.recipeId)
      .subscribe(recipe => {
        this.recipe = recipe;
        this.headerPhoto=recipe.recipePhotos.find(x=>x.isMain).url;
        this.recipeDescription = recipe.description.split(';');
      });
  }

  OnDestroy() {
    this.paramMapsActiveSub.forEach((sub: Subscription) => {
      sub.unsubscribe();
    });
  }
  scrollToComment(el: ElementRef,clicked: boolean) {
     if (clicked) {
     this.comment.nativeElement.scrollIntoView({behavior: 'smooth'}); 
     }
  }
}
