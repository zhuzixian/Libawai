import { QueryParameters } from 'src/app/shared/QueryParameters';

export class PostParameters extends QueryParameters {
    title?: string;

    constructor(init?: Partial<PostParameters>) {
        super(init);
        Object.assign(this, init);
    }
}
