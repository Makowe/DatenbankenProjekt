import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { Recipe } from 'src/app/models/recipe';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-recipe-detail',
    templateUrl: './recipe-detail.component.html',
    styleUrls: ['./recipe-detail.component.scss']
})
export class RecipeDetailComponent implements OnInit {

    recipe?: Recipe;
    id: number = 0;

    constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.id = params['id'];
        });
        this.loadRecipe(this.id);
    }

    loadRecipe(recipeId: number): void {
        console.log(`Get Recipe with id ${recipeId}`);
        this.dataService.getRecipeById(recipeId).subscribe((data: Recipe) => {
            this.recipe = data;
        });
    }

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'edit':
                this.router.navigate(['Recipe', 'Edit', this.id]);
                break;
            case 'delete':
                this.deleteRecipe();
        }
    }

    deleteRecipe(): void { }
}
