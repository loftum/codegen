using Microsoft.CodeAnalysis;

namespace Example.CodeGen;

internal static class SymbolExtensions
{
    public static IEnumerable<INamedTypeSymbol> GetTargets(this IAssemblySymbol assembly) =>
        assembly.GlobalNamespace.GetTargets();
    
    public static IEnumerable<INamedTypeSymbol> GetTargets(this INamespaceSymbol ns)
    {
        foreach (var symbol in ns.GetMembers())
        {
            switch (symbol)
            {
                case INamespaceSymbol sub:
                {
                    foreach (var type in GetTargets(sub))
                    {
                        yield return type;
                    }

                    break;
                }
                case INamedTypeSymbol {IsReferenceType: true, IsAbstract: false} t when t.Name.EndsWith("Target") :
                {
                    yield return t;
                    break;
                }
            }
        }
    }
}