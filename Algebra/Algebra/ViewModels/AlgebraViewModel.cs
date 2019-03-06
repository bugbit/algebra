using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Algebra.ViewModels
{
    public class AlgebraViewModel : BaseViewModel
    {
        private string _Name, _Version, _Description, _License, _Website;
        private Core.Session mSession;

        public AlgebraViewModel()
        {
            var pAssembly = Assembly.GetExecutingAssembly();

            Name = pAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            Version = pAssembly.GetName().Version.ToString();
            Description = pAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            License = pAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            Website = "https://github.com/bugbit/algebra";
            Session = new Core.Session();
        }

        public string Name { get => _Name; set => SetProperty(ref _Name, value); }
        public string Version { get => _Version; set => SetProperty(ref _Version, value); }
        public string Description { get => _Description; set => SetProperty(ref _Description, value); }
        public string License { get => _License; set => SetProperty(ref _License, value); }
        public string Website { get => _Website; set => SetProperty(ref _Website, value); }
        public Core.Session Session { get => mSession; set => SetProperty(ref mSession, value); }
    }
}
