using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// Atlas提取每一个Sprite。
/// </summary>

public class AtlasExtract : Editor
{
    [MenuItem("Tools/Extract Sprites Of Atlas！")]
    public static void Extract()
    {
        UIAtlas[] atlasArray = Resources.FindObjectsOfTypeAll<UIAtlas>();

        Debug.LogError(atlasArray.Length);

        for (int i = atlasArray.Length - 1; i >= 0; i--)
        {
            UIAtlas atlas = atlasArray[i];
            Debug.Log(i + " " + atlas.name);

            ExtractSprite(atlas, Application.dataPath);
        }
    }

    private static void ExtractSprite(UIAtlas mAtlas, string mDirectory)
    {
        Debug.Log("Generate " + mAtlas.name);

        string rootPath = mDirectory;

        if (!Directory.Exists(rootPath + "/AllUI"))
        {
            Directory.CreateDirectory(rootPath + "/AllUI");
        }
        rootPath = mDirectory + "/AllUI";

        if (!string.IsNullOrEmpty(rootPath))
        {
            if (!Directory.Exists(rootPath + "/" + mAtlas.name))//判断文件夹是否存在
            {
                Directory.CreateDirectory(rootPath + "/" + mAtlas.name);
            }
            else
            {
                Debug.LogError("Same Atlas Name！" + mAtlas.name);
                return;
                //Directory.CreateDirectory(rootPath + "/" + mAtlas.name + "1");
            }

            string folderPath = rootPath + "/" + mAtlas.name;

            foreach (UISpriteData mSpriteData in mAtlas.spriteList)
            {
                UIAtlasMaker.SpriteEntry spriteEntry = UIAtlasMaker.ExtractSprite(mAtlas, mSpriteData.name);
                string spritePath = folderPath + "/" + mSpriteData.name + ".png";

                if (spriteEntry != null)
                {
                    byte[] bytes = spriteEntry.tex.EncodeToPNG();
                    File.WriteAllBytes(spritePath, bytes);
                    AssetDatabase.ImportAsset(spritePath);
                    if (spriteEntry.temporaryTexture)
                    {
                        DestroyImmediate(spriteEntry.tex);
                    }
                }
            }
        }
    }
}
