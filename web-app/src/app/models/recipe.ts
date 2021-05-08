import { Instruction } from './instruction';
import { RecipeComponent } from './recipeComponent';

export interface Recipe {
    name: string;
    id?: number;
    people: number;
    components?: RecipeComponent[];
    instructions?: Instruction[];
}