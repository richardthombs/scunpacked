const purgecss = require('@fullhuman/postcss-purgecss')({

  whitelist: [".dark"],

  // Specify the paths to all of the template files in your project
  content: ['./src/**/*.html', './src/**/*.component.ts'],

  // Include any special characters you're using in this regular expression
  defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || []
});

module.exports = (config, options) => {
  console.log(`Using '${config.mode}' mode`);
  config.module.rules.push({
    test: /\.scss$/,
    use: [
      {
        loader: 'postcss-loader',
        options: {
          postcssOptions: {
            plugins: [
              require('tailwindcss')('./tailwind.config.js'),
              require('autoprefixer'),
              ...(config.mode === 'production' ? [purgecss] : [])
            ]
          }
        }
      }
    ]
  });
  return config;
};
