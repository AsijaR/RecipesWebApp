import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddRecipeComponent } from './add-recipe/add-recipe.component';
import { AdminComponent } from './admin/admin.component';
import { BookmarkComponent } from './bookmark/bookmark.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';
import { MyRecipesComponent } from './my-recipes/my-recipes.component';
import { NavigationComponent } from './navigation/navigation.component';
import { OrdersComponent } from './orders/orders.component';
import { RecipeComponent } from './recipe/recipe.component';
import { RecipesComponent } from './recipes/recipes.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

const routes: Routes = [
  { path: " ", component: NavigationComponent },
  { path: "category/:categoryId", component: RecipesComponent },
  { path: "recipe/:recipeId", component: RecipeComponent },
  { path: "recipes", component: RecipesComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: "bookmarks", component: BookmarkComponent },
      { path: "my-recipes", component: MyRecipesComponent },
      { path: "orders", component: OrdersComponent },
      { path: "add-recipe", component: AddRecipeComponent },
      { path: "add-recipe/:recipeId", component: AddRecipeComponent },
      { path: "my-profile", component:UserProfileComponent},
      {path: "admin", component: AdminComponent, canActivate: [AdminGuard]}]
  }

];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
