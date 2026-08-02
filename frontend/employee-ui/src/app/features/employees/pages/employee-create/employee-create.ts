import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployeeService } from '../../../../services/employee-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee-create',
  imports: [ReactiveFormsModule],
  templateUrl: './employee-create.html',
  styleUrl: './employee-create.css',
})
export class EmployeeCreate {

    private fb = inject(FormBuilder);
    private employeeService = inject(EmployeeService);
    private router = inject(Router);

    employeeForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    department: ['', Validators.required],
    salary: [0, [Validators.required, Validators.min(1)]]
  });


    onSubmit(): void {

    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.employeeService.createEmployee(this.employeeForm.getRawValue())
      .subscribe({
        next: () => {
          alert('Employee created successfully');
          this.router.navigate(['/employees']);
        },
        error: (error: any) => {
          console.error(error);
          alert('Failed to create employee');
        }
      });
  }


}
