import { Component, OnInit } from '@angular/core';
import { ThemeService, Theme } from './theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  theme: Theme = "light";

  constructor(private themeSvc: ThemeService) { }

  ngOnInit() {
    this.themeSvc.theme$.subscribe(themeSetting => {
      let nextTheme: Theme;

      if (themeSetting == "auto") {
        const mq = window.matchMedia("(prefers-color-scheme: dark)");
        nextTheme = mq.matches ? "dark" : "light";
      } else nextTheme = themeSetting;

      this.theme = nextTheme;
    });
  }
}
