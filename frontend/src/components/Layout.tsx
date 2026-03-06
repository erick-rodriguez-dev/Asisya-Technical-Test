import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';

export function Layout() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/login', { replace: true });
  }

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <nav className="bg-white border-b border-gray-200 px-6 py-3 flex items-center justify-between shadow-sm">
        <span className="text-xl font-bold text-indigo-600">Asisya</span>
        <div className="flex gap-6">
          <Link
            to="/products"
            className="text-gray-600 hover:text-indigo-600 font-medium transition-colors"
          >
            Productos
          </Link>
        </div>
        <button
          onClick={handleLogout}
          className="text-sm text-gray-500 hover:text-red-500 font-medium transition-colors cursor-pointer"
        >
          Cerrar sesión
        </button>
      </nav>
      <main className="flex-1 p-6 max-w-7xl mx-auto w-full">
        <Outlet />
      </main>
    </div>
  );
}
