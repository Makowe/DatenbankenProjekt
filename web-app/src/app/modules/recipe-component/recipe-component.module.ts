import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentListComponent } from './components/component-list/component-list.component';
import { ComponentDetailComponent } from './components/component-detail/component-detail.component';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ComponentNewComponent } from './components/component-new/component-new.component';
import { ComponentEditComponent } from './components/component-edit/component-edit.component';
import { MainModule } from '../main/main.module';


@NgModule({
    declarations: [
        ComponentListComponent,
        ComponentDetailComponent,
        ComponentNewComponent,
        ComponentEditComponent
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
        MainModule
    ],
    exports: [
        ComponentListComponent,
        ComponentDetailComponent,
        ComponentNewComponent,
        ComponentEditComponent
    ],
    schemas: [
        CUSTOM_ELEMENTS_SCHEMA
    ]
})
export class RecipeComponentModule { }
