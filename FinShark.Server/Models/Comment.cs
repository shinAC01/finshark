﻿namespace FinShark.Server.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }

        //A esto se le llama propiedad de navegación, es como para tener al padre instanciado
        //y tener acceso a sus propiedades, desde el hijo (similar a herencia)
        public Stock? Stock { get; set; }
    }
}
