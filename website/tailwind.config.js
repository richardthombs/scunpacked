module.exports = {
  theme: {
    extend: {
      colors: {
        primary: "deeppink"
      }
    },

    // tailwindcss-typography
    textShadow: {
      "sm": "1px 1px 1px rgba(0,0,0,.5)"
    },

    // tailwindcss-dark-mode
    darkSelector: ".dark"
  },

  variants: {
    borderWidth: ['last'],
    margin: ['responsive', 'last'],
    backgroundColor: ['dark', 'dark-hover', 'dark-group-hover'],
    borderColor: ['dark', 'dark-focus', 'dark-focus-within'],
    textColor: ['hover', 'dark', 'dark-hover']
  },

  plugins: [
    require("tailwindcss-bg-alpha")(),
    require("tailwindcss-typography")(),
    require("tailwindcss-dark-mode")()
  ],
}
