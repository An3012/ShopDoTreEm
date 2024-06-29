
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

(function ($) {
    "use strict";

    /* 
    ------------------------------------------------
    Sidebar open close animated humberger icon
    ------------------------------------------------*/

    $(".hamburger").on('click', function () {
        $(this).toggleClass("is-active");
    });


    /*  
    -------------------
    List item active
    -------------------*/
    $('.header li, .sidebar li').on('click', function () {
        $(".header li.active, .sidebar li.active").removeClass("active");
        $(this).addClass('active');
    });

    $(".header li").on("click", function (event) {
        event.stopPropagation();
    });

    $(document).on("click", function () {
        $(".header li").removeClass("active");

    });




    /* 
    ---------------
    LobiPanel Function
    ---------------*/
    $(function () {
        $('.lobipanel-basic').lobiPanel({
            sortable: false,
            reload: false,
            editTitle: false,
            unpin: false,
            minimize: {
                icon: 'ti-angle-up',
                icon2: 'ti-angle-down'
            },
            close: {
                icon: 'ti-close'
            },
            expand: {
                icon: 'ti-fullscreen',
                icon2: 'fa fa-compress'
            }
        });
    });


    (function () {
        // Select node, and escape special characters
        var el = document.querySelector('.html'),
            esc = _.escape(el.innerHTML);

        // Reasign escaped to node and initialize highlight.js
        el.innerHTML = esc;
        hljs.highlightBlock(el);
        hljs.initHighlightingOnLoad();


    })();









})(jQuery);