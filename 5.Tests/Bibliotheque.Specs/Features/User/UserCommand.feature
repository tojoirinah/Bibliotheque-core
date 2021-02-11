Feature: UserCommand
	This feature change the status of user

	Scenario Outline: Add a new inexisting user
		When I enter a new User member or visitor or supervisor or librarian with information <lastname>, <firstname>, <login>, <password>, <securitysalt>, <roleid>, <statusid>
		And I call RegisterUser to store the user
		Then list of <roleid> should be <expectedCount>

		Examples: 
		| lastname                 | firstname                 | login                          | password | securitysalt | roleid | statusid | expectedCount |
		| 'membre_lastname_23'     | 'membre_firstname_23'     | 'membre_login_23@test.com'     | '123456' | '123456'     | 4      | 3        | 13            |
		| 'visitor_lastname_24'    | 'visitor_firstname_24'    | 'visitor_login_24@test.com'    | '123456' | '123456'     | 5      | 3        | 1             |
		| 'supervisor_lastname_25' | 'supervisor_firstname_25' | 'supervisor_login_25@test.com' | '123456' | '123456'     | 2      | 1        | 1             |
		| 'librarian_lastname_26'  | 'librarian_firstname_26'  | 'librarian_login_26@test.com'  | '123456' | '123456'     | 3      | 3        | 1             |

	Scenario Outline: Add an existing member
		When I enter a new member <lastname>, <firstname>, <login>, <password>, <securitysalt>, <roleid>, <statusid>
		And I call RegisterUser to store the user
		Then throw error UserAlreadyExistException

		Examples: 
		| lastname             | firstname             | login            | password | securitysalt | roleid | statusid |
		| 'membre_lastname_27' | 'membre_firstname_27' | 'admin@test.com' | '123456' | '123456'     | 1      | 3        |

	Scenario Outline: Update inexisting user
		When I enter a new information (<id>, <lastname>, <firstname>) of user by id
		And I call ChangeUser to update user information
		Then throw error UserNotFoundException

		Examples: 
		| id   | lastname  | firstname |
		| 1000 | 'Smith'   | 'John'    |
		| 1001 | 'Sparrow' | 'Jack'    |

	Scenario Outline: Update a lastname or firstname
		When I enter a new information (<id>, <lastname>, <firstname>) of user by id
		And I call ChangeUser to update user information
		Then user information updated (<id>, <lastname>, <firstname>)

		Examples: 
		| id | lastname  | firstname |
		| 12 | 'Smith'   | 'John'    |
		| 13 | 'Sparrow' | 'Jack'    |

	Scenario Outline: Change user status
		When I change status of user by userid with id: <id> and status: <statusId>
		And I call ChangeUser to update user status
		Then user status information updated with id: <id> and status: <statusId>

		Examples: 
		| id | statusId |
		| 10 | 2        |
		| 11 | 1        |

	Scenario Outline: Remove user
		When I select user <id>
		And I call service UnregisterUser to remove user
		Then user with <id> should be removed

		Examples: 
		| id |
		| 13 |
		| 17 |
