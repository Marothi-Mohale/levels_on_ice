document.addEventListener("DOMContentLoaded", () => {
    const navLinks = document.querySelectorAll(".navbar-collapse .nav-link");
    const navbarCollapse = document.querySelector(".navbar-collapse");
    const siteHeader = document.querySelector(".site-header");
    const galleryFilterButtons = document.querySelectorAll("[data-gallery-filter]");
    const galleryItems = document.querySelectorAll("[data-gallery-item]");
    const galleryLightbox = document.getElementById("galleryLightbox");
    const galleryLightboxImage = document.getElementById("galleryLightboxImage");
    const galleryLightboxTitle = document.getElementById("galleryLightboxTitle");
    const galleryLightboxCategory = document.getElementById("galleryLightboxCategory");
    const galleryLightboxCaption = document.getElementById("galleryLightboxCaption");

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

    galleryFilterButtons.forEach(button => {
        button.addEventListener("click", () => {
            const selectedCategory = button.getAttribute("data-gallery-filter");

            galleryFilterButtons.forEach(filterButton => {
                filterButton.classList.toggle("is-active", filterButton === button);
            });

            galleryItems.forEach(item => {
                const itemCategory = item.getAttribute("data-category");
                const isVisible = selectedCategory === "all" || itemCategory === selectedCategory;
                item.hidden = !isVisible;
            });
        });
    });

    if (galleryLightbox) {
        galleryLightbox.addEventListener("show.bs.modal", event => {
            const trigger = event.relatedTarget;
            if (!(trigger instanceof HTMLElement)) {
                return;
            }

            if (galleryLightboxImage) {
                galleryLightboxImage.src = trigger.dataset.galleryImage ?? "";
                galleryLightboxImage.alt = trigger.dataset.galleryAlt ?? "";
            }

            if (galleryLightboxTitle) {
                galleryLightboxTitle.textContent = trigger.dataset.galleryTitle ?? "";
            }

            if (galleryLightboxCategory) {
                galleryLightboxCategory.textContent = trigger.dataset.galleryCategory ?? "";
            }

            if (galleryLightboxCaption) {
                galleryLightboxCaption.textContent = trigger.dataset.galleryCaption ?? "";
            }
        });

        galleryLightbox.addEventListener("hidden.bs.modal", () => {
            if (galleryLightboxImage) {
                galleryLightboxImage.src = "";
                galleryLightboxImage.alt = "";
            }
        });
    }
});
