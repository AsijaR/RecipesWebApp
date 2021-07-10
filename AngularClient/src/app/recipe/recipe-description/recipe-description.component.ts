import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-recipe-description',
  templateUrl: './recipe-description.component.html',
  styleUrls: ['./recipe-description.component.css']
})
export class RecipeDescriptionComponent implements OnInit {

  @Input() recipeDescription:[];
  constructor() { }

  ngOnInit(): void {
  }

}
