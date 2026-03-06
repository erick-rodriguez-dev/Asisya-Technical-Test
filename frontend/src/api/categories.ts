import client from './client';
import type { CategoryDto } from '../types/api';

export async function getCategories(): Promise<CategoryDto[]> {
  const { data } = await client.get<CategoryDto[]>('/Category');
  return data;
}
