import { useEffect, useState } from 'react';
import { makotoApi } from '../../services/api';

export default function DashboardCuotasView() {
  const [cuotas, setCuotas] = useState([]);
  const [error, setError] = useState('');

  const cargarCuotas = async () => {
    setError('');
    try {
      const data = await makotoApi.obtenerCuotas();
      setCuotas(data);
    } catch (err) {
      setError(err.message);
    }
  };

  useEffect(() => {
    cargarCuotas();
  }, []);

  return (
    <div className="card">
      <h2>Dashboard · Balanceo de Cuotas (Matriz 2×3)</h2>
      <button type="button" onClick={cargarCuotas}>↻ Actualizar cuotas</button>
      {error && <p className="error">{error}</p>}
      <table>
        <thead>
          <tr>
            <th>Código</th>
            <th>Género</th>
            <th>Frecuencia</th>
            <th>F0 Base (Hz)</th>
            <th>Iniciadas</th>
            <th>Completadas</th>
          </tr>
        </thead>
        <tbody>
          {cuotas.map((c) => (
            <tr key={c.id}>
              <td><code>{c.codigo}</code></td>
              <td>{c.genero}</td>
              <td>{c.nivelFrecuencia}</td>
              <td>{c.frecuenciaBaseHz} Hz</td>
              <td>{c.totalIniciadas}</td>
              <td><strong>{c.totalCompletadas}</strong></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}