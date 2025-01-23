namespace CarteiraDigital.Domain.Models;

public class ApiResult
{
    public bool Success { get; set; }
    
    public string Message { get; set; }
    
    public object Data { get; set; }

    public List<object> Errors { get; set; }

    public ApiResult()
    {

    }

}
