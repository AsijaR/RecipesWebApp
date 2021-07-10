import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { LoginDialogComponent } from '../login-dialog/login-dialog.component';
import { User } from '../model/user.model';
import { AccountService } from '../service/account.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  userLogged=false;
 constructor(public dialog: MatDialog,public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.setCurrentUser();
  }
submitForm(data){
    console.log(data);
  }
  openLoginDialog(clicked)
  {
    if(clicked)
    {
      const dialogRef = this.dialog.open(LoginDialogComponent);
      dialogRef.afterClosed().subscribe(()=>{
         this.userLogged=dialogRef.componentInstance.userLoged;
      });
    }
  }
  
  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this.accountService.setCurrentUser(user);
      this.userLogged=true;
    }
  }
  
}
