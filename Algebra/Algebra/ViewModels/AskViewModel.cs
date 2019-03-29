#region LICENSE
/*
    Algebra Software free CAS
    Copyright © 2018 Óscar Hernández Bañó
    This file is part of Algebra.
    Algebra is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Algebra.ViewModels
{
    public class AskViewModel : BaseViewModel
    {
        private Core.Session mSession;
        private Models.MenuItem mMenu;
        private object mResult;
        private IBoard mBoard;

        public Core.Session Session { get => mSession; set => SetProperty(ref mSession, value); }
        public Models.MenuItem MenuItem { get => mMenu; set => SetProperty(ref mMenu, value); }
        public object Result { get => mResult; set => SetProperty(ref mResult, value); }
        public IBoard Board { get => mBoard; set => SetProperty(ref mBoard, value); }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        public virtual async Task Calculate(CancellationToken t) { }
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        public async Task AddInBoard(CancellationToken t)
        {
            await Calculate(t);
            await Board.AddInBoard(this);
        }
    }
    public class AskViewModel<T> : AskViewModel
    {
        public new T Result { get => (T)base.Result; set => base.Result = value; }
    }
}
