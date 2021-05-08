import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Recipe } from 'src/app/models/recipe';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { ResponseMessage } from 'src/app/models/responseMessage';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponentModule } from '../../recipe-component.module';

@Component({
    selector: 'app-component-edit',
    templateUrl: './component-edit.component.html',
    styleUrls: ['./component-edit.component.scss']
})
export class ComponentEditComponent implements OnInit {
    id: number = 0;

    componentName: string = '';

    constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.id = params['id'];
        });
        this.loadComponent(this.id);
    }

    loadComponent(recipeId: number): void {
        this.dataService.getComponentById(recipeId).subscribe(
            (data: RecipeComponent) => {
                this.componentName = data.name;
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    toolbarClicked(buttonName: string) {
        switch (buttonName) {
            case 'save':
                this.saveComponent();
                break;
            case 'close':
                this.router.navigate(['Component']);
                break;
        }
    }

    saveComponent(): void {
        console.log("save");
        let newComponent: RecipeComponent = {
            name: this.componentName,
            id: this.id,
        };

        this.dataService.editComponent(newComponent).subscribe(
            (returnValue: ResponseMessage) => {
                let id = returnValue.value;
                if (id) {
                    this.router.navigate(['Component', 'Show', id]);
                }
                this.snackbar.open(returnValue.message, 'Schließen', {
                    duration: 5000,
                });
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }
}
