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
using UnityEngine;

namespace Ox4b404ec.Tools.Tools.RRSaveLoad
{
    public class RRSaveLoadExecuteBinary : IRRSaveLoadExecute
    {
        public void Save(object objectToSave, FileStream saveFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(saveFile, objectToSave);
            Debug.Log($"{saveFile.Name}");
            saveFile.Close();
        }

        public object Load(Type objectType, FileStream saveFile)
        {
            object savedObject;
            BinaryFormatter formatter = new BinaryFormatter();
            savedObject = formatter.Deserialize(saveFile);
            saveFile.Close();
            return savedObject;
        }
    }
}