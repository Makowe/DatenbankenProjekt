import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MAT_DIALOG_SCROLL_STRATEGY_FACTORY } from '@angular/material/dialog';
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

    constructor(
        private dataService: DataService,
        private router: Router,
        private snackbar: MatSnackBar,
        @Optional() public dialogRef?: MatDialogRef<ComponentNewComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) public data: string = '') {
        if (dialogRef != undefined) { this.componentName = data; }
    }

    ngOnInit(): void { }

    toolbarClicked(buttonName: string) {
        switch (buttonName) {
            case 'save':
                this.saveComponent();
                break;
            case 'close':
                if (this.dialogRef) { this.closeDialog(); }
                else { this.router.navigate(['Component']); }
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
                    if (this.dialogRef) { this.closeDialog(); }
                    else { this.router.navigate(['Component', 'Show', id]); }
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

    closeDialog(): void {
        this.dialogRef?.close();
    }
}
