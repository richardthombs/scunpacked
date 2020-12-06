const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = (isProd) => ({
  purge: {
    enabled: isProd,
    mode: "layers",
    content: [
      "./src/**/*.html",
      "./src/**/*.component.ts"
    ],
    options: {
    }
  },
  theme: {
    extend: {
      colors: {
        primary: "deeppink"
      },
      fontFamily: {
        sans: ["Inter var", ...defaultTheme.fontFamily.sans]
      }
    }
  },

  variants: {
    borderWidth: ['last'],
    margin: ['responsive', 'last'],
  },

  plugins: [
    require("@tailwindcss/forms")
  ]
});
