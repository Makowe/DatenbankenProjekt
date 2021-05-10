import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecipeListComponent } from './components/recipe-list/recipe-list.component';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RecipeDetailComponent } from './components/recipe-detail/recipe-detail.component';
import { MainModule } from '../main/main.module';
import { RecipeEditComponent } from './components/recipe-edit/recipe-edit.component';
import { RecipeNewComponent } from './components/recipe-new/recipe-new.component';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';


@NgModule({
    declarations: [
        RecipeListComponent,
        RecipeDetailComponent,
        RecipeEditComponent,
        RecipeNewComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        BrowserAnimationsModule,
        MatCardModule,
        MatToolbarModule,
        MatSidenavModule,
        MatButtonModule,
        MatIconModule,
        CommonModule,
        MatButtonModule,
        MatIconModule,
        MainModule,
        MatInputModule,
        FormsModule,
        ReactiveFormsModule,
        MatAutocompleteModule,
        MatSelectModule

    ],
    exports: [
        RecipeListComponent,
        RecipeDetailComponent,
        RecipeEditComponent,
        RecipeNewComponent
    ],
    schemas: [
        CUSTOM_ELEMENTS_SCHEMA
    ]
})
export class RecipeModule { }
