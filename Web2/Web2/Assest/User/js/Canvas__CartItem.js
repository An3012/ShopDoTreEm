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