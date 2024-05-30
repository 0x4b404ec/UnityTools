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
using System.Text;
using Newtonsoft.Json;

namespace Ox4b404ec.Tools.Tools.RRSaveLoad
{
    public class RRSaveLoadExecuteJson : IRRSaveLoadExecute
    {
        public void Save(object objectToSave, FileStream saveFile)
        {
            string json = JsonConvert.SerializeObject(objectToSave);
            StreamWriter streamWriter = new StreamWriter(saveFile);
            streamWriter.Write(json);
            streamWriter.Close();
            saveFile.Close();
        }

        public object Load(Type objectType, FileStream saveFile)
        {
            object savedObject; // = System.Activator.CreateInstance(objectType);
            StreamReader streamReader = new StreamReader(saveFile, Encoding.UTF8);
            string json = streamReader.ReadToEnd();
            savedObject = JsonConvert.DeserializeObject(json,objectType);
            streamReader.Close();
            saveFile.Close();
            return savedObject;
        }
    }
}