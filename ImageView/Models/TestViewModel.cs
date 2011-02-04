using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedNite.Flux;

namespace ImageView.PropertyBags
{
    public class TestViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; Notify("Name"); }
        }
    }
}
