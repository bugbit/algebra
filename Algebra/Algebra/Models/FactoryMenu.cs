using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Algebra.Models
{
    public class FactoryMenu
    {
        private static readonly Lazy<FactoryMenu> mInstance = new Lazy<FactoryMenu>(() => new FactoryMenu());

        public static Grupos Menu => mInstance.Value.mMenu;

        private Grupos mMenu = new Grupos();
        private Dictionary<EMenu, Grupo> mMapGrupos = new Dictionary<EMenu, Grupo>();

        private FactoryMenu()
        {
            CreateGrupos();
        }

        private void CreateGrupos()
        {
            var dataType = Enum.GetUnderlyingType(typeof(EMenu));
            var pMenus =
                from f in typeof(EMenu).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                let a = f.GetCustomAttribute<MenuOfAttribute>()
                where a != null
                select new { f, a, v = (EMenu)Convert.ChangeType(f.GetValue(null), dataType) };

            foreach (var m in pMenus)
                CreateMenu(m.v, m.a);
        }

        private Grupo GetGrupo(EMenu argMenu)
        {
            Grupo pGrupo;

            if (!mMapGrupos.TryGetValue(argMenu, out pGrupo))
            {
                pGrupo = new Grupo
                {
                    Id = argMenu,
                    Title = Core.CultureManager.CreateAlgebraIRString($"{argMenu}_Title")
                };

                mMenu.Add(pGrupo);
                mMapGrupos[argMenu] = pGrupo;
            }

            return pGrupo;
        }

        private Menu CreateMenu(EMenu argMenu, MenuOfAttribute argAttr)
        {
            var pGrupo = GetGrupo(argAttr.Grupo);
            var pMenu = new Menu
            {
                Id = argMenu,
                Title = Core.CultureManager.CreateAlgebraIRString($"{argMenu}_Title")
            };

            pGrupo.Add(pMenu);

            return pMenu;
        }
    }
}
