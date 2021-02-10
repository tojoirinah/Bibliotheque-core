Feature: Authentication user
	The user try to log to application and when he authenticated, systems returns his information
	with token to access for all services in the application otherwise, systems returns errors

	@CatchException
	Scenario: An non existing user try to log in
		Given the login is 'admin1@test.com'
		And the password is '123456'
		When calling authenciation
		Then throws uknown or disabled error
		And user is null

	Scenario: An existing user with bad password try to log in
		Given the login is 'admin@test.com'
		And the password is '1234567'
		When calling authenciation
		Then throws credential error
		And user is null

	Scenario: An existing user with correct password try to log in
		Given the login is 'admin@test.com'
		And the password is '123456'
		When calling authenciation
		Then user is not null

	Scenario: Logged with a disabled user
		Given the login is 'member_01@test.com'
		And the password is '123456'
		When calling authenciation
		Then throws uknown or disabled error
		And user is null

	Scenario: Logged with a waiting user
		Given the login is 'member_02@test.com'
		And the password is '123456'
		When calling authenciation
		Then throws waiting error
		And user is null