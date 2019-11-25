import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BlogAppComponent } from './blog-app.component';
import { PostListComponent } from './components/post-list/post-list.component';
import { RequireAuthenticatedUserRouteGuard } from '../shared/oidc/require-authenticated-user-route.guard';
import { WritePostComponent } from './components/write-post/write-post.component';


const routes: Routes = [
  { path: '', component: BlogAppComponent,
    children: [
      {
        path: 'post-list', component: PostListComponent
      },
      {
        path: 'write-post', component: WritePostComponent,
        canActivate: [RequireAuthenticatedUserRouteGuard]
      },
      { path: 'dashboard', component: DashboardComponent },
      { path: '**', redirectTo: 'post-list' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
