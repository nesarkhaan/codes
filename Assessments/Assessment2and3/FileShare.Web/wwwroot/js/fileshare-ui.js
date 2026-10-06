(() => {
    const openDialog = (dialog) => {
        if (!dialog) return;
        if (typeof dialog.showModal === "function") {
            dialog.showModal();
        } else {
            dialog.setAttribute("open", "");
        }
    };

    const closeDialog = (dialog) => {
        if (!dialog) return;
        if (typeof dialog.close === "function") {
            dialog.close();
        } else {
            dialog.removeAttribute("open");
        }
    };
    
    document.querySelectorAll("[data-dialog-open]").forEach((button) => {
        button.addEventListener("click", () => {
            openDialog(document.getElementById(button.dataset.dialogOpen));
        });
    });

    document.querySelectorAll("[data-dialog-close]").forEach((button) => {
        button.addEventListener("click", () => closeDialog(button.closest("dialog")));
    });

    document.querySelectorAll("dialog").forEach((dialog) => {
        dialog.addEventListener("click", (event) => {
            if (event.target === dialog) closeDialog(dialog);
        });
    });

    const detailDialog = document.getElementById("package-details-dialog");
    document.querySelectorAll("[data-package-details]").forEach((button) => {
        button.addEventListener("click", () => {
            detailDialog.querySelector("[data-detail-name]").textContent = button.dataset.name;
            detailDialog.querySelector("[data-detail-identifier]").textContent = button.dataset.identifier;
            detailDialog.querySelector("[data-detail-target]").textContent = button.dataset.target;
            detailDialog.querySelector("[data-detail-blocks]").textContent = button.dataset.blocks;
            detailDialog.querySelector("[data-detail-peers]").textContent = button.dataset.peers;
            detailDialog.querySelector("[data-detail-state]").textContent = button.dataset.state;
            openDialog(detailDialog);
        });
    });

    const removeDialog = document.getElementById("remove-package-dialog");
    document.querySelectorAll("[data-package-remove]").forEach((button) => {
        button.addEventListener("click", () => {
            removeDialog.querySelector("[data-remove-name]").textContent = button.dataset.name;
            removeDialog.querySelector("[data-remove-identifier]").value = button.dataset.identifier;
            openDialog(removeDialog);
        });
    });

    document.querySelector("[data-notice-close]")?.addEventListener("click", (event) => {
        event.currentTarget.closest("[data-notice]")?.remove();
    });

    const menuButton = document.querySelector("[data-menu-toggle]");
    const sidebar = document.querySelector("[data-sidebar]");
    menuButton?.addEventListener("click", () => {
        const isOpen = sidebar.classList.toggle("sidebar-open");
        menuButton.setAttribute("aria-expanded", String(isOpen));
    });
})();
