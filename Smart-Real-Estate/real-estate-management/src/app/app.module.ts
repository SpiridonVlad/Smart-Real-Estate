import { NgModule } from '@angular/core';

import { bootstrapApplication, BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { appRoutes } from './app.routes';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthInterceptor } from './services/auth.interceptor';
import { HomeComponent, PredictionDialog } from './components/home/home.component';
import { AuthGuard } from './services/auth.guard';
import { RecordListComponent } from './components/record-list/record-list.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
@NgModule({
    declarations: [
    ],
  imports: [
    LoginComponent,
    RegisterComponent,
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
    HomeComponent,
    RecordListComponent,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }

  ],


})
export class AppModule { }
