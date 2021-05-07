import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponentModule } from '../../recipe-component.module';

@Component({
    selector: 'app-component-detail',
    templateUrl: './component-detail.component.html',
    styleUrls: ['./component-detail.component.scss']
})
export class ComponentDetailComponent implements OnInit {

    component?: RecipeComponent;
    id: number = 0;

    constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.id = params['id'];
        });
        this.loadComponent(this.id);
    }

    loadComponent(recipeId: number): void {
        console.log(`Get Recipe with id ${recipeId}`);
        this.dataService.getComponentById(recipeId).subscribe((data: RecipeComponent) => {
            this.component = data;
        });
    }

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'edit':
                this.router.navigate(['Recipe', 'Edit', this.id],);
                break;
            case 'delete':
                this.deleteComponent();
        }
    }

    deleteComponent(): void { }
}
