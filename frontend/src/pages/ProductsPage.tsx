import { useEffect, useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { getProducts, deleteProduct } from '../api/products';
import type { ProductSummary, PagedResult } from '../types/api';

const PAGE_SIZE = 20;

export function ProductsPage() {
  const [searchParams, setSearchParams] = useSearchParams();

  const page = Number(searchParams.get('page') ?? '1');
  const search = searchParams.get('search') ?? '';
  const discontinuedParam = searchParams.get('discontinued');

  const [result, setResult] = useState<PagedResult<ProductSummary> | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [searchInput, setSearchInput] = useState(search);

  useEffect(() => {
    setLoading(true);
    setError('');

    const filter = {
      page,
      pageSize: PAGE_SIZE,
      search: search || undefined,
      discontinued:
        discontinuedParam === 'true' ? true : discontinuedParam === 'false' ? false : undefined,
    };

    getProducts(filter)
      .then(setResult)
      .catch(() => setError('Error al cargar los productos.'))
      .finally(() => setLoading(false));
  }, [page, search, discontinuedParam]);

  function applySearch() {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev);
      if (searchInput) next.set('search', searchInput);
      else next.delete('search');
      next.set('page', '1');
      return next;
    });
  }

  function setDiscontinued(value: string) {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev);
      if (value) next.set('discontinued', value);
      else next.delete('discontinued');
      next.set('page', '1');
      return next;
    });
  }

  function changePage(newPage: number) {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev);
      next.set('page', String(newPage));
      return next;
    });
  }

  async function handleDelete(id: number, name: string) {
    if (!confirm(`¿Eliminar "${name}"?`)) return;
    try {
      await deleteProduct(id);
      setResult((prev) =>
        prev
          ? {
              ...prev,
              items: prev.items.filter((p) => p.productID !== id),
              totalCount: prev.totalCount - 1,
            }
          : prev
      );
    } catch {
      alert('Error al eliminar el producto.');
    }
  }

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Productos</h2>
        <Link
          to="/products/new"
          className="bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-semibold px-4 py-2 rounded-lg transition-colors"
        >
          + Nuevo producto
        </Link>
      </div>

      <div className="bg-white border border-gray-200 rounded-xl p-4 mb-4 flex flex-wrap gap-3 items-end shadow-sm">
        <div className="flex-1 min-w-48">
          <label className="block text-xs font-medium text-gray-600 mb-1">Buscar</label>
          <div className="flex gap-2">
            <input
              type="text"
              value={searchInput}
              onChange={(e) => setSearchInput(e.target.value)}
              onKeyDown={(e) => e.key === 'Enter' && applySearch()}
              placeholder="Nombre del producto..."
              className="flex-1 border border-gray-300 rounded-lg px-3 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            <button
              onClick={applySearch}
              className="bg-indigo-600 hover:bg-indigo-700 text-white text-sm px-3 py-1.5 rounded-lg transition-colors cursor-pointer"
            >
              Buscar
            </button>
          </div>
        </div>

        <div className="min-w-40">
          <label className="block text-xs font-medium text-gray-600 mb-1">Estado</label>
          <select
            value={discontinuedParam ?? ''}
            onChange={(e) => setDiscontinued(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          >
            <option value="">Todos</option>
            <option value="false">Activos</option>
            <option value="true">Descontinuados</option>
          </select>
        </div>

        {(search || discontinuedParam) && (
          <button
            onClick={() => {
              setSearchInput('');
              setSearchParams({});
            }}
            className="text-sm text-gray-500 hover:text-red-500 underline cursor-pointer self-end pb-1.5"
          >
            Limpiar filtros
          </button>
        )}
      </div>

      <div className="bg-white border border-gray-200 rounded-xl shadow-sm overflow-hidden">
        {loading && (
          <div className="text-center py-12 text-gray-500 text-sm">Cargando...</div>
        )}

        {error && !loading && (
          <div className="text-center py-12 text-red-500 text-sm">{error}</div>
        )}

        {!loading && !error && result && (
          <>
            <table className="w-full text-sm">
              <thead className="bg-gray-50 border-b border-gray-200">
                <tr>
                  <th className="text-left px-4 py-3 font-semibold text-gray-600">#</th>
                  <th className="text-left px-4 py-3 font-semibold text-gray-600">Nombre</th>
                  <th className="text-left px-4 py-3 font-semibold text-gray-600">Categoría</th>
                  <th className="text-right px-4 py-3 font-semibold text-gray-600">Precio</th>
                  <th className="text-right px-4 py-3 font-semibold text-gray-600">Stock</th>
                  <th className="text-center px-4 py-3 font-semibold text-gray-600">Estado</th>
                  <th className="text-center px-4 py-3 font-semibold text-gray-600">Acciones</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {result.items.length === 0 && (
                  <tr>
                    <td colSpan={7} className="text-center py-10 text-gray-400">
                      No se encontraron productos.
                    </td>
                  </tr>
                )}
                {result.items.map((p) => (
                  <tr key={p.productID} className="hover:bg-gray-50 transition-colors">
                    <td className="px-4 py-3 text-gray-400">{p.productID}</td>
                    <td className="px-4 py-3 font-medium text-gray-800">{p.productName}</td>
                    <td className="px-4 py-3 text-gray-500">{p.categoryName ?? '—'}</td>
                    <td className="px-4 py-3 text-right text-gray-700">
                      {p.unitPrice != null ? `$${p.unitPrice.toFixed(2)}` : '—'}
                    </td>
                    <td className="px-4 py-3 text-right text-gray-700">
                      {p.unitsInStock ?? '—'}
                    </td>
                    <td className="px-4 py-3 text-center">
                      <span
                        className={`inline-block text-xs font-semibold px-2 py-0.5 rounded-full ${
                          p.discontinued
                            ? 'bg-red-100 text-red-600'
                            : 'bg-green-100 text-green-700'
                        }`}
                      >
                        {p.discontinued ? 'Descontinuado' : 'Activo'}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-center">
                      <div className="flex justify-center gap-2">
                        <Link
                          to={`/products/${p.productID}/edit`}
                          className="text-indigo-600 hover:underline text-xs font-medium"
                        >
                          Editar
                        </Link>
                        <button
                          onClick={() => handleDelete(p.productID, p.productName)}
                          className="text-red-500 hover:underline text-xs font-medium cursor-pointer"
                        >
                          Eliminar
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <div className="flex items-center justify-between px-4 py-3 border-t border-gray-100 text-sm text-gray-500">
              <span>
                {result.totalCount} resultado{result.totalCount !== 1 ? 's' : ''} — página{' '}
                {result.page} de {result.totalPages}
              </span>
              <div className="flex gap-1">
                <button
                  onClick={() => changePage(page - 1)}
                  disabled={page <= 1}
                  className="px-3 py-1 border border-gray-300 rounded-lg disabled:opacity-40 hover:bg-gray-50 cursor-pointer"
                >
                  ← Anterior
                </button>
                <button
                  onClick={() => changePage(page + 1)}
                  disabled={page >= result.totalPages}
                  className="px-3 py-1 border border-gray-300 rounded-lg disabled:opacity-40 hover:bg-gray-50 cursor-pointer"
                >
                  Siguiente →
                </button>
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
}
