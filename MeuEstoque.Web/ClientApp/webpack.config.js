const path = require('path');

module.exports = (env, options) => {
  const isWatching = env["WEBPACK_WATCH"] || false;
  const isDevelopment = options.mode == "development";

  const getCaching = () => {
    const opts = {
      type: isWatching ? "memory" : "filesystem",
      buildDependencies: {
        // By default webpack and loaders are build dependencies
        // This makes all dependencies of this file - build dependencies
        config: [__filename],
      }
    };

    if (isWatching) {
      delete opts.buildDependencies;
    }

    return opts;
  };

  return {
    entry: './src/index.tsx',
    cache: (isDevelopment) ? getCaching() : false,
    module: {
      rules: [
        {
          test: /\.s[ac]ss$/i,
          use: [
            "style-loader",
            "css-loader",
            "sass-loader",
          ],
        },
        {
          test: /\.tsx?$/,
          loader: "ts-loader",
          exclude: /node_modules/,
        },
      ],
    },
    resolve: {
      extensions: [".js", ".jsx", ".ts", ".tsx", ".scss"],
      alias: {
        ["~"]: path.resolve(__dirname, "src"),
        ["@"]: path.resolve(__dirname, "src/components"),
      },
    },
    output: {
      path: path.resolve(__dirname, 'public'),
      filename: 'main.js',
    },
  };
};
