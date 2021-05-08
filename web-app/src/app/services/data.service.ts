import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Recipe } from '../models/recipe';
import { RecipeComponent } from '../models/recipeComponent';
import { ResponseMessage } from '../models/responseMessage';
import { RecipeComponentModule } from '../modules/recipe-component/recipe-component.module';

@Injectable({
    providedIn: 'root'
})
export class DataService {

    apiUrl: string = environment.apiUrl;

    constructor(private http: HttpClient) { }

    //RECIPE

    getAllRecipes(): Observable<Recipe[]> {
        return this.http.get<Recipe[]>(`${this.apiUrl}Recipe`);
    }

    getRecipeById(id: number): Observable<Recipe> {
        return this.http.get<Recipe>(`${this.apiUrl}Recipe/${id}`);
    }

    //COMPONENT

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
}
