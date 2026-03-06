import { useEffect, useState } from 'react';
import { useForm, Controller } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { createProduct, getProductById, updateProduct } from '../api/products';
import { getCategories } from '../api/categories';
import { getSuppliers } from '../api/suppliers';
import { SearchableSelect } from '../components/SearchableSelect';
import type { UpdateProductForm, CreateProductForm, CategoryDto, SupplierDto } from '../types/api';

interface ProductFormValues {
  productName: string;
  quantityPerUnit: number | '';
  unitPrice: number | '';
  unitsInStock: number | '';
  unitsOnOrder: number | '';
  reorderLevel: number | '';
  discontinued: boolean;
  categoryID: number | '';
  supplierID: number | '';
}

export function ProductFormPage() {
  const { id } = useParams<{ id: string }>();
  const isEdit = id !== undefined;
  const navigate = useNavigate();

  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [suppliers, setSuppliers] = useState<SupplierDto[]>([]);

  const {
    register,
    handleSubmit,
    reset,
    control,
    formState: { errors, isSubmitting },
  } = useForm<ProductFormValues>({
    defaultValues: {
      productName: '',
      quantityPerUnit: '' as number | '',
      unitPrice: '',
      unitsInStock: '',
      unitsOnOrder: '',
      reorderLevel: '',
      discontinued: false,
      categoryID: '',
      supplierID: '',
    },
  });

  useEffect(() => {
    getCategories().then(setCategories).catch(() => {});
    getSuppliers().then(setSuppliers).catch(() => {});
  }, []);

  useEffect(() => {
    if (!isEdit) return;
    getProductById(Number(id)).then((product) => {
      reset({
        productName: product.productName,
        quantityPerUnit: product.quantityPerUnit ?? '',
        unitPrice: product.unitPrice ?? '',
        unitsInStock: product.unitsInStock ?? '',
        unitsOnOrder: product.unitsOnOrder ?? '',
        reorderLevel: product.reorderLevel ?? '',
        discontinued: product.discontinued,
        categoryID: product.categoryID ?? '',
        supplierID: product.supplierID ?? '',
      });
    });
  }, [id, isEdit, reset]);

  async function onSubmit(values: ProductFormValues) {
    const quantityPerUnit = values.quantityPerUnit !== '' ? Number(values.quantityPerUnit) : undefined;
    const unitPrice = values.unitPrice !== '' ? Number(values.unitPrice) : undefined;
    const unitsInStock = values.unitsInStock !== '' ? Number(values.unitsInStock) : undefined;
    const unitsOnOrder = values.unitsOnOrder !== '' ? Number(values.unitsOnOrder) : undefined;
    const reorderLevel = values.reorderLevel !== '' ? Number(values.reorderLevel) : undefined;
    const categoryID = values.categoryID !== '' ? Number(values.categoryID) : undefined;
    const supplierID = values.supplierID !== '' ? Number(values.supplierID) : undefined;

    if (isEdit) {
      const dto: UpdateProductForm = {
        productName: values.productName || undefined,
        quantityPerUnit,
        unitPrice,
        unitsInStock,
        unitsOnOrder,
        reorderLevel,
        discontinued: values.discontinued,
        categoryID,
        supplierID,
      };
      await updateProduct(Number(id), dto);
    } else {
      const dto: CreateProductForm = {
        productName: values.productName,
        quantityPerUnit,
        unitPrice,
        unitsInStock,
        unitsOnOrder,
        reorderLevel,
        discontinued: values.discontinued,
        categoryID,
        supplierID,
      };
      await createProduct(dto);
    }

    navigate('/products');
  }

  const inputClass = (hasError: boolean) =>
    `w-full border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 ${
      hasError ? 'border-red-400' : 'border-gray-300'
    }`;

  const categoryOptions = categories.map((c) => ({ id: c.categoryID, label: c.categoryName }));
  const supplierOptions = suppliers.map((s) => ({ id: s.supplierID, label: s.companyName }));

  return (
    <div className="max-w-2xl mx-auto">
      <div className="flex items-center gap-3 mb-6">
        <button
          onClick={() => navigate('/products')}
          className="text-gray-400 hover:text-gray-600 transition-colors cursor-pointer"
        >
          ← Volver
        </button>
        <h2 className="text-2xl font-bold text-gray-800">
          {isEdit ? 'Editar producto' : 'Nuevo producto'}
        </h2>
      </div>

      <div className="bg-white border border-gray-200 rounded-xl shadow-sm p-6">
        <form onSubmit={handleSubmit(onSubmit)} noValidate className="space-y-5">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Nombre del producto <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              className={inputClass(!!errors.productName)}
              {...register('productName', {
                required: 'El nombre es requerido',
                minLength: { value: 2, message: 'Mínimo 2 caracteres' },
                maxLength: { value: 100, message: 'Máximo 100 caracteres' },
              })}
            />
            {errors.productName && (
              <p className="text-red-500 text-xs mt-1">{errors.productName.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Cantidad por unidad
            </label>
            <input
              type="number"
              min="1"
              max="32767"
              placeholder="Ej: 24"
              className={inputClass(!!errors.quantityPerUnit)}
              {...register('quantityPerUnit', {
                min: { value: 1, message: 'Debe ser mayor a 0' },
                max: { value: 32767, message: 'Valor máximo: 32767' },
              })}
            />
            {errors.quantityPerUnit && (
              <p className="text-red-500 text-xs mt-1">{errors.quantityPerUnit.message}</p>
            )}
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Precio unitario <span className="text-red-500">*</span>
              </label>
              <input
                type="number"
                step="0.01"
                min="0"
                className={inputClass(!!errors.unitPrice)}
                {...register('unitPrice', {
                  required: 'El precio es requerido',
                  min: { value: 0, message: 'El precio no puede ser negativo' },
                })}
              />
              {errors.unitPrice && (
                <p className="text-red-500 text-xs mt-1">{errors.unitPrice.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Unidades en stock
              </label>
              <input
                type="number"
                min="0"
                className={inputClass(!!errors.unitsInStock)}
                {...register('unitsInStock', {
                  min: { value: 0, message: 'No puede ser negativo' },
                })}
              />
              {errors.unitsInStock && (
                <p className="text-red-500 text-xs mt-1">{errors.unitsInStock.message}</p>
              )}
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Unidades en pedido
              </label>
              <input
                type="number"
                min="0"
                className={inputClass(false)}
                {...register('unitsOnOrder', {
                  min: { value: 0, message: 'No puede ser negativo' },
                })}
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Nivel de reorden
              </label>
              <input
                type="number"
                min="0"
                className={inputClass(false)}
                {...register('reorderLevel', {
                  min: { value: 0, message: 'No puede ser negativo' },
                })}
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <Controller
              name="categoryID"
              control={control}
              render={({ field }) => (
                <SearchableSelect
                  id="categoryID"
                  label="Categoría"
                  options={categoryOptions}
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="Buscar categoría..."
                />
              )}
            />
            <Controller
              name="supplierID"
              control={control}
              render={({ field }) => (
                <SearchableSelect
                  id="supplierID"
                  label="Proveedor"
                  options={supplierOptions}
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="Buscar proveedor..."
                />
              )}
            />
          </div>

          <div className="flex items-center gap-3">
            <input
              id="discontinued"
              type="checkbox"
              className="w-4 h-4 text-indigo-600 rounded"
              {...register('discontinued')}
            />
            <label htmlFor="discontinued" className="text-sm font-medium text-gray-700">
              Producto descontinuado
            </label>
          </div>

          <div className="flex gap-3 pt-2">
            <button
              type="submit"
              disabled={isSubmitting}
              className="flex-1 bg-indigo-600 hover:bg-indigo-700 disabled:opacity-50 text-white font-semibold py-2 rounded-lg transition-colors cursor-pointer"
            >
              {isSubmitting ? 'Guardando...' : isEdit ? 'Guardar cambios' : 'Crear producto'}
            </button>
            <button
              type="button"
              onClick={() => navigate('/products')}
              className="px-6 py-2 border border-gray-300 rounded-lg text-gray-600 hover:bg-gray-50 transition-colors cursor-pointer"
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
