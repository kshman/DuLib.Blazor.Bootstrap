// offcanvas
export function show(e, orf, scrl, esck) {
  if (window.efofcs) window.efofcs.c?.hide();
  if (!e) return;
  e.orf = orf;
  e.addEventListener('shown.bs.offcanvas', onShown);
  e.addEventListener('hidden.bs.offcanvas', onHidden);
  e.c = new bootstrap.Offcanvas(e, { scroll: scrl, keyboard: esck });
  window.efofcs = e;
  e.c?.show();
}
export function hide(e) {
  e.c?.hide();
}
export function dispose(e) {
  if (!e) return;
  e.removeEventListener('shown.bs.offcanvas', onShown);
  e.removeEventListener('hidden.bs.offcanvas', onHidden);
  e.orf = null;
  e.c?.dispose();
  if (window.efofcs === e) window.efofcs = null;
}

function onShown(e) {
  e.target.orf.invokeMethodAsync('ivk_ofcs_os');
}
function onHidden(e) {
  e.target.orf.invokeMethodAsync('ivk_ofcs_oh');
}
