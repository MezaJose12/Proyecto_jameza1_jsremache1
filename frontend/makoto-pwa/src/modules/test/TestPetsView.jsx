import { useState } from 'react';
import { makotoApi } from '../../services/api';

export default function TestPetsView({ sesion, onCompletado }) {
  const [form, setForm] = useState({
    petsItem1: 4,
    petsItem2: 4,
    petsItem3: 4,
    petsItem4: 4,
    petsItem5: 4,
    calidezPercibida: 4,
    naturalidadVoz: 4,
    generoPercibido: 'Femenino',
    tonoPercibido: 'Neutra',
    comentarioCualitativo: '',
  });
  const [resultado, setResultado] = useState(null);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const data = await makotoApi.registrarEvaluacion(sesion.sesionId, form);
      setResultado(data);
    } catch (err) {
      setError(err.message);
    }
  };

  if (resultado) {
    return (
      <div className="card">
        <h2>Evaluación Registrada</h2>
        <p>Estado de la sesión: <strong>{resultado.estadoSesion}</strong></p>
        <p>Promedio Escala PETS: <strong>{resultado.promedioPets.toFixed(2)} / 7.00</strong></p>
        <button type="button" onClick={onCompletado}>Ver cuotas 2×3 en Dashboard</button>
      </div>
    );
  }

  return (
    <form onSubmit={handleSubmit} className="card">
      <h2>3. Escala PETS y Chequeo de Percepción</h2>
      {[1, 2, 3, 4, 5].map((n) => (
        <label key={n}>
          Ítem PETS {n} (1 = Totalmente en desacuerdo, 7 = Totalmente de acuerdo):
          <input
            type="number"
            min="1"
            max="7"
            value={form[`petsItem${n}`]}
            onChange={(e) => setForm({ ...form, [`petsItem${n}`]: Number(e.target.value) })}
          />
        </label>
      ))}
      <label>
        Calidez percibida (1–7):
        <input
          type="number"
          min="1"
          max="7"
          value={form.calidezPercibida}
          onChange={(e) => setForm({ ...form, calidezPercibida: Number(e.target.value) })}
        />
      </label>
      <label>
        Naturalidad de la voz (1–7):
        <input
          type="number"
          min="1"
          max="7"
          value={form.naturalidadVoz}
          onChange={(e) => setForm({ ...form, naturalidadVoz: Number(e.target.value) })}
        />
      </label>
      <label>
        Género vocal percibido:
        <select
          value={form.generoPercibido}
          onChange={(e) => setForm({ ...form, generoPercibido: e.target.value })}
        >
          <option value="Femenino">Femenino</option>
          <option value="Masculino">Masculino</option>
        </select>
      </label>
      <label>
        Tono / Frecuencia percibida:
        <select
          value={form.tonoPercibido}
          onChange={(e) => setForm({ ...form, tonoPercibido: e.target.value })}
        >
          <option value="Grave">Grave</option>
          <option value="Neutra">Neutra</option>
          <option value="Aguda">Aguda</option>
        </select>
      </label>
      {error && <p className="error">{error}</p>}
      <button type="submit">Guardar evaluación y cerrar sesión</button>
    </form>
  );
}