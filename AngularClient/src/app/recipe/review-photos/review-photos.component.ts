import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions, NgxGalleryOrder } from '@kolkov/ngx-gallery';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { Recipe } from 'src/app/model/recipe.model';
import { RecipePhoto } from 'src/app/model/recipePhoto.model';
import { User } from 'src/app/model/user.model';
import { AccountService } from 'src/app/service/account.service';
import { UserProfileComponent } from 'src/app/user-profile/user-profile.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-review-photos',
  templateUrl: './review-photos.component.html',
  styleUrls: ['./review-photos.component.css']
})
export class ReviewPhotosComponent implements OnInit {

  @Input() photos:any;
  @Input() recipeId:number;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  uploader:FileUploader;
  recipeOwnerisLogged:boolean;
  hasBaseDropZoneOver:boolean;
  user: User;
  baseUrl = environment.apiUrl+"Recipes/add-photo/";
  constructor(private accountService: AccountService,private snackbar: MatSnackBar) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>
      {
        if(user!=null)
          this.user=user;
      }
      );
  }

  ngOnInit(): void {
    if(this.user!=null||this.user!=undefined)
      this.initializeUploader();
    this.galleryOptions = [
      {
         width: '80em',
         height: '20em',
        //imagePercent: 100,
        image:false,
        thumbnailsColumns: 4,
        thumbnailsMoveSize:1,
        thumbnailsSwipe:true,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailsRemainingCount :true,
        //preview: true,
        previewCloseOnClick:true,
        previewFullscreen:true,
        previewArrows:true,
        preview:true,
       previewAnimation :true,
       thumbnailsPercent: 2,
      previewKeyboardNavigation:true,
     
      },{ "breakpoint": 500, "width": "100%" }
    ]

    this.galleryImages = this.getImages();
  }
  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.photos) {
      if(!photo.isMain){
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    }
    return imageUrls;
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl+this.recipeId ,
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

   this.uploader.onAfterAddingFile = (file) => {
     file.withCredentials = false;
   } 
   this.uploader.onSuccessItem = (item, response, status, headers) => {
    if (response) {
      const photo: RecipePhoto = JSON.parse(response);
      this.photos.push(photo);
      this.galleryImages = this.getImages();
      let snackRef = this.snackbar.open("Image is added", "", {
        duration: 10 * 1000,
        panelClass: ["opa"],
        verticalPosition: 'bottom',
        horizontalPosition: 'center'
      });
    }
  }
 }
}
