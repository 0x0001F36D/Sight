// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

namespace Sight.Console
{
    using System.Text;

    using Org.BouncyCastle.Crypto.Digests;

    public static class Utils
    {
        #region Methods

        public static string WithHash(this string raw)
        {
            var src = Encoding.Unicode.GetBytes(raw);

            var sha512 = new Sha512Digest();
            sha512.BlockUpdate(src, 0, src.Length);
            var dest = new byte[sha512.GetDigestSize()];
            sha512.DoFinal(dest, 0);
            var result = Encoding.Unicode.GetString(dest);
            return result;
        }

        #endregion Methods
    }
}