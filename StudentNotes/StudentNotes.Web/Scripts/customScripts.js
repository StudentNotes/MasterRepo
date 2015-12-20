$(document).ready(function () {
    $('*[data-autocomplete-url]')
        .each(function () {
            $(this).autocomplete({
                source: $(this).data("autocomplete-url")
            });
        });
});
function loadingAnimationOn() {
    $("#loading-content").addClass("shown");
}

function loadingAnimationOff() {
    $("#loading-content").removeClass("shown");
}