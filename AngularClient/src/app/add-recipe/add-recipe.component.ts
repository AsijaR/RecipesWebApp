import { Component, ComponentFactoryResolver, ComponentRef, OnDestroy, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NewRecipe } from '../model/addRecipe.model';
import { Ingredient } from '../model/ingredient.model';
import { Recipe } from '../model/recipe.model';
import { RecipeService } from '../service/recipe.service';
import { AddHeaderPhotoComponent } from './add-header-photo/add-header-photo.component';
import { DirectionsComponent } from './directions/directions.component';
import { IngredientsComponent } from './ingredients/ingredients.component';


export class Direction {
  direction: string = "";
}

//  export class Ingredient{
//    amount:string;
//    name:string;
//  }

@Component({
  selector: 'app-add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.css']
})


export class AddRecipeComponent implements OnInit, OnDestroy {

  @ViewChild('dir') directions: DirectionsComponent;
  @ViewChild('ing') ingredients: IngredientsComponent;
  @ViewChild('photo') photoRecipe: AddHeaderPhotoComponent;

  public recipe: Recipe = new Recipe("", "", 0, "", "", "", false, 0, "", 0);
  count: number = 1;
  public recipeId: number;
  mainForm: FormGroup;
  public recipeDescription: string[] = [];
  private properiesValueChanges: Subscription;
  submitted = false;
  hasNoFiles = false;
  emptyIng = false;
  emptyDir = false;
  constructor(private recipeService: RecipeService,
    private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private snackbar: MatSnackBar) {

  }

