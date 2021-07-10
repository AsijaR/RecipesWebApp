import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Recipe } from '../model/recipe.model';
import { BookmarkService } from '../service/bookmark.service';
import { RecipeService } from '../service/recipe.service';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-recipe-cards',
  templateUrl: './recipe-cards.component.html',
  styleUrls: ['./recipe-cards.component.css']
})
export class RecipeCardsComponent implements OnInit {

  @Input() recipes:Recipe[];
  @Input() recipeOwner:boolean=false;
  @Input() bookmarkPage:boolean=false;
  @Output() recipeId = new EventEmitter<number>();
  constructor(public dialog: MatDialog, private recipeService:RecipeService,private bookmarkService:BookmarkService) { 
  }

  ngOnInit(): void {
  }
  openDeleteDialog(recipeId:number){
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '40em',
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result==true){
        this.recipeService.deleteRecipe(recipeId);
        this.recipes.forEach((rec,index)=>{
          if(rec.recipeId==recipeId) this.recipes.splice(index,1);
       });
      }
    });
    
  } 
  removeFromBookmark(recipeId)
  {
    this.recipeId.emit(recipeId);
    // // console.log(recipeId);
    
    //   this.bookmarkService.removeRecipe(recipeId).subscribe(res=>
    //     {
    //       this.recipes.forEach((rec,index)=>{
    //         if(rec.recipeId==recipeId) this.recipes.splice(index,1);
    //      });
    //     });
  }
}
