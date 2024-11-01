using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Example.CodeGen;

internal static class SyntaxExtensions
{
    public static bool TryGetNamespaceName(this SyntaxNode syntax, [MaybeNullWhen(false)] out string namespaceName)
    {
        var parent = syntax.Parent;
        while (parent != null)
        {
            switch (parent)
            {
                case NamespaceDeclarationSyntax ns:
                    namespaceName = ns.Name.ToString();
                    return true;
                default:
                    parent = parent.Parent;
                    break;
            }
        }

        namespaceName = default;
        return false;
    }

    public static string GetFullName(this ClassDeclarationSyntax syntax)
    {
        return syntax.TryGetNamespaceName(out var v) ? $"{v}.{syntax.Identifier}" : $"Pølse.{syntax.Identifier}";
    }
}