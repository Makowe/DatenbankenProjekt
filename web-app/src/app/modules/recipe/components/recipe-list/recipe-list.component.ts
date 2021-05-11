import { Component, EventEmitter, OnChanges, OnInit, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Recipe } from 'src/app/models/recipe';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-recipe-list',
    templateUrl: './recipe-list.component.html',
    styleUrls: ['./recipe-list.component.scss']
})
export class RecipeListComponent implements OnInit {

    @Output() selectedRecipe = new EventEmitter<number>();

    allRecipes: Recipe[] = [];

    constructor(private dataService: DataService, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit() {
        this.loadAllRecipes();
    }

    loadAllRecipes(): void {
        this.dataService.getAllRecipes().subscribe(
            (data: Recipe[]) => {
                this.allRecipes = data;
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    selectRecipe(id: number | undefined): void {
        this.router.navigate(['Recipe', 'Show', id],);
    }

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'add':
                this.router.navigate(['Recipe', 'New'],);
        }
    }
}
