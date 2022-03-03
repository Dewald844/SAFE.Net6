module.exports = {
    content: [
        './*.{html,js,fs}'
    ],
    theme: {
      extend: {},
      fontFamily: {
          'nunito': ['nunito extra bold', 'sans-serif'],
        },
    },
    plugins: [
        require("@tailwindcss/typography"),
        require("daisyui")
    ],

    daisyui: {
        themes: [
            {
                Agrico: {
                    primary: "#43b02a",
                    secondary: "#01421e",
                    accent: "#b1dee5",
                    neutral: "#686868",
                    dark: "#000000",
                    charcoal: "#231f20",
                    lightgrey: "#f3f3f3",
                },
            },
        ]
    },
}
