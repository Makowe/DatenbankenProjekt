import { EventEmitter } from '@angular/core';
import { Component, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

    @Output() clickMenu = new EventEmitter<void>();

    constructor(public router: Router) { }

    ngOnInit(): void {
    }

}
