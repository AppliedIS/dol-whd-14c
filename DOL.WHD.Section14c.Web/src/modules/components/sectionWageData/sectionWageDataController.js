'use strict';

module.exports = function(ngModule) {
  ngModule.controller('sectionWageDataController', function(
    $scope,
    $location,
    $document,
    stateService,
    navService,
    responsesService,
    validationService,
    _constants
  ) {
    'ngInject';
    'use strict';

    $scope.formData = stateService.formData;
    $scope.validate = validationService.getValidationErrors;
    $scope.showAllHelp = false;

    // the Wage Data section should not be completed for Initial applications,
    // so redirect if necessary.
    if (
      $scope.formData.applicationTypeId ===
      _constants.responses.applicationType.initial
    ) {
      navService.clearNextQuery();
      navService.goNext();
    }

    // multiple choice responses
    let questionKeys = ['PayType'];
    responsesService.getQuestionResponses(questionKeys).then(responses => {
      $scope.responses = responses;
    });

    let query = $location.search();

    var vm = this;
    vm.activeTab = query.t ? query.t : 1;
    vm.showLinks = false;

    vm.onTabClick = function(id) {
      vm.activeTab = id;

      vm.setNextTabQuery(id);
    };

    vm.setNextTabQuery = function(id) {
      if (id === 1) {
        navService.setNextQuery(
          { t: 2 },
          'Next: Add Piece Rate',
          'wagedata_tab_box'
        );
      } else {
        navService.clearNextQuery();
      }
    };

    vm.tabPanelFocus = function(id) {
      if (id === 1) {
          $document[0].getElementById('hourlyTabPanel').focus();
      } else {
          $document[0].getElementById('pieceRateTabPanel').focus();
      }
    };

    $scope.$on('$routeUpdate', function() {
      query = $location.search();
      vm.activeTab = query.t ? query.t : 1;
      vm.setNextTabQuery(vm.activeTab);
    });

    $scope.$watch('formData.payTypeId', function(value) {
      if (value === 23 && vm.activeTab === 1) {
        vm.setNextTabQuery(1);
      } else {
        vm.setNextTabQuery();
      }
    });

    this.toggleAllHelpText = function() {
      $scope.showAllHelp = !$scope.showAllHelp;
    }    

    // Sliding Panel (Helpful Links)
    $('.cd-btn').on('click', function(event){
      event.preventDefault();
      $('.cd-panel').addClass('is-visible');
    });

    //close the panel
    $('.cd-panel').on('click', function(event){
      if( $(event.target).is('.cd-panel') || $(event.target).is('.cd-panel-close') ) {
        $('.cd-panel').removeClass('is-visible');
        event.preventDefault();
      }
    });

  });
};
