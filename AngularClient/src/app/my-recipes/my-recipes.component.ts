import { Component, OnInit } from '@angular/core';
import { Recipe } from '../model/recipe.model';
import { RecipeService } from '../service/recipe.service';

@Component({
  selector: 'app-my-recipes',
  templateUrl: './my-recipes.component.html',
  styleUrls: ['./my-recipes.component.css']
})
export class MyRecipesComponent implements OnInit {

  public recipes:Recipe[];
  constructor(private recipeService:RecipeService) {
    this.recipeService.getChefsRecipes().subscribe(rec=>this.recipes=rec);
   }
  ngOnInit(): void {
  }

}
