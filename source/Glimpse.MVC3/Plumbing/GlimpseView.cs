﻿using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;

namespace Glimpse.Mvc3.Plumbing
{
    public class GlimpseView : IView
    {
        public IView View { get; set; }
        internal ViewContext ViewContext { get; set; }
        public string  ViewName { get; set; }

        internal GlimpseView(IView view)
        {
            View = view;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            ViewContext = viewContext;

            var id = Guid.NewGuid().ToString();


            writer.Write("<!--VIEW("+ViewName+")-->");

            View.Render(viewContext, writer);

            writer.Write("<!--/VIEW(" + ViewName + ")-->");

        }
    }
}