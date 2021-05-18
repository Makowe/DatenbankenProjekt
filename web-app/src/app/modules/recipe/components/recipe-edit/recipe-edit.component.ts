import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Instruction } from 'src/app/models/instruction';
import { Recipe } from 'src/app/models/recipe';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { ResponseMessage } from 'src/app/models/responseMessage';
import { Tag } from 'src/app/models/tag';
import { Unit } from 'src/app/models/unit';
import { ComponentNewComponent } from 'src/app/modules/recipe-component/components/component-new/component-new.component';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-recipe-edit',
    templateUrl: './recipe-edit.component.html',
    styleUrls: ['./recipe-edit.component.scss']
})
export class RecipeEditComponent implements OnInit {

    id: number = 0;
    recipeName: string = '';
    people: number = 2;

    availableComponents: RecipeComponent[] = [];
    filteredComponents: RecipeComponent[] = [];
    availableUnits: Unit[] = [];
    availableTags: Tag[] = [];

    componentsLegal: boolean[] = [];

    currentTags: Tag[] = [];
    currentComponents: RecipeComponent[] = [];
    currentInstructions: Instruction[] = [];

    constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router, private snackbar: MatSnackBar, private dialog: MatDialog) {
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'];
        });
        this.loadRecipe(this.id);
        this.loadAllTags();
        this.loadAllComponents();
        this.loadAllUnits();
    }

    loadRecipe(recipeId: number): void {
        this.dataService.getRecipeById(recipeId).subscribe(
            (data: Recipe) => {
                this.recipeName = data.name;
                this.people = data.people;
                if (data.components != undefined) {
                    this.currentComponents = data.components;
                }
                if (data.instructions != undefined) {
                    this.currentInstructions = data.instructions;
                }
                if (data.tags != undefined) {
                    this.currentTags = data.tags;
                }
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    loadAllTags(): void {
        this.dataService.getAllTags().subscribe(
            (data: Tag[]) => {
                if (data != undefined) {
                    this.availableTags = data.sort((a, b) => a.name > b.name ? 1 : -1);
                }
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
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
                this.router.navigate(['Recipe', 'Show', this.id]);
        }
    }

    tagToolbar(action: string): void {
        switch (action) {
            case 'add':
                this.addTagToList();
                break;
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

    addTagToList(): void {
        let newTag: Tag = {
            name: "",
        };
        this.currentTags.push(newTag);
    }

    addComponentToList(): void {
        let newComponent: RecipeComponent = {
            name: '',
            amount: 0,
            unitName: 'Gramm',
            unitShortname: 'g'
        };
        this.currentComponents.push(newComponent);
        this.componentsLegal.push(false);
    }

    addInstructionToList(): void {
        let newInstruction: Instruction = {
            step: this.currentInstructions.length + 1,
            description: ''
        };
        this.currentInstructions.push(newInstruction);
    }

    checkComponentLegal(index: number): void {
        // the type component name is legal
        if (this.availableComponents.map(component => component.name).includes(this.currentComponents[index].name)) {
            this.componentsLegal[index] = true;
        }
        else {
            this.componentsLegal[index] = false;
        }
    }

    removeTag(index: number) {
        this.currentTags.splice(index, 1);
    }

    removeComponent(index: number) {
        this.currentComponents.splice(index, 1);
        this.componentsLegal.splice(index, 1);
    }

    removeInstruction(index: number) {
        this.currentInstructions.splice(index, 1);
    }

    saveRecipe() {
        let undefinedComponentIndex = -1;
        this.currentComponents.forEach((component, index) => {
            const existingComponent = this.availableComponents.find(availableComponent =>
                availableComponent.name === component.name
            );

            if (existingComponent === undefined) {
                component.id = 0;
                undefinedComponentIndex = index;
            }
            else {
                component.id = existingComponent.id;
            }
        });

        if (undefinedComponentIndex !== -1) {
            let snackbarRef = this.snackbar.open(`Die Zutat ${this.currentComponents[undefinedComponentIndex].name} exisitert nicht`, 'Hinzufügen', { duration: 10000 });

            snackbarRef.onAction().subscribe(() => {
                let dialogConfig = new MatDialogConfig;
                dialogConfig.width = '600px';
                dialogConfig.data = this.currentComponents[undefinedComponentIndex].name;
                dialogConfig.disableClose = true;
                dialogConfig.autoFocus = true;
                dialogConfig.hasBackdrop = true;
                let dialogRef = this.dialog.open(ComponentNewComponent, dialogConfig);

                dialogRef.afterClosed().subscribe(result => {
                    this.loadAllComponents();
                });
            });
            return;
        }

        const recipe: Recipe = {
            id: this.id,
            name: this.recipeName,
            people: this.people,
            components: this.currentComponents,
            instructions: this.currentInstructions,
            tags: this.currentTags
        };

        this.dataService.editRecipe(recipe).subscribe((returnValue: ResponseMessage) => {
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
