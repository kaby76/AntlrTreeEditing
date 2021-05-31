
build:
	dotnet restore
	dotnet build

publish:
	dotnet nuget push bin/Debug/AntlrTreeEditing.${version}.nupkg --api-key ${trashkey} --source https://api.nuget.org/v3/index.json

clean:
	rm -rf */obj */bin
	rm -rf ${USERPROFILE}/.nuget/packages/AntlrTreeEditing
	rm -f bin/Debug/*.nupkg bin/Debug/*.snupkg

