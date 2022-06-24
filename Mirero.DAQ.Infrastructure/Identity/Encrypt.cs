using System.Security.Cryptography;
using System.Text;

namespace Mirero.DAQ.Infrastructure.Identity;

public static class Encrypt
{
    public static string HashToSHA256(string password)
    {
        byte[] encryptBytes = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(encryptBytes);
    }
}