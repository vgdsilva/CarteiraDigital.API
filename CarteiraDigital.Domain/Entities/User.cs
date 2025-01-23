namespace CarteiraDigital.Domain.Entities;
public class User
{
    public string Id { get; set; } = Guid.Empty.ToString();
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
