module.exports = (config) => {

  const isProd = config.mode == "production";
  const tailwindConfig = require("./tailwind.config.js")(isProd);

  config.module.rules.push({
    test: /\.scss$/,
    use: [
      {
        loader: 'postcss-loader',
        options: {
          postcssOptions: {
            ident: "postcss",
            syntax: "postcss-scss",
            plugins: [
              require('tailwindcss')(tailwindConfig),
              require('autoprefixer')
            ]
          }
        }
      }
    ]
  });
  return config;
};
