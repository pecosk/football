angular.module('footballApp')
    .provider('$navBar', function () {
    var defaults = this.defaults = {
        activeClass: 'active',
        routeAttr: 'data-match-route',
        strict: false
    };

    this.$get = function () {
        return { defaults: defaults };
    };
})
    .directive('flNavbar', function ($window, $location, $navBar) {
        var defaults = $navBar.defaults;

        return {
            restrict: 'A',
            link: function postLink(scope, element, attr, controller) {
                // Directive options
                var options = angular.copy(defaults);
                angular.forEach(Object.keys(defaults), function (key) {
                    if (angular.isDefined(attr[key])) options[key] = attr[key];
                });

                // Watch for the $location
                scope.$watch(function () {

                    return $location.path();

                }, function (newValue, oldValue) {

                    var liElements = element[0].querySelectorAll('li[' + options.routeAttr + ']');

                    angular.forEach(liElements, function (li) {

                        var liElement = angular.element(li);
                        var pattern = liElement.attr(options.routeAttr).replace('/', '\\/');
                        if (options.strict) {
                            pattern = '^' + pattern + '$';
                        }
                        var regexp = new RegExp(pattern, 'i');
                        var dotIndex = newValue.indexOf('.');
                        var value = dotIndex === -1 ? newValue : newValue.substr(0, dotIndex);
                        if (regexp.test(value)) {
                            liElement.addClass(options.activeClass);
                        } else {
                            liElement.removeClass(options.activeClass);
                        }

                    });

                });
            }
        }
    });