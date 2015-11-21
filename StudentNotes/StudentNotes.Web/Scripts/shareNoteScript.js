$(".btn-share-modal").each(function () {
    $(this).on("click", function () {
        console.log("Show share modal");
        var fileName = $(this).attr("name");
        $("#fileNameSpan").text(fileName);

        ClearSelect(document.getElementById("privateSchoolSelect"));
        ClearSelect(document.getElementById("privateSubjectSelect"));
        ClearSelect(document.getElementById("privateSemesterSelect"));
        ClearSelect(document.getElementById("privateUserSelect"));

        DisableSelect(document.getElementById("privateSubjectSelect"));
        DisableSelect(document.getElementById("privateSemesterSelect"));
        DisableSelect(document.getElementById("privateUserSelect"));

        $.getJSON("/LoggedIn/GetSchoolsJson", null, handleJsonSchoolsPrivate);
        $("#shareFileWithUserButton").attr("disabled", "true");

    });
});

$("#shareUserCheckBox").on("change", function () {
    $("#shareUserForm").removeAttr("hidden");
    $("#shareGroupForm").attr("hidden", true);

    ClearSelect(document.getElementById("privateSchoolSelect"));
    ClearSelect(document.getElementById("privateSubjectSelect"));
    ClearSelect(document.getElementById("privateSemesterSelect"));
    ClearSelect(document.getElementById("privateUserSelect"));

    DisableSelect(document.getElementById("privateSubjectSelect"));
    DisableSelect(document.getElementById("privateSemesterSelect"));
    DisableSelect(document.getElementById("privateUserSelect"));

    $.getJSON("/LoggedIn/GetSchoolsJson", null, handleJsonSchoolsPrivate);
    $("#shareFileWithUserButton").attr("disabled", "true");

    console.log("Ukrywanie forma grupowego");
});

$("#privateSchoolSelect").on("change", PrivateSchoolChosen);
$("#privateSubjectSelect").on("change", PrivateSubjectChosen);
$("#privateSemesterSelect").on("change", PrivateSemesterChosen);
$("#privateUserSelect").on("change", PrivateUserChosen);

$("#shareGroupCheckBox").on("change", function () {
    $("#shareGroupForm").removeAttr("hidden");
    $("#shareUserForm").attr("hidden", true);

    ClearSelect(document.getElementById("groupSelect"));
    ClearSelect(document.getElementById("groupSemesterSelect"));
    ClearSelect(document.getElementById("groupSemesterSubjectSelect"));

    DisableSelect(document.getElementById("groupSemesterSelect"));
    DisableSelect(document.getElementById("groupSemesterSubjectSelect"));

    $.getJSON("/LoggedIn/GetGroupsJson", null, handleJsonGroups);
    $("#shareFileWithGroupButton").attr("disabled", "true");
    console.log("Ukrywanie forma prywatnego");
});

$("#groupSelect").on("change", GroupChosen);
$("#groupSemesterSelect").on("change", SemesterChosen);
$("#groupSemesterSubjectSelect").on("change", SemesterSubjectChosen);

function handleJsonSchoolsPrivate(data) {
    var select = document.getElementById('privateSchoolSelect');
    handleJsonData(data, select);
}

function handleJsonSubjectsPrivate(data) {
    var select = document.getElementById('privateSubjectSelect');
    handleJsonData(data, select);
}

function handleJsonSemestersPrivate(data) {
    var select = document.getElementById('privateSemesterSelect');
    handleJsonData(data, select);
}

function handleJsonUsersPrivate(data) {
    var select = document.getElementById('privateUserSelect');
    handleJsonData(data, select);
}


function handleJsonGroups(data) {
    var select = document.getElementById('groupSelect');
    handleJsonData(data, select);
}

function handleJsonGroupSemesters(data) {
    var select = document.getElementById('groupSemesterSelect');
    handleJsonData(data, select);
}

function handleJsonGroupSemesterSubjects(data) {
    var select = document.getElementById('groupSemesterSubjectSelect');
    handleJsonData(data, select);
}

function GroupChosen() {
    var groupSelectValue = $('#groupSelect').val();

    DisableSelect(document.getElementById('groupSemesterSelect'));
    DisableSelect(document.getElementById('groupSemesterSubjectSelect'));
    ClearSelect(document.getElementById('groupSemesterSelect'));
    ClearSelect(document.getElementById('groupSemesterSubjectSelect'));

    if (groupSelectValue != 0) {
        console.log("Wybrałeś grupę. " + groupSelectValue);
        $.getJSON("/LoggedIn/GetGroupSemestersJson", { groupId: groupSelectValue }, handleJsonGroupSemesters);
        EnableSelect(document.getElementById('groupSemesterSelect'));
    } else {
        $('#shareFileWithGroupButton').attr("disabled", "true");
    }
}

