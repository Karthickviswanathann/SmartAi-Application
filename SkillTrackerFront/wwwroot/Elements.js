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


function downloadFile(filename, content) {
        const blob = new Blob([content], { type: "text/plain" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();
}

async function downloadPDF(content) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    doc.text(content, 10, 10);
    doc.save("note.pdf");
}

window.setEditorContent = (html) => {
    document.getElementById("editor").innerHTML = html;
};

//window.getCleanEditorContent = () => {
//    const editor = document.getElementById("editor");

//    let clone = editor.cloneNode(true);

//    clone.querySelectorAll("*").forEach(el => {
//        if (!el.textContent.trim() && el.children.length === 0) {
//            el.remove();
//        }
//    });

//    clone.querySelectorAll("b b, i i, u u").forEach(el => {
//        el.replaceWith(...el.childNodes);
//    });

//    return clone.innerHTML.trim();    
//};

window.getEditorContent = () => {
    return document.getElementById("editor").innerHTML;
};
window.formatText = (command) => {
    document.getElementById("editor").focus();
    document.execCommand(command, false, null);
};

window.getPlainText = (html) => {
    const temp = document.createElement("div");
    temp.innerHTML = html;
    return temp.innerText.trim();
};

