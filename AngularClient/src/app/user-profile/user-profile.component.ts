import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FileUploader } from 'ng2-file-upload';
import { catchError, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../model/member.model';
import { User } from '../model/user.model';
import { UserPhoto } from '../model/userPhoto.model';
import { AccountService } from '../service/account.service';
import { MemberService } from '../service/member.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  member: Member;
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  baseUrl = environment.apiUrl + "User/add-photo/";
  errorMatchPassword = false;
  wrongPassword = false;
  errorMessage:string="";
  sameValue = false;
  profileForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    address: new FormControl(),
    city: new FormControl(""),
    state: new FormControl(""),
    zip: new FormControl("",[Validators.pattern("^[0-9]*$")])
  });
  shippingPriceForm = new FormGroup({
    shippingPrice: new FormControl('',[Validators.pattern("^[0-9]*$")])
  });
  changePasswordForm = new FormGroup({
    currentPassword: new FormControl('',[Validators.required]),
    newPassword: new FormControl('', [Validators.required, Validators.pattern('(?=\\D*\\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}')]),
    newPassword2: new FormControl('',[Validators.required])
  });
  user: User;
  shippingPrice = new FormControl();
  constructor(public memberService: MemberService, private accountService: AccountService, private snackbar: MatSnackBar) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
  
    this.initializeUploader();
    this.memberService.getMember().subscribe(m => {
      this.member = m;
    }) 
     
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
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      console.log(JSON.parse(response));
      if (response) {
        console.log("uspelo je");
        const photo: UserPhoto = JSON.parse(response);
        this.member.photoUrl = photo.url;
        //  this.accountService.setCurrentUser(this.user);
      }
    }
  }
  uploadPhoto() {
    this.uploader.uploadAll();

  }
  updateProfile(data: any) {
    this.memberService.updateMemberInfo(data).subscribe(x => {
      let snackRef = this.snackbar.open("Data successfully changed", "", {
        duration: 10 * 1000,
        panelClass: ["opa"],
        verticalPosition: 'bottom',
        horizontalPosition: 'center'
      });
    }
    );
  }
  get newPassword() { return this.changePasswordForm.get('newPassword'); }
  updateShippingPrice(data: any) {
    this.memberService.updateShippingPrice(data).subscribe(x => {
      let snackRef = this.snackbar.open("Data successfully changed", "", {
        duration: 10 * 1000,
        panelClass: ["opa"],
        verticalPosition: 'bottom',
        horizontalPosition: 'center'
      });
    }
    );
  }
  updatePassword(data: any) {
    if (this.changePasswordForm.get('newPassword').value != this.changePasswordForm.get('newPassword2').value)
      this.errorMatchPassword = true;
      // if (this.changePasswordForm.valid) {
    this.memberService.changePassword(data).subscribe(x => {
        // let snackRef = this.snackbar.open("Password successfully changed", "", {
        //   duration: 10 * 1000,
        //   panelClass: ["opa"],
        //   verticalPosition: 'bottom',
        //   horizontalPosition: 'center'
        // });
      }, error => {
        if(error.status===200)
        {
          let snackRef = this.snackbar.open("Password successfully changed", "", {
            duration: 10 * 1000,
            panelClass: ["opa"],
            verticalPosition: 'bottom',
            horizontalPosition: 'center'
          });

        }
        else {
        this.wrongPassword = true;
        this.errorMessage=error.error;}
      }
      );
    //   }
    // console.log(this.changePasswordForm.valid.valueOf());
  }
}
