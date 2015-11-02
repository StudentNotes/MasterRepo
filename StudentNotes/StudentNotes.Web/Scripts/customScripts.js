//var setupUniversityAutocomplete = function() {
//    console.log("Setting up UniversityAutocomplete!!!");
//    var $universityInput = $("#searchUniversity");

//    var universityOptions = {
//        source: $universityInput.attr("data-autocomplete-source"),
//        select: function(event, ui) {
//            var $universityInput = $("#searchUniversity");
//            $universityInput.val(ui.item.label);

//        }
//    }

//    $universityInput.autocomplete(universityOptions);

//}

//setupUniversityAutocomplete();



$(document).ready(function () {
    $('*[data-autocomplete-url]')
        .each(function () {
            $(this).autocomplete({
                source: $(this).data("autocomplete-url")
            });
        });


    //$('#searchUniversity').autocomplete({
    //    source: '@Url.Action("UniversitySuggestions", "Search")'
    //});
    console.log("Dokument jest gotowy");
});