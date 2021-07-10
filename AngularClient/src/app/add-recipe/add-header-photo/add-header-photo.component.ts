import {Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { User } from 'src/app/model/user.model';
import { AccountService } from 'src/app/service/account.service';
import { RecipeService } from 'src/app/service/recipe.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-header-photo',
  templateUrl: './add-header-photo.component.html',
  styleUrls: ['./add-header-photo.component.css']
})
export class AddHeaderPhotoComponent implements OnInit {

  @Input() hasNoFiles:boolean;
  userChangesImage=false;
  uploader:FileUploader;
  hasBaseDropZoneOver:boolean;
  baseUrl = environment.apiUrl+"Recipes/add-photo/";
  user: User;
  fileToUpload:any;
  constructor(private accountService: AccountService,private recipeService:RecipeService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
    this.initializeUploader();
  }
  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
     this.uploader = new FileUploader({
       url: this.baseUrl,
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
    //this.uploader.response.subscribe( res =>console.log(res) );
     this.uploader.onSuccessItem = (item, response, status, headers) => {
       console.log(JSON.parse(response));
    //   if (response) {
    //     const photo: RecipePhoto = JSON.parse(response);
    //     this.recipe.photos.push(photo);
    //      if (photo.isMain) {
    //        this.user.photoUrl = photo.url;
    //        this.member.photoUrl = photo.url;
    //        this.accountService.setCurrentUser(this.user);
    //      }
    //   }
     }
  }
  onFileChange(event) {
    this.userChangesImage=true;
    console.log("hoce");
}
}
