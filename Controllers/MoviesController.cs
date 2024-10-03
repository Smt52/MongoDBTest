using Microsoft.AspNetCore.Mvc;
using MongoTest.Models;
using MongoTest.Services;

namespace MongoTest.Controllers;

[Controller]
[Route("api/[controller]")]
public class MoviesController(MongoDBService movieService) : Controller
{

    [HttpGet]
    public async Task<List<Movie>> GetMoviesAsync()
    {
        return await movieService.GetMoviesAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
    {
        await movieService.CreateAsync(movie);
        return CreatedAtAction(nameof(CreateMovie), movie.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(string id, [FromBody] Movie movie)
    {
        await movieService.UpdateAMovie(id, movie);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(string id)
    {
        await movieService.DeleteMovie(id);
        return NoContent();
    }
}