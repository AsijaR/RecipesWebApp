import { Component, Output, OnInit, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';
import { Category } from 'src/app/model/category.model';
import { CategoryService } from 'src/app/service/category.service';

@Component({
  selector: 'app-basic-info',
  templateUrl: './basic-info.component.html',
  styleUrls: ['./basic-info.component.css']
})
export class BasicInfoComponent implements OnInit {

  @Input() formGroupName: string;
  @Input() submitted:boolean;

  public categories:Category[];
  basicInfoForm:FormGroup;
  complexity=["Simple","Medium","Expert"];
  constructor(public categoryService:CategoryService,private rootFormGroup: FormGroupDirective) { 
  
  }

  ngOnInit(): void {
  this.categoryService.getCategories().subscribe(cat=>{this.categories=cat;});
  this.basicInfoForm = this.rootFormGroup.control.get(this.formGroupName) as FormGroup;
  }

}
