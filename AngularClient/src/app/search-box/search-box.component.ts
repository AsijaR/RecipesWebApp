import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { RecipeParams } from '../model/recipeParams.model';
import { RecipeService } from '../service/recipe.service';
import { SearchIngredientsDialogComponent } from './search-ingredients-dialog/search-ingredients-dialog.component';

@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.css']
})
export class SearchBoxComponent implements OnInit {


  searchingParams: RecipeParams = new RecipeParams();
  ingredient1: string;
  ingredient2: string;
  ingredient3: string;

  searchForm = new FormGroup({
    title: new FormControl('')
  });
  @Output() searchcriteria = new EventEmitter<String>();
  @Output() recParams = new EventEmitter<RecipeParams>();

  constructor(public dialog: MatDialog) {

  }

  ngOnInit(): void {
    //this.recipeService.setUserParams(this.recipeParams);
  }
  openDialog() {
    const dialogRef = this.dialog.open(SearchIngredientsDialogComponent, {

    });

    dialogRef.afterClosed().subscribe(result => {
      if (dialogRef.componentInstance.submitted) {
        this.searchingParams.ingredient1 = dialogRef.componentInstance.searchByIngForm.get("ingredient1").value;
        this.searchingParams.ingredient2 = dialogRef.componentInstance.searchByIngForm.get("ingredient2").value;
        this.searchingParams.ingredient3 = dialogRef.componentInstance.searchByIngForm.get("ingredient3").value;
        this.recParams.emit(this.searchingParams);
      }
    });
  }
  searchThis() {
    this.recParams.emit(this.searchingParams);
  }

}
