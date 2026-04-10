window.togglePasswordView = function (show) {
    var pwd = document.getElementById("password");
    pwd.type = show ? "text" : "password";
};

window.setDarkMode = (enabled) => {
    if (enabled)
        document.documentElement.classList.add("dark-mode");
    else
        document.documentElement.classList.remove("dark-mode");
};


window.applyAccentColor = (color) => {
    console.log("Applying color:", color);
    document.documentElement.style.setProperty("--accent-color", color);
};


window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text)
        .then(() => console.log("Copied!"))
        .catch(err => console.error("Copy failed", err));
};