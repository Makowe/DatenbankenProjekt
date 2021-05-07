import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-sidenav',
    templateUrl: './sidenav.component.html',
    styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {

    constructor(private router: Router) { }

    @Output() triggerClose = new EventEmitter<void>();

    options: { name: string, route: string[]; }[] = [
        { name: 'Rezepte', route: ['Recipe'] },
        { name: 'Zutaten', route: ['Component'] },
        { name: 'Impressum', route: ['Impressum'] },
    ];

    ngOnInit(): void {
    }

    navigate(option: { name: string, route: string[]; }) {
        this.router.navigate(option.route);
        this.triggerClose.emit();
    }

}
