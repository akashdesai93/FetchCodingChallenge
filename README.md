# Fetch Coding Challenge - Find the Fake Gold Bar

## Overview

This project is designed to solve the problem of identifying a fake gold bar from a set of nine bars using a balance scale. The solution is implemented using C#, Selenium, SpecFlow, and follows Behavior-Driven Development (BDD) principles.

The project includes automated tests that interact with a web application to weigh groups of gold bars, identify the fake one, and verify the results.

## Project Structure

- **StepDefinitions**: Contains the step definition class `FindFakeGoldBarStepDefinitions.cs` which implements the logic to interact with the web application and perform the tests.
- **Features**: Contains the SpecFlow feature files that describe the test scenarios in Gherkin syntax (not included in this example, but typically located here).
- **Program.cs**: If a console application is used, this file would contain the entry point to run the tests directly without opening Visual Studio (if implemented).
- **README.md**: This document explaining the project and how to run it.

## Prerequisites

- Visual Studio 2019 or later
- .NET Core SDK 3.1 or later (included with Visual Studio)
- Chrome browser (for running Selenium tests)
- NuGet packages:
  - `Selenium.WebDriver`
  - `SpecFlow`
  - `SpecFlow.NUnit`
  - `SpecFlow.Tools.MsBuild.Generation`
  - `WebDriver.ChromeDriver`

## Setup Instructions

1. **Clone the Repository**:
   - Clone this repository to your local machine using:
     ```bash
     git clone https://github.com/yourusername/FetchCodingChallenge.git
     ```

2. **Open in Visual Studio**:
   - Open the solution file (`FetchCodingChallenge.sln`) in Visual Studio.

3. **Restore NuGet Packages**:
   - Visual Studio should automatically restore the required NuGet packages. If not, right-click on the solution in the Solution Explorer and select `Restore NuGet Packages`.

4. **Build the Solution**:
   - Build the solution by selecting `Build > Build Solution` from the menu or pressing `Ctrl+Shift+B`.

5. **Configure the Browser**:
   - Ensure that Google Chrome is installed as it is the browser used by Selenium in this project.

## Running the Tests

### Running Tests from Visual Studio

1. **Open Test Explorer**:
   - Open the Test Explorer by selecting `Test > Test Explorer` from the menu.

2. **Run All Tests**:
   - In the Test Explorer, you should see the available tests. Click `Run All` to execute all tests.

3. **View Test Results**:
   - After the tests have run, the results will be displayed in the Test Explorer. You can view which tests passed or failed and see the detailed output.

### Running from Command Line (Optional)

If you prefer to run the tests from the command line, you can use the `dotnet test` command:

1. **Navigate to the Project Directory**:
   ```bash
   cd FetchCodingChallenge
2. **Run the Tests**:
   ```bash
   dotnet test
