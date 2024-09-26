/*
    Copyright(c) 2015 Neodymium

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
*/

//shamelessly stolen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeWalker.GameFiles
{
    public class GTACrypto
    {


        public static byte[] DecryptAES(byte[] data)
        {
            return DecryptAESData(data, GTA5Keys.XB_AES_KEY);
        }
        public static byte[] EncryptAES(byte[] data)
        {
            return EncryptAESData(data, GTA5Keys.XB_AES_KEY);
        }

        public static byte[] DecryptAESData(byte[] data, byte[] key, int rounds = 1)
        {
            var rijndael = Rijndael.Create();
            rijndael.KeySize = 256;
            rijndael.Key = key;
            rijndael.BlockSize = 128;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.None;

            var buffer = (byte[])data.Clone();
            var length = data.Length - data.Length % 16;

            // decrypt...
            if (length > 0)
            {
                var decryptor = rijndael.CreateDecryptor();
                for (var roundIndex = 0; roundIndex < rounds; roundIndex++)
                    decryptor.TransformBlock(buffer, 0, length, buffer, 0);
            }

            return buffer;
        }
        public static byte[] EncryptAESData(byte[] data, byte[] key, int rounds = 1)
        {
            var rijndael = Rijndael.Create();
            rijndael.KeySize = 256;
            rijndael.Key = key;
            rijndael.BlockSize = 128;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.None;

            var buffer = (byte[])data.Clone();
            var length = data.Length - data.Length % 16;

            // encrypt...
            if (length > 0)
            {
                var encryptor = rijndael.CreateEncryptor();
                for (var roundIndex = 0; roundIndex < rounds; roundIndex++)
                    encryptor.TransformBlock(buffer, 0, length, buffer, 0);
            }

            return buffer;
        }

        public static byte[] GetNGKey(string name, uint length)
        {
            uint hash = GTA5Hash.CalculateHash(name);
            uint keyidx = (hash + length) % 0x65;
            return GTA5Keys.XBS_NG_KEYS[keyidx];
        }


        public static byte[] DecryptNG(byte[] data, string name, uint length)
        {
            byte[] key = GetNGKey(name, length);
            return DecryptNG(data, key);
        }

        public static byte[] DecryptNG(byte[] data, byte[] key)
        {
            return DecryptAESData(data, key);
        }
        
        public static byte[] EncryptNG(byte[] data, string name, uint length)
        {
            byte[] key = GetNGKey(name, length);
            return EncryptNG(data, key);
        }

        public static byte[] EncryptNG(byte[] data, byte[] key)
        {
            return EncryptAESData(data, key);
        }
    }
}
