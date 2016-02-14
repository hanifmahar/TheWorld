//simpleControls.js

(function () {

    "use strict";

    //Create module for simple controls

    angular.module("simpleControls", [])
    .directive("waitCursor", waitCursor);

    function waitCursor() {

        return {
            scope:{
                show: "=displayWhen"
            },
            restrict:"E",
            templateUrl: "/views/waitCursor.html"
        };
    }

})();