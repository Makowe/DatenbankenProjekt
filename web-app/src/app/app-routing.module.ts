import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ComponentDetailComponent } from './modules/recipe-component/components/component-detail/component-detail.component';
import { ComponentEditComponent } from './modules/recipe-component/components/component-edit/component-edit.component';
import { ComponentListComponent } from './modules/recipe-component/components/component-list/component-list.component';
import { ComponentNewComponent } from './modules/recipe-component/components/component-new/component-new.component';
import { RecipeDetailComponent } from './modules/recipe/components/recipe-detail/recipe-detail.component';
import { RecipeEditComponent } from './modules/recipe/components/recipe-edit/recipe-edit.component';
import { RecipeListComponent } from './modules/recipe/components/recipe-list/recipe-list.component';
import { RecipeNewComponent } from './modules/recipe/components/recipe-new/recipe-new.component';

const routes: Routes = [
    { path: 'Recipe', component: RecipeListComponent },
    { path: 'Recipe/Show/:id', component: RecipeDetailComponent },
    { path: 'Recipe/Edit/:id', component: RecipeEditComponent },
    { path: 'Recipe/New', component: RecipeNewComponent },
    { path: 'Component', component: ComponentListComponent },
    { path: 'Component/Show/:id', component: ComponentDetailComponent },
    { path: 'Component/Edit/:id', component: ComponentEditComponent },
    { path: 'Component/New', component: ComponentNewComponent }

];

@NgModule({
    imports: [
        RouterModule.forRoot(
            routes,
            { enableTracing: false }
        ),
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
