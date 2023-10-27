namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1500
{

    private const string Namespace1 = @"
namespace ConsistencyAnalyzer { }
";

    private const string Namespace2 = @"
namespace ConsistencyAnalyzer
{
}
";

    private const string UsingOutside = @"
using System;

namespace ConsistencyAnalyzer
{
}
";

    private const string UsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
}
";

    private const string EnumDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes { }
}
";

    private const string EnumDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes
    {
    }
}
";

    private const string ClassDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes { }
}
";

    private const string ClassDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
    }
}
";

    private const string StructDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public struct CodeFixes { }
}
";

    private const string StructDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public struct CodeFixes
    {
    }
}
";

    private const string RecordDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public record CodeFixes { }
}
";

    private const string RecordDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public record CodeFixes
    {
    }
}
";

    private const string InterfaceDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public interface ICodeFixes { }
}
";

    private const string InterfaceDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public interface ICodeFixes
    {
    }
}
";

    private const string DelegateDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public delegate void Test();
    }
}
";

    private const string EventDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public event System.EventHandler Test;
    }
}
";

    private const string FieldDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test;
    }
}
";

    private const string MethodDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() {}
    }
}
";

    private const string MethodDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test()
        {
        }
    }
}
";

    private const string PropertyDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test { get; set; }
    }
}
";

    private const string PropertyDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test
        {
            get;
            set;
        }
    }
}
";

    private const string PropertyDeclaration3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test
        {
            get { return 0; }
            set { }
        }
    }
}
";

    private const string PropertyDeclaration4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test
        {
            get 
            {
                return 0;
            }
            set
            {
            }
        }
    }
}
";

    private const string EnumMemberDeclaration = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes
    {
        Analyzer,
    }
}
";

    private const string ForStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++) { }
        }
    }
}
";

    private const string ForBreakContinueStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                continue;
                break;
            }
        }
    }
}
";

    private const string ForBreakContinueStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                {
                    continue;
                    break;
                }
            }
        }
    }
}
";

    private const string ForBreakContinueStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            {
                for (int i = 0; i < 1; i++)
                {
                    continue;
                    break;
                }
            }
        }
    }
}
";

    private const string ForBreakContinueStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
                for (int j = 0; j < 1; j++)
                {
                    continue;
                    break;
                }
        }
    }
}
";

    private const string CheckedStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            checked { }
        }
    }
}
";

    private const string CheckedStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            checked
            {
            }
        }
    }
}
";

    private const string ForEachStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            foreach (int Item in new int[] { 0 }) { }
        }
    }
}
";

    private const string ForEachStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            foreach (int Item in new int[] { 0 })
            {
            }
        }
    }
}
";

    private const string DoStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do { } while (true);
        }
    }
}
";

    private const string DoStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do { }
            while (true);
        }
    }
}
";

    private const string DoStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do
            {
            } while (true);
        }
    }
}
";

    private const string DoStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do
            {
            }
            while (true);
        }
    }
}
";

    private const string FixedStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        unsafe public void Test() 
        {
            Point Point = new Point();
            fixed (int* p = &Point.x) { }
        }
    }

    public class Point
    {
        public int x;
        public int y;
    }
}
";

    private const string FixedStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        unsafe public void Test() 
        {
            Point Point = new Point();
            fixed (int* p = &Point.x)
            {
            }
        }
    }

    public class Point
    {
        public int x;
        public int y;
    }
}
";

    private const string IfElseStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true) { } else { }
        }
    }
}
";

    private const string IfElseStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true)
            {
            } else { }
        }
    }
}
";

    private const string IfElseStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true) { }
            else { }
        }
    }
}
";

    private const string IfElseStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true) { }
            else
            {
            }
        }
    }
}
";

    private const string IfElseStatement5 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true)
            {
            }
            else
            {
            }
        }
    }
}
";

    private const string IfElseStatement6 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true)
            {
            }
            else if (true)
            {
            }
            else if (true)
            {
            }
            else
            {
            }
        }
    }
}
";

    private const string LocalDeclarationStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            int i = 0;
        }
    }
}
";

    private const string LockStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            int[] x = new int[0];
            lock (x) { }
        }
    }
}
";

    private const string LockStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            int[] x = new int[0];
            lock (x)
            {
            }
        }
    }
}
";

    private const string ReturnStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test() 
        {
            return 0;
        }
    }
}
";

    private const string SwitchBreakStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i) { case 0: break; default: break; }
        }
    }
}
";

    private const string SwitchBreakStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i)
            { case 0: break; default: break;
            }
        }
    }
}
";

    private const string SwitchBreakStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i)
            {
                case 0: break;
                default: break;
            }
        }
    }
}
";

    private const string SwitchBreakStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i)
            {
                case 0:
                    break;
                default:
                    break;
            }
        }
    }
}
";

    private const string ThrowStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            throw new System.Exception(""msg"");
        }
    }
}
";

    private const string TryCatchStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try { return; } catch { return; }
        }
    }
}
";

    private const string TryCatchStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try
            {
                return;
            } catch { return; }
        }
    }
}
";

    private const string TryCatchStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try { return; }
            catch
            {
                return;
            }
        }
    }
}
";

    private const string TryCatchStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try
            {
                return;
            }
            catch
            { return; }
        }
    }
}
";

    private const string TryCatchStatement5 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try
            {
                return;
            }
            catch
            {
                return;
            }
        }
    }
}
";

    private const string UnsafeStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            unsafe
            {
            }
        }
    }
}
";

    private const string UnsafeStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            unsafe { }
        }
    }
}
";

    private const string UsingStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(string.Empty, System.IO.FileMode.Open))
            {
            }
        }
    }
}
";

    private const string UsingStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(string.Empty, System.IO.FileMode.Open)) { }
        }
    }
}
";

    private const string WhileStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            while (true) { }
        }
    }
}
";

    private const string WhileStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            while (true)
            {
            }
        }
    }
}
";

    private const string YieldBreakStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
                yield break;
            }
        }
    }
}
";

    private const string YieldReturnStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
                yield return i;
            }
        }
    }
}
";

    private const string test = @"