function SemesterChosen() {
    var groupSemesterValue = $('#groupSemesterSelect').val();

    DisableSelect(document.getElementById('groupSemesterSubjectSelect'));
    ClearSelect(document.getElementById('groupSemesterSubjectSelect'));

    if (groupSemesterValue != 0) {
        console.log("Wybrałeś semestr. " + groupSemesterValue);
        $.getJSON("/LoggedIn/GetGroupSemesterSubjectsJson", { semesterId: groupSemesterValue }, handleJsonGroupSemesterSubjects);
        EnableSelect(document.getElementById('groupSemesterSubjectSelect'));
    } else {
        $('#shareFileWithGroupButton').attr("disabled", "true");
    }
}

function SemesterSubjectChosen() {
    var groupSemesterSubjectSelectValue = $('#groupSemesterSubjectSelect').val();

    if (groupSemesterSubjectSelectValue != 0) {
        console.log("Wybrałeś przedmiot. " + groupSemesterSubjectSelectValue);
        $("#fileIdTb2").val($("#shareFileButton").val());
        $("#shareFileWithGroupButton").removeAttr("disabled");
    } else {
        $("#shareFileWithGroupButton").attr("disabled", "true");
        $("#fileIdTb2").val(0);
    }
}

function PrivateSchoolChosen() {
    var schoolSelectValue = $("#privateSchoolSelect").val();

    ClearSelect(document.getElementById("privateSubjectSelect"));
    ClearSelect(document.getElementById("privateSemesterSelect"));
    ClearSelect(document.getElementById("privateUserSelect"));
    DisableSelect(document.getElementById("privateSubjectSelect"));
    DisableSelect(document.getElementById("privateSemesterSelect"));
    DisableSelect(document.getElementById("privateUserSelect"));

    if (schoolSelectValue != 0) {
        console.log("Wybrałeś szkołę. " + schoolSelectValue);
        $.getJSON("/LoggedIn/GetStudySubjectsJson", { universityId: schoolSelectValue }, handleJsonSubjectsPrivate);
        EnableSelect(document.getElementById('privateSubjectSelect'));
    } else {
        $('#shareFileWithUserButton').attr("disabled", "true");
    }
}

function PrivateSubjectChosen() {
    var subjectSelectValue = $("#privateSubjectSelect").val();

    ClearSelect(document.getElementById("privateSemesterSelect"));
    ClearSelect(document.getElementById("privateUserSelect"));
    DisableSelect(document.getElementById("privateSemesterSelect"));
    DisableSelect(document.getElementById("privateUserSelect"));

    if (subjectSelectValue != 0) {
        console.log("Wybrałeś kierunek. " + subjectSelectValue);
        $.getJSON("/LoggedIn/GetSemestersJson", { studySubjectId: subjectSelectValue }, handleJsonSemestersPrivate);
        EnableSelect(document.getElementById('privateSemesterSelect'));
    } else {
        $('#shareFileWithUserButton').attr("disabled", "true");
    }
}

function PrivateSemesterChosen() {
    var semesterSelectValue = $("#privateSemesterSelect").val();

    ClearSelect(document.getElementById("privateUserSelect"));
    DisableSelect(document.getElementById("privateUserSelect"));

    if (semesterSelectValue != 0) {
        console.log("Wybrałeś semestr. " + semesterSelectValue);
        $.getJSON("/LoggedIn/GetSemesterUsersJson", { semesterId: semesterSelectValue }, handleJsonUsersPrivate);
        EnableSelect(document.getElementById('privateUserSelect'));
    } else {
        $('#shareFileWithUserButton').attr("disabled", "true");
    }
}

function PrivateUserChosen() {
    var userSelectValue = $("#privateUserSelect").val();

    if (userSelectValue != 0) {
        console.log("Wybrałeś użytkownika. " + userSelectValue);
        $("#fileIdTb1").val($("#shareFileButton").val());
        $("#shareFileWithUserButton").removeAttr("disabled");
    } else {
        $("#shareFileWithUserButton").attr("disabled", "true");
    }
}