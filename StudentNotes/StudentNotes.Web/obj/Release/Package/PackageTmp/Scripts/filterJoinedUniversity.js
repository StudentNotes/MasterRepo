document.getElementById('privateFileUploadCheckBox').addEventListener('change', HideUploadForm);
document.getElementById('universityFileUploadCheckBox').addEventListener('change', ShowUploadForm);

document.getElementById('universitiesSelect').addEventListener('change', UniversityChosen);
document.getElementById('studySubjectsSelect').addEventListener('change', StudySubjectChosen);
document.getElementById('semestersSelect').addEventListener('change', SemesterChosen);
document.getElementById('semesterSubjectsSelect').addEventListener('change', SemesterSubjectChosen);

$('#addGroupModal').on('show.bs.modal', InitializeAddGroupModal);
$('#addGroupModal').on('hide.bs.modal', ClearAddGroupModal);
document.getElementById("universitiesAddGroupSelect").addEventListener("change", UniversityForAddGroupChosen);
document.getElementById("studySubjectsAddGroupSelect").addEventListener("change", StudySubjectForAddGroupChosen);
document.getElementById("semestersAddGroupSelect").addEventListener("change", SemesterForAddGroupChosen);

function HideUploadForm() {
    var uploadForm = document.getElementById('showUploadFileForm');
    uploadForm.setAttribute("class", "form-horizontal hidden");
    $('#sendButton').removeAttr("disabled");
}

function ShowUploadForm() {
    var uploadForm = document.getElementById('showUploadFileForm');
    uploadForm.setAttribute("class", "form-horizontal");

    ClearSelect(document.getElementById('universitiesSelect'));
    ClearSelect(document.getElementById('studySubjectsSelect'));
    ClearSelect(document.getElementById('semestersSelect'));
    ClearSelect(document.getElementById('semesterSubjectsSelect'));

    DisableSelect(document.getElementById('studySubjectsSelect'));
    DisableSelect(document.getElementById('semestersSelect'));
    DisableSelect(document.getElementById('semesterSubjectsSelect'));

    ClearPath();
    $('#inputStudySubjectId').val("");
    $('#sendButton').attr("disabled", "true");
    $.getJSON("/LoggedIn/GetSchoolsJson", null, handleJsonSchools);
}

function ClearSelect(select) {
    while (select.firstChild) {
        select.removeChild(select.firstChild);
    }
    var option = document.createElement("option");
    option.setAttribute("value", 0);
    var text = document.createTextNode("-");

    option.appendChild(text);
    select.appendChild(option);
}

function DisableSelect(select) {
    select.setAttribute("disabled", "true");
}
function EnableSelect(select) {
    select.removeAttribute("disabled");
}
function InitializeAddGroupModal() {
    console.log("Initialize add group modal");
    ClearSelect(document.getElementById("universitiesAddGroupSelect"));
    ClearSelect(document.getElementById("studySubjectsAddGroupSelect"));
    ClearSelect(document.getElementById("semestersAddGroupSelect"));

    DisableSelect(document.getElementById("studySubjectsAddGroupSelect"));
    DisableSelect(document.getElementById("semestersAddGroupSelect"));
    $.getJSON("/LoggedIn/GetSchoolsJson", null, handleJsonForUniversityAddGroup);

    $("#createGroupButton").attr("disabled", "true");
}
function ClearAddGroupModal() {
    ClearSelect(document.getElementById("universitiesAddGroupSelect"));
    ClearSelect(document.getElementById("studySubjectsAddGroupSelect"));
    ClearSelect(document.getElementById("semestersAddGroupSelect"));

    DisableSelect(document.getElementById("studySubjectsAddGroupSelect"));
    DisableSelect(document.getElementById("semestersAddGroupSelect"));
    $("#createGroupButton").attr("disabled", "true");
}
function handleJsonData(data, select) {
    for (var i = 0; i < data.length; i++) {
        var option = document.createElement("option");
        option.setAttribute("value", data[i].FieldId);
        var text = document.createTextNode(data[i].FieldName);

        option.appendChild(text);
        select.appendChild(option);
    }
}
function handleJsonForUniversityAddGroup(data) {
    var select = document.getElementById("universitiesAddGroupSelect");
    handleJsonData(data, select);
}
function handleJsonForStudySubjectAddGroup(data) {
    var select = document.getElementById("studySubjectsAddGroupSelect");
    EnableSelect(select);
    ClearSelect(select);
    handleJsonData(data, select);   
}
function handleJsonForSemesterAddGroup(data) {
    var select = document.getElementById("semestersAddGroupSelect");
    EnableSelect(select);
    ClearSelect(select);
    handleJsonData(data, select);   
}
function UniversityForAddGroupChosen() {
    var universityValue = $("#universitiesAddGroupSelect").val();
    if (universityValue != 0) {
        console.log("Wybrałeś uczelnię. " + universityValue);
        $.getJSON("/LoggedIn/GetStudySubjectsJson", { universityId: universityValue }, handleJsonForStudySubjectAddGroup);
    } else {
        $("#createGroupButton").attr("disabled", "true");
    }
    DisableSelect(document.getElementById("studySubjectsAddGroupSelect"));
    DisableSelect(document.getElementById("semestersAddGroupSelect"));
    ClearSelect(document.getElementById("studySubjectsAddGroupSelect"));
    ClearSelect(document.getElementById("semestersAddGroupSelect"));
}
function StudySubjectForAddGroupChosen() {
    var studySubjectValue = $("#studySubjectsAddGroupSelect").val();
    if (studySubjectValue != 0) {
        console.log("Wybrałeś uczelnię.");
        $.getJSON("/LoggedIn/GetSemestersJson", { studySubjectId: studySubjectValue }, handleJsonForSemesterAddGroup);
    } else {
        $("#createGroupButton").attr("disabled", "true");
    }
    DisableSelect(document.getElementById("semestersAddGroupSelect"));
    ClearSelect(document.getElementById("semestersAddGroupSelect"));
}
function SemesterForAddGroupChosen() {
    var semesterValue = $("#semestersAddGroupSelect").val();
    if (semesterValue != 0) {
        console.log("Wybrałeś semestr o numerze id: " + semesterValue);
        $("#createGroupButton").removeAttr("disabled");
        $('#semesterIdForGroup').val(semesterValue);
    } else {
        $("#createGroupButton").attr("disabled", "true");
    }
}

