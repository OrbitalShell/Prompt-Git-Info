using OrbitalShell.Component.Shell.Module;
using OrbitalShell.Lib;

/// <summary>
/// declare a shell module
/// </summary>
[assembly: ShellModule("OrbitalShell-Module-PromptGitInfo")]
[assembly: ModuleTargetPlateform(TargetPlatform.Any)]
[assembly: ModuleShellMinVersion("1.0.9")]
[assembly: ModuleAuthors("Orbital Shell team")]
namespace OrbitalShell.Module.PromptGitInfo
{
    public class ModuleInfo { }
}
