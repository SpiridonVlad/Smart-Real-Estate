import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { UserUpdateComponent } from './components/user-update/user-update.component';
import { ListingListComponent } from './components/listing-list/listing-list.component';
import { ListingCreateComponent } from './components/listing-create/listing-create.component';
export const appRoutes: Routes = [
  { path: '', redirectTo: '/listings/create', pathMatch: 'full' },
  { path: 'users', component: UserListComponent },
  { path: 'users/create', component: UserCreateComponent},
  { path: 'users/update/:id', component: UserUpdateComponent },
  { path: 'listings', component: ListingListComponent },
  { path: 'listings/create', component: ListingCreateComponent },
  // { path: 'listings/update/:id', component: UpdateListingComponent }
];
