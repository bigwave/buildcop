Read Me
=======

A sample buildcop.config is added to the project when the package installed.

If you want to use a standard centralised configuration you can include external files as shown below:

<!DOCTYPE buildCopConfiguration [
  <!ENTITY buildGroups SYSTEM "../somewhere/buildcop.buildGroups.config" >
  <!ENTITY sharedRules SYSTEM "../somewhere/buildcop.sharedRules.config" >
  <!ENTITY formatters SYSTEM "../somewhere/buildcop.formatters.config" >
]>
<buildCopConfiguration xmlns="http://schemas.jelle.druyts.net/BuildCop">
  &buildGroups;
  &sharedRules;
  &formatters;
</buildCopConfiguration>
