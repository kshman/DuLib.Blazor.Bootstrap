// modal
export function initialize(e, orf, open) {
  if (!e) return;
  e.orf = orf;
  e.addEventListener('shown.bs.modal', onShown);
  e.addEventListener('hidden.bs.modal', onHidden);
  e.c = new bootstrap.Modal(e, { keyboard: true });
  if (open) e.c?.show();
}
export function dispose(e) {
  if (!e) return;
  e.removeEventListener('shown.bs.modal', onShown);
  e.removeEventListener('hidden.bs.modal', onHidden);
  e.orf = null;
  e.c?.dispose();
}
export function show(e, orf) {
  if (!e) return;
  e.c?.show();
}
export function hide(e) {
  if (!e) return;
  e.c?.hide();
}

function onShown(e) {
  e.target.orf.invokeMethodAsync('ivk_mdl_os');
}
function onHidden(e) {
  e.target.orf.invokeMethodAsync('ivk_mdl_oh');
}
