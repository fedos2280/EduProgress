document.addEventListener("DOMContentLoaded", function () {
    const html = document.documentElement;
    const themeToggle = document.getElementById("themeToggle");
    const savedTheme = localStorage.getItem("theme");

    if (savedTheme) {
        html.setAttribute("data-theme", savedTheme);
        updateThemeIcon(savedTheme);
    } else {
        html.setAttribute("data-theme", "light");
        updateThemeIcon("light");
    }

    if (themeToggle) {
        themeToggle.addEventListener("click", function () {
            const currentTheme = html.getAttribute("data-theme");
            const newTheme = currentTheme === "dark" ? "light" : "dark";

            html.setAttribute("data-theme", newTheme);
            localStorage.setItem("theme", newTheme);
            updateThemeIcon(newTheme);
        });
    }

    setActiveNav();
});

function updateThemeIcon(theme) {
    const icon = document.querySelector("#themeToggle i");
    if (!icon) return;

    if (theme === "dark") {
        icon.className = "bi bi-sun-fill";
    } else {
        icon.className = "bi bi-moon-stars-fill";
    }
}

function setActiveNav() {
    const currentPath = window.location.pathname.toLowerCase();
    const links = document.querySelectorAll(".nav-item");

    links.forEach(link => {
        const href = link.getAttribute("href");
        if (!href) return;

        if (
            currentPath === href.toLowerCase() ||
            (href.toLowerCase() !== "/" && currentPath.startsWith(href.toLowerCase()))
        ) {
            link.classList.add("active");
        }
    });
}