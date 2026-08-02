import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee';
import { HttpClient } from '@angular/common/http';

@Service()
export class EmployeeService {
     private http = inject(HttpClient);

  private apiUrl = '/api/employees';

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  createEmployee(employee: any) {
  return this.http.post(this.apiUrl, employee);
}
}
