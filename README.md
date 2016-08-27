# PSShell
PSShell gets the job done when harsh group policy restrictions are in place.

### What is it:

PSShell is an application written in C# that does not rely on powershell.exe but runs powershell commands and functions within a powershell runspace environment (.NET). It doesn't need to be "installed" so it's very portable.

Because it calls powershell directly through the .NET framework it might help bypassing security controls like GPO, SRP, App Locker.

### Release 2.0 - How to Compile it:

To compile PSShell 2.0 you need to import this project within Microsoft Visual Studio. If you don't have access to a Visual Studio installation stick with version 1.0 for now. See below hot to build it with csc.exe.

Since PSShell 2.0 uses [Unmanaged Exports](https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports) from Robert Giesecke follow the steps below to build the project.

1. Import the project in Visual Studio
2. Install [NuGet Package Manager](https://docs.nuget.org/consume/installing-nuget)
3. In Visual Studio go to Tools -> NuGet Package Manager -> Package Manager Console
4. Proceed as follows:
```
PM> Install-Package UnmanagedExports
Installing 'UnmanagedExports 1.2.7'.
Successfully installed 'UnmanagedExports 1.2.7'.
Adding 'UnmanagedExports 1.2.7' to PSShell.
Successfully added 'UnmanagedExports 1.2.7' to PSShell.
```
Finally Press F7

### Release 2.0 - How to use it:

```
rundll32 PSShell.dll,EntryPoint
```

Or take advantage of @subTee's 'regsvr32' tricks to bypass AppLocker/White Listing:
```
regsvr32 PSShell.dll
```
or
```
regsvr32 /u PSShell.dll
```

### Release 1.0 - How to Compile it:

To compile PSShell you need to import this project within Microsoft Visual Studio or if you don't have access to a Visual Studio installation, you can compile it as follows:

To Compile as x86 binary:

```
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319 (Or newer .NET version folder)

csc.exe /unsafe /reference:"C:\path\to\System.Management.Automation.dll" /reference:System.IO.Compression.dll /out:C:\users\username\PowerOPS_x86.exe /platform:x86 "C:\path\to\PowerOPS\PowerOPS\*.cs"
```

To Compile as x64 binary:

```
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319 (Or newer .NET version folder)

csc.exe /unsafe /reference:"C:\path\to\System.Management.Automation.dll" /reference:System.IO.Compression.dll /out:C:\users\username\PowerOPS_x64.exe /platform:x64 "C:\path\to\PowerOPS\PowerOPS\*.cs"
```

PSShell uses the System.Management.Automation namespace, so make sure you have the System.Management.Automation.dll within your source path when compiling outside of Visual Studio.

### Release 1.0 - How to use it:

Just run the executables or take advantage of @subTee's InstallUtil AppLocker/White Listing bypass:

```
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /logfile= /LogToConsole=false /U PSShell.exe
```

### Credits

- The 'regsvr32' tricks, as usual were 'stolen' from @subTee [gists](https://gist.github.com/subTee/f6123584a3258783e497481690ccc38d).
- The 'InstallUtil' trick, was again 'stolen' from @subTee [gists](https://gist.github.com/subTee/af5c60a07977180c8bad).
- The DLLExport is courtesy of [Robert Giesecke](https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports) and the idea of using it came from the following @subTee blog [post](http://subt0x10.blogspot.co.uk/2016/06/what-you-probably-didnt-know-about.html) about 'regsvr32'.
- The code to run a console window from a Windows Application was 'stolen' from [here](https://social.msdn.microsoft.com/Forums/en-US/b7a14400-6d72-4fbf-9927-0966f69ef4a2/how-to-open-console-window-in-windows-apllication?forum=csharplanguage).

