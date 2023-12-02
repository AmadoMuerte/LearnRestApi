using WebApplication2.Models;
using Npgsql;

namespace WebApplication2.DataAccess;

public class PostDataAccess
{
    private readonly string _connectionString;

    public PostDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<PostModel> GetAllPosts()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            var sql = "SELECT * FROM Posts";
            using (var command = new NpgsqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var posts = new List<PostModel>();

                    while (reader.Read())
                    {
                        PostModel post = new PostModel
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        };
                        posts.Add(post);
                    }

                    return posts;
                }
            }
        }
    }

    public PostModel GetPostById(int id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var sql = "SELECT * FROM Posts WHERE Id = @Id";
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    PostModel post = new PostModel();
                    if (reader.Read())
                    {
                        post.Id = reader.GetInt32(0);
                        post.Title = reader.GetString(1);
                        post.Description = reader.IsDBNull(2) ? null : reader.GetString(2);
                    }
                    return post;
                }
            }
        }
    }
    
    public int CreatePost(PostModel post)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int postId;
            connection.Open();
            var sql = "INSERT INTO Posts (Title, Description) VALUES (@Title, @Description) RETURNING Id";
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Title", post.Title);
                command.Parameters.AddWithValue("@Description", post.Description);
                postId = (int)command.ExecuteScalar();
            }
            return postId;
        }
    }
}