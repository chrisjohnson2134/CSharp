using RoslynParsing.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "..\\..\\Test", "TestClass.cs");
            var content = File.ReadAllText(filePath);

            var csharpClass = CsharpClassParser.Parse(content);

            Console.WriteLine("properties : ");
            foreach (var item in csharpClass.Properties)
            {
                Console.WriteLine($"{item.Name} - {item.Type}");
            }

            Console.WriteLine("methods : ");
            foreach (var item in csharpClass.Methods)
            {
                Console.WriteLine(item.Name);
            }

            Console.WriteLine("constructors : ");
            foreach (var item in csharpClass.Constructors)
            {
                Console.WriteLine(item.ImpliedName + " : ");
                foreach (var param in item.Parameters)
                {
                    Console.WriteLine(param.Name);
                }
            }

            Console.WriteLine("Inherited classes L1 : ");
            Console.WriteLine(csharpClass.inheritedClass.Name);
            foreach (var item in csharpClass.inheritedClass.Properties)
            {
                Console.WriteLine(item.Name);
            }

            Console.WriteLine("Inherited classes L2 : ");
            var reInheritedClass = csharpClass.inheritedClass.inheritedClass;
            Console.WriteLine(reInheritedClass.Name);
            foreach (var item in reInheritedClass.Properties)
            {
                Console.WriteLine(item.Name);
            }

            Console.Read();
        }


    }
}
