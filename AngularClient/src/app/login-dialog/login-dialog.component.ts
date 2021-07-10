import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AccountService } from '../service/account.service';

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
export class LoginDialogComponent implements OnInit {

  @Input() c:boolean;
 signUpForm:FormGroup;
  loginUserForm = new FormGroup({
    username: new FormControl('',[Validators.required]),
    password: new FormControl('', [Validators.required])
  });
  registerForm = new FormGroup({
    username: new FormControl('',[Validators.required,Validators.min(4)]),
    firstName: new FormControl('', [Validators.required,Validators.pattern("^[a-zA-z ]*$")]),
    lastName: new FormControl('', [Validators.required,Validators.pattern("^[a-zA-z ]*$")]),
    email: new FormControl('', [Validators.required,Validators.email]),
    password: new FormControl('', [Validators.required, Validators.pattern('(?=\\D*\\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}')])
  });
  userLoged=false;
  serverErrorResponse=false;
  errorMessage:string;
  constructor(private accountService: AccountService,private dialogRef: MatDialogRef<LoginDialogComponent>) { }

  ngOnInit(): void {
  }

  changeTemplate(){
    this.c=!this.c;
  }
  login() {
    if(this.loginUserForm.valid){
      this.accountService.login(this.loginUserForm.value).subscribe(response => { 
        this.userLoged=true;
        this.dialogRef.close();
       },error=>{
         this.serverErrorResponse=true;
         this.errorMessage=error.error;
         if(error.status===401)
         this.errorMessage="Username or password is not correct.";
        });
      }
  }
  signUp() {
    if(this.registerForm.valid){
      this.accountService.register(this.registerForm.value).subscribe(response => { 
       //console.log("odgovor"+response);
        this.dialogRef.close();
       },error=>{
        this.serverErrorResponse=true;
         this.errorMessage=error.error.text;
      });
      }
  }
}
