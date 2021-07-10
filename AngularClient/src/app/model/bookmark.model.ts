import { Recipe } from "./recipe.model";

export interface Bookmark {
    bookmarkId:number;
    recipes: Recipe[];
}
