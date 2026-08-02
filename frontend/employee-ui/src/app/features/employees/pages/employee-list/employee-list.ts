import { Component, inject, signal } from '@angular/core';
import { EmployeeService } from '../../../../services/employee-service';
import { Employee } from '../../../../models/employee';

@Component({
  selector: 'app-employee-list',
  imports: [],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeList {
 
   private employeeService = inject(EmployeeService);
  employees = signal<Employee[]>([]);

  ngOnInit() {
    this.loadEmployees();
  } 

  private loadEmployees(): void {
  this.employeeService.getEmployees().subscribe({
    next: (employees) => {
      this.employees.set(employees);
    },
    error: (error) => {
      console.error(error);
    }
  });
  }
}
