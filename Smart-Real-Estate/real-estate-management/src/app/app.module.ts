import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { appRoutes } from './app.routes';
import { UserService } from './services/user.service';

@NgModule({
    
  imports: [
    UserListComponent,
    BrowserModule,
    AppComponent,
    CommonModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule, // Import HttpClientModule here
    RouterModule.forRoot(appRoutes),
    AppComponent, 
    UserListComponent, 
    UserCreateComponent 
  ],
  providers: [UserService]
})
export class AppModule { }