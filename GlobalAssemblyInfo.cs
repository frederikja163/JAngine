using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyProduct("JAngine")]
[assembly: AssemblyCompany("FrederikJA")]
[assembly: AssemblyCopyright("Copyright © FrederikJA 2020-2023")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// TODO: Versioning, take a look at: https://devtut.github.io/csharp/assemblyinfo-cs-examples.html#assemblyversion
[assembly: AssemblyVersion("1.0.0.0")] 
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("JAngine.Tests")]
