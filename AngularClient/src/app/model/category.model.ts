import { Recipe} from "./recipe.model";

export interface Category {
    categoryId:number;
    name:string;
    recipes:Recipe[];
}
