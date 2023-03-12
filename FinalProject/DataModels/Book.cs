﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.DataModels
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
       
        public int GenreId { get; set; }
       // public string GenreName { get; set; }

        public Genre Genre { get; set; } 

     
        public int AuthorId { get; set; }
        public Author Author  { get; set; }
        [NotMapped]
        public ICollection<SelectListItem> AuthorList { get; set; }
        [NotMapped]
        public ICollection<SelectListItem> GenreList { get; set; }



    }
}
