// collapse
export function initialize(e, orf, tg) {
  if (!e) return;
  e.orf = orf;
  e.addEventListener('show.bs.collapse', onShow);
  e.addEventListener('shown.bs.collapse', onShown);
  e.addEventListener('hide.bs.collapse', onHide);
  e.addEventListener('hidden.bs.collapse', onHidden);
  e.c = new bootstrap.Collapse(e, { toggle: tg });
}
export function dispose(e) {
  if (!e) return;
  e.removeEventListener('show.bs.collapse', onShow);
  e.removeEventListener('shown.bs.collapse', onShown);
  e.removeEventListener('hide.bs.collapse', onHide);
  e.removeEventListener('hidden.bs.collapse', onHidden);
  e.orf = null;
  e.c?.dispose();
}
export function show(e) {
  e.c?.show();

}
export function hide(e) {
  e.c?.hide();
}

function onShow(e) {
  e.target.orf.invokeMethodAsync('ivk_clps_bs');
}
function onShown(e) {
  e.target.orf.invokeMethodAsync('ivk_clps_bsn');
}
function onHide(e) {
  e.target.orf.invokeMethodAsync('ivk_clps_eh');
}
function onHidden(e) {
  e.target.orf.invokeMethodAsync('ivk_clps_ehn');
}
