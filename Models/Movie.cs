using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoTest.Models;

public class Movie
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("plot")]
    public string Plot { get; set; } = null!;
    [BsonElement("genres")]
    public List<string> Genres { get; set; } = null!;
    public int Runtime { get; set; }
    [BsonElement("cast")]
    public List<string> Cast { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Poster { get; set; } = null!;
    public DateTime Released { get; set; }
    public List<string> Directors { get; set; } = null!;
    public List<string> Languages { get; set; } = null!;
}