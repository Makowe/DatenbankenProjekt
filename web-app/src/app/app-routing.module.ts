import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RecipeDetailComponent } from './modules/recipe/components/recipe-detail/recipe-detail.component';
import { RecipeListComponent } from './modules/recipe/components/recipe-list/recipe-list.component';
import { RecipeModule } from './modules/recipe/recipe.module';

const routes: Routes = [
    { path: '', component: RecipeListComponent },
    { path: 'Recipe', component: RecipeDetailComponent }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            routes,
            { enableTracing: true }
        ),
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
