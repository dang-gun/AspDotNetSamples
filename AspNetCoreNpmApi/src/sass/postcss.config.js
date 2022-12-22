module.exports = ({ options }) => {
  const plugins = [];
  if (options.env === 'production') plugins.push(require('cssnano'));
  return {
      plugins: plugins
  };
};