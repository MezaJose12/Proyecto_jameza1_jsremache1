const API_BASE = (import.meta.env.VITE_API_URL || 'http://localhost:8080').replace(/\/$/, '');

async function request(path, options = {}) {
  const res = await fetch(`${API_BASE}${path}`, {
    headers: { 'Content-Type': 'application/json', ...(options.headers || {}) },
    ...options,
  });
  const data = await res.json().catch(() => ({}));
  if (!res.ok) throw new Error(data.error || `Error HTTP ${res.status}`);
  return data;
}

export const makotoApi = {
  checkHealth: () => request('/api/health'),
  iniciarSesion: (payload) =>
    request('/api/sesiones/iniciar', { method: 'POST', body: JSON.stringify(payload) }),
  registrarEvento: (sesionId, payload) =>
    request(`/api/sesiones/${sesionId}/eventos`, { method: 'POST', body: JSON.stringify(payload) }),
  registrarEvaluacion: (sesionId, payload) =>
    request(`/api/sesiones/${sesionId}/evaluacion`, { method: 'POST', body: JSON.stringify(payload) }),
  obtenerCuotas: () => request('/api/sesiones/cuotas'),
};