function UniversityChosen() {
    var universityValue = $('#universitiesSelect').val();
    if (universityValue != 0) {
        console.log("Wybrałeś uczelnię. " + universityValue);
        $.getJSON("/LoggedIn/GetStudySubjectsJson", { universityId: universityValue }, handleJsonStudySubjects);
    } else {
        $('#sendButton').attr("disabled", "true");
    }
    DisableSelect(document.getElementById('studySubjectsSelect'));
    DisableSelect(document.getElementById('semestersSelect'));
    DisableSelect(document.getElementById('semesterSubjectsSelect'));
    ClearSelect(document.getElementById('studySubjectsSelect'));
    ClearSelect(document.getElementById('semestersSelect'));
    ClearSelect(document.getElementById('semesterSubjectsSelect'));
    ClearPath();
}

function StudySubjectChosen() {
    var studySubjectValue = $('#studySubjectsSelect').val();
    if (studySubjectValue != 0) {
        console.log("Wybrałeś uczelnię.");
        $.getJSON("/LoggedIn/GetSemestersJson", { studySubjectId: studySubjectValue }, handleJsonSemesters);
    } else {
        $('#sendButton').attr("disabled", "true");
    }
    DisableSelect(document.getElementById('semestersSelect'));
    DisableSelect(document.getElementById('semesterSubjectsSelect'));
    ClearSelect(document.getElementById('semestersSelect'));
    ClearSelect(document.getElementById('semesterSubjectsSelect'));
    ClearPath();
}

function SemesterChosen() {
    var semesterValue = $('#semestersSelect').val();
    if (semesterValue != 0) {
        console.log("Wybrałeś uczelnię.");
        $.getJSON("/LoggedIn/GetSemesterSubjectsJson", { semesterId: semesterValue }, handleJsonSemesterSubjects);
    } else {
        $('#sendButton').attr("disabled", "true");
    }
    DisableSelect(document.getElementById('semesterSubjectsSelect'));
    ClearSelect(document.getElementById('semesterSubjectsSelect'));
    ClearPath();
}

function SemesterSubjectChosen() {
    var semesterSubjectSelect = $('#semesterSubjectsSelect');
    console.log("SemesterSubjectValue = " + semesterSubjectSelect.val());
    if (semesterSubjectSelect.val() != 0) {
        console.log("Wybrałeś przedmiot.");
        CreatePath();
        $('#inputStudySubjectId').val(semesterSubjectSelect.val());
    } else {
        console.log("Clearing path!");
        $('#sendButton').attr("disabled", "true");
        ClearPath();
    }
}

function handleJsonSchools(data) {
    var select = document.getElementById('universitiesSelect');
    handleJsonData(data, select);
}



function handleJsonStudySubjects(data) {
    var select = document.getElementById('studySubjectsSelect');
    EnableSelect(select);
    ClearSelect(select);
    handleJsonData(data, select);
}

function handleJsonSemesters(data) {
    var select = document.getElementById('semestersSelect');
    EnableSelect(select);
    ClearSelect(select);
    handleJsonData(data, select);
}

function handleJsonSemesterSubjects(data) {

    var select = document.getElementById('semesterSubjectsSelect');
    EnableSelect(select);
    ClearSelect(select);
    handleJsonData(data, select);
}

function CreatePath() {
    var fullPath = "";
    var part1 = $('#universitiesSelect option:selected').text();
    var part2_3 = $('#studySubjectsSelect option:selected').text().split(" - ");
    var part2 = part2_3[0];
    var part3 = part2_3[1];
    var part4 = $('#semestersSelect option:selected').text();
    var part5 = $('#semesterSubjectsSelect option:selected').text();

    fullPath += part1 + "/" + part2 + "/" + part3 + "/" + part4 + "/" + part5;
    console.log(fullPath);

    $('#inputPath').val(fullPath);
    $('#sendButton').removeAttr("disabled");
}
function ClearPath() {
    $('#inputPath').val("");
}