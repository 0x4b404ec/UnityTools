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
using Ox4b404ec.Tools.Tools.RRSaveLoad;
using UnityEngine;

namespace Tests.Runtime.Tools
{
    
    
    public class RRSaveLoadTest : MonoBehaviour
    {
        [Serializable]
        public class TestEn
        {
            public string key1;
            public int key2;
        }
        public void SaveJson()
        {
            TestEn en = new TestEn()
            {
                key1 = "123",
                key2 = 456
            };

            RRSaveLoadManager.Save(en, "RRSaveLoadTestBody", "RRSaveFolder", SaveFileTypeEnum.JSON);
        }
     
        
    }
}