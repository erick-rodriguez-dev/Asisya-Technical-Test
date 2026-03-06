import { createBrowserRouter, Navigate } from 'react-router-dom'
import { AuthGuard } from '../components/AuthGuard'
import { Layout } from '../components/Layout'
import { LoginPage } from '../pages/LoginPage'
import { ProductsPage } from '../pages/ProductsPage'
import { ProductFormPage } from '../pages/ProductFormPage'

export const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />
  },
  {
    element: <AuthGuard />,
    children: [
      {
        element: <Layout />,
        children: [
          { path: '/', element: <Navigate to='/products' replace /> },
          { path: '/products', element: <ProductsPage /> },
          { path: '/products/new', element: <ProductFormPage /> },
          { path: '/products/:id/edit', element: <ProductFormPage /> }
        ]
      }
    ]
  }
])
