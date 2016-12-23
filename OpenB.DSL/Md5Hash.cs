using System.Text;
using System.Security.Cryptography;

namespace OpenB.DSL
{
    public struct Md5Hash
    {
        readonly string md5hash;

        public Md5Hash(string source)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(source);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            md5hash = sb.ToString();
        }
        
        public static bool operator == (Md5Hash left, Md5Hash right)
        {
            return left.md5hash == right.md5hash;
        }

        public static bool operator !=(Md5Hash left, Md5Hash right)
        {
            return left.md5hash != right.md5hash;
        }
    }
}