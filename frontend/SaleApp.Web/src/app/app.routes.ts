import { Routes } from '@angular/router';
import { ListSalesPage } from './pages/list-sales-page/list-sales-page';
import { DetailSalePage } from './pages/detail-sale-page/detail-sale-page';
import { NewSalePage } from './pages/new-sale-page/new-sale-page';

export const routes: Routes = [
    { path: '', component: ListSalesPage },
    { path: 'new', component: NewSalePage },
    { path: 'detail/:id', component: DetailSalePage }
];
