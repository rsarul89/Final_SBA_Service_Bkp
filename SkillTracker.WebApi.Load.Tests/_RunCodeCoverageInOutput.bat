﻿"..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" 
-target:"..\..\..\packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe" 
-targetargs:"..\SkillTracker.WebApi.Load.Tests.dll" 
-filter:"+[*]* -[*.Tests*]* -[*]*.*Config"  
-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute"
-register:user 
-output:"_CodeCoverageResult.xml"
@pause

"..\..\..\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe" 
"-reports:_CodeCoverageResult.xml" "-targetdir:_CodeCoverageReport"
@pause

:RunLaunchReport
start "report" "_CodeCoverageReport\index.htm"
exit /b %errorlevel%