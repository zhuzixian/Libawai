import { Component, OnInit, ViewChild } from '@angular/core';
import { PageMeta } from 'src/app/shared/models/page-meta';
import { PostParameters } from '../../models/post-parameters';
import { Post } from '../../models/post';
import { Subject } from 'rxjs';
import { MatPaginator, MatSort, Sort, PageEvent } from '@angular/material';
import { PostService } from '../../services/post.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ResultWithLinks } from 'src/app/shared/models/result-with-links';

@Component({
  selector: 'app-post-table',
  templateUrl: './post-table.component.html',
  styleUrls: ['./post-table.component.scss']
})
export class PostTableComponent implements OnInit {

  pageMeta: PageMeta;
  postParameter = new PostParameters({ orderBy: 'id desc', pageSize: 10, pageIndex: 0 });

  displayedColumns: string[] = ['id', 'title', 'author', 'lastModified'];
  dataSource: Post[];
  searchKeyUp = new Subject<string>();

  @ViewChild(MatPaginator, {static: false}) paginagor: MatPaginator;
  @ViewChild(MatSort, {static: false}) sort: MatSort;

  constructor(private postService: PostService) {
    const subscription = this.searchKeyUp.pipe(
      debounceTime(500),
    distinctUntilChanged()
    ).subscribe(() => {
      this.load();
    });
  }

  ngOnInit() {
    this.load();
  }

  load() {
    this.postService.getPagedPosts(this.postParameter).subscribe(resp => {
      this.pageMeta = JSON.parse(resp.headers.get('X-Pagination')) as PageMeta;
      const pagedResult = { ...resp.body } as ResultWithLinks<Post>;
      this.dataSource = pagedResult.value;
    });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.postParameter.title = filterValue;
    this.load();
  }

  sortData(sort: Sort) {
    this.postParameter.orderBy = null;
    if (sort.direction) {
      this.postParameter.orderBy = sort.active;
      if (sort.direction == 'desc') {
        this.postParameter.orderBy += ' desc';
      }
    }
    this.load();
  }

  onPaging(pageEvent: PageEvent) {
    this.postParameter.pageIndex = pageEvent.pageIndex;
    this.postParameter.pageSize = pageEvent.pageSize;
    this.load();
  }

}
