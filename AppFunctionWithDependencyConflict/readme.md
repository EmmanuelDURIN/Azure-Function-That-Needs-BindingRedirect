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