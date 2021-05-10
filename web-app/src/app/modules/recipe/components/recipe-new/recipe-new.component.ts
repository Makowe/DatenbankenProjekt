import { componentFactoryName } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Observable, Observer, Subject } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Instruction } from 'src/app/models/instruction';
import { Recipe } from 'src/app/models/recipe';
import { ResponseMessage } from 'src/app/models/responseMessage';
import { Unit } from 'src/app/models/unit';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponent } from '../../../../models/recipeComponent';

@Component({
    selector: 'app-recipe-new',
    templateUrl: './recipe-new.component.html',
    styleUrls: ['./recipe-new.component.scss']
})
export class RecipeNewComponent implements OnInit {

    recipeName: string = '';
    people: number = 2;

    availableComponents: RecipeComponent[] = [];
    filteredComponents: RecipeComponent[] = [];
    currentComponents: RecipeComponent[] = [];

    availableUnits: Unit[] = [];
    filteredUnits: Unit[] = [];

    currentInstructions: Instruction[] = [];

    constructor(private dataService: DataService, private router: Router, private snackbar: MatSnackBar) {
    }

    ngOnInit() {
        this.loadAllComponents();
        this.loadAllUnits();
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

    loadAllUnits(): void {
        this.dataService.getAllUnits().subscribe(
            (data: Unit[]) => {
                this.availableUnits = data;
                console.log(this.availableUnits);
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    mainToolbarClicked(action: string): void {
        switch (action) {
            case 'save':
                this.saveRecipe();
                break;
            case 'close':
                this.router.navigate(['Recipe']);
        }
    }

    componentToolbar(action: string): void {
        switch (action) {
            case 'add':
                this.addComponentToList();
                break;
        }
    }

    instructionToolbar(action: string): void {
        switch (action) {
            case 'add':
                this.addInstructionToList();
                break;
        }
    }

    updateComponentFilter(inputString: string): void {
        this.filteredComponents = this.availableComponents.filter((component: RecipeComponent) => {
            return component.name.toLowerCase().includes(inputString.toLowerCase());
        });
    }

    addComponentToList(): void {
        let newComponent: RecipeComponent = {
            name: '',
            amount: 0,
            unitName: 'Gramm',
            unitShortname: 'g'
        };
        this.currentComponents.push(newComponent);
    }

    addInstructionToList(): void {
        let newInstruction: Instruction = {
            step: this.currentInstructions.length + 1,
            description: ''
        };
        this.currentInstructions.push(newInstruction);
    }

    removeComponent(index: number) {
        this.currentComponents.splice(index, 1);
    }

    removeInstruction(index: number) {
        this.currentComponents.splice(index, 1);
    }

    saveRecipe() {
        const recipe: Recipe = {
            name: this.recipeName,
            people: this.people,
            components: this.currentComponents,
            instructions: this.currentInstructions
        };

        this.dataService.postNewRecipe(recipe).subscribe((returnValue: ResponseMessage) => {
            let id = returnValue.value;
            if (id) {
                this.router.navigate(['Recipe', 'Show', id]);
            }
            this.snackbar.open(returnValue.message, 'Schließen', { duration: 5000 });
        },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }
}
