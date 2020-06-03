using System.Collections.Generic;
using System.IO;
using LiteDB;
using LiteDB.Engine;

namespace ET.Data
{
    public class DataContext : LiteDatabase
    {
        public DataContext(string connectionString, BsonMapper mapper = null) : base(connectionString, mapper)
        {
        }

        public DataContext(ConnectionString connectionString, BsonMapper mapper = null) : base(connectionString, mapper)
        {
        }

        public DataContext(Stream stream, BsonMapper mapper = null) : base(stream, mapper)
        {
        }

        public DataContext(ILiteEngine engine, BsonMapper mapper = null, bool disposeOnClose = true) : base(engine,
            mapper, disposeOnClose)
        {
        }

        public ILiteCollection<Meaning> Meanings => this.GetCollection<Meaning>(Meaning.CollectionName);
        public ILiteCollection<Language> Languages => this.GetCollection<Language>(Language.CollectionName);
        public ILiteCollection<Topic> Topics => this.GetCollection<Topic>(Topic.CollectionName);
        public ILiteCollection<Translation> Translations => this.GetCollection<Translation>(Translation.CollectionName);
        public ILiteCollection<Class> Classes => this.GetCollection<Class>(Class.CollectionName);
        public ILiteCollection<Comment> Comments => this.GetCollection<Comment>(Comment.CollectionName);
        public ILiteCollection<Word> Words => this.GetCollection<Word>(Word.CollectionName);
        public ILiteCollection<Word2Class> Words2Classes => this.GetCollection<Word2Class>(Word2Class.CollectionName);
    }
}