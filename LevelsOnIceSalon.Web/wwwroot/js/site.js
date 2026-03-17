document.addEventListener("DOMContentLoaded", () => {
    const navLinks = document.querySelectorAll(".navbar-collapse .nav-link");
    const navbarCollapse = document.querySelector(".navbar-collapse");
    const siteHeader = document.querySelector(".site-header");

    navLinks.forEach(link => {
        link.addEventListener("click", () => {
            if (navbarCollapse?.classList.contains("show")) {
                new bootstrap.Collapse(navbarCollapse).hide();
            }
        });
    });

    const updateHeaderState = () => {
        if (!siteHeader) {
            return;
        }

        siteHeader.classList.toggle("is-scrolled", window.scrollY > 12);
    };

    updateHeaderState();
    window.addEventListener("scroll", updateHeaderState, { passive: true });
});
