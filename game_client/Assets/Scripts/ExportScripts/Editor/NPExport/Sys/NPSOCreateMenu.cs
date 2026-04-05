using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEditor;

using ALPackage;
using GOE;

namespace GOE
{
	public class NPSOCreateMenu
	{
	    //生成空对象
	    [MenuItem("NPAssets/NPSOCreate/NPPSOLoginCommonInfo")]
	    public static void makeNPSOLoginCommonInfo()
	    {
	        if (!Directory.Exists(Application.dataPath + "/Resources/GameRes/common")) {
	            AssetDatabase.CreateFolder("Assets/Resources/GameRes", "common");
	        }

	        if (!Directory.Exists(Application.dataPath + "/Resources/GameRes/common/__NPPCommonInfo"))
	            AssetDatabase.CreateFolder("Assets/Resources/GameRes/common", "__NPPCommonInfo");

	        string path = "Assets/Resources/GameRes/common/__NPPCommonInfo/" + NPPSOLoginCommonInfo.objName;

	        NPPSOLoginCommonInfo newObj = ALCreateCommonFunc.createSOObjByFullPath<NPPSOLoginCommonInfo>(path);
	        if (null == newObj)
	        {
	            UnityEngine.Debug.LogError("make NPSOLoginCommonInfo Error!");
	            return;
	        }

	        //设置对应的assetbundle路径
	        ALAssetBundleCommon.setAssetBundleName(path, NPPSOLoginCommonInfo.assetPath);

	    }
	    //生成空对象
	    [MenuItem("NPAssets/NPSOCreate/NPGSOGameCommonInfo")]
	    public static void makeNPSOGameCommonInfo()
	    {
	        if (!Directory.Exists(Application.dataPath + "/Resources/GameRes/common")) {
	            AssetDatabase.CreateFolder("Assets/Resources/GameRes", "common");
	        }

	        if (!Directory.Exists(Application.dataPath + "/Resources/GameRes/common/__NPGCommonInfo"))
	            AssetDatabase.CreateFolder("Assets/Resources/GameRes/common", "__NPGCommonInfo");

	        string path = "Assets/Resources/GameRes/common/__NPGCommonInfo/" + NPGSOGameCommonInfo.objName;
	        NPGSOGameCommonInfo newObj = ALCreateCommonFunc.createSOObjByFullPath<NPGSOGameCommonInfo>(path);
	        if(null == newObj)
	        {
	            UnityEngine.Debug.LogError("make NPSOGameCommonInfo Error!");
	            return;
	        }

	        //设置对应的assetbundle路径
	        ALAssetBundleCommon.setAssetBundleName(path, NPGSOGameCommonInfo.assetPath);
	    }
    
#if NP_GAME
	    //生成空对象
	    [MenuItem("NPAssets/NPSOCreate/NPGSOGameCustomInfo")]
	    public static void makeNPSOGameCustomInfo()
	    {
	        if (!Directory.Exists(Application.dataPath + "/App_Resources/Resources")) {
	            AssetDatabase.CreateFolder("Assets/App_Resources", "Resources");
	        }
        
	        string path = "Assets/App_Resources/Resources/" + NPGSOGameCustomInfo.objName;
	        NPGSOGameCustomInfo newObj = ALCreateCommonFunc.createSOObjByFullPath<NPGSOGameCustomInfo>(path);
	        if(null == newObj)
	        {
	            UnityEngine.Debug.LogError("make NPSOGameCustomInfo Error!");
	            return;
	        }

	    }
#endif
    
#if NP_GAME
	    //生成空对象
	    [MenuItem("NPAssets/NPSOCreate/NPClientVersionSetting")]
	    public static void makeClientVersionSetting()
	    {
	        if (!Directory.Exists(Application.dataPath + "/App_Resources/Resources")) {
	            AssetDatabase.CreateFolder("Assets/App_Resources", "Resources");
	        }
        
	        ClientVersionSetting newObj = ALCreateCommonFunc.createSOObjByFullPath<ClientVersionSetting>(ClientVersionSetting.assetPath);
	        if(null == newObj)
	        {
	            UnityEngine.Debug.LogError("make ClientVersionSetting Error!");
	            return;
	        }

	    }
#endif

	}
}