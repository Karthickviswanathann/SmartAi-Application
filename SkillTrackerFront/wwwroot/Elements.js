window.togglePasswordView = function (show) {
    var pwd = document.getElementById("password");
    pwd.type = show ? "text" : "password";
};

window.addDarkMode = () => {
    document.documentElement.classList.add("dark-mode");
};

window.removeDarkMode = () => {
    document.documentElement.classList.remove("dark-mode");
};
