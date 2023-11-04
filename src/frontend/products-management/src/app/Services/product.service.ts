import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Product } from '../Models/product.model';

@Injectable({providedIn: 'root'})
export class ProductService implements IProductService {

    private authToken: string = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJyb2RyaWdvQHRlc3RlLmNvbSIsImV4cCI6MTY5NDc4NzU1NCwiaXNzIjoiRXhlbXBsb0lzc3VlciIsImF1ZCI6IkV4ZW1wbG9BdWRpZW5jZSJ9.7FJseBTk4RFk5AfF-RI0S0DXtKJjgTGO7mfL0wQnUyg";

    constructor(private httpClient: HttpClient) { }

    createProduct(product: Product): void {
        throw new Error('Method not implemented.');
    }
    deleteProduct(id: number): void {
        throw new Error('Method not implemented.');
    }
    updateProduct(id: number, product: Product): void {
        throw new Error('Method not implemented.');
    }

    getProductById(id: number) {
        throw new Error('Method not implemented.');
    }

    getProducts(): any {

        const headers = new HttpHeaders({
            'Authorization': `Bearer ${this.authToken}`
        });

        return this.httpClient.get<Product[]>('http://localhost:5117/api/Products', { headers } );
    }
    
}

interface IProductService {
    getProducts(): any;
    getProductById(id: number): any;
    createProduct(product: Product): void;
    deleteProduct(id: number): void;
    updateProduct(id: number, product: Product): void;
}