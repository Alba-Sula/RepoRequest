$(document).ready(function () {
    $(".new-request-helper").click(function () {
        $(".fa-plus").addClass("attention-animation");
        setTimeout(function () {
            $(".fa-plus").removeClass("attention-animation");
        }, 4100);
    });
    $(".download-helper").click(function () {
        $(".download-icon i").addClass("attention-animation");
        setTimeout(function () {
            $(".download-icon i").removeClass("attention-animation");
        }, 4100);
    });
    $(".edit-delete-helper").click(function () {
        $(".edit-icon i").addClass("attention-animation");
        setTimeout(function () {
            $(".edit-icon i").removeClass("attention-animation");
        }, 4100);
    });
});
