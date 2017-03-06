Param(   
   [Parameter(Mandatory=$True)][String]$configuration
)
$destDir = ".\HttpTriggerCSharp1\bin"
If ( ! (Test-Path -Path $destDir) ){
	mkdir $destDir
}
Copy-Item ..\ClassLibrary1\bin\$configuration\*.* $destDir 
if ( ! $? ){
	Exit 1
}
