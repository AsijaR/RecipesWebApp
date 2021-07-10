import { ChangeDetectionStrategy, Component, Input, OnChanges, OnInit } from '@angular/core';
import {  Comments } from 'src/app/model/comment.model';

@Component({
  selector: 'app-recipe-comments',
  templateUrl: './recipe-comments.component.html',
  styleUrls: ['./recipe-comments.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush    
})
export class RecipeCommentsComponent implements OnInit {

  @Input() comments:Comments[];
  constructor() { }

  ngOnInit(): void {
  }
  ngOnChanges() {
  }
}
