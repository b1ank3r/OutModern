
// close ads
(() => {
    let adsClsoeBtn = document.querySelector(".nav-ads .fa-solid.fa-xmark");
    adsClsoeBtn.addEventListener("click", () => {
        adsClsoeBtn.parentElement.remove();
    })
})();

// rotate the arrow in nav bar
(() => {
    let navDropDown = document.querySelectorAll(".nav-dropdown");

    navDropDown.forEach(item => {
        item.addEventListener("mouseenter", function () {
            icon = this.querySelector(".fa-angle-down");
            if (!icon) return;
            icon.classList.add("fa-rotate-180");
        })
        item.addEventListener("mouseleave", function () {
            icon = this.querySelector(".fa-angle-down");
            if (!icon) return;
            icon.classList.remove("fa-rotate-180");
        })
    })
})();

// drop down
(() => {
    dropDownAll = document.querySelectorAll(".nav-dropdown");

    dropDownAll.forEach(dropDown => {
        const dropDownList = dropDown.querySelector(".drop-down");

        dropDown.addEventListener("mouseenter", () => {
            dropDownList.style.height = "auto";
            const { height } = dropDownList.getBoundingClientRect();
            dropDownList.style.height = "0";
            dropDownList.style.paddingTop = "0";
            dropDownList.style.paddingBottom = "0";

            dropDownList.offsetHeight; // force render

            dropDownList.style.height = `${height}px`;
            dropDownList.style.paddingTop = "0.5rem";
            dropDownList.style.paddingBottom = "0.5rem";

        })

        dropDown.addEventListener("mouseleave", (e) => {
            dropDownList.style.height = "0"
            dropDownList.style.paddingTop = "0";
            dropDownList.style.paddingBottom = "0";
        })
    });

})();