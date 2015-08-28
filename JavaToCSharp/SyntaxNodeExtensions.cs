using System.Collections.Generic;
using System.Linq;
using com.github.javaparser.ast;
using com.github.javaparser.ast.comments;
using com.github.javaparser.ast.stmt;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp
{
    public static class SyntaxNodeExtensions
    {
        public static T AddComment<T>(this T syntax, ConversionContext context, Node node) where T : SyntaxNode
        {
            if (context.Options.IncludeComments)
            {
                var comment = (node as Comment) ?? node.getComment();
                if (comment != null)
                {
                    // Get the orphaned comments directly above the node
                    var leadingComments =
                        node.getParentNode()?.getChildrenNodes().AsEnumerable<Node>()
                        .Where(n => n.isPositionedBefore(node.getBeginLine(), node.getBeginColumn()))
                        .OrderByDescending(n => n.getBeginLine())
                        .TakeWhile(n => (n as Comment)?.isOrphan() ?? false)
                        .Cast<Comment>()
                        .Reverse();

                    // See if the comment is before or after the node
                    var isLeadingComment = comment.getBeginLine() < node.getBeginLine() ||
                        comment.getBeginLine() == node.getBeginLine() && comment.getEndColumn() <= node.getBeginColumn();

                    if (!isLeadingComment)
                    {
                        syntax = syntax.WithTrailingTrivia(SyntaxFactory.Comment(comment.toString().TrimStart('\r', '\n')));
                    }
                    else
                    {
                        leadingComments = leadingComments.Concat(new[] {comment});
                    }

                    syntax =
                        syntax.WithLeadingTrivia(
                            leadingComments.Select(c => SyntaxFactory.Comment(c.toString().TrimEnd('\r', '\n'))));
                }
            }

            return syntax;
        }
    }
}
