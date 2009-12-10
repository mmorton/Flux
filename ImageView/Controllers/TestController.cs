﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedNite.Flux;
using ImageView.Views;
using ImageView.PropertyBags;

namespace ImageView.Controllers
{
    public class TestController : Controller<TestView, TestPropertyBag>
    {
        public void Go()
        {
            PropertyBag.Name = "Gone!";
        }
    }
}
