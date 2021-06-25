using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExemplosMongoDB.Contexto
{
    class ContextoMongo<T>
    {
        private string _stringConnection;
        private string _bancoDeDados;
        private string _colecao;
        private IMongoClient _cliente;
        private IMongoDatabase _database;
        private IMongoCollection<T> _Colecao;

        public ContextoMongo(string stringConnection, string bancoDeDados, string colecao)
        {
            _stringConnection = stringConnection;
            _bancoDeDados = bancoDeDados;
            _colecao = colecao;
        }

        public void IniciarConexao()
        {
            _cliente = new MongoClient(_stringConnection);
            _database = _cliente.GetDatabase(_bancoDeDados);
            _Colecao = _database.GetCollection<T>(_colecao);
        }
        public IMongoClient Cliente { get { return _cliente; } }
        public IMongoCollection<T> Colecao 
        {
            get
            {
                return _Colecao;
            }
        }
       
    }
}
