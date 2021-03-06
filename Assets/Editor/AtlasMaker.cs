﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

namespace MiniWeChat.Editor
{
    public class AtlasMaker
    {
        /// <summary>
        /// 将Raw/Image目录下面的图片转化为Prefab存在Resources目录下
        /// </summary>
        [MenuItem("Tools/AtlasMaker")]
        static private void MakeAtlas()
        {

            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Raw/Image");
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                string prefabDirPath = dirInfo.FullName;
                prefabDirPath = prefabDirPath.Insert(prefabDirPath.IndexOf("Raw"), @"Resources\");

                Log4U.LogInfo(prefabDirPath);

                if (!Directory.Exists(prefabDirPath))
                {
                    Directory.CreateDirectory(prefabDirPath);
                }

                foreach (FileInfo pngFile in dirInfo.GetFiles())
                {
                    if (pngFile.FullName.EndsWith("png") || pngFile.FullName.EndsWith("jpg"))
                    {
                        string allPath = pngFile.FullName;
                        string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                        string prefabPath = assetPath.Insert(assetPath.IndexOf("Raw"), @"Resources\");
                        prefabPath = prefabPath.Replace("png", "prefab");
                        prefabPath = prefabPath.Replace("jpg", "prefab");
                        prefabPath = prefabPath.Replace(@"\", @"/");
                        string allPrefabPath = Application.dataPath + "/" + prefabPath.Substring(prefabPath.LastIndexOf("Resources"));
                        if (!File.Exists(allPrefabPath))
                        {
                            Sprite sprite = Resources.LoadAssetAtPath<Sprite>(assetPath);
                            GameObject go = new GameObject(sprite.name);
                            go.AddComponent<SpriteRenderer>().sprite = sprite;
                            PrefabUtility.CreatePrefab(prefabPath, go);
                            GameObject.DestroyImmediate(go);
                        }
					}
                }
            }
        }


    }
}

