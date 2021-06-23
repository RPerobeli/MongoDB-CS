using ExemplosMongoDB.Contexto;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExemplosMongoDB
{
    class Program
    {
        static string stringConnection = "mongodb://localhost:27017";
        static string database = "Biblioteca";
        static string colecao = "Livros";
        static void Main(string[] args)
        {
            Task T = AlterandoMuitosDocs();
            //Task T = AlterandoDocs();
            //Task T = ExibirListaDeAcordoComCriterio();
            //Task T = ListandoTodosLivrosComFiltro();
            //Task T = ListandoTodosLivros();
            //Task T = ManipulandoClassesExternas();
            //Task T = ManipulandoClasses();
            //Task T = AcessandoMongoDB(args);
            //Task T = ManipulandoDocumentosAsync(args);
            //Task T = MainAsync(args);
            //MainSync(args);
            Console.WriteLine("Pressione ENTER");
            Console.ReadLine();
        }

        private static async Task AlterandoMuitosDocs()
        {
            ContextoMongo<Livro> ctx = new ContextoMongo<Livro>(stringConnection, database, colecao);
            ctx.IniciarConexao();

            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.Titulo, "Guerra dos Tronos");

            var listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
            var construtorAlteracao = Builders<Livro>.Update;
            var condicaoAlteracao = construtorAlteracao.Set(s => s.Ano, 2001);
            await ctx.Colecao.UpdateOneAsync(condicao, condicaoAlteracao);
            
            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }

            construtorAlteracao = Builders<Livro>.Update;
            condicaoAlteracao = construtorAlteracao.Set(s => s.Autor, "Georginho Ronaldo Robson Martins");
            await ctx.Colecao.UpdateManyAsync(condicao, condicaoAlteracao);

            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
        }

        private static async Task AlterandoDocs()
        {
            ContextoMongo<Livro> ctx = new ContextoMongo<Livro>(stringConnection, database, colecao);
            ctx.IniciarConexao();

            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.Titulo, "Guerra dos Tronos");

            var listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
                item.Ano = 2000;
                item.Paginas = 900;
                await ctx.Colecao.ReplaceOneAsync(condicao, item);
            }

            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }


        }

        private static async Task ExibirListaDeAcordoComCriterio()
        {
          
            ContextoMongo<Livro> ctx = new ContextoMongo<Livro>(stringConnection, database, colecao);
            ctx.IniciarConexao();

            Console.WriteLine("Filtrando pela classe");
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Gt(x => x.Paginas, 300);
            
            var listaLivros = await ctx.Colecao.Find(condicao)
                                                        .SortBy(x => x.Titulo )
                                                        .Limit(1)
                                                        .ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
        }

        private static async Task ListandoTodosLivrosComFiltro()
        {

            ContextoMongo<Livro> ctx = new ContextoMongo<Livro>(stringConnection, database, colecao);
            ctx.IniciarConexao();

            var filtro = new BsonDocument()
            {
                {"Autor", "George R R Martin" }
            };

            Console.WriteLine("Listando Livros");
            var listaLivros = await ctx.Colecao.Find(filtro).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
            Console.WriteLine();
            Console.WriteLine("Filtrando pela classe Autor");
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.Autor, "George R R Martin");
            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
            Console.WriteLine();
            Console.WriteLine("Filtrando pela classe Ano de Publicacao");
            construtor = Builders<Livro>.Filter;
            condicao = construtor.Gte(x => x.Ano, 1999);
            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
            Console.WriteLine();
            Console.WriteLine("Filtrando pela classe Ano de Publicacao maior que 1999 e que tenha mais de 300 páginas");
            construtor = Builders<Livro>.Filter;
            condicao = construtor.Gte(x => x.Ano, 1999) & construtor.Gte(x=>x.Paginas,300);
            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
            Console.WriteLine();
            Console.WriteLine("Filtrando pela classe buscando dentro do array");
            construtor = Builders<Livro>.Filter;
            condicao = construtor.AnyEq(x => x.Assunto, "Ficcao cientifica");
            listaLivros = await ctx.Colecao.Find(condicao).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }

        }

        private static async Task ListandoTodosLivros()
        {
            string stringConnection = "mongodb://localhost:27017";
            string database = "Biblioteca";
            string colecao = "Livros";
            ContextoMongo<Livro> ctx = new ContextoMongo<Livro>(stringConnection, database, colecao);
            ctx.IniciarConexao();
            Console.WriteLine("Listando Livros");
            var listaLivros = await ctx.Colecao.Find(new BsonDocument()).ToListAsync();
            foreach (var item in listaLivros)
            {
                Console.WriteLine(item.ToJson<Livro>());
            }
        }

        private static async Task ManipulandoClassesExternas()
        {
            string stringConnection = "mongodb://localhost:27017";
            string database = "Biblioteca";
            string colecao = "Livros";
            ContextoMongo<Livro> ctx = new ContextoMongo<Livro>(stringConnection,database,colecao);
            ctx.IniciarConexao();

            Livro livro = new Livro("Star Wars Legends", "Timomthy Zahm", 2010, 245);
            var listaAssunto = new List<string>();
            listaAssunto.Add("Ficcao cientifica");
            listaAssunto.Add("Acao");
            livro.Assunto = listaAssunto;

            await ctx.Colecao.InsertOneAsync(livro);
            Console.WriteLine("livro Incluido");
        }

        private static async Task ManipulandoClasses()
        {
            //acesso ao server
            string stringConnection = "mongodb://localhost:27017";
            IMongoClient cliente = new MongoClient(stringConnection);
            //acesso ao db
            IMongoDatabase bancoDados = cliente.GetDatabase("Biblioteca");
            //Acesso a colecao
            IMongoCollection<Livro> colecao = bancoDados.GetCollection<Livro>("Livros");

            Livro livro = new Livro("Sob a Redoma", "Stephan King", 2012,679);
            var listaAssunto = new List<string>();
            listaAssunto.Add("Ficcao cientifica");
            listaAssunto.Add("Terror");
            listaAssunto.Add("Acao");
            livro.Assunto = listaAssunto;

            //Incluindo documento
            await colecao.InsertOneAsync(livro);
            Console.WriteLine("livro Incluido");
        }

        private static async Task AcessandoMongoDB(string[] args)
        {
            var doc = new BsonDocument
            {
                {"Titulo", "Guerra dos Tronos"}
            };
            doc.Add("Autor", "George R R Martin");
            doc.Add("Ano", 1999);
            doc.Add("Paginas", 856);

            var assuntoArray = new BsonArray();
            assuntoArray.Add("Fantasia");
            assuntoArray.Add("Acao");

            doc.Add("Assunto", assuntoArray);

            Console.WriteLine(doc);

            //acesso ao server
            string stringConnection = "mongodb://localhost:27017";
            IMongoClient cliente = new MongoClient(stringConnection);
            //acesso ao db
            IMongoDatabase bancoDados = cliente.GetDatabase("Biblioteca");
            //Acesso a colecao
            IMongoCollection<BsonDocument> colecao = bancoDados.GetCollection<BsonDocument>("Livros");
            //Incluindo documento
            await colecao.InsertOneAsync(doc);
            Console.WriteLine("Doc Incluido");
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Espera 10s");
            await Task.Delay(10000);
            Console.WriteLine("Esperei 10s");
        }

        private static void MainSync(string[] args)
        {
            Console.WriteLine("Espera 10s");
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Esperei 10s");
        }

        private static async Task ManipulandoDocumentosAsync(string[] args)
        {
            var doc = new BsonDocument
            {
                {"Titulo", "Guerra dos Tronos"}
            };
            doc.Add("Autor", "George R R Martin");
            doc.Add("Ano",1999);
            doc.Add("Paginas", 856);

            var assuntoArray = new BsonArray();
            assuntoArray.Add("Fantasia");
            assuntoArray.Add("Acao");

            doc.Add("Assunto", assuntoArray);

            Console.WriteLine(doc);
        }


    }
}
