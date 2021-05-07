namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Testing;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing.Verifiers;

    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    Project? Project = solution.GetProject(projectId);
                    CompilationOptions? CompilationOptions = Project?.CompilationOptions;
                    CompilationOptions = CompilationOptions?.WithSpecificDiagnosticOptions(CompilationOptions?.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));

                    if (CompilationOptions != null)
                        solution = solution.WithProjectCompilationOptions(projectId, CompilationOptions);

                    return solution;
                });
            }
        }
    }
}
