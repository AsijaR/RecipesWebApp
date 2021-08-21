import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ɵɵstylePropInterpolate7 } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Bookmark } from '../model/bookmark.model';
import { Recipe } from '../model/recipe.model';
import { BookmarkService } from '../service/bookmark.service';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.component.html',
  styleUrls: ['./bookmark.component.css']
})
export class BookmarkComponent implements OnInit {

  public bookmark: Bookmark;
  //public recipes:Observable<Recipe[]>;
  constructor(private bookmarkService:BookmarkService,private router:Router,private snackBar: MatSnackBar) { 
  }
  ngOnInit(): void {
  this.bookmarkService.getBookmarks().subscribe(
    r=>{
      console.log("ovo mi je "+JSON.stringify(r));
      this.bookmark=r;
    });
  }
  reloadCurrentRoute() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate([currentUrl]);
    });
}
  removeFromBookmark(recipeId)
  {
    this.bookmarkService.removeRecipe(recipeId).subscribe(res=>
      {
        let snackRef = this.snackBar.open(res, null, {
          duration: 10 * 1000,
          panelClass: ["opa"],
          verticalPosition: 'bottom',
          horizontalPosition: 'center'
        }); 
      
      });
      this.bookmark.recipes.forEach((rec,index)=>{
        if(rec.recipeId==recipeId) this.bookmark.recipes.splice(index,1);
      });
    }
}
