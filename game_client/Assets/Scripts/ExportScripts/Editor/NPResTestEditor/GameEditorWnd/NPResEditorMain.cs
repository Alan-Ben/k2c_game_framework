using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEditor;
using ALPackage;
using UnityEngine;

public class NPResEditorMain
{

    [MenuItem("NPAssets/引导UI快捷编辑窗口")]
    public static void generalTutorialEffectExport()
    {
        NPTutorialStepEditorWnd.showExportWnd("引导UI快捷编辑窗口");
    }

    
    [MenuItem("NPAssets/保存当前的模糊背景到jpg，放到Assets根目录下了")]
    public static void saveBlueRT()
    {
#if NP_GAME
        ScreenBlurMgr.instance.showBlur(null, () =>
        {
            RenderTexture targetRT = ScreenBlurMgr.instance.curBlurRT;
            if(null == targetRT)
                return;
        
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = targetRT;
            Texture2D png = new Texture2D(targetRT.width, targetRT.height, TextureFormat.ARGB32, false);
            png.ReadPixels(new Rect(0, 0, targetRT.width, targetRT.height), 0, 0);
            byte[] bytes = png.EncodeToPNG();
            FileStream file = File.Open(Application.dataPath + "/rt.jpg", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(bytes);
            file.Close();
            Texture2D.DestroyImmediate(png);
            png = null;
            RenderTexture.active = prev;
            AssetDatabase.Refresh(); 
        });
#endif
    }
}
