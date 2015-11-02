var DatepickerDemoCtrl = function ($scope, $filter, $http) {
    $scope.today = function () {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function () {
        $scope.minDate = new Date(2014, 10, 29);
    };
    $scope.toggleMin();

    $scope.maxDate = new Date();

    $scope.open = function ($event) {
        $scope.status.opened = true;
    };
    
    $scope.changedate = function ($event) {
        var dateEx = $filter('date')($scope.dt, 'shortDate');
        console.log(dateEx);

        $http({
            method: 'GET',
            url: '/Home/QueryServiceWebRole',
            data: { DateEx: $filter('date')($scope.dt, 'shortDate') },
            dataType: 'json',
            cache: false
        }).success(function (data, status, headers, config) {
            $scope.wod = data[0].Title;
        });
        
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];

    $scope.status = {
        opened: false
    };

    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var afterTomorrow = new Date();
    afterTomorrow.setDate(tomorrow.getDate() + 2);
    $scope.events =
      [
        {
            date: tomorrow,
            status: 'full'
        },
        {
            date: afterTomorrow,
            status: 'partially'
        }
      ];

    $scope.getDayClass = function (date, mode) {
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    };
};
DatepickerDemoCtrl.$inject = ['$scope', '$filter', '$http'];