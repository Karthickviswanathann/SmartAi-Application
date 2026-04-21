function barmessage(type, message, duration) {
    let bgColor = getBgColor(type);

    let popup = document.createElement("div");

    popup.innerHTML = `
                <div class="popup-header">
        <span class="popup-icon capitalize">${getIcon(type)} ${type}</span>
        <span class="popup-close"><i class="fas fa-times"></i></span>

    </div>
    <div class="popup-content">${message}</div>`;

    stylePopup(popup, bgColor);

    document.body.appendChild(popup);

    const timer = setTimeout(() => popup.remove(), duration);

    const closeBtn = popup.querySelector(".popup-close");
    closeBtn.addEventListener("click", () => {
        clearTimeout(timer);
        popup.remove();
    });
}

function getBgColor(type) {
    switch (type) {
        case "Success": return "#198754d6";
        case "Error": return "#dc3545bd";
        default: return "#0d6efde3";
    }
}

function getIcon(type) {
    switch (type) {
        case "Success": return "✔️";
        case "Error": return "❌";
        default: return "ℹ️";
    }
}

function stylePopup(popup, bgColor) {

    Object.assign(popup.style, {
        position: "fixed",
        bottom: "20px",
        right: "20px",
        background: bgColor,
        color: "white",
        padding: "12px 18px",
        borderRadius: "10px",
        fontSize: "14px",
        fontFamily: "Arial, sans-serif",
        zIndex: 2000,
        minWidth: "260px",
        maxWidth: "320px",
        boxShadow: "0 10px 25px rgba(0,0,0,0.2)",
        opacity: "0",
        transform: "translateY(20px)",
        transition: "all 0.3s ease"
    });

    // Animate IN
    setTimeout(() => {
        popup.style.opacity = "1";
        popup.style.transform = "translateY(0)";
    }, 10);

    // Header
    const header = popup.querySelector(".popup-header");
    Object.assign(header.style, {
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        marginBottom: "5px"
    });

    const icon = popup.querySelector(".popup-icon");
    Object.assign(icon.style, {
        fontSize: "14px",
        fontWeight: "600"
    });

    const closeBtn = popup.querySelector(".popup-close");
    Object.assign(closeBtn.style, {
        cursor: "pointer",
        fontSize: "16px",
        marginLeft: "10px"
    });

    const content = popup.querySelector(".popup-content");
    Object.assign(content.style, {
        fontSize: "13px",
        opacity: "0.9"
    });
}

//function stylePopup(popup, bgColor) {
//    Object.assign(popup.style, {
//        position: "fixed",
//        bottom: "50%",
//        left: "50%",
//        transform: "translateX(-50%)",
//        background: bgColor,
//        color: "white",
//        padding: "10px 20px",
//        borderRadius: "6px",
//        fontSize: "16px",
//        fontFamily: "Arial, sans-serif",
//        zIndex: 2000,
//        maxWidth: "40%",
//        boxShadow: "0 2px 10px rgba(0,0,0,0.2)"
//    });


//    const header = popup.querySelector(".popup-header");
//    Object.assign(header.style, {
//        display: "flex",
//        justifyContent: "space-between",
//        alignItems: "center",
//        marginBottom: "6px",
//        width: "400px",
//    });

//    const icon = popup.querySelector(".popup-icon");
//    Object.assign(icon.style, {
//        fontSize: "18px",
//        opacity: "0.9"
//    });

//    const closeBtn = popup.querySelector(".popup-close");
//    Object.assign(closeBtn.style, {
//        fontSize: "28px",
//        cursor: "pointer",
//        fontWeight: "bold",
//        marginLeft: "auto"
//    });


//    const content = popup.querySelector(".popup-content");
//    Object.assign(content.style, {
//        fontSize: "16px"
//    });


//}