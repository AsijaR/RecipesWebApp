import { Comments } from "./comment.model";
import { Ingredient } from "./ingredient.model";
import { RecipePhoto } from "./recipePhoto.model";

export class Recipe 
{
    recipeId: number;
    chefName: string;
    chefPhoto:string;
    title: string;
    headerUrl: any;
    complexity: string;
    servingNumber: number;
    timeNeededToPrepare: string;
    description: string;
    note: string;
    mealCanBeOrdered: boolean;
    price: number; 
    shippingPrice:number;
    noteForShipping: string;
    categoryId: number;
    ingredients: Ingredient[];
    comments: Comments[];
    recipePhotos: RecipePhoto[];

    constructor(title: string, 
                complexity: string, 
                servingNumber: number,
                timeNeededToPrepare: string,
                description: string,
                note: string,
                mealCanBeOrdered: boolean, 
                price: number,
                noteForShipping: string, 
                categoryId: number){
                    this.title = title;
                    this.complexity = complexity;
                    this.servingNumber = servingNumber;
                    this.timeNeededToPrepare = timeNeededToPrepare;
                    this.description = description;
                    this.note = note;
                    this.mealCanBeOrdered = mealCanBeOrdered;
                    this.price = price;
                    this.noteForShipping = noteForShipping;
                    this.categoryId = categoryId;
                }
}