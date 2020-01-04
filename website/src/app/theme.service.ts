import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export type Theme = "light" | "dark";
export type ThemeSetting = Theme | "auto";

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private themeSubject: BehaviorSubject<ThemeSetting>;

  theme$: Observable<ThemeSetting>;

  constructor() {
    let theme = this.getInitialTheme();
    this.themeSubject = new BehaviorSubject<ThemeSetting>(theme);
    this.theme$ = this.themeSubject.asObservable();
    this.setTheme(theme);
  }

  setTheme(theme: ThemeSetting) {
    this.themeSubject.next(theme);
    window.localStorage.setItem("theme", theme);
  }

  private getInitialTheme(): ThemeSetting {
    let theme = window.localStorage.getItem("theme") || "auto";

    switch (theme) {
      case "auto":
      case "light":
      case "dark":
        return theme;

      default:
        return "auto";
    }
  }
}
