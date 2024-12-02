import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { appRoutes } from './app.routes';
import { UserService } from '../app/services/user.service';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Import FormsModule


@NgModule({
  declarations: [
   
  ],
  imports: [
    BrowserModule,
    CommonModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes),
    FormsModule
  ],
  providers: [provideHttpClient(),UserService],
})
export class AppModule { }