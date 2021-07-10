import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RecipeParams } from 'src/app/model/recipeParams.model';

@Component({
  selector: 'app-search-ingredients-dialog',
  templateUrl: './search-ingredients-dialog.component.html',
  styleUrls: ['./search-ingredients-dialog.component.css']
})
export class SearchIngredientsDialogComponent implements OnInit {

  searchByIngForm: FormGroup;
  submitted = false;

  constructor(public formBuilder: FormBuilder, public dialogRef: MatDialogRef<SearchIngredientsDialogComponent>) { }

  ngOnInit(): void {
    this.intitializeForm();
  }
  intitializeForm() {
    this.searchByIngForm = this.formBuilder.group({
      ingredient1: [],
      ingredient2: [],
      ingredient3: []
    })
  }
  public submitForm(data: any) {
    this.submitted=true;
    this.dialogRef.close();
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}
