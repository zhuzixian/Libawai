const PROXY_CONFIG = [
    {
        context: [
            "/api",
            "/uploads"
        ],
        target: "https://localhost:4841",
        secure: false
    }
];

module.exports = PROXY_CONFIG;
