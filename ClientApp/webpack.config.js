const path = require('path');

module.exports = {
  entry: './src/index.tsx',
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
