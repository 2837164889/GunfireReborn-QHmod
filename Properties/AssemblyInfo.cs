using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(QHMod.BuildInfo.Description)]
[assembly: AssemblyDescription(QHMod.BuildInfo.Description)]
[assembly: AssemblyCompany(QHMod.BuildInfo.Company)]
[assembly: AssemblyProduct(QHMod.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + QHMod.BuildInfo.Author)]
[assembly: AssemblyTrademark(QHMod.BuildInfo.Company)]
[assembly: AssemblyVersion(QHMod.BuildInfo.Version)]
[assembly: AssemblyFileVersion(QHMod.BuildInfo.Version)]
[assembly: MelonInfo(typeof(QHMod.QHMod), QHMod.BuildInfo.Name, QHMod.BuildInfo.Version, QHMod.BuildInfo.Author, QHMod.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]