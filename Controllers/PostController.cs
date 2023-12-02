using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.DataAccess;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly PostDataAccess _postDataAccess;
    
    // в конструкторе контроллера создаем экземпляр класса базы данных для работы с постами
    public PostController()
    {
        _postDataAccess = new PostDataAccess("Host=localhost;Port=5432;Database=TestApi;Username=postgres;Password=154585;");
    }
    
    // указывает что это будет гет запрос без параметров, для получения всех постов
    [HttpGet]
    public IActionResult Get()
    {
        // должен вернуться список постов
        List<PostModel> posts = _postDataAccess.GetAllPosts();
        if (posts == null)
        {
            return NotFound();
        }
        return Ok(posts);
    }

    // указывает что это будет гет запрос с параметром id 
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var post = _postDataAccess.GetPostById(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }
    
    // указываем что это пост запрос и получаем данные из него дня создания поста
    [HttpPost]
    public IActionResult Create([FromBody] PostModel post)
    {
        int postId = _postDataAccess.CreatePost(post);
        var createdPost = new PostModel { Id = postId, Title = post.Title, Description = post.Description };
        return CreatedAtAction(nameof(GetById), new { id = postId }, createdPost);
    }
}