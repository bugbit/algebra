using System;
using System.Collections.Generic;
using System.Linq;
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
            var pMenus = typeof(EMenu).GetCustomAttributes(typeof(MenuOfAttribute), false).OfType<MenuOfAttribute>();
        }

        private Grupo GetGrupo(EMenu argMenu)
        {
            Grupo pGrupo;

            if (!mMapGrupos.TryGetValue(argMenu, out pGrupo))
            {
                pGrupo = new Grupo
                {
                    Id = argMenu,
                    Title = Core.Algebra_Resources.ResourceManager.GetString($"{argMenu}_Title")
                };

                mMapGrupos[argMenu] = pGrupo;
            }

            return pGrupo;
        }
    }
}
