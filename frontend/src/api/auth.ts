import client from './client';
import type { TokenDto } from '../types/api';

export async function login(username: string, password: string): Promise<TokenDto> {
  const { data } = await client.post<TokenDto>('/Auth/login', { username, password });
  return data;
}
