import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ComponentDetailComponent } from './modules/recipe-component/components/component-detail/component-detail.component';
import { ComponentListComponent } from './modules/recipe-component/components/component-list/component-list.component';
import { RecipeDetailComponent } from './modules/recipe/components/recipe-detail/recipe-detail.component';
import { RecipeListComponent } from './modules/recipe/components/recipe-list/recipe-list.component';
import { RecipeModule } from './modules/recipe/recipe.module';

const routes: Routes = [
    { path: 'Recipe', component: RecipeListComponent },
    { path: 'Recipe/:id', component: RecipeDetailComponent },
    { path: 'Component', component: ComponentListComponent },
    { path: 'Component/:id', component: ComponentDetailComponent }

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
