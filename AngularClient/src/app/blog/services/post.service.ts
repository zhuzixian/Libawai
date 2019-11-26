import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from 'src/app/shared/base.service';
import { PostParameters } from '../models/post-parameters';
import { Post } from '../models/post';
import { PostAdd } from '../models/post-add';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getPagedPosts(postParameter?: any | PostParameters) {
    return this.http.get(`${this.apiUrlBase}/posts`, {
      headers: new HttpHeaders()
      .set('accept', 'application/vnd.libawai.hateoas+json')
      .set('Content-Type', 'application/json'),
      observe: 'response',
      params: postParameter
    });
  }

  getPostById(id: number | string): Observable<Post> {
    return this.http.get<Post>(`${this.apiUrlBase}/posts/${id}`);
  }

  addPost(post: PostAdd) {
    const httOptions = {
      headers: new HttpHeaders()
      .set('Content-Type', 'application/vnd.libawai.create+json')
      .set('Accept', 'application/vnd.libawai.hateoas+json'),
    };

    return this.http.post<Post>(`${this.apiUrlBase}/posts`, post, httOptions);
  }

  partiallyUpdatePost(id: number | string, patchDocument: Operation[]): Observable<any> {
    return this.http.patch(`${this.apiUrlBase}/posts/${id}`, patchDocument,
    {
      headers: { 'Content-Type': 'application/json-patch+json' }
    });
  }
}
