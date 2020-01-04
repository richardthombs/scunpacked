import { Component, OnInit } from '@angular/core';
import { ThemeService, ThemeSetting } from '../theme.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  themeSetting?: ThemeSetting;

  constructor(private themeSvc: ThemeService) { }

  ngOnInit() {
    this.themeSvc.theme$.subscribe(themeSetting => {
      this.themeSetting = themeSetting;
    });
  }

  toggleTheme() {
    if (this.themeSetting == "auto") this.themeSvc.setTheme("light");
    else if (this.themeSetting == "light") this.themeSvc.setTheme("dark");
    else if (this.themeSetting == "dark") this.themeSvc.setTheme("auto");
  }

}
