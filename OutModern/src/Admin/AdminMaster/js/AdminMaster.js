// Collapse or expand Side Menu
(() => {
    const sideMenu = document.querySelector("#side-menu");
    const logo = document.querySelector("#companyLogo");
    const menuTitle = document.querySelectorAll(".menu-title"); //all the text in menu category
    const menuItemText = document.querySelectorAll(".menu-item-text"); //all the text in menu item
    const itemNotiNum = document.querySelectorAll(".item-notification-num");

    // check menu is collapsed or not
    const menuCollapse = parseInt(sessionStorage.getItem("menu-collapse")) || 0;

    // Menu collapse and expand toggler
    const menuToggler = document.querySelector("#menu-toggler");

    // get size of text logo
    const logoTextWidth = logo.children[1].clientWidth;

    // get size of section title in menu
    const menuTitleWidthArr = [];
    menuTitle.forEach(text => {
        const menuTitleWidth = text.clientWidth;
        menuTitleWidthArr.push(menuTitleWidth);
    })

    // get size of item in menu
    const menuItemWidthArr = [];
    menuItemText.forEach(text => {
        const menuItemWidth = text.clientWidth;
        menuItemWidthArr.push(menuItemWidth);
    })

    changeMenuTransition("none");
    if (menuCollapse === 0) { //if not collapse
        menuToggler.classList.add("collapsed")
    }
    toggleMenu(); //to add transition
    logo.children[0].clientWidth; // force render
    changeMenuTransition("0.25s")


    menuToggler.addEventListener("click", toggleMenu);
    function toggleMenu() {
        if (menuToggler.classList.contains("collapsed")) {
            expandMenu();
        }
        else {
            collapseMenu();
        }
    }

    function changeMenuTransition(duration) {
        sideMenu.style.transition = duration;

        logo.children[1].style.transition = duration;

        for (let i = 0; i < menuTitle.length; i++) {
            menuTitle[i].style.transition = duration;
        }

        // icon description for each item
        for (let i = 0; i < menuItemText.length; i++) {
            menuItemText[i].style.transition = duration;
        }

        // noti number for all item
        for (let i = 0; i < itemNotiNum.length; i++) {
            itemNotiNum[i].style.transition = duration;
        }
    }

    function collapseMenu() {
        menuToggler.classList.add("collapsed");
        sessionStorage.setItem("menu-collapse", 1)

        sideMenu.style.paddingLeft = "0.75rem";
        sideMenu.style.paddingRight = "0.75rem";

        logo.children[0].style.width = "45px"; // img logo
        logo.children[1].style.width = "0";

        // title for each category
        menuTitle.forEach(text => {
            text.style.width = 0;
            text.style.height = 0;

        })

        // icon description for each item
        menuItemText.forEach(text => {
            text.style.width = 0;
        })

        // noti number for all item
        for (let i = 0; i < itemNotiNum.length; i++) {
            itemNotiNum[i].style.right = "0.25em";
            itemNotiNum[i].style.top = "0";
        }
    }

    function expandMenu() {
        menuToggler.classList.remove("collapsed");
        sessionStorage.setItem("menu-collapse", 0);

        sideMenu.style.paddingLeft = "1.25rem";
        sideMenu.style.paddingRight = "1.25rem";

        logo.children[0].style.width = "0"; // img logo
        logo.children[1].style.width = `${logoTextWidth}px`;

        // title for each category
        for (let i = 0; i < menuTitle.length; i++) {
            menuTitle[i].style.width = `${menuTitleWidthArr[i]}px`;
            menuTitle[i].style.height = "1.75rem";
        }

        // icon description for each item
        for (let i = 0; i < menuItemText.length; i++) {
            menuItemText[i].style.width = `${menuItemWidthArr[i]}px`;
        }

        // noti number for all item
        for (let i = 0; i < itemNotiNum.length; i++) {
            itemNotiNum[i].style.right = "0.5em";
            itemNotiNum[i].style.top = "0.5em";
        }
    }
})();

// Profile drop down and flip arrow 
(() => {
    const profile = document.querySelector("#nav-profile");
    const profileArrowDown = profile.querySelector("i.fa-angle-down");

    profile.addEventListener("mouseenter", () => {
        profileArrowDown.classList.add("fa-rotate-180");
    });

    profile.addEventListener("mouseleave", () => {
        profileArrowDown.classList.remove("fa-rotate-180");
    });

})();

//Scroll Top Button
(() => {
    scrollTopBtn = document.querySelector("#scrollTopBtn");

    scrollTopBtn.addEventListener("click", () => {
        document.documentElement.style.scrollBehavior = "smooth";
        document.documentElement.scrollTop = 0;
        document.body.scrollTop = 0;

        document.documentElement.style.scrollBehavior = "auto";

    })

    window.onscroll = () => {
        if (document.documentElement.scrollTop > 79 || document.body.scrollTop > 79) {
            scrollTopBtn.style.right = "1.25rem";
        } else {
            scrollTopBtn.style.right = "-5rem";
        }
    }
})();


