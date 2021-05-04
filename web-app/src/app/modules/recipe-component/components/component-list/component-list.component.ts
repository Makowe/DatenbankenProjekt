import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Recipe } from 'src/app/models/recipe';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponentModule } from '../../recipe-component.module';

@Component({
    selector: 'app-component-list',
    templateUrl: './component-list.component.html',
    styleUrls: ['./component-list.component.scss']
})
export class ComponentListComponent implements OnInit {

    @Output() selectedRecipe = new EventEmitter<number>();

    allComponents: RecipeComponent[] = [];

    constructor(private dataService: DataService, private router: Router) { }

    ngOnInit() {
        this.loadAllRecipes();
    }

    loadAllRecipes(): void {
        this.dataService.getAllComponents().subscribe((data: RecipeComponent[]) => {
            this.allComponents = data;
        });
    }

    selectComponent(id: number): void {
        this.router.navigate(['Component', id],);
    }

}
