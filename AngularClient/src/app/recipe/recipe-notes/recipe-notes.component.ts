import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-recipe-notes',
  templateUrl: './recipe-notes.component.html',
  styleUrls: ['./recipe-notes.component.css']
})
export class RecipeNotesComponent implements OnInit {

  @Input() notes:string;
  constructor() { }

  ngOnInit(): void {
    
  }

}
