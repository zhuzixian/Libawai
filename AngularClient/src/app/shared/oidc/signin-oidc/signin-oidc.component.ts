import { OnInit, Component } from '@angular/core';
import { OpenIdConnectService } from '../open-id-connect.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-signin-oidc',
    templateUrl: './signin-oidc.component.html',
    styleUrls: ['./signin-oidc.component.scss']
})

export class SigninOidcComponent implements OnInit {

    constructor(private openIdConnectService: OpenIdConnectService, private router: Router) {

     }

    ngOnInit() {
        this.openIdConnectService.userLoaded$.subscribe((userLoaded) => {
            if (userLoaded) {
                this.router.navigate(['./']);
            } else {
                if (!environment.production) {
                    console.log('An error happened: user wash\'t loaded.' );
                }
            }
        });

        this.openIdConnectService.handleCallback();
    }
}
