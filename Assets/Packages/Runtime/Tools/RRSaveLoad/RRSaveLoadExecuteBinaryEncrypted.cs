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
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

namespace Ox4b404ec.Tools.Tools.RRSaveLoad
{
    public class RRSaveLoadExecuteBinaryEncrypted :RRSaveLoadEncrypter, IRRSaveLoadExecute
    {
        public void Save(object objectToSave, FileStream saveFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, objectToSave);
            memoryStream.Position = 0;
            Encrypt(memoryStream, saveFile, Key);
            saveFile.Flush();
            memoryStream.Close();
            saveFile.Close();
        }

        public object Load(Type objectType, FileStream saveFile)
        {
            object savedObject;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
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
            savedObject = formatter.Deserialize(memoryStream);
            memoryStream.Close();
            saveFile.Close();
            return savedObject; 
        }
    }
}