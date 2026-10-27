// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Menú de tres puntos del encabezado
document.addEventListener("DOMContentLoaded", function () {
    const botonMenu = document.querySelector(".menu-toggle");
    const menu = document.getElementById("menuPrincipal");

    if (!botonMenu || !menu) {
        return;
    }

    botonMenu.addEventListener("click", function () {
        const abierto = menu.classList.toggle("menu-abierto");

        botonMenu.setAttribute("aria-expanded", String(abierto));
        botonMenu.setAttribute(
            "aria-label",
            abierto ? "Cerrar menú" : "Abrir menú"
        );
    });

    menu.querySelectorAll("a").forEach(function (enlace) {
        enlace.addEventListener("click", function () {
            menu.classList.remove("menu-abierto");
            botonMenu.setAttribute("aria-expanded", "false");
            botonMenu.setAttribute("aria-label", "Abrir menú");
        });
    });

    document.addEventListener("click", function (evento) {
        if (!menu.contains(evento.target) &&
            !botonMenu.contains(evento.target)) {
            menu.classList.remove("menu-abierto");
            botonMenu.setAttribute("aria-expanded", "false");
            botonMenu.setAttribute("aria-label", "Abrir menú");
        }
    });
});