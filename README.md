DependencyUpdater
=================

Updates dependency versions in .nuspec file from packages.config file

Usage: DependencyUpdater.exe example.nuspec [packages.config]

If packages.config is not specified it searches for it in the current dir and "../.."
Make sure that you .nuspec file is always copied to your build dir

If you want to integrate it into MSBuild here is a handy way to detect path to exe:

```xml
<ItemGroup>
  <DependencyUpdater Include="..\packages\DependencyUpdater.*\tools\DependencyUpdater.exe">
    <InProject>False</InProject>
  </DependencyUpdater>
</ItemGroup>
<Target Name="AfterBuild">
  <Error Condition="!Exists(@(DependencyUpdater->'%(FullPath)'))" Text="You are trying to use the DependencyUpdater package, but it is not installed. Please install DependencyUpdater from the Package Manager." />
  <Exec Command="@(DependencyUpdater->'%(FullPath)') $(OutDir)$(TargetName).nuspec" />
</Target>
```