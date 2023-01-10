const colors = require('tailwindcss/colors');

/**
 * Themes
 */
const themes = {
    // Default theme is required for theming system to work correctly
    'default': {
        primary  : {
            ...colors.indigo,
            DEFAULT: colors.indigo[600]
        },
        accent   : {
            ...colors.blueGray,
            DEFAULT: colors.blueGray[800]
        },
        warn     : {
            ...colors.red,
            DEFAULT: colors.red[600]
        },
        'on-warn': {
            500: colors.red['50']
        }
    }
};

/**
 * Tailwind configuration
 */
const config = {};

module.exports = config;
