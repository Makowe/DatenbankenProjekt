import { Component, EventEmitter, OnChanges, OnInit, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Recipe } from 'src/app/models/recipe';
import { Tag } from 'src/app/models/tag';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-recipe-list',
    templateUrl: './recipe-list.component.html',
    styleUrls: ['./recipe-list.component.scss']
})
export class RecipeListComponent implements OnInit {

    @Output() selectedRecipe = new EventEmitter<number>();

    allRecipes: Recipe[] = [];
    filteredRecipes: Recipe[] = [];

    availableTags: Tag[] = [];
    filterTags: Tag[] = [];
    filterString = '';

    constructor(private dataService: DataService, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit() {
        this.loadAllRecipes();
        this.loadAllTags();
    }

    loadAllRecipes(): void {
        this.dataService.getAllRecipes().subscribe(
            (data: Recipe[]) => {
                if (data != undefined) {
                    this.allRecipes = data.sort((a, b) => a.name > b.name ? 1 : -1);
                }
                this.updateFilter();
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

    updateFilter(): void {
        this.filteredRecipes = this.allRecipes.filter((recipe: Recipe) => {
            if (!recipe.name.toLowerCase().includes(this.filterString.toLowerCase())) { return false; }
            if (this.filterTags.length == 0) { return true; }

            let allTagsContained = true;
            this.filterTags.forEach(tag => {
                allTagsContained = allTagsContained && recipe.tags != undefined && recipe.tags.map(tag => tag.name).includes(tag.name);
            });
            return allTagsContained;
        });
    }

    selectRecipe(id: number | undefined): void {
        this.router.navigate(['Recipe', 'Show', id],);
    };

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'add':
                this.router.navigate(['Recipe', 'New'],);
        }
    }
}
