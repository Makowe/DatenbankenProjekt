import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { ResponseMessage } from 'src/app/models/responseMessage';
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
                if (data != undefined) {
                    this.component = data;
                }
            },
            error => {
                this.snackbar.open('Verbindung zum Server konnte nicht hergestellt werden', 'Schließen', { duration: 5000 });
            }
        );
    }

    toolbarClicked(buttonName: string): void {
        switch (buttonName) {
            case 'undo':
                this.router.navigate(['Component']);
                break;
            case 'edit':
                this.router.navigate(['Component', 'Edit', this.id],);
                break;
            case 'add':
                this.router.navigate(['Component', 'New']);
                break;
            case 'delete':
                this.deleteComponent();
        }
    }

    deleteComponent(): void {
        this.dataService.deleteComponent(this.id).subscribe(
            (returnValue: ResponseMessage) => {
                this.snackbar.open(returnValue.message, 'Schließen', { duration: 5000 });
                this.router.navigate(['Component']);
            }
        );
    }
}
