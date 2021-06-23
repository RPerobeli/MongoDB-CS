using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ExemplosMongoDB
{
    public class Livro
    {
        public Livro(string titulo, string autor, int ano, int paginas, params string[] assuntos)
        {
            Titulo = titulo;
            Autor = autor;
            Ano = ano;
            Paginas = paginas;
            foreach (var ass in assuntos)
            {
                Assunto.Add(ass);
            }
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }
        public int Paginas { get; set; }
        public List<string> Assunto { get; set; }
    }
}
