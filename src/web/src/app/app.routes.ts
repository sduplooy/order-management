import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AppComponent } from './app.component';
import { ListProductsComponent } from './list-products/list-products.component';

export const routes: Routes = [
    { path: 'products', component: ListProductsComponent },
    { path: '', component: ListProductsComponent, pathMatch: 'full' },
    { path: '**', component: PageNotFoundComponent }
];
