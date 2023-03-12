namespace FinalProject.DataModels
{
    public class AuthorGenres
    {
        public int AuthorId { get; set; } 

        public Author Author { get; set; }  

        public int GenreId { get; set; }    

        public Genre Genre { get; set; }

        /*public AuthorGenres(Author author, Genre genre)
        {
            this.Author = author;
            this.Genre = genre;
        }*/
    }
}
