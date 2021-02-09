module.exports = {
  preset: "jest-preset-angular",
  globals: {
    "ts-jest": {
      tsConfig: "<rootDir>/tsconfig.spec.json",
      stringifyContentPathRegex: "\\.html$",
      astTransformers: [
        "jest-preset-angular/build/InlineFilesTransformer",
        "jest-preset-angular/build/StripStylesTransformer"
      ]
    }
  },
  coverageThreshold: {
    global: {
      statements: 80,
      lines: 80,
      branches: 70,
      functions: 80
    }
  },
  transform: {
    "^.+\\.(ts|js|html)$": "ts-jest"
  },
  coverageReporters: [
    "json",
    "lcov",
    "text",
    "clover",
    "json-summary",
    "cobertura"
  ],
  moduleFileExtensions: ["ts", "html", "js", "json"],
  setupFilesAfterEnv: ["<rootDir>/src/setupJest.ts"],
  modulePaths: ["<rootDir>"],
  moduleDirectories: ["src", "node_modules"],
  moduleNameMapper: {
    "^@app(.*)$": "<rootDir>/src/app/$1",
    "^assets/(.*)$": "<rootDir>/src/assets/$1",
  },
  transformIgnorePatterns: ["node_modules/(?!@ngrx)"],
  testPathIgnorePatterns: [
    "<rootDir>/src/test.ts",
    "<rootDir>/src/app/tests/helpers",
    "<rootDir>/src/app/tests/mocks",
    "<rootDir>/src/environments"
  ],
  snapshotSerializers: [
    "jest-preset-angular/build/AngularNoNgAttributesSnapshotSerializer.js",
    "jest-preset-angular/build/AngularSnapshotSerializer.js",
    "jest-preset-angular/build/HTMLCommentSerializer.js"
  ]
};
