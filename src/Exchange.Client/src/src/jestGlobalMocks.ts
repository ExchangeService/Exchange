/* eslint-disable */
Object.defineProperty(window, "CSS", { value: null });
Object.defineProperty(document, "doctype", {
  value: "<!DOCTYPE html>"
});
Object.defineProperty(window, "getComputedStyle", {
  value: () => {
    return {
      display: "none",
      appearance: ["-webkit-appearance"]
    };
  }
});
Object.defineProperty(window, "getComputedStyle", {
  value: () => ({
    getPropertyValue: (_prop: HTMLElement) => {
      return "";
    }
  })
});
/**
 * ISSUE: https://github.com/angular/material2/issues/7101
 * Workaround for JSDOM missing transform property
 */
Object.defineProperty(document.body.style, "transform", {
  value: () => {
    return {
      enumerable: true,
      configurable: true
    };
  }
});

window.matchMedia = jest.fn().mockImplementation(query => {
  return {
    matches: false,
    media: query,
    onchange: null,
    addListener: jest.fn(), // deprecated
    removeListener: jest.fn(), // deprecated
    addEventListener: jest.fn(),
    removeEventListener: jest.fn(),
    dispatchEvent: jest.fn()
  };
});

const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  clear: jest.fn(),
  removeItem: jest.fn()
};

Object.defineProperty(window, "localStorage", {
  value: localStorageMock
});

Object.defineProperty(window, "saveAs", {
  value: jest.fn(),
  writable: true
});
