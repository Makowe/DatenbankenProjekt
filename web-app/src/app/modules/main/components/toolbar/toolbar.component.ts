import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
    selector: 'app-toolbar',
    templateUrl: './toolbar.component.html',
    styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {

    @Input() disabled: boolean[] = [];

    @Input() small: boolean = false;

    @Input() actions: string[] = [];

    @Output() clicked = new EventEmitter<string>();

    constructor() { }

    ngOnInit(): void {
    }

    sendOutput(action: string) {
        this.clicked.emit(action);
    }

    isDisabled(i: number): boolean {
        if (i >= this.disabled.length) { return false; }
        else { return this.disabled[i]; }
    }

}
