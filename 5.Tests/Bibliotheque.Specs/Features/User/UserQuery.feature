Feature: UserQueries
	This feature retrieve all query around the user

	Scenario Outline: Search a list user when criteria contain query
		When I enter <name> as name
		And I call userService SearchUser
		Then Response should return the list of user
		And list user should count <count>
		Examples: 
		| name      | count |
		| ''        | 13    |
		| 'member'  | 12    |
		| 'admin'   | 1     |
		| 'strator' | 1     |
		| 'john'    | 0     |

	Scenario Outline: Search an existing user by id
		When I call <id> as userId
		And I call userService RetrieveOneUserById
		Then Response should return the user
		And username should by <username>
		Examples: 
		| id | username         |
		| 1  | 'admin@test.com' |

	Scenario Outline: Search an inexisting user by id
		When I call  <id> as userId
		And I call userService RetrieveOneUserById
		Then Return should be null
		Examples: 
		| id   |
		| 1053 |
		| 72   |

	Scenario Outline: Search an inexisting user by username
		When I enter exactly the '<username>' as username
		And I call userService RetriveOneUserByUsername
		Then Return should be null
		Examples: 
		| username     |
		| 'babali'     |
		| 'monusername |


	Scenario Outline: Search an existing user by username
		When I enter exactly the <username> as username
		And I call userService RetriveOneUserByUsername
		Then Response should return the user
		And username in database should be <expected> as expected
		Examples: 
		| username             | expected             |
		| 'member_01@test.com' | 'member_01@test.com' |
		| 'member_02@test.com' | 'member_02@test.com' |
		| 'admin@test.com'     | 'admin@test.com'     |