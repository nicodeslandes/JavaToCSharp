﻿using com.github.javaparser.ast.body;
using java.lang.reflect;
using JavaToCSharp.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Declarations
{
    public class FieldDeclarationVisitor : BodyDeclarationVisitor<FieldDeclaration>
    {
        public override MemberDeclarationSyntax VisitForClass(ConversionContext context, ClassDeclarationSyntax classSyntax, FieldDeclaration fieldDecl)
        {
            var variables = new List<VariableDeclaratorSyntax>();

            string typeName = fieldDecl.getType().toString();

            foreach (var item in fieldDecl.getVariables().ToList<VariableDeclarator>())
            {
                var id = item.getId();
                string name = id.getName();

                if (id.getArrayCount() > 0)
                {
                    if (!typeName.EndsWith("[]"))
                        typeName += "[]";
                    if (name.EndsWith("[]"))
                        name = name.Substring(0, name.Length - 2);
                }

                var initexpr = item.getInit();

                if (initexpr != null)
                {
                    var initsyn = ExpressionVisitor.VisitExpression(context, initexpr);
                    var vardeclsyn = SyntaxFactory.VariableDeclarator(name).WithInitializer(SyntaxFactory.EqualsValueClause(initsyn));
                    variables.Add(vardeclsyn);
                }
                else
                    variables.Add(SyntaxFactory.VariableDeclarator(name));
            }

            typeName = TypeHelper.ConvertType(typeName);

            var fieldSyntax = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.ParseTypeName(typeName),
                    SyntaxFactory.SeparatedList(variables, Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken), variables.Count - 1))));

            var mods = fieldDecl.getModifiers();

            if (mods.HasFlag(Modifier.PUBLIC))
                fieldSyntax = fieldSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            if (mods.HasFlag(Modifier.PROTECTED))
                fieldSyntax = fieldSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
            if (mods.HasFlag(Modifier.PRIVATE))
                fieldSyntax = fieldSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
            if (mods.HasFlag(Modifier.STATIC))
                fieldSyntax = fieldSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
            if (mods.HasFlag(Modifier.FINAL))
                fieldSyntax = fieldSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
            if (mods.HasFlag(Modifier.VOLATILE))
                fieldSyntax = fieldSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.VolatileKeyword));

            return fieldSyntax.AddComment(context, fieldDecl);
        }

        public override MemberDeclarationSyntax VisitForInterface(ConversionContext context, InterfaceDeclarationSyntax interfaceSyntax, FieldDeclaration declaration)
        {
            throw new NotImplementedException("Need to implement diversion of static fields from interface declaration to static class");
        }
    }
}
