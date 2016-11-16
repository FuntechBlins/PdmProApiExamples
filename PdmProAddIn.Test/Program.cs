using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProAddIn.Test
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            UITests test = new Test.UITests();

            //test.ShowWindowTest();
            test.ShowWindowPdmTest();
        }
    }
}
