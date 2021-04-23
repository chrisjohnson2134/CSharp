using RoslynParsing.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynParsing.Generators
{
    public static class Generator
    {
        public static string expectedFieldsSetup(CsharpClass csharpClass)
        {
            string outputString = String.Empty;

            foreach (var item in GetAllProperties(csharpClass))
	        {
                outputString += $"protected readonly {item.Type} {item.expectedName} \n";
                //outputString += "protected readonly " + item.Type + " " + item.expectedName + "\n";
	        }   

            return outputString;
        }

        public static List<CsharpProperty> GetAllProperties(CsharpClass csharpClass)
        {
            var list = new List<CsharpProperty>();
            if (csharpClass.Properties != null)
            {
                foreach (var item in csharpClass.Properties)
                {
                    list.Add(item);
                }   

                if(csharpClass.inheritedClass != null)
                {
                    foreach (var item in GetAllProperties(csharpClass.inheritedClass))
                    {
                        list.Add(item);
                    }
                    return list;
                }

             }

            return list;
        }
    }
}
