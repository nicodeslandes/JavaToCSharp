﻿using com.github.javaparser.ast.body;
using java.lang.reflect;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Declarations
{
    public class EnumDeclarationVisitor : BodyDeclarationVisitor<EnumDeclaration>
    {
        public override MemberDeclarationSyntax VisitForClass(ConversionContext context, ClassDeclarationSyntax classSyntax, EnumDeclaration enumDecl)
        {
            var name = enumDecl.getName();

            var members = enumDecl.getMembers().ToList<BodyDeclaration>();

            var entries = enumDecl.getEntries().ToList<EnumConstantDeclaration>();
            var memberSyntaxes = new List<EnumMemberDeclarationSyntax>();

            foreach (var entry in entries)
            {
                // TODO: support "equals" value
                memberSyntaxes.Add(SyntaxFactory.EnumMemberDeclaration(entry.getName()));
            }

            if (members != null && members.Count > 0)
                context.Options.Warning("Members found in enum " + name + " will not be ported. Check for correctness.", enumDecl.getBeginLine());

            var enumSyntax = SyntaxFactory.EnumDeclaration(name)
                .AddMembers(memberSyntaxes.ToArray());

            var mods = enumDecl.getModifiers();

            if (mods.HasFlag(Modifier.PRIVATE))
                enumSyntax = enumSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
            if (mods.HasFlag(Modifier.PROTECTED))
                enumSyntax = enumSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
            if (mods.HasFlag(Modifier.PUBLIC))
                enumSyntax = enumSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            return enumSyntax.AddComment(context, enumDecl);
        }

        public override MemberDeclarationSyntax VisitForInterface(ConversionContext context, InterfaceDeclarationSyntax interfaceSyntax, EnumDeclaration declaration)
        {
            throw new NotImplementedException("Need to implement diversion of nested enums in interfaces to non-nested.");
        }
    }
}
