import client from './client';
import type { SupplierDto } from '../types/api';

export async function getSuppliers(): Promise<SupplierDto[]> {
  const { data } = await client.get<SupplierDto[]>('/Supplier');
  return data;
}
