using System;
using System.Collections.Generic;
using System.Text;

namespace PCCore.Common
{
    public partial class ComCommand
    {

        public  string _ForTest;
        public  string ForTest
        {
            set { _ForTest = value; }
            get { return _ForTest; }
        }
    }
}
