using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoTest.Models;

namespace MongoTest.Services;

public class MongoDBService
{
    private readonly IMongoCollection<Movie> _moviesCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.ConnectionName);
        _moviesCollection = database.GetCollection<Movie>(mongoDBSettings.Value.CollectionName);
    }


    public async Task CreateAsync(Movie movie)
    {
        try
        {
            await _moviesCollection.InsertOneAsync(movie);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while inserting the movie: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var movieCursor = await _moviesCollection.FindAsync(new BsonDocument());

        return await movieCursor.ToListAsync();
    }

    public async Task<bool> UpdateAMovie(string id, Movie movie)
    {
        var unchangedMovie = await _moviesCollection.Find<Movie>(id).FirstOrDefaultAsync();

        if (unchangedMovie == null)
            return false;
        var updateDefinition = GetUpdateDefinition(unchangedMovie, movie);
        if (updateDefinition == null)
            return false;
        var result = await _moviesCollection.UpdateOneAsync(Builders<Movie>.Filter.Eq("Id", id), updateDefinition);

        return result.MatchedCount > 0 && result.ModifiedCount > 0;
    }


    public async Task DeleteMovie(string id)
    {
        await _moviesCollection.DeleteOneAsync(Builders<Movie>.Filter.Eq("Id", id));
    }

    private UpdateDefinition<T> GetUpdateDefinition<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(T unchangedDocument, T updatedDocument)
    {
        var updateDefinitions = new List<UpdateDefinition<T>>();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var originalValue = property.GetValue(unchangedDocument);
            var newValue = property.GetValue(updatedDocument);

            if (!Equals(originalValue, newValue))
            {
                updateDefinitions.Add(Builders<T>.Update.Set(property.Name, newValue));
            }
        }

        return updateDefinitions.Count > 0 ? Builders<T>.Update.Combine(updateDefinitions) : null;
    }

}