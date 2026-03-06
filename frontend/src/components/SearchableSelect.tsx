import { useEffect, useState } from 'react';

interface Option {
  id: number;
  label: string;
}

interface SearchableSelectProps {
  id: string;
  label: string;
  options: Option[];
  value: number | '';
  onChange: (id: number | '') => void;
  placeholder?: string;
  hasError?: boolean;
}

export function SearchableSelect({
  id,
  label,
  options,
  value,
  onChange,
  placeholder = 'Escribe para buscar...',
  hasError = false,
}: SearchableSelectProps) {
  const [inputText, setInputText] = useState('');

  useEffect(() => {
    if (value === '') {
      setInputText('');
    } else {
      const found = options.find((o) => o.id === value);
      if (found) setInputText(found.label);
    }
  }, [value, options]);

  function handleChange(text: string) {
    setInputText(text);
    const match = options.find((o) => o.label.toLowerCase() === text.toLowerCase());
    onChange(match ? match.id : '');
  }

  const listId = `${id}-list`;

  return (
    <div>
      <label htmlFor={id} className="block text-sm font-medium text-gray-700 mb-1">
        {label}
      </label>
      <input
        id={id}
        type="text"
        list={listId}
        value={inputText}
        onChange={(e) => handleChange(e.target.value)}
        placeholder={placeholder}
        autoComplete="off"
        className={`w-full border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 ${
          hasError ? 'border-red-400' : 'border-gray-300'
        }`}
      />
      <datalist id={listId}>
        {options.map((o) => (
          <option key={o.id} value={o.label} />
        ))}
      </datalist>
      {value !== '' && (
        <p className="text-xs text-gray-400 mt-0.5">ID seleccionado: {value}</p>
      )}
    </div>
  );
}
