import { useState } from 'react';
import ExperimentoView from './modules/experimento/ExperimentoView';
import TestPetsView from './modules/test/TestPetsView';
import DashboardCuotasView from './modules/dashboard/DashboardCuotasView';

export default function App() {
  const [vista, setVista] = useState('experimento');
  const [sesionActiva, setSesionActiva] = useState(null);

  return (
    <main style={{ maxWidth: '760px', margin: '2rem auto', fontFamily: 'system-ui, sans-serif' }}>
      <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1>Makoto · Instrumento Experimental</h1>
        <nav style={{ display: 'flex', gap: '0.5rem' }}>
          <button onClick={() => { setSesionActiva(null); setVista('experimento'); }}>
            Experimento
          </button>
          <button onClick={() => setVista('dashboard')}>
            Dashboard Cuotas
          </button>
        </nav>
      </header>

      {vista === 'experimento' && (
        <ExperimentoView
          onSesionLista={(s) => {
            setSesionActiva(s);
            setVista('test');
          }}
        />
      )}

      {vista === 'test' && sesionActiva && (
        <TestPetsView
          sesion={sesionActiva}
          onCompletado={() => setVista('dashboard')}
        />
      )}

      {vista === 'dashboard' && <DashboardCuotasView />}
    </main>
  );
}