  ngOnInit(): void {
    if (this.router.url === '/add-recipe') {
      this.inicializeEmptyForm();
    }
    else {
      this.inicializeEmptyForm();
      this.getRecipeById();

    }

    const priceFormControl = this.mainForm.get("orderInfo.price");
    const shippingNoteFormControl = this.mainForm.get("orderInfo.shippingNote");

    this.properiesValueChanges = this.mainForm.get("orderInfo.mealCanBeOrdered").valueChanges.subscribe(value => {
      if (!value) {
        priceFormControl.disable();
        shippingNoteFormControl.disable();
      } else {
        priceFormControl.enable();
        priceFormControl.setValidators([Validators.required,Validators.pattern("^[0-9]*$")]);
        shippingNoteFormControl.enable();
      }
      priceFormControl.updateValueAndValidity();
    });
  }
  selectedFile: File = null;
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
  }
  inicializeEmptyForm() {
    this.mainForm = this.fb.group({
      basicInfo: this.fb.group({
        title: ['', [Validators.required, Validators.pattern("^[a-zA-z0-9!-?@#$%^&* (),.]*$")]],
        complexity: [],
        category: ['', [Validators.required]],
        servingNumber: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
        preparationTime: ['', [Validators.required, Validators.pattern("^[0-9]*$")]]
      }),
      orderInfo: this.fb.group({
        price: [0, Validators.pattern("^[0-9]*$")],
        shippingNote: [null, [Validators.pattern("^[a-zA-z0-9?!@#$ ,.%^&*()]*$")]],
        mealCanBeOrdered: [false]
      }),
      noteForm: new FormControl(null, [Validators.pattern("^[a-zA-z0-9?!@#$%^& -?.,*()]*$")])
    });

  }
  getRecipeById() {
    this.recipeId = Number(this.route.snapshot.paramMap.get('recipeId'));
    this.recipeService.getRecipeById(this.recipeId).subscribe(recipe => {
      this.mainForm.get("basicInfo").patchValue({
        title: recipe.title,
        complexity: recipe.complexity,
        category: recipe.categoryId,
        servingNumber: recipe.servingNumber,
        preparationTime: recipe.timeNeededToPrepare
      });
      this.mainForm.get("orderInfo").patchValue({
        price: recipe.price,
        //maxServingNumber: [],
        shippingNote: recipe.noteForShipping,
        mealCanBeOrdered: recipe.mealCanBeOrdered
      });
      var directions = recipe.description.split(';');
      var c: Direction[] = [];
      var emptyDir: Direction = new Direction();
      emptyDir.direction = "";
      directions.forEach(x => c.push({ direction: x }))
      c.push(emptyDir);
      var p = this.directions.setDirections(c);
      this.directions.directionsForm.setControl("directions", p);

      var ingredients: Ingredient[] = recipe.ingredients;
      var emptyIng: Ingredient = new Ingredient("", "");
      ingredients.push(emptyIng);
      var i = this.ingredients.setDirections(ingredients);
      this.ingredients.ingredientsForm.setControl("ingredients", i)
      this.mainForm.get("noteForm").setValue(recipe.note);
    });
  }



  submitRecipe() {
    this.submitted = true;
    //console.log(this.mainForm.value);
    var dirValues = this.directions.directionsForm.get('directions') as FormArray;
    var dirArray = dirValues.value as Array<Direction>;
    if (dirArray.length === 1) { this.emptyDir = true; }
    else this.emptyDir = false;
    var description = dirArray.filter(d => d.direction !== "").map(e => e.direction).join(";").slice(0, -1);
    var c = new Recipe(this.mainForm.get("basicInfo.title").value,
      this.mainForm.get("basicInfo.complexity").value,
      this.mainForm.get("basicInfo.servingNumber").value,
      this.mainForm.get("basicInfo.preparationTime").value,
      description,
      this.mainForm.get("noteForm").value,
      this.mainForm.get("orderInfo.mealCanBeOrdered").value,
      this.mainForm.get("orderInfo.price").value,
      this.mainForm.get("orderInfo.shippingNote").value,
      this.mainForm.get("basicInfo.category").value);

    var ingValues = this.ingredients.ingredientsForm.get('ingredients') as FormArray;
    var ingArray = ingValues.value as Array<Ingredient>;
    let n: Ingredient[] = [];
    if (ingArray.length === 1) { this.emptyIng = true; }
    else this.emptyIng = false;
    ingArray.filter(i => i.amount !== "" || i.name).map(d => n.push(d));


    var newRecipe = new NewRecipe(c, n);

    if (this.mainForm.valid && !this.emptyDir && !this.emptyIng) 
    {
      if (this.recipeId != null) 
      {
        if (this.photoRecipe.userChangesImage) {
          this.photoRecipe.uploader.onBeforeUploadItem = (item) => {
            item.url = this.photoRecipe.baseUrl + this.recipeId;
          };
          this.photoRecipe.uploader.uploadAll();
          let snackRef = this.snackbar.open('Recipe is succefully updated', null, {
            duration: 10 * 1000,
            panelClass: ["opa"],
            verticalPosition: 'bottom',
            horizontalPosition: 'center'
          });
        }
        else
        {this.recipeService.editRecipe(this.recipeId, newRecipe).subscribe(res => {
          
        }, error => {
          if (error.status == 200) {
            let snackRef = this.snackbar.open('Recipe is succefully updated', null, {
              duration: 10 * 1000,
              panelClass: ["opa"],
              verticalPosition: 'bottom',
              horizontalPosition: 'center'
            });
          }
        });
      }
      }
      else 
      {
        if (this.photoRecipe.uploader.queue.length == 0)
          this.hasNoFiles = true;
        else this.hasNoFiles = false;
        if (!this.hasNoFiles) {
          this.recipeService.addNewRecipe(newRecipe).subscribe(res => {
            this.photoRecipe.uploader.onBeforeUploadItem = (item) => {
              item.url = this.photoRecipe.baseUrl + res
            }
            this.photoRecipe.uploader.uploadAll();
            let snackRef = this.snackbar.open('Recipe is succefully added', null, {
              duration: 10 * 1000,
              panelClass: ["opa"],
              verticalPosition: 'bottom',
              horizontalPosition: 'center'
            });
          }
          );
        }
      }
    }
  }

  ngOnDestroy(): void {
    this.properiesValueChanges?.unsubscribe()
  }
}