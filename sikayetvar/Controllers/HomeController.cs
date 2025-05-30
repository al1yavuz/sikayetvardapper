using System.Data;
using Microsoft.AspNetCore.Mvc;
using sikayetvar.Models;
using Dapper;



namespace sikayetvar.Controllers;

public class HomeController : Controller
{
    private readonly IDbConnection _connection;

    public HomeController(IDbConnection connection)
    {
        _connection = connection;
    }
    public IActionResult Index()
    {

        var posts = _connection.Query<Post>("SELECT * FROM Posts ORDER BY CreatedDate DESC").ToList();

        return View(posts);
    }
    [HttpGet]
    public IActionResult Detail(int id)
    {

        var post = _connection.QuerySingleOrDefault<Post>("SELECT * FROM Posts WHERE Id = @id", new { id });
        var comments = _connection.Query<Comment>("SELECT * FROM Comments Where PostId = @id AND IsApproved = 1", new { id }).ToList();
        var model = new DetailViewModel { Post = post, Comments = comments };
        return View(model);
    }

    [HttpPost]
    public IActionResult Detail(int id, Comment model)
    {

        var sql = @"INSERT INTO Comments
            (PostId, Name, Email, Body, IsApproved, CreatedDate)
            VALUES (@PostId, @Name, @Email, @Body, @IsApproved, @CreatedDate)";
        model.CreatedDate = DateTime.Now;
        model.IsApproved = false;
        model.PostId = id;
        _connection.Execute(sql, model);

        ViewData["PostId"] = id;
        return View("AddCommentSuccess");


    }
    [HttpGet]
    public IActionResult Search(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return RedirectToAction("Index");
        }


        var posts = _connection.Query<Post>(
            "SELECT * FROM Posts WHERE Title LIKE @Query OR Content LIKE @Query ORDER BY CreatedDate DESC",
            new { Query = "%" + query + "%" }).ToList();

        return View("SearchResults", posts);
    }


}
