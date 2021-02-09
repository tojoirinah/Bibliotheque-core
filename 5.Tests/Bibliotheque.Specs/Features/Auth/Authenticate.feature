Feature: Authentication user
	The user try to log to application and when he authenticated, systems returns his information
	with token to access for all services in the application otherwise, systems returns errors

	@CatchException
	Scenario: An non existing user try to log in
		Given the login is 'admin1@test.com'
		And the password is '123456'
		When calling authenciation
		Then throws not found error
		And user is null

	Scenario: An existing user with bad password try to log in
		Given the login is 'admin@test.com'
		And the password is '1234567'
		When calling authenciation
		Then throws credential error
		And user is null

	Scenario: An exising user with correct password ty to log in
		Given the login is 'admin@test.com'
		And the password is '123456'
		When calling authenciation
		Then user is not null