import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Recipe } from '../models/recipe';
import { RecipeComponent } from '../models/recipeComponent';
import { ResponseMessage } from '../models/responseMessage';
import { Tag } from '../models/tag';
import { Unit } from '../models/unit';
import { RecipeComponentModule } from '../modules/recipe-component/recipe-component.module';

@Injectable({
    providedIn: 'root'
})
export class DataService {

    apiUrl: string = environment.apiUrl;

    constructor(private http: HttpClient) { }

    //RECIPES

    getAllRecipes(): Observable<Recipe[]> {
        return this.http.get<Recipe[]>(`${this.apiUrl}Recipe`);
    }

    getRecipeById(id: number): Observable<Recipe> {
        return this.http.get<Recipe>(`${this.apiUrl}Recipe/${id}`);
    }

    postNewRecipe(recipe: Recipe): Observable<ResponseMessage> {
        return this.http.post<ResponseMessage>(`${this.apiUrl}Recipe`, recipe);
    }

    editRecipe(recipe: Recipe): Observable<ResponseMessage> {
        return this.http.put<ResponseMessage>(`${this.apiUrl}Recipe`, recipe);
    }

    deleteRecipe(id: number): Observable<ResponseMessage> {
        return this.http.delete<ResponseMessage>(`${this.apiUrl}Recipe/${id}`);
    }

    //COMPONENTS

    getAllComponents(): Observable<RecipeComponent[]> {
        return this.http.get<RecipeComponent[]>(`${this.apiUrl}Component`);
    }

    getComponentById(id: number): Observable<RecipeComponent> {
        return this.http.get<RecipeComponent>(`${this.apiUrl}Component/${id}`);
    }

    postNewComponent(component: RecipeComponent): Observable<ResponseMessage> {
        return this.http.post<ResponseMessage>(`${this.apiUrl}Component`, component);
    }

    editComponent(component: RecipeComponent): Observable<ResponseMessage> {
        return this.http.put<ResponseMessage>(`${this.apiUrl}Component`, component);
    }

    deleteComponent(id: number): Observable<ResponseMessage> {
        return this.http.delete<ResponseMessage>(`${this.apiUrl}Component/${id}`);
    }

    // TAG

    getAllTags(): Observable<Tag[]> {
        return this.http.get<Tag[]>(`${this.apiUrl}Tag`);
    }

    getTagById(id: number): Observable<Tag> {
        return this.http.get<Tag>(`${this.apiUrl}Tag/${id}`);
    }

    postNewTag(tag: Tag): Observable<ResponseMessage> {
        return this.http.post<ResponseMessage>(`${this.apiUrl}Tag`, tag);
    }

    editTag(tag: Tag): Observable<ResponseMessage> {
        return this.http.put<ResponseMessage>(`${this.apiUrl}Tag`, tag);
    }

    deleteTag(id: number): Observable<ResponseMessage> {
        return this.http.delete<ResponseMessage>(`${this.apiUrl}Tag/${id}`);
    }

    // UNIT

    getAllUnits(): Observable<Unit[]> {
        return this.http.get<Unit[]>(`${this.apiUrl}Unit`);
    }
}
