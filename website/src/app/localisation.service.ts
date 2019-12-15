import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalisationService {

  static translations: { [id: string]: string } | undefined;

  getText(label: string, defaultValue?: string): string {
    if (!label || label == "@LOC_PLACEHOLDER" || label == "@LOC_UNINITIALIZED" || label == "@LOC_EMPTY") return defaultValue || "";

    if (label.startsWith("@")) label = label.substring(1);

    let text = LocalisationService.translations ? LocalisationService.translations[label] : undefined;
    if (!text) return defaultValue || label;

    return text.replace(/\\n/g, "\n");
  }

  static SetLabels(translations: { [id: string]: string }) {
    this.translations = translations;
  }
}
