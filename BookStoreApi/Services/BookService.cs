using BookStoreApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace BookStoreApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _bookCollection;

        public BookService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _bookCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync()
        {
            return await _bookCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Book?> GetAsync(string id)
        {
           return await _bookCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Book book) =>
            await _bookCollection.InsertOneAsync(book);

        public async Task UpdateAsync(string id, Book updatedBook) =>
           await _bookCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _bookCollection.DeleteOneAsync(x => x.Id == id);


    }
}
