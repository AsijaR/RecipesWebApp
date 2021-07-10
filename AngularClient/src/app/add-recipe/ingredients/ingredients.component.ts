import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormGroupDirective } from '@angular/forms';
import { Ingredient } from 'src/app/model/ingredient.model';

@Component({
  selector: 'app-ingredients',
  templateUrl: './ingredients.component.html',
  styleUrls: ['./ingredients.component.css']
})
export class IngredientsComponent implements OnInit {

  
  ingredientsForm: FormGroup;
  count = 0;
  ingredientsArray : Ingredient[]=[];
  constructor(private fb: FormBuilder) {
   this.ingredientsForm = this.fb.group({
    ingredients: this.fb.array([this.fb.group(new Ingredient("",""))])
  });
  }

  ngOnInit(): void {
    
  }
  get ingredients(): FormArray {
    return this.ingredientsForm.get("ingredients") as FormArray
  }

  newIngredient(): FormGroup {
    return this.fb.group(new Ingredient("",""))
  }

  setDirections(setIngredients: Ingredient[]): FormArray {
    const formArray = new FormArray([]);
    setIngredients.forEach(d => {
      formArray.push(this.fb.group({
        amount: d.amount,
        name:d.name
      }));
    });
    this.count=setIngredients.length-1;
   // this.count=setDirections.length;
    return formArray;
  }

  createComponent(i: number) {
   if (i == this.count) {
      this.ingredients.push(this.newIngredient());
      this.count++;
     }
  }

  removeSkill(i: number) {
    if(i==0) return;
    this.ingredients.removeAt(i);
    this.count--;
  }

  onSubmit() {
    console.log(this.ingredientsForm.value);
  }
  
}
