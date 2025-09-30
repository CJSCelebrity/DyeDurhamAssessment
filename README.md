#  Dye and Durham Assessment - Name Sorter Application
## Overview

The following application is built within the dotnet 9 environment and it is a console applicatiom. This application takes in a text file that has unsorted names
and creates an output file titled sorted-names-list.txt that is stored in the Output folder of the project. ***It uses a simple LINQ statement to order that set first by last name, then by any given names the person may have.***

The application also makes use of ***Github Actions*** to validate the unit tests and the application integrity when a new commit is created or a pull request is created.
You will also be able to see the output of the ordered names of the application in the ***Run Application*** build step

## Prerequisites

The application makes use of .NET 9. Due to this, you may need to install the latest dotnet SDK. You can download it from the official site here - https://dotnet.microsoft.com/en-us/download/dotnet/9.0

## Installation

To clone the repo, perform the following in my command prompt

```
git clone https://github.com/CJSCelebrity/DyeDurhamAssessment.git
```

## Ensure Dependencies are loaded

When the project is cloned, open it up in your IDE. Thereafter, perform a clean and build of the solution to ensure that the dependencies are loaded

## Usage

To use the application with your list, navigate to the ```Assets``` folder in the ``DyeDurhamAssessment.Core`` project and replace the ``unsorted-names-list.txt`` file with your own. Please ensure that the file that you replace it with has the following name ``unsorted-names-list.txt`` 

## Troubleshooting

If throughout the use of the application you encounter any bugs, the application makes use of Serilog which will output the errors to the console and to a log file. 
The log file can be found in the Debug folder of the application. The path would typically look like the following

```
C:\YourUser\YourProjectsYouClonedTheProjectTo\DyeDurhamAssessment\DyeDurhamAssessment.Core\bin\Debug\net9.0\logs
```

## Contribution

If you need to add a new file type to be read into the application, you can create a new reader service in the services folder of the ```DyeDurhamAssessment.Application``` project.
You can then add this reader service into the ```FileReaderFactory``` class. The following code is an example of what it may look like

```
private readonly List<IFileReader> _readers = new()
{
    new TextFileReaderService(),
    new YourFileReaderService()
};
```

## Future Improvements

Currently the file only allows for ````.txt```` files to be read into it. There are future plans to include the following extensions ``.xls`` , ``.xlsx`` and ``.csv``