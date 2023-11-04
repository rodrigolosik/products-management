import { Component, OnInit } from '@angular/core';
import { ProductService } from '../Services/product.service';
import { Product } from '../Models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  public products: Product[] = [];

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.listProducts();
  }

  listProducts(): void {
    this.productService.getProducts()
      .subscribe({
        next: (products: Product[]) => {
          this.products = products;
        },
        error: (error: any) => {
          console.error(error);
          window.alert(error.message);
        },
      });
  }
}
