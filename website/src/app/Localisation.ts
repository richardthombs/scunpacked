import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';

export type LocalisationLabels = {
  [id: string]: string;
}

@Injectable({
  providedIn: 'root'
})
export class LabelsApi {
  constructor(private $http: HttpClient) { }
  get(): Promise<LocalisationLabels> {
    return this.$http.get<LocalisationLabels>(`${environment.api}/labels.json`).toPromise();
  }
}

@Injectable({
  providedIn: 'root'
})
export class LocalisationService {

  static translations: LocalisationLabels | undefined;

  getText(label: string, defaultValue?: string): string {
    if (!label || label == "@LOC_PLACEHOLDER" || label == "@LOC_UNINITIALIZED" || label == "@LOC_EMPTY") return defaultValue || "";

    if (label.startsWith("@")) label = label.substring(1);

    let text = LocalisationService.translations ? LocalisationService.translations[label] : undefined;
    if (!text) return defaultValue || label;

    return text.replace(/\\n/g, "\n");
  }

  static SetLabels(translations: LocalisationLabels) {
    this.translations = translations;
  }
}
