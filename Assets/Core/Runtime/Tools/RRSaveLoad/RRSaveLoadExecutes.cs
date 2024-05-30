/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *     Unity Tools (https://github.com/0x4b404ec/UnityTools)    *
 *                                                              *
 *  Author:                                                     *
 *    0x4b404ec (https://github.com/0x4b404ec)                  *
 *                                                              *
 ****************************************************************/

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ox4b404ec.Tools.Tools.RRSaveLoad
{
    public enum SaveFileTypeEnum
    {
        BINARY,
        BINARY_ENCRYPTED,
        JSON,
        JSON_ENCRYPTED,
        XML,
        XML_ENCRYPTED,
    }
    
    public interface IRRSaveLoadExecute
    {
        void Save(object objectToSave, FileStream saveFile);
        object Load(System.Type objectType, FileStream saveFile);
    }
    
    public abstract class RRSaveLoadEncrypter
    {
        public virtual string Key { get; set; } = "yourDefaultKey";

        private readonly string m_saltText = "0x4b404ec-0x4b404ec-0x4b404ec";
            
        protected virtual void Encrypt(Stream inputStream, Stream outputStream, string sKey)
        {
            RijndaelManaged algorithm = new RijndaelManaged();
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sKey, Encoding.ASCII.GetBytes(m_saltText));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(inputStream, algorithm.CreateEncryptor(), CryptoStreamMode.Read);
            cryptostream.CopyTo(outputStream);
        }
            
        protected virtual void Decrypt(Stream inputStream, Stream outputStream, string sKey)
        {
            RijndaelManaged algorithm = new RijndaelManaged();
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sKey, Encoding.ASCII.GetBytes(m_saltText));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(inputStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            cryptostream.CopyTo(outputStream);
        }
    }
}