function showDuLibBlazor(message) {
  return prompt(message, "DuLib.Blazor!");
}

// cookie
DUCKIE = {
  rd: function () {
    return (document.cookie.length === 0) ? 'z=0' : decodeURIComponent(document.cookie);
  },
  wr: function (name, value, days) {
    document.cookie = `${name}=${value}${this.exr(days)};path=/`;
  },
  rm: function (name) {
    document.cookie = `${name}=;path=/;expires=${(new Date()).toGMTString()}`;
  },
  exr: function (days) {
    if (!Number.isFinite(days))
      return "";
    return `;expires=${this.afs(days).toGMTString()}`;
  },
  afs: function (days) {
    const date = new Date();
    date.setDate(date.getDate() + days);
    return date;
  }
}

// carousel
DUCARS = {
  init: function (e, orf, opt) {
    if (!e) return;
    e.orf = orf;
    e.addEventListener('slide.bs.carousel', this.bs);
    e.addEventListener('slid.bs.carousel', this.es);
    e.c = new bootstrap.Carousel(e, opt);
    if (opt.ride === "carousel") e.c?.cycle();
  },
  disp: function (e) {
    if (!e) return;
    e.removeEventListener('slide.bs.carousel', this.bs);
    e.removeEventListener('slid.bs.carousel', this.es);
    e.orf = null;
    e.c?.dispose();
  },
  to: function (e, i) {
    e.c?.to(i);
  },
  prev: function (e) {
    e.c?.prev();
  },
  next: function (e) {
    e.c?.next();
  },
  cycle: function (e) {
    e.c?.cycle();
  },
  pause: function (e) {
    e.c?.pause();
  },
  bs: (e) => e.target.orf.invokeMethodAsync('ivk_cars_bs', e.from, e.to, e.direction === 'left'),
  es: (e) => e.target.orf.invokeMethodAsync('ivk_cars_es', e.from, e.to, e.direction === 'left')
}

// collapse
DUCLPS = {
  init: function (e, orf, tg) {
    if (!e) return;
    e.orf = orf;
    e.addEventListener('show.bs.collapse', this.es);
    e.addEventListener('shown.bs.collapse', this.esn);
    e.addEventListener('hide.bs.collapse', this.eh);
    e.addEventListener('hidden.bs.collapse', this.ehn);
    e.c = new bootstrap.Collapse(e, { toggle: tg });
  },
  disp: function (e) {
    if (!e) return;
    e.removeEventListener('show.bs.collapse', this.es);
    e.removeEventListener('shown.bs.collapse', this.esn);
    e.removeEventListener('hide.bs.collapse', this.eh);
    e.removeEventListener('hidden.bs.collapse', this.ehn);
    e.orf = null;
    e.c?.dispose();
  },
  show: function (e) {
    e.c?.show();
  },
  hide: function (e) {
    e.c?.hide();
  },
  es: (e) => e.target.orf.invokeMethodAsync('ivk_clps_bs'),
  esn: (e) => e.target.orf.invokeMethodAsync('ivk_clps_bsn'),
  eh: (e) => e.target.orf.invokeMethodAsync('ivk_clps_eh'),
  ehn: (e) => e.target.orf.invokeMethodAsync('ivk_clps_ehn')
}

// dropdown toggle
DUDROP = {
  init: function (e, orf) {
    if (!e) return;
    e.orf = orf;
    e.addEventListener('shown.bs.dropdown', this.ds);
    e.addEventListener('hidden.bs.dropdown', this.dh);
    e.c = new bootstrap.Dropdown(e);
  },
  disp: function (e) {
    if (!e) return;
    e.addEventListener('shown.bs.dropdown', this.ds);
    e.addEventListener('hidden.bs.dropdown', this.dh);
    e.orf = null;
    e.c?.dispose();
  },
  show: function (e) {
    e.c?.show();
  },
  hide: function (e) {
    e.c?.hide();
  },
  ds: (e) => e.target.orf.invokeMethodAsync('ivk_drop_show'),
  dh: (e) => e.target.orf.invokeMethodAsync('ivk_drop_hide')
}