namespace Packager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Xml.Linq;
    using Contracts;

    /// <summary>
    /// Reads and parses a project file.
    /// </summary>
    [DebuggerDisplay(""{ProjectName}, {RelativePath}, {ProjectGuid}"")]
    internal class Project
    {
        #region Init
        static Project()
        {
            ConsoleDebug.Write(""Loading ProjectInSolution assembly..."");

            ProjectInSolutionType = ReflectionTools.GetProjectInSolutionType(""ProjectInSolution"");

            ProjectInSolutionProjectName = ReflectionTools.GetTypeProperty(ProjectInSolutionType, nameof(ProjectName));
            ProjectInSolutionRelativePath = ReflectionTools.GetTypeProperty(ProjectInSolutionType, nameof(RelativePath));
            ProjectInSolutionProjectGuid = ReflectionTools.GetTypeProperty(ProjectInSolutionType, nameof(ProjectGuid));
            ProjectInSolutionProjectType = ReflectionTools.GetTypeProperty(ProjectInSolutionType, nameof(ProjectType));
        }

        private static readonly Type ProjectInSolutionType;
        private static readonly PropertyInfo ProjectInSolutionProjectName;
        private static readonly PropertyInfo ProjectInSolutionRelativePath;
        private static readonly PropertyInfo ProjectInSolutionProjectGuid;
        private static readonly PropertyInfo ProjectInSolutionProjectType;

        /// <summary>
        /// Initializes a new instance of the <see cref=""Project""/> class.
        /// </summary>
        /// <param name=""solutionProject"">The project as loaded from a solution.</param>
        public Project(object solutionProject)
        {
            ProjectName = (string)ReflectionTools.GetPropertyValue(ProjectInSolutionProjectName, solutionProject);
            RelativePath = (string)ReflectionTools.GetPropertyValue(ProjectInSolutionRelativePath, solutionProject);
            ProjectGuid = (string)ReflectionTools.GetPropertyValue(ProjectInSolutionProjectGuid, solutionProject);

            object Type = ReflectionTools.GetPropertyValue(ProjectInSolutionProjectType, solutionProject);
            Contract.RequireNotNull(Type.ToString(), out string ProjectTypeName);
            ProjectType = ProjectTypeName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the project name.
        /// </summary>
        public string ProjectName { get; init; }

        /// <summary>
        /// Gets the project relative path.
        /// </summary>
        public string RelativePath { get; init; }

        /// <summary>
        /// Gets the project GUID.
        /// </summary>
        public string ProjectGuid { get; init; }

        /// <summary>
        /// Gets the project type.
        /// </summary>
        public string ProjectType { get; init; }

        /// <summary>
        /// Gets the project version.
        /// </summary>
        public string Version { get; private set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the project has a version.
        /// </summary>
        public bool HasVersion => Version.Length > 0;

        /// <summary>
        /// Gets a value indicating whether the project has a valid assembly version.
        /// </summary>
        public bool IsAssemblyVersionValid { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the project has a valid file version.
        /// </summary>
        public bool IsFileVersionValid { get; private set; }

        /// <summary>
        /// Gets the project author.
        /// </summary>
        public string Author { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the project description.
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the project copyright text.
        /// </summary>
        public string Copyright { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the project repository URL.
        /// </summary>
        public Uri? RepositoryUrl { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the project has a repository URL.
        /// </summary>
        public bool HasRepositoryUrl => RepositoryUrl != null;

        /// <summary>
        /// Gets the list of parsed project frameworks.
        /// </summary>
        public IReadOnlyList<Framework> FrameworkList { get; private set; } = new List<Framework>().AsReadOnly();

        /// <summary>
        /// Gets a value indicating whether the project has target frameworks.
        /// </summary>
        public bool HasTargetFrameworks => FrameworkList.Count > 0;
        #endregion

        #region Client Interface
        /// <summary>
        /// Parses a loaded project.
        /// </summary>
        /// <param name=""hasErrors"">Set to true upon return if an error was found.</param>
        public void Parse(ref bool hasErrors)
        {
            ParsePropertyGroupElements(out string AssemblyVersion, out string FileVersion);

            if (HasVersion)
            {
                IsAssemblyVersionValid = AssemblyVersion.StartsWith(Version, StringComparison.InvariantCulture);
                if (!IsAssemblyVersionValid)
                {
                    hasErrors = true;
                    ConsoleDebug.Write($""    ERROR: {AssemblyVersion} not compatible with {Version}"", true);
                }

                IsFileVersionValid = FileVersion.StartsWith(Version, StringComparison.InvariantCulture);
                if (!IsFileVersionValid)
                {
                    hasErrors = true;
                    ConsoleDebug.Write($""    ERROR: {FileVersion} not compatible with {Version}"", true);
                }
            }
            else
                ConsoleDebug.Write(""    Ignored because no version"");

            List<Framework> ParsedFrameworkList = new List<Framework>();

            if (TargetFrameworks.Length > 0)
                ParseTargetFrameworks(ParsedFrameworkList);

            FrameworkList = ParsedFrameworkList.AsReadOnly();
        }

        /// <summary>
        /// Returns a nuspec from this project.
        /// </summary>
        /// <returns>The created nuspec.</returns>
        public Nuspec ToNuspec()
        {
            Contract.RequireNotNull(RepositoryUrl, out Uri ParsedUrl);

            return new Nuspec(ProjectName, RelativePath, Version, Author, Description, Copyright, ParsedUrl, FrameworkList);
        }
        #endregion

        #region Implementation
        private void ParsePropertyGroupElements(out string assemblyVersion, out string fileVersion)
        {
            Version = string.Empty;

            assemblyVersion = string.Empty;
            fileVersion = string.Empty;

            XElement Root = XElement.Load(RelativePath);

            foreach (XElement ProjectElement in Root.Descendants(""PropertyGroup""))
                ParseProjectElement(ProjectElement, ref assemblyVersion, ref fileVersion);
        }

        private void ParseProjectElement(XElement projectElement, ref string assemblyVersion, ref string fileVersion)
        {
            ParseProjectElementVersion(projectElement, ref assemblyVersion, ref fileVersion);
            ParseProjectElementInfo(projectElement);
            ParseProjectElementFrameworks(projectElement);
        }

        private void ParseProjectElementVersion(XElement projectElement, ref string assemblyVersion, ref string fileVersion)
        {
            XElement? VersionElement = projectElement.Element(""Version"");
            if (VersionElement != null)
            {
                Version = VersionElement.Value;
                ConsoleDebug.Write($""    Version: {Version}"");
            }

            XElement? AssemblyVersionElement = projectElement.Element(""AssemblyVersion"");
            if (AssemblyVersionElement != null)
            {
                assemblyVersion = AssemblyVersionElement.Value;
                ConsoleDebug.Write($""    AssemblyVersion: {assemblyVersion}"");
            }

            XElement? FileVersionElement = projectElement.Element(""FileVersion"");
            if (FileVersionElement != null)
            {
                fileVersion = FileVersionElement.Value;
                ConsoleDebug.Write($""    FileVersion: {fileVersion}"");
            }
        }

        private void ParseProjectElementInfo(XElement projectElement)
        {
            XElement? AuthorElement = projectElement.Element(""Authors"");
            if (AuthorElement != null)
                Author = AuthorElement.Value;

            XElement? DescriptionElement = projectElement.Element(""Description"");
            if (DescriptionElement != null)
                Description = DescriptionElement.Value;

            XElement? CopyrightElement = projectElement.Element(""Copyright"");
            if (CopyrightElement != null)
                Copyright = CopyrightElement.Value;

            XElement? RepositoryUrlElement = projectElement.Element(""RepositoryUrl"");
            if (RepositoryUrlElement != null)
            {
                RepositoryUrl = new Uri(RepositoryUrlElement.Value);
                ConsoleDebug.Write($""    RepositoryUrl: {RepositoryUrl}"");
            }
        }

        private void ParseProjectElementFrameworks(XElement projectElement)
        {
            XElement? TargetFrameworkElement = projectElement.Element(""TargetFramework"");
            if (TargetFrameworkElement != null)
            {
                TargetFrameworks = TargetFrameworkElement.Value;
                ConsoleDebug.Write($""    TargetFramework: {TargetFrameworks}"");
            }
            else
            {
                XElement? TargetFrameworksElement = projectElement.Element(""TargetFrameworks"");
                if (TargetFrameworksElement != null)
                {
                    TargetFrameworks = TargetFrameworksElement.Value;
                    ConsoleDebug.Write($""    TargetFrameworks: {TargetFrameworks}"");
                }
            }
        }

        private void ParseTargetFrameworks(List<Framework> parsedFrameworkList)
        {
            string[] Frameworks = TargetFrameworks.Split(';');

            foreach (string Framework in Frameworks)
                ParseTargetFramework(parsedFrameworkList, Framework);
        }

        private void ParseTargetFramework(List<Framework> parsedFrameworkList, string framework)
        {
            string FrameworkString = framework;

            string NetStandardPattern = ""netstandard"";
            string NetCorePattern = ""netcoreapp"";
            string NetFrameworkPattern = ""net"";

            Framework? NewFramework = null;
            int Major;
            int Minor;
            FrameworkMoniker Moniker = FrameworkMoniker.none;

            foreach (FrameworkMoniker MonikerValue in typeof(FrameworkMoniker).GetEnumValues())
            {
                if (MonikerValue == FrameworkMoniker.none)
                    continue;

                string MonikerName = MonikerValue.ToString();
                string MonikerPattern = $""-{MonikerName}"";
                if (FrameworkString.EndsWith(MonikerPattern, StringComparison.InvariantCulture))
                {
                    Moniker = MonikerValue;
                    FrameworkString = FrameworkString.Substring(0, FrameworkString.Length - MonikerPattern.Length);
                    break;
                }
            }

            if (FrameworkString.StartsWith(NetStandardPattern, StringComparison.InvariantCulture) && ParseNetVersion(FrameworkString.Substring(NetStandardPattern.Length), out Major, out Minor))
                NewFramework = new Framework(FrameworkType.NetStandard, Major, Minor, Moniker);
            else if (FrameworkString.StartsWith(NetCorePattern, StringComparison.InvariantCulture) && ParseNetVersion(FrameworkString.Substring(NetCorePattern.Length), out Major, out Minor))
                NewFramework = new Framework(FrameworkType.NetCore, Major, Minor, Moniker);
            else if (FrameworkString.StartsWith(NetFrameworkPattern, StringComparison.InvariantCulture) && ParseNetVersion(FrameworkString.Substring(NetFrameworkPattern.Length), out Major, out Minor))
                NewFramework = new Framework(FrameworkType.NetFramework, Major, Minor, Moniker);

            if (NewFramework != null)
                parsedFrameworkList.Add(NewFramework);
        }

        private static bool ParseNetVersion(string text, out int major, out int minor)
        {
            major = -1;
            minor = -1;

            string[] Versions = text.Split('.');
            if (Versions.Length == 2)
            {
                if (int.TryParse(Versions[0], out major) && int.TryParse(Versions[1], out minor))
                    return true;
            }
            else if (Versions.Length == 1)
            {
                string Version = Versions[0];
                if (Version.Length > 1 && int.TryParse(Version.Substring(0, 1), out major) && int.TryParse(Version.Substring(1), out minor))
                    return true;
            }

            return false;
        }

        private string TargetFrameworks = string.Empty;
        #endregion
    }
}
";
    [DataTestMethod]
    [
    DataRow(Namespace1),
    DataRow(Namespace2),
    DataRow(UsingOutside),
    DataRow(UsingInside),
    DataRow(EnumDeclaration1),
    DataRow(EnumDeclaration2),
    DataRow(ClassDeclaration1),
    DataRow(ClassDeclaration2),
    DataRow(StructDeclaration1),
    DataRow(StructDeclaration2),
    DataRow(RecordDeclaration1),
    DataRow(RecordDeclaration2),
    DataRow(InterfaceDeclaration1),
    DataRow(InterfaceDeclaration2),
    DataRow(DelegateDeclaration),
    DataRow(EventDeclaration),
    DataRow(FieldDeclaration),
    DataRow(MethodDeclaration1),
    DataRow(MethodDeclaration2),
    DataRow(PropertyDeclaration1),
    DataRow(PropertyDeclaration2),
    DataRow(PropertyDeclaration3),
    DataRow(PropertyDeclaration4),
    DataRow(EnumMemberDeclaration),
    DataRow(ForStatement),
    DataRow(ForBreakContinueStatement1),
    DataRow(ForBreakContinueStatement2),
    DataRow(ForBreakContinueStatement3),
    DataRow(ForBreakContinueStatement4),
    DataRow(CheckedStatement1),
    DataRow(CheckedStatement2),
    DataRow(ForEachStatement1),
    DataRow(ForEachStatement2),
    DataRow(DoStatement1),
    DataRow(DoStatement2),
    DataRow(DoStatement3),
    DataRow(DoStatement4),
    DataRow(FixedStatement1),
    DataRow(FixedStatement2),
    DataRow(IfElseStatement1),
    DataRow(IfElseStatement2),
    DataRow(IfElseStatement3),
    DataRow(IfElseStatement4),
    DataRow(IfElseStatement5),
    DataRow(IfElseStatement6),
    DataRow(LocalDeclarationStatement),
    DataRow(LockStatement1),
    DataRow(LockStatement2),
    DataRow(ReturnStatement),
    DataRow(SwitchBreakStatement1),
    DataRow(SwitchBreakStatement2),
    DataRow(SwitchBreakStatement3),
    DataRow(SwitchBreakStatement4),
    DataRow(ThrowStatement),
    DataRow(TryCatchStatement1),
    DataRow(TryCatchStatement2),
    DataRow(TryCatchStatement3),
    DataRow(TryCatchStatement4),
    DataRow(TryCatchStatement5),
    DataRow(UnsafeStatement1),
    DataRow(UnsafeStatement2),
    DataRow(UsingStatement1),
    DataRow(UsingStatement2),
    DataRow(WhileStatement1),
    DataRow(WhileStatement2),
    DataRow(YieldBreakStatement),
    DataRow(YieldReturnStatement),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
