using CppAst;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public partial class CppClassConverter : BaseCppConverter<CppClass>
{
    public CppClassConverter(CppClass cppElement)
        : base(cppElement)
    {
    }

    public override CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit)
    {
        return compilationUnit.AddMembers(CppElement.IsDefinition
            ? CreateClassOrStruct()
            : CreatePointerStruct());
    }

    private static MemberDeclarationSyntax CreateField(CppField cppField)
    {
        // Determine type
        var type = cppField.Type.ToCSharp();

        // Determine property/field
        var variable = VariableDeclaration(type).AddVariables(VariableDeclarator(cppField.Name));

        var field = FieldDeclaration(variable)
            .WithModifiers(cppField.Visibility.ToCSharp());

        return field;
    }
}