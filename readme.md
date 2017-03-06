My goal is to use log4net.elasticsearch in an Azure App function, as well much legacy code, with tenth of dlls
I get some class loading problems with multiple Dll requiring different versions


Here is the dependency diagram between the Dlls :

AppFunctionWithDependencyConflict
                A                        
                |                        
                |
          ClassLibrary1.dll
                A       
                |       
		        |       
           log4net.dll ---> Dynamic loading of log4net.elastic 2.3.3 search through XML file description     
		     2.0.3                        A       
		                                  |       
		                                  | Static Dependency  to log4net 1.2.13.0
		                                  |  But log4net.dll is already loaded in another version !     
		                                  |
								       log4net.dll

**ConsoleAppThatWorks** is not an azure function, it is a standard console application 
Everything works in ConsoleAppThatWorks provided the binding redirection is in app.config :

    <runtime>
      <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
        <dependentAssembly>
          <assemblyIdentity name="log4net" publicKeyToken="669E0DDF0BB1AA2A" culture="neutral"/>
          <bindingRedirect oldVersion="0.0.0.0-2.0.7.0" newVersion="2.0.7.0"/>
        </dependentAssembly>
      </assemblyBinding>
    </runtime>

Here is a fusion viewer log (fuslovw.exe) style log that I can get while running my "real, full" (not the sample provided here)
application and that shows I get the loading poroblem because of the function version.
Sorry I couldn't display the msg in the here provided sample. I don't know the reason  !
Sorry, it's in french, but notice :

    AVT : la comparaison du nom de l'assembly a entraîné l'incompatibilité : Version principale

meaning comparison of assembly is incompatible : principal version 
	
	
    JRN : cette liaison démarre dans le contexte de chargement de LoadFrom.
    AVT : l'image native ne sera pas détectée dans le contexte LoadFrom. Elle sera détectée uniquement dans le contexte de chargement par défaut, comme pour Assembly.Load().
    JRN : utilisation du fichier de configuration de l'application : C:\Users\e_dur\AppData\Local\Azure.Functions.Cli\1.0.0-beta.91\func.exe.Config
    JRN : utilisation du fichier de configuration d'hôte :
    JRN : utilisation du fichier de configuration de l'ordinateur à partir de C:\Windows\Microsoft.NET\Framework64\v4.0.30319\config\machine.config.
    JRN : référence post-stratégie : log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/AppData/Local/Azure.Functions.Cli/1.0.0-beta.91/log4net.DLL.
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/AppData/Local/Azure.Functions.Cli/1.0.0-beta.91/log4net/log4net.DLL.
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/AppData/Local/Azure.Functions.Cli/1.0.0-beta.91/log4net.EXE.
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/AppData/Local/Azure.Functions.Cli/1.0.0-beta.91/log4net/log4net.EXE.
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/Documents/zenith-development/zenith-workers/MailFunctionApp-CsxLess/MailSenderFunction/log4net.DLL.
    AVT : la comparaison du nom de l'assembly a entraîné l'incompatibilité : Version principale
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/Documents/zenith-development/zenith-workers/MailFunctionApp-CsxLess/MailSenderFunction/log4net/log4net.DLL.
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/Documents/zenith-development/zenith-workers/MailFunctionApp-CsxLess/MailSenderFunction/log4net.EXE.
    JRN : tentative de téléchargement de la nouvelle URL file:///C:/Users/e_dur/Documents/zenith-development/zenith-workers/MailFunctionApp-CsxLess/MailSenderFunction/log4net/log4net.EXE.

	
Here is the evidence that log4net.elasticsearch needs  log4net in version 1.2.13.0 :
	
    Get-DllDependencies.ps1 .\log4net.ElasticSearch.dll
    
    log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, 1.2.13.0
    mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, 4.0.0.0
    System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, 4.0.0.0
    System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, 4.0.0.0
    System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, 4.0.0.0
    System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, 4.0.0.0
