import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Comments } from 'src/app/model/comment.model';
import { AccountService } from 'src/app/service/account.service';
import { RecipeService } from 'src/app/service/recipe.service';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent implements OnInit {

 

  @Output() newComment = new EventEmitter<Comments>();
  model: any = {};
  public errrorMessage:string="";

  public shortComment:boolean=false;
  public invalidChar:boolean=false;
  private recipeId= Number(this.route.snapshot.paramMap.get('recipeId'));
  constructor(public recipeService:RecipeService,public accountService: AccountService,
    public router:Router,public route:ActivatedRoute,
    ) {
    
   }

  ngOnInit(): void {
  }
  reloadCurrentRoute() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate([currentUrl]);
    });
}
keyPressAlphaNumericWithCharacters(event) {

  var inp = String.fromCharCode(event.keyCode);
  // Allow numbers, alpahbets, space, underscore
  if (/[a-zA-Z0-9-_ ]/.test(inp)) {
    return true;
  } else {
    event.preventDefault();
    return false;
  }
}
 PostAComment()
  {
    if(this.model.message.length<2){
     
      this.shortComment=true;
    }
    if (/[@#$%^&*{}:|<>]/.test(this.model.message)){
      this.invalidChar=true;
    }
    if( this.shortComment==false && this.invalidChar==false){
    this.recipeService.addCommentToRecipe(this.recipeId,this.model).subscribe(res=>
      {
        this.reloadCurrentRoute();
      })
      ,err=>
      {
        console.log("Error Occured " + err);
      }
    }
  }

}
