$(document).ready(function () {
    
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