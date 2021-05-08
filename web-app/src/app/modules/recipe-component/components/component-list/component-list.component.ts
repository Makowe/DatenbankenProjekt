import { componentFactoryName } from '@angular/compiler';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Recipe } from 'src/app/models/recipe';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponentModule } from '../../recipe-component.module';
import { ComponentNewComponent } from '../component-new/component-new.component';

@Component({
    selector: 'app-component-list',
    templateUrl: './component-list.component.html',
    styleUrls: ['./component-list.component.scss']
})
export class ComponentListComponent implements OnInit {

    @Output() selectedRecipe = new EventEmitter<number>();

    allComponents: RecipeComponent[] = [];

    constructor(private dataService: DataService, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit() {
        this.loadAllRecipes();
    }

    loadAllRecipes(): void {
        this.dataService.getAllComponents().subscribe(
            (data: RecipeComponent[]) => {
                this.allComponents = data;
                this.allComponents = this.allComponents.filter(component => component.id && component.id > 0);
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    selectComponent(id: number | undefined): void {
        this.router.navigate(['Component', 'Show', id],);
    }

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'add':
                this.router.navigate(['Component', 'New']);
                break;
        }
    }
}
