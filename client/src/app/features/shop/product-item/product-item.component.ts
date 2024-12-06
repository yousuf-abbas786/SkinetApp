import { Component, Input } from '@angular/core';
import { Product } from '../../../shared/models/product';
import { MatCardModule, MatCardContent, MatCardActions } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [MatCardModule, MatCardContent, CurrencyPipe, MatCardActions, MatButtonModule, MatIconModule],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {

  @Input() product?: Product;

}
