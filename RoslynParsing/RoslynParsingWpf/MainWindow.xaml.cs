using RoslynParsing.Generators;
using RoslynParsing.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoslynParsingWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = System.IO.Path.Combine(currentDirectory, "..\\..\\Test", "TestClass.cs");
            var content = File.ReadAllText(filePath);

            var csharpClass = CsharpClassParser.Parse(content);

            ParsedOutput.Text += Generator.expectedFieldsSetup(csharpClass);

            ParsedOutput.Text += "\n" + Generator.MethodTestSetup(csharpClass);
        }
    }
}
