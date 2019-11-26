import { FormGroup } from '@angular/forms';

export class ValidationErrorHandler {
    static handleFormValidationErrors(form: FormGroup, validationResult: any): void {
        for (const property in validationResult) {
            if (validationResult.hasOwnProperty(property)) {
                if (form.controls[property]) {
                    // single field
                    const validationErrorsFormFormField = {};
                    for (const validationError of validationResult[property]) {
                        validationErrorsFormFormField[validationError.validatorKey] = true;
                    }
                    form.controls[property].setErrors(validationErrorsFormFormField);
                } else {
                    // cross field
                    const validationErrorsForForm = {};
                    for (const validationError of validationResult[property]) {
                        validationErrorsForForm[validationError.validatorKey] = true;
                    }
                    form.setErrors(validationErrorsForForm);
                }
            }
        }
    }
}
