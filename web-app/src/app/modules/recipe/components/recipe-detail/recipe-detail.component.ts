import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { Recipe } from 'src/app/models/recipe';
import { ResponseMessage } from 'src/app/models/responseMessage';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-recipe-detail',
    templateUrl: './recipe-detail.component.html',
    styleUrls: ['./recipe-detail.component.scss']
})
export class RecipeDetailComponent implements OnInit {

    recipe?: Recipe;
    id: number = 0;

    constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.id = params['id'];
        });
        this.loadRecipe(this.id);
    }

    loadRecipe(recipeId: number): void {
        this.dataService.getRecipeById(recipeId).subscribe(
            (data: Recipe) => {
                this.recipe = data;
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'undo':
                this.router.navigate(['Recipe']);
                break;
            case 'edit':
                this.router.navigate(['Recipe', 'Edit', this.id]);
                break;
            case 'add':
                this.router.navigate(['Recipe', 'New']);
                break;
            case 'delete':
                this.deleteRecipe();
                this.router.navigate(['Recipe']);
        }
    }

    deleteRecipe(): void {
        this.dataService.deleteRecipe(this.id).subscribe(
            (returnValue: ResponseMessage) => {
                this.snackbar.open(returnValue.message, 'Schließen', { duration: 5000 });
                this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
                    this.router.navigate(['Recipe']);
                });
            }
        );
    }
}
