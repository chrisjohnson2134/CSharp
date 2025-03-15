using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using RoslynParsing.Parser;

namespace DTOTestMaker
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _selectModelFolderCommand;
        private ICommand _selectDtoFolderCommand;
        private List<string> _dtoFiles;
        private List<string> _modelFiles;
        private string _selectedModelFile;
        private string _selectedDTOFile;
        private string _outputTestText;
        private string _dtoFolderSelected;
        private string _modelFolderSelected;

        public MainWindowVM()
        {
            DtoFiles = new List<string>();
            ModelFiles = new List<string>();
        }

        public ICommand SelectModelFolderCommand
        {
            get
            {
                return _selectModelFolderCommand ?? (_selectModelFolderCommand = new RelayCommand(param => SelectModelFolder()));
            }
        }

        public ICommand SelectDtoFolderCommand
        {
            get
            {
                return _selectDtoFolderCommand ?? (_selectDtoFolderCommand = new RelayCommand(_ => SelectDTOFolder()));
            }
        }

        public string DtoFolderSelected 
        {
            get => _dtoFolderSelected;
            set
            {
                _dtoFolderSelected = value;
                OnPropertyChanged(nameof(DtoFolderSelected));
            }
        }

        public List<string> DtoFiles
        {
            get { return _dtoFiles; }
            set
            {
                _dtoFiles = value;
                OnPropertyChanged(nameof(DtoFiles));
            }
        }

        public string ModelFolderSelected
        {
            get => _modelFolderSelected;
            set
            {
                _modelFolderSelected = value;
                OnPropertyChanged(nameof(ModelFolderSelected));
            }
        }

        public List<string> ModelFiles
        {
            get { return _modelFiles; }
            set
            {
                _modelFiles = value;
                OnPropertyChanged(nameof(ModelFiles));
            }
        }

        public string SelectedModelFile
        {
            get => _selectedModelFile;
            set
            {
                _selectedModelFile = value;
                OnPropertyChanged(nameof(SelectedModelFile));
                UpdateOutputTesttext();
            }
        }

        public string SelectedDTOFile
        {
            get => _selectedDTOFile;
            set
            {
                _selectedDTOFile = value;
                OnPropertyChanged(nameof(SelectedDTOFile));
                UpdateOutputTesttext();
            }
        }

        public string OutputTestText
        {
            get => _outputTestText;
            set
            {
                _outputTestText = value;
                OnPropertyChanged(nameof(OutputTestText));
            }
        }

        private void UpdateOutputTesttext()
        {
            if(!String.IsNullOrEmpty(SelectedModelFile) && !String.IsNullOrEmpty(SelectedDTOFile))
            {
                var modelFile = ModelFolderSelected + "\\" + SelectedModelFile;
                var fileText = File.ReadAllText(modelFile);
                var modelClass = CsharpClassParser.Parse(fileText);

                var dtoFile = DtoFolderSelected + "\\" + SelectedDTOFile;
                var dtoFileText = File.ReadAllText(dtoFile);
                var dtoClass = CsharpClassParser.Parse(dtoFileText);

                var output = string.Empty;
                foreach (var propertyName in modelClass.Properties)
                {
                    if(dtoClass.Properties.Any(t => t.Name == propertyName.Name))
                    {
                        output += $"model.{propertyName.Name}.Should().Be(dto.{propertyName.Name});\n";
                    }
                    else
                    {
                        output += $"//could not find! model.{propertyName.Name}.Should().Be(dto.{propertyName.Name});\n";
                    }
                }

                OutputTestText = output;
            }
        }

        private void SelectDTOFolder()
        {
            var directory = SelectFolder();
            DtoFolderSelected = directory.FolderName;
            DtoFiles = GetFiles(directory.FolderName);
        }

        private void SelectModelFolder()
        {
            var directory = SelectFolder();
            ModelFolderSelected = directory.FolderName;
            ModelFiles = GetFiles(directory.FolderName);
        }

        private OpenFolderDialog SelectFolder()
        {
            OpenFolderDialog dialog = new();

            bool? result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(dialog.FolderName))
            {
                // Get all files from the selected directory
                return dialog;
            }

            return null;
        }

        private List<string> GetFiles(string folderName)
        {
            return Directory.GetFiles(folderName).Select(Path.GetFileName).ToList();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
