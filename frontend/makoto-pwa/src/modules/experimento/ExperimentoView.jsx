import { useState } from 'react';
import { makotoApi } from '../../services/api';

export default function ExperimentoView({ onSesionLista }) {
  const [codigo, setCodigo] = useState('');
  const [modalidad, setModalidad] = useState('GuionFijo');
  const [acepta, setAcepta] = useState(false);
  const [sesion, setSesion] = useState(null);
  const [audioTerminado, setAudioTerminado] = useState(false);
  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState('');

  const handleIniciar = async (e) => {
    e.preventDefault();
    setError('');
    setCargando(true);
    try {
      const data = await makotoApi.iniciarSesion({
        codigoParticipante: codigo,
        modalidad,
        consentimientoAceptado: acepta,
      });
      setSesion(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setCargando(false);
    }
  };

  const registrarTelemetria = async (tipoEvento) => {
    if (!sesion) return;
    await makotoApi.registrarEvento(sesion.sesionId, {
      tipoEvento,
      turno: 1,
      duracionSegundos: null,
      detalleJson: JSON.stringify({ condicion: sesion.condicionAsignada.codigo }),
    });
    if (tipoEvento === 'AUDIO_END') setAudioTerminado(true);
  };

  if (!sesion) {
    return (
      <form onSubmit={handleIniciar} className="card">
        <h2>1. Consentimiento e Inicio Experimental</h2>
        <label>
          Código anónimo del participante:
          <input
            type="text"
            required
            placeholder="Ej: ESPE-001"
            value={codigo}
            onChange={(e) => setCodigo(e.target.value)}
          />
        </label>
        <label>
          Modalidad:
          <select value={modalidad} onChange={(e) => setModalidad(e.target.value)}>
            <option value="GuionFijo">Guion Fijo (Estímulo único)</option>
            <option value="Longitudinal">Interacción Longitudinal</option>
          </select>
        </label>
        <label className="checkbox-row">
          <input type="checkbox" checked={acepta} onChange={(e) => setAcepta(e.target.checked)} />
          Acepto participar voluntariamente en el estudio experimental de voz.
        </label>
        {error && <p className="error">{error}</p>}
        <button type="submit" disabled={!acepta || cargando}>
          {cargando ? 'Asignando condición 2×3...' : 'Comenzar experimento'}
        </button>
      </form>
    );
  }

  return (
    <div className="card">
      <h2>2. Estímulo Vocal Asignado</h2>
      <p>
        Sesión: <code>{sesion.sesionId}</code> · Condición:{' '}
        <strong>{sesion.condicionAsignada.codigo}</strong> ({sesion.condicionAsignada.genero} /{' '}
        {sesion.condicionAsignada.nivelFrecuencia})
      </p>

      {/* Simulador / Reproductor con telemetría bloqueada */}
      <div className="audio-controls">
        <button type="button" onClick={() => registrarTelemetria('AUDIO_START')}>
          ▶ Iniciar reproducción (AUDIO_START)
        </button>
        <button type="button" onClick={() => registrarTelemetria('AUDIO_END')}>
          ⏹ Finalizar reproducción (AUDIO_END)
        </button>
      </div>

      <button
        type="button"
        disabled={!audioTerminado}
        onClick={() => onSesionLista(sesion)}
        style={{ marginTop: '1rem' }}
      >
        Continuar a evaluación PETS →
      </button>
    </div>
  );
}