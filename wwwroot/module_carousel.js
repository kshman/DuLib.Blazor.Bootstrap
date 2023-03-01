// carousel
export function initialize(e, orf, opt) {
  if (!e) return;
  e.orf = orf;
  e.addEventListener('slide.bs.carousel', onSlide);
  e.addEventListener('slid.bs.carousel', onSlid);
  e.c = new bootstrap.Carousel(e, opt);
  if (opt.ride === "carousel") e.c?.cycle();
}
export function dispose(e) {
  if (!e) return;
  e.removeEventListener('slide.bs.carousel', onSlid);
  e.removeEventListener('slid.bs.carousel', onSlid);
  e.orf = null;
  e.c?.dispose();
}
export function goTo(e) {
  e.c?.to(i);
}
export function goPrev(e) {
  e.c?.prev();
}
export function goNext(e) {
  e.c?.next();
}
export function cycle(e) {
  e.c?.cycle();
}
export function pause(e) {
  e.c?.pause();
}
function onSlide(e) {
  e.target.orf.invokeMethodAsync('ivk_cars_bs', e.from, e.to, e.direction);
}

function onSlid(e) {
  e.target.orf.invokeMethodAsync('ivk_cars_es', e.from, e.to, e.direction);
}
