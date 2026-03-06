import client from './client';
import type { Product, ProductFilter, ProductSummary, PagedResult, UpdateProductForm } from '../types/api';

export async function getProducts(filter: ProductFilter): Promise<PagedResult<ProductSummary>> {
  const params: Record<string, unknown> = {
    page: filter.page,
    pageSize: filter.pageSize,
  };
  if (filter.search) params.search = filter.search;
  if (filter.categoryID !== undefined) params.categoryID = filter.categoryID;
  if (filter.discontinued !== undefined) params.discontinued = filter.discontinued;

  const { data } = await client.get<PagedResult<ProductSummary>>('/Products', { params });
  return data;
}

export async function getProductById(id: number): Promise<Product> {
  const { data } = await client.get<Product>(`/Products/${id}`);
  return data;
}

export async function updateProduct(id: number, dto: UpdateProductForm): Promise<Product> {
  const { data } = await client.put<Product>(`/Products/${id}`, dto);
  return data;
}

export async function deleteProduct(id: number): Promise<void> {
  await client.delete(`/Products/${id}`);
}
