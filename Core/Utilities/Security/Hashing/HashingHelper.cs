using System.Text;

namespace Core.Utilities.Security.Hashing;

public class HashingHelper
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512()) //Crypto sınıfında kullanılan algoritmanın oluşturduğu key
        {
            //Verilen passwordun salt ve hash değerini oluşturmaya yarar
            passwordSalt = hmac.Key; //Hmacde bulunan salt değer
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); //Bir stringin byte karşılığını almak için
        }

    }


    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //Doğrulama yapılan key anahtarı
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i]) 
                {
                    throw new BusinessException(Messages.CoreMessages.UserNotFound);
                }
            }
            return true;
        }
    }
}
