import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Recipe } from '../model/recipe.model';
import { User } from '../model/user.model';
import { AdminService } from '../service/admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  users: User[];
  recipes:Recipe[];
  displayedColumns: string[] = ['UserId', 'Username', 'FullName', 'Delete'];
  displayedColumnsRecipe: string[] = ['title', 'chefName', 'Delete'];
  
  constructor(private adminService: AdminService,private router:Router, private _snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.getAllUsers();
    this.getAllRecipes();
  }
  getAllUsers() {
    this.adminService.getAllUsers().subscribe(users => {
     this.users = users.filter(x=>x.appUserId!=1);
    }, error => console.log(error));
  }
  deleteUser(data) {
    this.adminService.deleteUser(data).subscribe(users=>{
      this._snackBar.open("User is deleted", 'Close', {
        duration: 4 * 1000,
        panelClass:["opa"],
        verticalPosition:'bottom',
        horizontalPosition:'center'
      });
     this.router.navigateByUrl('/admin');
    });

  }
  getAllRecipes()
  {
    this.adminService.getRecipes().subscribe(res=>{
      this.recipes=res.result;
    }, error => console.log(error));
  }
  deleteRecipe(data)
  {
    this.adminService.deleteRecipe(data).subscribe(users=>{
      this._snackBar.open("Recipe is deleted", 'Close', {
        duration: 4 * 1000,
        panelClass:["opa"],
        verticalPosition:'bottom',
        horizontalPosition:'center'
      });
     this.router.navigateByUrl('/admin');
    });
  }
}
