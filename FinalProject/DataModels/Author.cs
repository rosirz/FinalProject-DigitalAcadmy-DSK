using System.Collections.Generic;

namespace FinalProject.DataModels
{
    public class Author
    {
        
        public int AuthorId { get; set; }

        public string Name  { get; set; }

        public string Details { get; set; }

        public ICollection<Book> Books { get; set; }

        

        public ICollection<AuthorGenres> AuthorGenres { get; set; }


    }
}
