import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee';
import { HttpClient } from '@angular/common/http';

@Service()
export class EmployeeService {
     private http = inject(HttpClient);

  private apiUrl = 'https://enterprise-api-gopal-aubaffcee0ehaebu.centralindia-01.azurewebsites.net/api/employees';

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  createEmployee(employee: any) {
  return this.http.post(this.apiUrl, employee);
}
}
