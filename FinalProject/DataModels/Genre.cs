using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DataModels
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        public string Name { get; set; }

        public ICollection <Book> Books { get; set; }   

        

        public ICollection<AuthorGenres> AuthorGenres { get; set; }

    }
}
