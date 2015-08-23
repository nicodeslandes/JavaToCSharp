﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaToCSharp
{
    public class ConversionContext
    {
        public ConversionContext(JavaConversionOptions options)
        {
            PendingAnonymousTypes = new Queue<ClassDeclarationSyntax>();
            UsedAnonymousTypeNames = new HashSet<string>();
            this.Options = options;
        }

        public Queue<ClassDeclarationSyntax> PendingAnonymousTypes { get; private set; }

        public ISet<string> UsedAnonymousTypeNames { get; private set; }

        public JavaConversionOptions Options { get; private set; }

        public string RootTypeName { get; set; }

        public string LastTypeName { get; set; }
    }
}
