$(document).ready(function () {
    $(".pagination__option a").on('click', function () {
        $(".pagination__option a").removeClass("active");
        $(this).addClass("active")
    })

    $(".image_selected").on('click', function () {
        console.log("123")
        $(".image_selected").removeClass("active");
        $(this).addClass("active")
        console.log($(this))
        $(".MauSP_Selected").text("#id")
    })

})

// Toast function
// Toast function
function toast({ title = "", message = "", type = "info", duration = 1000 }) {
    const main = document.getElementById("toast");
    if (main) {
        const toast = document.createElement("div");

        // Auto remove toast
        const autoRemoveId = setTimeout(function () {
            main.removeChild(toast);
        }, duration + 1000);

        // Remove toast when clicked
        toast.onclick = function (e) {
            if (e.target.closest(".toast__close")) {
                main.removeChild(toast);
                clearTimeout(autoRemoveId);
            }
        };

        const icons = {
            success: "fas fa-check-circle",
            info: "fas fa-info-circle",
            warning: "fas fa-exclamation-circle",
            error: "fas fa-exclamation-circle"
        };
        const icon = icons[type];
        const delay = (duration / 1000).toFixed(2);

        toast.classList.add("toast", `toast--${type}`);
        toast.style.animation = `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`;

        toast.innerHTML = `
                    <div class="toast__icon">
                        <i class="${icon}"></i>
                    </div>
                    <div class="toast__body">
                        <h3 class="toast__title">${title}</h3>
                        <p class="toast__msg">${message}</p>
                    </div>
                    <div class="toast__close">
                        <i class="fas fa-times"></i>
                    </div>
                `;
        main.appendChild(toast);
    }
}


function getQueryParams() {
    var queryParams = {};
    var queryString = window.location.search.substring(1);
    var vars = queryString.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        queryParams[pair[0]] = decodeURIComponent(pair[1]);
    }
    return queryParams;
}

jQuery(document).ready(function ($) {
    $(document).on('click', '.Cart_mini_item', function () {
        $('body').prepend('<div class="bs-canvas-overlay bg-dark position-fixed w-100 h-100"></div>');
        if ($(this).hasClass('Cart_mini_item'))
            $('.bs-canvas-right').addClass('mr-0');
        else
            $('.bs-canvas-right').addClass('ml-0');
        return false;
    });
    $(document).on('click', '.bs-canvas-close, .bs-canvas-overlay', function () {
        var elm = $(this).hasClass('bs-canvas-close') ? $(this).closest('.bs-canvas') : $('.bs-canvas');
        elm.removeClass('mr-0 ml-0');
        $('.bs-canvas-overlay').remove();
        return false;
    });
});
