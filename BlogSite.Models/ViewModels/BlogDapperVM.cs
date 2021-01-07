using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Models.ViewModels
{
    public class BlogDapperVM
    {
        public int Id { get; set; }
       
        public string Title { get; set; }

        public string SubTitle { get; set; }
 
        public string Content { get; set; }
        
        public string ImagePath { get; set; }

        public bool IsPublished { get; set; }
      
        public int CategoryId { get; set; }      

        public string ApplicationUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }
    }
}
