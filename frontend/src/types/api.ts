export interface CategoryDto {
  categoryID: number;
  categoryName: string;
  description: string | null;
}

export interface SupplierDto {
  supplierID: number;
  companyName: string;
  contactName: string | null;
  country: string | null;
}

export interface TokenDto {
  accessToken: string;
  expiresAt: string;
}

export interface ProductSummary {
  productID: number;
  productName: string;
  unitPrice: number | null;
  unitsInStock: number | null;
  discontinued: boolean;
  categoryName: string | null;
  supplierName: string | null;
}

export interface Product {
  productID: number;
  productName: string;
  quantityPerUnit: number | null;
  unitPrice: number | null;
  unitsInStock: number | null;
  unitsOnOrder: number | null;
  reorderLevel: number | null;
  discontinued: boolean;
  categoryID: number | null;
  categoryName: string | null;
  categoryPictureBase64: string | null;
  supplierID: number | null;
  supplierName: string | null;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface ProductFilter {
  page: number;
  pageSize: number;
  search?: string;
  categoryID?: number;
  discontinued?: boolean;
}

export interface UpdateProductForm {
  productName?: string;
  quantityPerUnit?: number;
  unitPrice?: number;
  unitsInStock?: number;
  unitsOnOrder?: number;
  reorderLevel?: number;
  discontinued?: boolean;
  categoryID?: number;
  supplierID?: number;
}

export interface CreateProductForm {
  productName: string;
  quantityPerUnit?: number;
  unitPrice?: number;
  unitsInStock?: number;
  unitsOnOrder?: number;
  reorderLevel?: number;
  discontinued: boolean;
  categoryID?: number;
  supplierID?: number;
}
