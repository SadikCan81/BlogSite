using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Models.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
