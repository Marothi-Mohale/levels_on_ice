document.addEventListener("DOMContentLoaded", () => {
    const navToggle = document.querySelector("[data-nav-toggle]");
    const navbarCollapse = document.getElementById("mainNav");
    const navLinks = document.querySelectorAll("#mainNav .nav-link");
    const siteHeader = document.querySelector(".site-header");
    const galleryFilterButtons = document.querySelectorAll("[data-gallery-filter]");
    const galleryItems = document.querySelectorAll("[data-gallery-item]");
    const galleryLightbox = document.getElementById("galleryLightbox");
    const galleryTriggers = document.querySelectorAll("[data-gallery-trigger]");
    const galleryLightboxImage = document.getElementById("galleryLightboxImage");
    const galleryLightboxTitle = document.getElementById("galleryLightboxTitle");
    const galleryLightboxCategory = document.getElementById("galleryLightboxCategory");
    const galleryLightboxCaption = document.getElementById("galleryLightboxCaption");
    const galleryCloseButton = galleryLightbox?.querySelector("[data-gallery-close]");
    const faqAccordions = document.querySelectorAll(".faq-accordion");
    let lastLightboxTrigger = null;

    const setNavExpanded = isExpanded => {
        if (!navToggle || !navbarCollapse) {
            return;
        }

        navToggle.setAttribute("aria-expanded", String(isExpanded));
        navbarCollapse.classList.toggle("show", isExpanded);
    };

    navToggle?.addEventListener("click", () => {
        const isExpanded = navToggle.getAttribute("aria-expanded") === "true";
        setNavExpanded(!isExpanded);
    });

    navLinks.forEach(link => {
        link.addEventListener("click", () => setNavExpanded(false));
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
                const isActive = filterButton === button;
                filterButton.classList.toggle("is-active", isActive);
                filterButton.setAttribute("aria-pressed", String(isActive));
            });

            galleryItems.forEach(item => {
                const itemCategory = item.getAttribute("data-category");
                const isVisible = selectedCategory === "all" || itemCategory === selectedCategory;
                item.hidden = !isVisible;
            });
        });
    });

    faqAccordions.forEach(accordion => {
        const triggers = accordion.querySelectorAll("[data-accordion-trigger]");
        const setPanelState = (trigger, isExpanded) => {
            const panelId = trigger.getAttribute("aria-controls");
            if (!panelId) {
                return;
            }

            const panel = document.getElementById(panelId);
            if (!panel) {
                return;
            }

            trigger.setAttribute("aria-expanded", String(isExpanded));
            trigger.classList.toggle("collapsed", !isExpanded);
            panel.classList.toggle("show", isExpanded);
            panel.hidden = !isExpanded;
        };

        triggers.forEach(trigger => {
            setPanelState(trigger, trigger.getAttribute("aria-expanded") === "true");

            trigger.addEventListener("click", () => {
                const willExpand = trigger.getAttribute("aria-expanded") !== "true";

                triggers.forEach(otherTrigger => {
                    setPanelState(otherTrigger, otherTrigger === trigger ? willExpand : false);
                });
            });
        });
    });

    const openGalleryLightbox = trigger => {
        if (!galleryLightbox || !(trigger instanceof HTMLElement)) {
            return;
        }

        lastLightboxTrigger = trigger;
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

        galleryLightbox.hidden = false;
        galleryLightbox.setAttribute("aria-hidden", "false");
        document.body.classList.add("is-lightbox-open");
        galleryCloseButton?.focus();
    };

    const closeGalleryLightbox = () => {
        if (!galleryLightbox || galleryLightbox.hidden) {
            return;
        }

        galleryLightbox.hidden = true;
        galleryLightbox.setAttribute("aria-hidden", "true");
        document.body.classList.remove("is-lightbox-open");

        if (galleryLightboxImage) {
            galleryLightboxImage.src = "";
            galleryLightboxImage.alt = "";
        }

        if (lastLightboxTrigger instanceof HTMLElement) {
            lastLightboxTrigger.focus();
        }
    };

    galleryTriggers.forEach(trigger => {
        trigger.addEventListener("click", () => openGalleryLightbox(trigger));
    });

    galleryCloseButton?.addEventListener("click", closeGalleryLightbox);

    galleryLightbox?.addEventListener("click", event => {
        if (event.target === galleryLightbox) {
            closeGalleryLightbox();
        }
    });

    document.addEventListener("keydown", event => {
        if (event.key === "Escape") {
            closeGalleryLightbox();
            setNavExpanded(false);
        }
    });
});
