describe('userLogin', function() {
  beforeEach(module('14c'));

  var element, rootScope;
  beforeEach(function() {
    element = angular.element('<user-login/>');
    inject(function($rootScope, $compile) {
      rootScope = $rootScope;
      $compile(element)(rootScope);
      rootScope.$digest();
    });
  });

  it('invoke directive', function() {
    rootScope.$digest();
    expect(element).toBeDefined();
  });
});
