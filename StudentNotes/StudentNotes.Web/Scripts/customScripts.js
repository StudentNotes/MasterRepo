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

function fileSelected() {
    var fileInput = document.getElementById("fileInput");
    var files = fileInput.files;

    var allFiles = "";

    for (var i = 0; i < files.length; i++) {
        allFiles += files[i].name;
        allFiles += "<br>";
    }

    document.getElementById("filePath").innerHTML = allFiles;
}

$(document).ready(function () {
    setUp();
});
function setUp() {
    $("#universityNameGuess").autocomplete({
        source: '@Url.Action("UniversitySuggestions", "Search")'
    });
}

$("#gradeYearGuess").keypress(function () {
    setupAutocompleteGradeGuess();
});

$('#universtitySubjectGuess').keypress(function () {
    setupAutocompleteStudySubjectGuess();
});
function setupAutocompleteGradeGuess() {
    $("#gradeYearGuess").autocomplete({
        source: ('@Url.Action("GradeSuggestions", "Search")' + '?universityNameGuess=' + encodeURIComponent($('#universityNameGuess').val()))
    });
}

function setupAutocompleteStudySubjectGuess() {
    $("#universtitySubjectGuess").autocomplete({
        source: ('@Url.Action("StudySubjectSuggestions", "Search")' + '?universityNameGuess=' + encodeURIComponent($('#universityNameGuess').val()) + '&gradeYearGuess=' + encodeURIComponent($('#gradeYearGuess').val()))

    });
}

$(document).ajaxComplete(function (event, xhr, options) {
    if (xhr.status == 401) {
        window.location.href = "/Account/SessionExpired";
    }
});

function checkErrorDiv() {
    var errorDiv = document.getElementById("login-errors");
    if (errorDiv.childElementCount > 0) {
        console.log("Checking errors div");
        errorDiv.removeAttribute("hidden");
    }
}