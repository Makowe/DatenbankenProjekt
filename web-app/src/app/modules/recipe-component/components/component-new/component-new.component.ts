import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { modelGenerator } from 'src/app/models/generator';
import { RecipeComponent } from 'src/app/models/recipeComponent';
import { ResponseMessage } from 'src/app/models/responseMessage';
import { DataService } from 'src/app/services/data.service';
import { RecipeComponentModule } from '../../recipe-component.module';

@Component({
    selector: 'app-component-new',
    templateUrl: './component-new.component.html',
    styleUrls: ['./component-new.component.scss']
})
export class ComponentNewComponent implements OnInit {

    componentName: string = '';
    constructor(private dataService: DataService, private router: Router, private snackbar: MatSnackBar) { }

    ngOnInit(): void { }

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
            unitName: '',
            unitShortname: ''
        };

        this.dataService.postNewComponent(newComponent).subscribe(
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
