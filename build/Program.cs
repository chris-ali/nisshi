using System;
using System.Collections.Generic;
using System.IO;
using GlobExpressions;
using static Bullseye.Targets;
using static SimpleExec.Command;

/// <summary>
/// Runner class for Bullseye targets to clean, build, publish Nisshi project
/// </summary>

const string Clean = "clean";
const string Build = "build";
const string Test = "test";
const string Format = "format";
const string Publish = "publish";
const string Default = "default";

Target(Clean, ForEach(Publish, "**/bin", "**/obj"), dir =>
{
    IEnumerable<string> GetDirectories(string d)
    {
        return Glob.Directories(".", d);
    }

    void RemoveDirectory(string d)
    {
        if (Directory.Exists(d))
        {
            Console.WriteLine($"Cleaning directory: {d}");
            Directory.Delete(d, true);
        }
    }

    foreach (var d in GetDirectories(dir))
    {
        RemoveDirectory(d);
    }
});

Target(Format, () =>
{
    Run("dotnet", "tool restore");
    Run("dotnet", "format --check");
});

Target(Build, DependsOn(Format), () =>
{
    Run("dotnet", "build . -c Release");
});

Target(Test, DependsOn(Build), () =>
{
    IEnumerable<string> GetFiles(string d)
    {
        return Glob.Files(".", d);
    }

    foreach (var file in GetFiles("tests/**/*.csproj"))
    {
        Run("dotnet", $"test {file} -c Release --no-restore --no-build --verbosity=normal");
    }
});

Target(Publish, DependsOn(Test), ForEach("src/Nisshi"), project =>
{
    Run("dotnet",
        $"publish {project} -c Release -f net5.0 -o ./publish --no-restore --no-build --verbosity=normal");
});

Target(Default, DependsOn(Publish), () =>
{
    Console.WriteLine("...done!");
});

await RunTargetsAndExitAsync(args);
