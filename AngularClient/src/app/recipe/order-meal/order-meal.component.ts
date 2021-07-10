import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {  MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Order } from 'src/app/model/order.model';
import { Recipe } from 'src/app/model/recipe.model';
import * as moment from 'moment';
import { RecipeService } from 'src/app/service/recipe.service';
import { OrderService } from 'src/app/service/order.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-order-meal',
  templateUrl: './order-meal.component.html',
  styleUrls: ['./order-meal.component.css']
})
export class OrderMealComponent implements OnInit {

  @Input() recipeId:number;
  @Output() orderedMeal: EventEmitter<boolean> = new EventEmitter();
  
  shippingPrice:string;
  total:number=0;
  submitted=false;
  onSave = new EventEmitter();
  orderForm:FormGroup;
  public price:string;
  positivNumber=false;
   minDate = moment(new Date()).format('YYYY-MM-DD');

  constructor( public formBuilder: FormBuilder,
              public dialogRef: MatDialogRef<OrderMealComponent>,
              @Inject(MAT_DIALOG_DATA) public data: Recipe,
              public orderService:OrderService,
              private _snackBar: MatSnackBar) {
      this.total=data.price;
      this.price=`$ ${this.data.price}`;
     }

  ngOnInit(): void {
    this.intitializeForm();
    this.orderForm.get('servingNumber').valueChanges.subscribe(() => {
      if(this.orderForm.get('servingNumber').value>=0)
        this.total=this.data.price*this.orderForm.get('servingNumber').value+this.data.shippingPrice;
    });
    
  }
  intitializeForm() {
    this.orderForm = this.formBuilder.group({
      fullName:['',[Validators.required,Validators.pattern("^[a-zA-z ]*$")]],
      address:[ '',[Validators.required,Validators.pattern("^[a-zA-z0-9 ]*$")]],
      city:['',[Validators.required,Validators.pattern("^[a-zA-z ]*$")]],
      state:['',[Validators.required,Validators.pattern("^[a-zA-z ]*$")]],
      zip: ['',[Validators.required,Validators.pattern("^[0-9]*$")]],
      servingNumber:['',[Validators.required,Validators.pattern("^[0-9]*$")]],
      dateMealShouldBeShipped:['',[Validators.required]],
      noteToChef : ['',[Validators.pattern("^[a-zA-z0-9 ]*$")]],
      recipeId:[this.data.recipeId],
      total:[this.total]
    })
  }
  public get fullName() {
    //da li postoje gresle i validaciji i koje su to greske
    //oni imaju svojsto error ili su null ili validation error
    return this.orderForm.get('fullName');
  }
  onNoClick(): void {
    this.dialogRef.close();
    
  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

  validFiledsInForm(){
    const invalid = [];
    const controls = this.orderForm.controls;
    for (const name in controls) {
        if (controls[name].invalid) {
            invalid.push(name);
        }
    }
     console.log(invalid);
  }
  public submitForm(data:any) {
    if(this.orderForm.get('servingNumber').value<=0){
      this.positivNumber=true;
      return
    }
    this.submitted=true;
   if(this.orderForm.valid)
     {
        this.orderForm.get('total').setValue(this.data.price*this.orderForm.get('servingNumber').value+this.data.shippingPrice);
        this.orderService.makeOrder(this.orderForm.value).subscribe(
        res => {
          this._snackBar.open(res, 'Close', {
            duration: 4 * 1000,
            panelClass:["opa"],
            verticalPosition:'bottom',
            horizontalPosition:'center'
          });
         // this.orderForm.reset();
          this.orderedMeal.emit(true);
          
          },
        err => console.log('HTTP Error', err));
     }
  }
}
