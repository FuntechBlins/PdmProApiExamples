using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdmProAddIn.ViewModels;
using System.Collections.ObjectModel;
using PdmProAddIn.Models;

namespace PdmProAddIn.Test
{
    [TestClass]
    public class UITests
    {
        //[TestMethod]
        //public void RealTest()
        //{

        //    Assert.Fail();
        //}

        public void ShowWindowTest()
        {
            var window = new AAFileRefsWindow();

            window.DataContext = new AAFileRefsViewModel(@"C:\foo\bar.sldprt", new AAFileRef[]
                {
                    new AAFileRef
                    {
                        Path = @"C:\foo\baz.pdf"
                    },
                    new AAFileRef
                    {
                        Path = @"C:\foo\quux.pdf"
                    },
                    new AAFileRef
                    {
                        Path = @"C:\foo\bar.pdf"
                    }
                })
            {
                ParentFilePath = @"C:\foo\bar.sldprt"
            };

            window.ShowDialog();
        }
    }
}
