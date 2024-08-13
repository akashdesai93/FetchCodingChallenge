Feature: Gold Bar Validation



Scenario: Identify the fake gold bar using the balance scale
	Given I have navigated to the gold bar weighing website
	When I find the fake gold bar
	And I click on the correct gold bar button
	Then I should see the message "Yay! You find it!"