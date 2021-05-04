import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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

    constructor(private dataService: DataService, private router: Router) { }

    ngOnInit() {
        this.loadAllRecipes();
    }

    loadAllRecipes(): void {
        this.dataService.getAllRecipes().subscribe((data: Recipe[]) => {
            this.allRecipes = data;
        });
    }

    selectRecipe(id: number): void {
        this.router.navigate(['Recipe', id],);
    }
}
