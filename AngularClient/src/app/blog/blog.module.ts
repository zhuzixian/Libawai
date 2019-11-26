import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogRoutingModule } from './blog-routing.module';
import { MaterialModule } from '../shared/material/material.module';
import { BlogAppComponent } from './blog-app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { PostService } from './services/post.service';
import { PostListComponent } from './components/post-list/post-list.component';
import { PostCardComponent } from './components/post-card/post-card.component';
import { WritePostComponent } from './components/write-post/write-post.component';
import { TinymceService } from './services/tinymce.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizationHeaderInterceptor } from '../shared/oidc/authorization-header.interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
import { HandleHttpErrorInterceptor } from '../shared/handle-http-error-interceptor';
import { EnsureAcceptHeaderInterceptor } from '../shared/ensure-accept-header.interceptor';
import { PostDetailComponent } from './components/post-detail/post-detail.component';
import { SafeHtmlPipe } from '../shared/safe-html.pipe';
import { EditPostComponent } from './components/edit-post/edit-post.component';


@NgModule({
  declarations: [
    BlogAppComponent,
    NavbarComponent,
    DashboardComponent,
    PostListComponent,
    PostCardComponent,
    WritePostComponent,
    PostDetailComponent,
    SafeHtmlPipe,
    EditPostComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
    CommonModule,
    BlogRoutingModule,
    MaterialModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
  ],
  providers: [
    PostService,
    TinymceService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: EnsureAcceptHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HandleHttpErrorInterceptor,
      multi: true
    }
  ]
})
export class BlogModule { }
