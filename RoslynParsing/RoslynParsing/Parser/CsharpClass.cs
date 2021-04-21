using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynParsing.Parser
{
    public class CsharpClass
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<CsharpProperty> Properties { get; set; }

        public List<CsharpMethod> Methods { get; set; }

        public List<CsharpConstructor> Constructors { get; set; }

        public List<CsharpField> Fields { get; set; }

        public CsharpClass inheritedClass { get; set; }

        public string PrimaryKeyType { get; set; }

        public CsharpClass()
        {
            Name = String.Empty;
            Properties = new List<CsharpProperty>();
            Methods = new List<CsharpMethod>();
            Constructors = new List<CsharpConstructor>();
            Fields = new List<CsharpField>();
        }

        public class CsharpProperty
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string expectedName => Name.Count() >= 2 ? "_" + Char.ToLower(Name[0]) + Name.Substring(1) + "Expected" : String.Empty;

            public CsharpProperty(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }

        public class CsharpMethod
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public CsharpMethod(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }

        public class CsharpParameters
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public CsharpParameters(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }

        public class CsharpField
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public CsharpField(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }

        public class CsharpConstructor
        {
            public string ClassName { get; set; }
            public string ImpliedName => Parameters.Count > 0 ? ClassName + Parameters.Count : ClassName;
            
            
            public List<CsharpParameters> Parameters { get; set; }

            public CsharpConstructor(string className)
            {
                ClassName = className;
                Parameters = new List<CsharpParameters>();
            }
        }

        
    }
}
