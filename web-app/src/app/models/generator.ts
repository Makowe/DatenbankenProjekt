import { componentFactoryName } from '@angular/compiler';
import { RecipeComponentModule } from '../modules/recipe-component/recipe-component.module';
import { RecipeComponent } from './recipeComponent';

export class modelGenerator {
    static getEmptyComponent(): RecipeComponent {
        let component: RecipeComponent = {
            name: '',
            id: undefined,
            unitShortname: '',
            unitName: '',
            amount: undefined
        };
        return component;
    }
}