(function (global) {

    if (typeof (global) === "undefined") {
        throw new error("window is undefined");
    }

    var _hash = "!";
    var noBackNavigation = function () {
        global.location.href += "#";

        global.setTimeout(function () {
            global.location.href += "!";
        }, 50);
    };

    global.onhashchange = function () {
        if (global.location.hash !== _hash) {
            global.location.hash = _hash;
        }
    };

    global.onload = function () {
        noBackNavigation();

        //Disables backspace on page exept on input fields and textarea
        document.body.onkeydown = function (e) {
            var element = e.target.nodeName.toLowerCase();
            if (e.which === 8 && (element !== 'input' && element !== 'textarea')) {
                e.preventDefault();
            };
        }
    }
})(window);