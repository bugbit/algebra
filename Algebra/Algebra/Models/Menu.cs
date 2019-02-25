using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra.Models
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MenuOfAttribute : Attribute
    {
        public EMenu Grupo { get; set; }

        public MenuOfAttribute(EMenu grupo)
        {
            Grupo = grupo;
        }
    }

    public enum EMenu
    {
        NumberTheory,
        [MenuOf(NumberTheory)]
        PrimeP
    }

    public class Menu : IEquatable<Menu>
    {
        public EMenu Id { get; set; }
        public string Title { get; set; }

        public bool Equals(Menu other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj) => (obj is Menu m) && Equals(m);

        public override int GetHashCode() => Id.GetHashCode();
    }

    public class Grupo : ObservableCollection<Menu>, IEquatable<Grupo>
    {
        public EMenu Id { get; set; }
        public string Title { get; set; }

        public bool Equals(Grupo other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj) => (obj is Grupo m) && Equals(m);

        public override int GetHashCode() => Id.GetHashCode();
    }

    public class Grupos : ObservableCollection<Grupo>
    {
        public Grupos()
        {           
        }
    }
}
