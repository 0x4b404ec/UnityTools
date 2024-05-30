/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *     Unity Tools (https://github.com/0x4b404ec/UnityTools)    *
 *                                                              *
 *  Author:                                                     *
 *    0x4b404ec (https://github.com/0x4b404ec)                  *
 *                                                              *
 ****************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Ox4b404ec.Tools.Tools.RRSaveLoad
{
    public class RRSaveLoadExecuteJsonEncrypted :RRSaveLoadEncrypter, IRRSaveLoadExecute
    {
        public void Save(object objectToSave, FileStream saveFile)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSave);
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                memoryStream.Position = 0;
                Encrypt(memoryStream, saveFile, Key);
            }
            saveFile.Close();
        }

        public object Load(Type objectType, FileStream saveFile)
        {
            object savedObject = null;
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader streamReader = new StreamReader(memoryStream))
            {
                try
                {
                    Decrypt(saveFile, memoryStream, Key);
                }
                catch (CryptographicException ce)
                {
                    Debug.LogError("[RRSaveLoad] Encryption key error: " + ce.Message);
                    return null;
                }
                memoryStream.Position = 0;
                savedObject = Newtonsoft.Json.JsonConvert.DeserializeObject(streamReader.ReadToEnd(), objectType); 
            }
            saveFile.Close();
            return savedObject;
        }
    }
}