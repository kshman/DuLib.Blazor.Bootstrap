// dropdown toggle
export function initialize(e, orf, tg) {
  if (!e) return;
  e.orf = orf;
  e.addEventListener('shown.bs.dropdown', onShown);
  e.addEventListener('hidden.bs.dropdown', onHidden);
  e.c = new bootstrap.Dropdown(e);
}
export function dispose(e) {
  if (!e) return;
  e.addEventListener('shown.bs.dropdown', onShown);
  e.addEventListener('hidden.bs.dropdown', onHidden);
  e.orf = null;
  e.c?.dispose();
}
export function show(e) {
  e.c?.show();

}
export function hide(e) {
  e.c?.hide();
}

function onShown(e) {
  e.target.orf.invokeMethodAsync('ivk_tgl_show');
}
function onHidden(e) {
  e.target.orf.invokeMethodAsync('ivk_tgl_hide');
}
