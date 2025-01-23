using System.Security.Cryptography;

namespace CarteiraDigital.Domain.Utils;

public class PasswordSecurity
{
    private const int SaltSize = 16; // Tamanho do salt em bytes
    private const int KeySize = 32;  // Tamanho da chave gerada
    private const int Iterations = 10000; // Número de iterações para o algoritmo de hash

    /// <summary>
    /// Gera um hash para a senha usando PBKDF2.
    /// </summary>
    /// <param name="password">A senha em texto puro.</param>
    /// <returns>O hash da senha no formato SALT:HASH.</returns>
    public static string HashPassword(string password)
    {
        // Gerar um salt aleatório
        using var rng = new RNGCryptoServiceProvider();
        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        // Gerar o hash usando o salt e a senha
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(KeySize);

        // Combinar salt e hash em uma string codificada em Base64
        string saltBase64 = Convert.ToBase64String(salt);
        string hashBase64 = Convert.ToBase64String(hash);

        return $"{saltBase64}:{hashBase64}";
    }

    /// <summary>
    /// Verifica se a senha fornecida corresponde ao hash armazenado.
    /// </summary>
    /// <param name="password">A senha em texto puro.</param>
    /// <param name="hashedPassword">O hash da senha armazenado no formato SALT:HASH.</param>
    /// <returns>Verdadeiro se as senhas correspondem; caso contrário, falso.</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Dividir o hash armazenado em salt e hash
        string[] parts = hashedPassword.Split(':');
        if (parts.Length != 2)
            throw new FormatException("Hash armazenado está em um formato inválido.");

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHash = Convert.FromBase64String(parts[1]);

        // Gerar um novo hash com o mesmo salt e senha fornecida
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] computedHash = pbkdf2.GetBytes(KeySize);

        // Comparar o hash armazenado com o hash gerado
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}
