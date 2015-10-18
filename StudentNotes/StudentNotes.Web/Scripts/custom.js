window.onload = function () {
    var panels = document.getElementsByClassName("collapsing-panel-header");
    console.log(panels.length);
    for (var i = 0; i < panels.length; i++) {
        panels[i].onmouseover = highlightPanel;
        panels[i].onmouseout = cancelPanelHighlight;
    }
};

function highlightPanel(eventObj) {
    var searchedParent = "panel-heading";
    var parent = eventObj.target.parentNode;

    while (parent.getAttribute('class') != searchedParent) {
        parent = parent.parentNode;
    }

    var parentClassName = parent.className;
    parentClassName += " highlighted";
    parent.setAttribute("class", parentClassName);
    console.log(parent);
}

function cancelPanelHighlight(eventObj) {
    var searchedParent = "panel-heading highlighted";
    var parent = eventObj.target.parentNode;

    while (parent.getAttribute('class') != searchedParent) {
        parent = parent.parentNode;
    }

    var parentClassName = parent.className;
    parentClassName = "panel-heading";
    parent.setAttribute("class", parentClassName);
    console.log(parent);
}

$('.collapse').collapse({
    toggle: false
});
$('.carousel').carousel({
    interval: 5000 //changes the speed
});