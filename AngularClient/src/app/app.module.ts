import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material/material.module';
import { RecipeComponent } from './recipe/recipe.component';
import { RecipeCommentsComponent } from './recipe/recipe-comments/recipe-comments.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NavigationComponent } from './navigation/navigation.component';
import { LeftNavigationComponent } from './navigation/left-navigation/left-navigation.component';
import { TopNavigationComponent } from './navigation/top-navigation/top-navigation.component';
import { RecipeDescriptionComponent } from './recipe/recipe-description/recipe-description.component';
import { RecipeInformationComponent } from './recipe/recipe-information/recipe-information.component';
import { RecipeIngredientsComponent } from './recipe/recipe-ingredients/recipe-ingredients.component';
import { RecipeNotesComponent } from './recipe/recipe-notes/recipe-notes.component';
import { AddCommentComponent } from './recipe/recipe-comments/add-comment/add-comment.component'; 
import { FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BookmarkComponent } from './bookmark/bookmark.component';
import { RecipesComponent } from './recipes/recipes.component';
import { RecipeCardsComponent } from './recipe-cards/recipe-cards.component';
import { AddRecipeComponent } from './add-recipe/add-recipe.component';
import { BasicInfoComponent } from './add-recipe/basic-info/basic-info.component';
import { OrderComponent } from './add-recipe/order/order.component';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { IngredientsComponent } from './add-recipe/ingredients/ingredients.component';
import { DirectionsComponent } from './add-recipe/directions/directions.component';
import { OrderMealComponent } from './recipe/order-meal/order-meal.component';
import { SearchBoxComponent } from './search-box/search-box.component';
import { MyRecipesComponent } from './my-recipes/my-recipes.component';
import { OrdersComponent } from './orders/orders.component';
import { RecipeOrdersComponent } from './orders/recipe-orders/recipe-orders.component';
import { MyOrdersComponent } from './orders/my-orders/my-orders.component';
import { DeleteDialogComponent } from './recipe-cards/delete-dialog/delete-dialog.component';
import { JwtInterceptor } from './interceptor/jwt.interceptor';
import { TextInputComponent } from './form/text-input/text-input.component';
import { AddHeaderPhotoComponent } from './add-recipe/add-header-photo/add-header-photo.component';
import {FileUploadModule} from 'ng2-file-upload';
import { ReviewPhotosComponent } from './recipe/review-photos/review-photos.component';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { SearchIngredientsDialogComponent } from './search-box/search-ingredients-dialog/search-ingredients-dialog.component';
import { LoginDialogComponent } from './login-dialog/login-dialog.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AdminComponent } from './admin/admin.component';
import { HasRoleDirective } from './directives/has-role.directive';
@NgModule({
  declarations: [
    AppComponent,
    RecipeComponent,
    RecipeCommentsComponent,
    NavigationComponent,
    LeftNavigationComponent,
    TopNavigationComponent,
    RecipeDescriptionComponent,
    RecipeInformationComponent,
    RecipeIngredientsComponent,
    RecipeNotesComponent,
    AddCommentComponent,
    BookmarkComponent,
    RecipesComponent,
    RecipeCardsComponent,
    AddRecipeComponent,
    BasicInfoComponent,
    OrderComponent,
    IngredientsComponent,
    DirectionsComponent,
    OrderMealComponent,
    SearchBoxComponent,
    MyRecipesComponent,
    OrdersComponent,
    RecipeOrdersComponent,
    MyOrdersComponent,
    DeleteDialogComponent,
    TextInputComponent,
    AddHeaderPhotoComponent,
    ReviewPhotosComponent,
    UserProfileComponent,
    SearchIngredientsDialogComponent,
    LoginDialogComponent,
    AdminComponent,
    HasRoleDirective,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    AngularSvgIconModule.forRoot(),
    FlexLayoutModule,
    ReactiveFormsModule,
    FileUploadModule,
    NgxGalleryModule,
    TabsModule.forRoot(),
    PaginationModule.forRoot()
  ],
  providers: [
    FormGroupDirective,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
