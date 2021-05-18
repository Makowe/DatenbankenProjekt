import { Instruction } from './instruction';
import { RecipeComponent } from './recipeComponent';
import { Tag } from './tag';

export interface Recipe {
    name: string;
    id?: number;
    people: number;
    components?: RecipeComponent[];
    instructions?: Instruction[];
    tags?: Tag[];
}