using System.Text.Json.Serialization;

namespace WebApplication2.Models;

public class PostModel
{
    // игнорирование айди для создания поста
    [JsonIgnore]
    public int Id {
        get;
        set;
    }
    public string? Title {
        get;
        set;
    }

    public string? Description
    {
        get;
        set;
    }
}