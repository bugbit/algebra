﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Algebra.Models
{
    public class FactoryMenu
    {
        private static readonly Lazy<FactoryMenu> mInstance = new Lazy<FactoryMenu>(() => new FactoryMenu());

        public static Menu Menu => mInstance.Value.mMenu;

        private Menu mMenu = new Menu();
        private Dictionary<EMenu, Grupo> mMapGrupos = new Dictionary<EMenu, Grupo>();
        private Dictionary<EMenu, MenuItem> mMapMenuItems = new Dictionary<EMenu, MenuItem>();

        private FactoryMenu()
        {
            CreateGrupos();
            mMenu.ResourceManager = Core.Algebra_Resources.ResourceManager;
            mMenu.UpdateIR(DependencyService.Get<Services.ILocate>().Culture);
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

            var pAskViews = typeof(EMenu).Assembly.GetCustomAttributes<AskViewAttribute>();

            foreach (var v in pAskViews)
            {
                foreach (var m in v.Ids)
                {
                    MenuItem pMenuItem;

                    if (mMapMenuItems.TryGetValue(m, out pMenuItem))
                        pMenuItem.AskViewType = v.ViewType;
                }
            }
        }

        private Grupo GetGrupo(EMenu argMenu)
        {
            Grupo pGrupo;

            if (!mMapGrupos.TryGetValue(argMenu, out pGrupo))
            {
                pGrupo = new Grupo
                {
                    Id = argMenu
                };

                mMenu.Add(pGrupo);
                mMapGrupos[argMenu] = pGrupo;
            }

            return pGrupo;
        }

        private MenuItem CreateMenu(EMenu argMenu, MenuOfAttribute argAttr)
        {
            var pGrupo = GetGrupo(argAttr.Grupo);
            var pMenuItem = new MenuItem
            {
                Id = argMenu
            };

            pGrupo.Add(pMenuItem);
            mMapMenuItems[argMenu] = pMenuItem;

            return pMenuItem;
        }
    }
}
