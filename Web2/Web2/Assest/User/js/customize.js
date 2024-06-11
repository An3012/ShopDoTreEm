$(document).ready(function () {
    $(".pagination__option a").on('click', function () {
        $(".pagination__option a").each(function (item) {
            console.log($(".pagination__option a"))
            $(item).removeClass("active");
        })
        $(this).addClass("active")
    })
})