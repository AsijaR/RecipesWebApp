import { Component, ContentChildren, EventEmitter, Input, OnInit, Output, QueryList } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormGroupDirective, FormGroupName } from '@angular/forms';
import { Direction } from '../add-recipe.component';

@Component({
  selector: 'app-directions',
  templateUrl: './directions.component.html',
  styleUrls: ['./directions.component.css']
})
export class DirectionsComponent implements OnInit {
  
  directionsForm: FormGroup;
  count=0;
  directionsArray:Direction[]=[]; 
  required:any[];
  constructor(private fb:FormBuilder) {
    
    this.directionsForm = this.fb.group({
      directions: this.fb.array([this.fb.group(new Direction)])
    });
   }
  
  ngOnInit(): void {
   
  }
  get directions() : FormArray {
    return this.directionsForm.get("directions") as FormArray
  }
 
  newDirection(): FormGroup {
    return this.fb.group(new Direction())
  }
 
 setDirections(setDirections: Direction[]): FormArray {
  const formArray = new FormArray([]);
  setDirections.forEach(d => {
    formArray.push(this.fb.group({
      direction: d.direction
    }));
  });
  this.count=setDirections.length-1;
  return formArray;
}

  createComponent(i:number) {
    if(i==this.count){
      this.directions.push(this.newDirection()); 
      this.count++;
  }
  }
 
  removeSkill(i:number) {
    if(i==0) return;
    this.directions.removeAt(i);
    this.count--;
  }
 
  onSubmit() {
    console.log(this.directionsForm.value);
  }
}
