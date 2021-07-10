import { Ingredient } from "./ingredient.model";
import { Recipe } from "./recipe.model";

export class NewRecipe {
    recipe: Recipe;
    ingredients: Ingredient[];
    constructor(recipe:Recipe,ingredients:Ingredient[])
    {
        this.recipe=recipe;
        this.ingredients=ingredients;
    }
}