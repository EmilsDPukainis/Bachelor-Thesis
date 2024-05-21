To make the application usable, these instructions need to be followed:
1. Download Visual Studio 17.6.5 professional or enterprise (https://learn.microsoft.com/en-us/visualstudio/releases/2022/release-history), if you have a newer version, it needs to be uninstalled.
2. Install visual studio with the checked workplaces being the .NET development environment and the .NET Multi-platform App UI development.

To run the actual application these steps need to be followed:
1. Open the solution using the Bachelor.sln file and navigate to the Backend project.
2. Open launchSettings.json which is in the Properties folder and follow the command line for applicationURL.
3. Open the BaseURL file in shared.other and do the same as in the applicationURL.
4. Two ways of running the backend is to either in the solution properties set startup projects as backend and frontend, or in command prompt, use cd and address where the backend project folder is located ( cd C:\YourAddress ), after this write dotnet run and it will start the backend process, after which you can run only the frontend and use the application.
5.  When starting the backend for the first time, you have to accept the prompt that lets the backend communicate on public and private networks.


Note: it doesn't seem to want to connect when the IP comes from a WI-FI.  If it doesn't connect, the fault could be with the firewall not allowing the project through, so adding it in the 'allow an app through firewall' could fix it. 
