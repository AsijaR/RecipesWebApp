import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Recipe } from '../model/recipe.model';
import { User } from '../model/user.model';
import { AdminService } from '../service/admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit,AfterViewInit  {

  users: User[];
  recipes:Recipe[];
  displayedColumns: string[] = ['UserId', 'Username', 'FullName', 'Delete'];
  displayedColumnsRecipe: string[] = ['title', 'Delete'];
  dataSourceR = new MatTableDataSource<Recipe>();
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private adminService: AdminService,private router:Router, private _snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.getAllUsers();
    this.getAllRecipes();
  }
  ngAfterViewInit() {
    this.dataSourceR.paginator = this.paginator;
  }
  getAllUsers() {
    this.adminService.getAllUsers().subscribe(users => {
      this.users = users.filter(x=>x.appUserId!=1);
    }, error => console.log(error));
  }
  deleteUser(data) {
    this.adminService.deleteUser(data).subscribe(users=>{
      //console.log(this.users);
    },error=>{
      this.users=this.users.filter(x=>x.appUserId!==data);
      this._snackBar.open("User is deleted", 'Close', {
        duration: 4 * 1000,
        panelClass:["opa"],
        verticalPosition:'bottom',
        horizontalPosition:'center'
      });
    });
  }
  getAllRecipes()
  {
    this.adminService.getRecipes().subscribe(res=>{
      this.recipes=res.result;
      console.log(this.recipes);
    }, error => console.log(error));
  }
  deleteRecipe(data)
  {
     this.adminService.deleteRecipe(data).subscribe(recipes=>{
      // this.recipes
    },error=>{
      this.recipes=this.recipes.filter(x=>x.recipeId!==data);
      this._snackBar.open("Recipe is deleted", 'Close', {
        duration: 4 * 1000,
        panelClass:["opa"],
        verticalPosition:'bottom',
        horizontalPosition:'center'
      });
    });
  }
}
