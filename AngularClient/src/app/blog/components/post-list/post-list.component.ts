import { Component, OnInit } from '@angular/core';
import { PostService } from '../../services/post.service';
import { PostParameters } from '../../models/post-parameters';
import { PageMeta } from 'src/app/shared/models/page-meta';
import { ResultWithLinks } from 'src/app/shared/models/result-with-links';
import { Post } from '../../models/post';
import { OpenIdConnectService } from 'src/app/shared/oidc/open-id-connect.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  posts: Post[];
  pageMeta: PageMeta;
  postParameter = new PostParameters({ orderBy: 'id desc', pageSize: 10, pageIndex: 0});

  constructor(
    private postService: PostService,
    private odic: OpenIdConnectService) { }

  ngOnInit() {
    this.getPosts();
  }

  getPosts() {
    this.postService.getPagedPosts(this.postParameter).subscribe(resp => {
      this.pageMeta = JSON.parse(resp.headers.get('X-Pagination')) as PageMeta;
      const result = {...resp.body} as ResultWithLinks<Post>;
      this.posts = result.value;
      console.log(this.posts);
    });
  }

}
