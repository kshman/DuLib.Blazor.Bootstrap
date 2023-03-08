// modal
export function show(e, orf) {
  if (!e) return;
  e.orf = orf;
  e.addEventListener('shown.bs.modal', onShown);
  e.addEventListener('hidden.bs.modal', onHidden);
  e.c = new bootstrap.Modal(e, { keyboard: true });
  e.c?.show();
}
export function hide(e) {
  e.c?.hide();
}
export function dispose(e) {
  if (!e) return;
  e.removeEventListener('shown.bs.modal', onShown);
  e.removeEventListener('hidden.bs.modal', onHidden);
  e.orf = null;
  e.c?.dispose();
}

function onShown(e) {
  e.target.orf.invokeMethodAsync('ivk_mdl_os');
}
function onHidden(e) {
  e.target.orf.invokeMethodAsync('ivk_mdl_oh');
  dispose(e.target);
}
