import { componentFactoryName } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponent } from '../../../../models/recipeComponent';

@Component({
    selector: 'app-recipe-new',
    templateUrl: './recipe-new.component.html',
    styleUrls: ['./recipe-new.component.scss']
})
export class RecipeNewComponent implements OnInit {

    componentFormControl = new FormControl();
    recipeName: string = '';
    people: number = 2;
    showComponents: boolean = true;

    availableComponents: RecipeComponent[] = [];
    filteredComponents: RecipeComponent[] = [];

    chosenComponents: RecipeComponent[] = [];

    constructor(private dataService: DataService, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit() {
        this.loadAllComponents();

    }

    loadAllComponents(): void {
        this.dataService.getAllComponents().subscribe(
            (data: RecipeComponent[]) => {
                this.availableComponents = data;
                this.availableComponents = this.availableComponents.filter(component => component.id && component.id > 0);
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    mainToolbarClicked(action: string): void {

    }

    componentToolbar(action: string): void {
        switch (action) {
            case 'horizontal_rule':
                this.chosenComponents.pop();
                break;
            case 'add':
                let newComponent: RecipeComponent = { name: "Test", amount: 0, unitName: 'Gramm', unitShortname: 'g' };
                this.chosenComponents.push(newComponent);
                break;
            case 'print':
                console.log(this.chosenComponents);
        }


    }

    instructionToolbar(action: string): void {

    }

    updateComponentFilter(inputString: string) {
        console.log(inputString);
        this.filteredComponents = this.availableComponents.filter((component: RecipeComponent) => {
            return component.name.toLowerCase().includes(inputString.toLowerCase());
        });
    }
}
