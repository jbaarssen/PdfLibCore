using System;
using CppSharp.AST;
using CppSharp.Passes;
using PdfLibCore.Types;

namespace PdfLibCore.CppSharp.Passes;

public class FixMethods : TranslationUnitPass
{
    public override bool VisitDeclaration(Declaration decl)
    {
        if (AlreadyVisited(decl))
        {
            return false;
        }

        if (decl is not Function method)
        {
            return true;
        }

        if ("FPDF_GetLastError".Equals(method.Name, StringComparison.InvariantCultureIgnoreCase) && TypeMaps.FindTypeMap(nameof(FPDF_ERR), out var err))
        {
            method.ReturnType = new QualifiedType(err.Type);
        }
        return true;
    }
}