import { NgModule } from '@angular/core';
import { bootstrapApplication, BrowserModule } from '@angular/platform-browser';
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
import { HomeComponent } from './components/home/home.component';

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
    UserCreateComponent,
    HomeComponent
  ],
  providers: [],
})
export class AppModule { }