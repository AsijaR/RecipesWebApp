import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { Category } from 'src/app/model/category.model';
import { AccountService } from 'src/app/service/account.service';
import { CategoryService } from 'src/app/service/category.service';

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.css']
})
export class TopNavigationComponent implements OnInit {

  @Output() clicked = new EventEmitter<boolean>();
  @Input() userLogged;
  public categories:Category[];
  constructor(public accountService: AccountService, 
              private router: Router,
              private categoryService:CategoryService) { 
     this.categoryService.getCategories().subscribe(c=>
         this.categories=c);
  }

  ngOnInit(): void {
  }

  buttonClicked(){    
    this.clicked.emit(true);
  }
 
  reloadCurrentRoute() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate([currentUrl]);
    });
}
  logout() {
    this.userLogged=false;
    this.accountService.logout();
    this.router.navigateByUrl('/category/1')
  }
}
