﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.github.javaparser;
using com.github.javaparser.ast;
using com.github.javaparser.ast.body;
using com.github.javaparser.ast.type;
using ikvm.io;
using java.lang.reflect;
using JavaToCSharp.Declarations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JavaToCSharp
{
    public static class JavaToCSharpConverter
    {
        public static string ConvertText(string javaText, JavaConversionOptions options = null)
        {
            if (options == null)
                options = new JavaConversionOptions();

            options.ConversionStateChanged(ConversionState.Starting);

            var context = new ConversionContext(options);

            var textBytes = Encoding.UTF8.GetBytes(javaText ?? string.Empty);

            using (var stringreader = new MemoryStream(textBytes))
            using (var wrapper = new InputStreamWrapper(stringreader))
            {
                options.ConversionStateChanged(ConversionState.ParsingJavaAST);

                var parsed = JavaParser.parse(wrapper);

                options.ConversionStateChanged(ConversionState.BuildingCSharpAST);

                var types = parsed.getTypes().ToList<TypeDeclaration>();
                var imports = parsed.getImports().ToList<ImportDeclaration>();
                var package = parsed.getPackage();

                var usings = new List<UsingDirectiveSyntax>();

                //foreach (var import in imports)
                //{
                //    var usingSyntax = Syntax.UsingDirective(Syntax.ParseName(import.getName().toString()));
                //    usings.Add(usingSyntax);
                //}

                if (options.IncludeUsings)
                {
                    foreach (var ns in options.Usings)
                    {
                        var usingSyntax = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns));
                        usings.Add(usingSyntax);
                    }
                }

                var rootMembers = new List<MemberDeclarationSyntax>();
                NamespaceDeclarationSyntax namespaceSyntax = null;

                if (options.IncludeNamespace)
                {
                    string packageName = package.getName().toString();

                    foreach (var packageReplacement in options.PackageReplacements)
                    {
                        packageName = packageReplacement.Replace(packageName);
                    }

                    packageName = TypeHelper.Capitalize(packageName);

                    namespaceSyntax = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(packageName));
                }

                foreach (var type in types)
                {
                    if (type is ClassOrInterfaceDeclaration)
                    {
                        var classOrIntType = type as ClassOrInterfaceDeclaration;

                        if (classOrIntType.isInterface())
                        {
                            var interfaceSyntax = VisitInterfaceDeclaration(context, classOrIntType, false);

                            if (options.IncludeNamespace)
                                namespaceSyntax = namespaceSyntax.AddMembers(interfaceSyntax);
                            else
                                rootMembers.Add(interfaceSyntax);
                        }
                        else
                        {
                            var classSyntax = VisitClassDeclaration(context, classOrIntType, false);

                            if (options.IncludeNamespace)
                                namespaceSyntax = namespaceSyntax.AddMembers(classSyntax);
                            else
                                rootMembers.Add(classSyntax);
                        }
                    }
                }

                if (options.IncludeNamespace)
                    rootMembers.Add(namespaceSyntax);

                var root = SyntaxFactory.CompilationUnit(
                    externs: new SyntaxList<ExternAliasDirectiveSyntax>(),
                    usings: SyntaxFactory.List(usings.ToArray()),
                    attributeLists: new SyntaxList<AttributeListSyntax>(),
                    members: SyntaxFactory.List<MemberDeclarationSyntax>(rootMembers))
                    .NormalizeWhitespace();

                var tree = CSharpSyntaxTree.Create(root);

                options.ConversionStateChanged(ConversionState.Done);

                return tree.GetText().ToString();
            }
        }

        private static InterfaceDeclarationSyntax VisitInterfaceDeclaration(ConversionContext context, ClassOrInterfaceDeclaration javai, bool isNested = false)
        {
            string name = "I" + javai.getName();

            if (!isNested)
                context.RootTypeName = name;

            context.LastTypeName = name;

            var classSyntax = SyntaxFactory.InterfaceDeclaration(name);

            var typeParams = javai.getTypeParameters().ToList<TypeParameter>();

            if (typeParams != null && typeParams.Count > 0)
            {
                classSyntax = classSyntax.AddTypeParameterListParameters(typeParams.Select(i => SyntaxFactory.TypeParameter(i.getName())).ToArray());
            }
            
            var mods = javai.getModifiers();

            if (mods.HasFlag(Modifier.PRIVATE))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
            if (mods.HasFlag(Modifier.PROTECTED))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
            if (mods.HasFlag(Modifier.PUBLIC))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            if (mods.HasFlag(Modifier.FINAL))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.SealedKeyword));

            var implements = javai.getImplements().ToList<ClassOrInterfaceType>();

            if (implements != null)
            {
                foreach (var implement in implements)
                {
                    classSyntax = classSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(TypeHelper.GetSyntaxFromType(implement)));
                }
            }

            var members = javai.getMembers().ToList<BodyDeclaration>();

            foreach (var member in members)
            {
                var syntax = BodyDeclarationVisitor.VisitBodyDeclarationForInterface(context, classSyntax, member);

                classSyntax = classSyntax.AddMembers(syntax);
            }

            return classSyntax;
        }

        private static ClassDeclarationSyntax VisitClassDeclaration(ConversionContext context, ClassOrInterfaceDeclaration javac, bool isNested = false)
        {
            string name = javac.getName();
            
            if (!isNested)
                context.RootTypeName = name;
            
            context.LastTypeName = name;

            var classSyntax = SyntaxFactory.ClassDeclaration(name);

            var typeParams = javac.getTypeParameters().ToList<TypeParameter>();

            if (typeParams != null && typeParams.Count > 0)
            {
                classSyntax = classSyntax.AddTypeParameterListParameters(typeParams.Select(i => SyntaxFactory.TypeParameter(i.getName())).ToArray());
            }

            var mods = javac.getModifiers();

            if (mods.HasFlag(Modifier.PRIVATE))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
            if (mods.HasFlag(Modifier.PROTECTED))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
            if (mods.HasFlag(Modifier.PUBLIC))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            if (mods.HasFlag(Modifier.ABSTRACT))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.AbstractKeyword));
            if (mods.HasFlag(Modifier.FINAL))
                classSyntax = classSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.SealedKeyword));

            var extends = javac.getExtends().ToList<ClassOrInterfaceType>();

            if (extends != null)
            {
                foreach (var extend in extends)
                {
                    classSyntax = classSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(TypeHelper.GetSyntaxFromType(extend)));
                }
            }

            var implements = javac.getImplements().ToList<ClassOrInterfaceType>();

            if (implements != null)
            {
                foreach (var implement in implements)
                {
                    classSyntax = classSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(TypeHelper.GetSyntaxFromType(implement, true)));
                }
            }

            var members = javac.getMembers().ToList<BodyDeclaration>();

            foreach (var member in members)
            {
                if (member is ClassOrInterfaceDeclaration)
                {
                    var childc = (ClassOrInterfaceDeclaration)member;

                    if (childc.isInterface())
                    {
                        var childInt = VisitInterfaceDeclaration(context, childc, true);

                        classSyntax = classSyntax.AddMembers(childInt);
                    }
                    else
                    {
                        var childClass = VisitClassDeclaration(context, childc, true);

                        classSyntax = classSyntax.AddMembers(childClass);
                    }
                }
                else
                {
                    var syntax = BodyDeclarationVisitor.VisitBodyDeclarationForClass(context, classSyntax, member);
                    classSyntax = classSyntax.AddMembers(syntax);
                }

                while (context.PendingAnonymousTypes.Count > 0)
                {
                    var anon = context.PendingAnonymousTypes.Dequeue();
                    classSyntax = classSyntax.AddMembers(anon);
                }
            }

            return classSyntax;
        }
   
        
    }
}
