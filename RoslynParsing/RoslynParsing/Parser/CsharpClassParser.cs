using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoslynParsing.Parser
{
    public static class CsharpClassParser
    {
        public static object AppConsts { get; private set; }

        public static CsharpClass Parse(string content)
        {
            var cls = new CsharpClass();
            var tree = CSharpSyntaxTree.ParseText(content);
            var members = tree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>();

            var className = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First().Identifier.ValueText;

            foreach (var member in members)
            {
                if (member is PropertyDeclarationSyntax property)
                {
                    cls.Properties.Add(new CsharpProperty(
                         property.Identifier.ValueText,
                         property.Type.ToString())
                     );
                }

                else if (member is FieldDeclarationSyntax fieldDeclaration)
                {
                    cls.Fields.Add(new CsharpField(
                         fieldDeclaration.Declaration.Variables.First().ToString(),
                         fieldDeclaration.Declaration.Type.ToString())
                     );
                }

                else if(member is MethodDeclarationSyntax method)
                {
                    cls.Methods.Add(new CsharpMethod(
                        method.Identifier.ValueText,
                        method.ReturnType.ToString()));
                }

                else if (member is NamespaceDeclarationSyntax namespaceDeclaration)
                {
                    cls.Namespace = namespaceDeclaration.Name.ToString();
                }

                else if (member is ClassDeclarationSyntax classDeclaration)
                {
                    cls.Name = classDeclaration.Identifier.ValueText;

                    try
                    {
                        var inheritedName = classDeclaration.BaseList.Types[0].ToString();
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Test", inheritedName + ".cs");
                        var subContent = File.ReadAllText(filePath);
                        cls.inheritedClass = CsharpClassParser.Parse(subContent);
                    }
                    catch { }
                    
                }

                else if(member is ConstructorDeclarationSyntax constructorDeclaration)
                {
                    var constructor = new CsharpConstructor(className);

                    foreach(var param in constructorDeclaration.ParameterList.Parameters)
                    {
                        constructor.Parameters.Add(new CsharpParameters(param.Identifier.ToString(),param.Type.ToString()));
                    }

                    cls.Constructors.Add(constructor);
                }

            }

            return cls;
        }

    }
}
