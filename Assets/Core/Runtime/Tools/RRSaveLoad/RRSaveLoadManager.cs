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
using UnityEngine;

namespace Ox4b404ec.Tools.Tools.RRSaveLoad
{
    public static class RRSaveLoadManager
    {
        private const string s_baseFolderName = "RRData";

        private const string s_defaultFolderName = "RRSaveLoadDefault";

        public static IRRSaveLoadExecute SaveLoadExecuteBinary = new RRSaveLoadExecuteBinary();
        public static IRRSaveLoadExecute SaveLoadExecuteJson = new RRSaveLoadExecuteJson();
        public static IRRSaveLoadExecute SaveLoadExecuteBinaryEncrypted = new RRSaveLoadExecuteBinaryEncrypted();
        public static IRRSaveLoadExecute SaveLoadExecuteJsonEncrypted = new RRSaveLoadExecuteJsonEncrypted();
        
        private static string GetSavePath(string folderName = s_defaultFolderName)
        {
            string ret = string.Empty;
#if UNITY_EDITOR
            ret = $"{Application.dataPath}/{s_baseFolderName}/{folderName}";
# else
            ret = $"{Application.persistentDataPath}/{s_baseFolderName}/{folderName}";
#endif
            return ret;
        }

        private static void DoSave(object saveObject, FileStream fileName, SaveFileTypeEnum saveFileType = SaveFileTypeEnum.BINARY)
        {
            switch (saveFileType)
            {
                case SaveFileTypeEnum.BINARY:
                    SaveLoadExecuteBinary.Save(saveObject, fileName);
                    break;
                case SaveFileTypeEnum.BINARY_ENCRYPTED:
                    SaveLoadExecuteBinaryEncrypted.Save(saveObject, fileName);
                    break;
                case SaveFileTypeEnum.JSON:
                    SaveLoadExecuteJson.Save(saveObject, fileName);
                    break;
                case SaveFileTypeEnum.JSON_ENCRYPTED:
                    SaveLoadExecuteJsonEncrypted.Save(saveObject, fileName);
                    break;
                case SaveFileTypeEnum.XML:
                    break;
                case SaveFileTypeEnum.XML_ENCRYPTED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveFileType), saveFileType, null);
            }
        }
        
        private static object DoLoad(Type objectType, FileStream saveFile, SaveFileTypeEnum saveFileType = SaveFileTypeEnum.BINARY)
        {
            switch (saveFileType)
            {
                case SaveFileTypeEnum.BINARY:
                    SaveLoadExecuteBinary.Load(objectType, saveFile);
                    return null;
                    break;
                case SaveFileTypeEnum.BINARY_ENCRYPTED:
                    SaveLoadExecuteBinaryEncrypted.Load(objectType, saveFile);
                    return null;
                    break;
                case SaveFileTypeEnum.JSON:
                    SaveLoadExecuteJson.Load(objectType, saveFile);
                    return null;
                    break;
                case SaveFileTypeEnum.JSON_ENCRYPTED:
                    SaveLoadExecuteJsonEncrypted.Load(objectType, saveFile);
                    return null;
                    break;
                case SaveFileTypeEnum.XML:
                    return null;
                    break;
                case SaveFileTypeEnum.XML_ENCRYPTED:
                    return null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveFileType), saveFileType, null);
            }
        }
        
        public static void Save(object saveObject, string fileName, string foldername = s_defaultFolderName, SaveFileTypeEnum saveFileType = SaveFileTypeEnum.BINARY)
        {
            string savePath = GetSavePath(foldername);
            
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            FileStream saveFile = File.Create($"{savePath}/{fileName}");
            DoSave(saveObject, saveFile, saveFileType);
            saveFile.Close();
        }
        
        
        public static object Load(Type objectType, string fileName, string foldername = s_defaultFolderName, SaveFileTypeEnum saveFileType = SaveFileTypeEnum.BINARY)
        {
            string savePath = GetSavePath(foldername);
            string saveFilePath = $"{savePath}/{fileName}";
            if (!Directory.Exists(savePath) || !File.Exists(saveFilePath))
                return null;
            FileStream saveFile = File.Open(saveFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            object returnObject = DoLoad(objectType, saveFile);
            saveFile.Close();
            return returnObject;
        }
        
        public static void DeleteSave(string fileName, string folderName = s_defaultFolderName)
        {
            string savePath = GetSavePath(folderName);
            string saveFilePath = $"{savePath}/{fileName}";
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }	
            if (File.Exists(saveFilePath + ".meta"))
            {
                File.Delete(saveFilePath + ".meta");
            }			
        }
        
        public static void DeleteAllSaveFiles()
        {
            string savePath = GetSavePath("");

            savePath = savePath.Substring(0, savePath.Length - 1);
            if (savePath.EndsWith("/"))
            {
                savePath = savePath.Substring(0, savePath.Length - 1);
            }

            if (Directory.Exists(savePath))
            {
                DeleteDirectory(savePath);
            }
        }
        
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);

            if (File.Exists(target_dir + ".meta"))
            {
                File.Delete(target_dir + ".meta");
            }
        }
    }
}