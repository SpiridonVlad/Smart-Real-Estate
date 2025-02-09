import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { UserUpdateComponent } from './components/user-update/user-update.component';
import { ListingListComponent } from './components/listing-list/listing-list.component';
import { ListingCreateComponent } from './components/listing-create/listing-create.component';
import { PropertyListComponent } from './components/property-list/property-list.component';
import { PropertyCreateComponent } from './components/property-create/property-create.component';
import { PropertyUpdateComponent } from './components/property-update/property-update.component';
import { ListingUpdateComponent } from './components/listing-update/listing-update.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { EmailVerificationComponent } from './components/email-verification/email-verification.component';
import { RegisterComponent } from './components/register/register.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { RecordListComponent } from './components/record-list/record-list.component';
import { MessagesComponent } from './components/messages/messages.component';
import { RecordDetailsComponent } from './components/record-details/record-details.component';
import { ComparePropertiesComponent } from './components/compare-properties/compare-properties.component';
export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'users', component: UserListComponent },
  { path: 'users/create', component: UserCreateComponent},
  { path: 'users/update/:id', component: UserUpdateComponent },
  { path: 'listings', component: ListingListComponent },
  { path: 'listings/create', component: ListingCreateComponent },
  { path: 'listings/create/:id', component: ListingCreateComponent },
  {path: 'properties',component:PropertyListComponent},
  {path: 'properties/create',component:PropertyCreateComponent},
  {path: 'properties/update/:id',component:PropertyUpdateComponent},
  { path: 'email-verification', component: EmailVerificationComponent },
  {path: 'user-profile',component:UserProfileComponent},
  { path: 'records', component: RecordListComponent },
  { path: 'records/:id', component: RecordDetailsComponent },
  {path: 'users/profile', component: UserProfileComponent},
  {path: 'users/profile/:id', component: UserProfileComponent},
  {path: 'messages/:id', component: MessagesComponent},
  { path: 'compare-properties', component: ComparePropertiesComponent },


  // { path: 'listings/update/:id', component: UpdateListingComponent }
  { path: 'listings/update/:id', component: ListingUpdateComponent }
];
export class AppRoutingModule {}
