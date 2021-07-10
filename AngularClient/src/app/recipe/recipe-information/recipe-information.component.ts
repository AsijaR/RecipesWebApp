import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Recipe } from 'src/app/model/recipe.model';
import { BookmarkService } from 'src/app/service/bookmark.service';
import { OrderMealComponent } from '../order-meal/order-meal.component';

@Component({
  selector: 'app-recipe-information',
  templateUrl: './recipe-information.component.html',
  styleUrls: ['./recipe-information.component.css']
})
export class RecipeInformationComponent implements OnInit {

 
  @Input() chefPicture:string;
  @Input() recipeId:number;
  @Input() headerPhoto:string;
  @Input() title:string;
  @Input() chefName:string;
  @Input() timeNeededToPrepare:string;
  @Input() servingNumber:number;
  @Input() complexity:string;
  @Input() price:number;
  @Input() mealCanBeOrdered:boolean;
  @Input() deleted:boolean;
  @Input() noteFromChef:string;
  @Input() shippingPrice:string;
  response:string;
  @Output() goToComments: EventEmitter<boolean> = new EventEmitter();
  public sended=false;
  
  //public recipe:Recipe;
  public btnText:string="Add To Bookmark";
  //horizontalPosition: MatSnackBarHorizontalPosition = 'start';
  //verticalPosition: MatSnackBarVerticalPosition = 'bottom';
  constructor(private _snackBar: MatSnackBar,public dialog: MatDialog,private bookmarkService:BookmarkService) {
    var dialogRef = this.dialog;
  }

  
  ngOnInit(): void {
    
  }
  AddToBookmark(){
    this.bookmarkService.addToBookmark(this.recipeId).subscribe(
      res => {
        this._snackBar.open(res, 'Close', {
          duration: 4 * 1000,
          panelClass:["opa"],
          verticalPosition:'bottom',
          horizontalPosition:'center'
        });
        },
      err => console.log('HTTP Error', err));
  }
  onComment(){
    this.goToComments.emit(true);
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(OrderMealComponent, {
      width: '50em',
     data: {recipeId:this.recipeId,price:this.price,noteForShipping: this.noteFromChef,shippingPrice:this.shippingPrice}
    });
    dialogRef.componentInstance.orderedMeal.subscribe(() => {
      dialogRef.close();
    });
   
  }
 

}
