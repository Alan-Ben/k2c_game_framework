using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;

#if NP_GAME
using Google.Android.AppBundle.Editor.Internal.AssetPacks;
using Google.Android.AppBundle.Editor.AssetPacks;
using Google.Android.AppBundle.Editor.Internal;
using UnityEngine.SceneManagement;
#endif

using GOE;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/** 给蛋总的导出设置函数 */
public class TCClientExportFunc
{
    /** 设置游戏包名 */
    public static void setProductName( string _productName, string _packageName, bool _useObb, string _companyName= "mechanist")
    {
        //产品名
        PlayerSettings.companyName = _companyName;
        PlayerSettings.productName = _productName;
        //包名
        PlayerSettings.applicationIdentifier = _packageName;

        //是否使用Obb
        PlayerSettings.Android.useAPKExpansionFiles = _useObb;

        UnityEngine.Debug.LogError("设置App信息 - 公司名: " + _companyName + "  产品名: _productName" + "  包名: " + _packageName);
        UnityEngine.Debug.LogError("打包方式设置为: " + (PlayerSettings.Android.useAPKExpansionFiles ? "使用Obb" : "整包方式"));
    }

    //设置打包Key
    public static void setKeyStoreInfo(string _keyStorePath, string _keyStorePass, string _keyAliasName, string _keyAliasPass)
    {
        //设置打包key
        PlayerSettings.Android.keystoreName = _keyStorePath;
        PlayerSettings.Android.keyaliasName = _keyAliasName;
        PlayerSettings.keystorePass = _keyStorePass;
        PlayerSettings.keyaliasPass = _keyAliasPass;

        UnityEngine.Debug.LogError("设置打包KeyStore为: " + _keyStorePath + " 对应 Alias: " + _keyAliasName);
    }

    //设置打包版本号
    public static void setVersionCode(string _version, int _buildCode)
    {
        PlayerSettings.bundleVersion = _version;
        if (0 == _buildCode) {
#if UNITY_ANDROID
            UnityEngine.Debug.LogError($"设置 Android bundleVersionCode：为默认值{PlayerSettings.Android.bundleVersionCode}，不作修改");
#elif UNITY_IPHONE
            UnityEngine.Debug.LogError($"设置 IOS buildNumber： 为默认值{PlayerSettings.iOS.buildNumber}，不作修改");
#endif
        }
        else {
#if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = _buildCode;
            UnityEngine.Debug.LogError("设置 Android bundleVersionCode： " + _buildCode);
#elif UNITY_IPHONE
            PlayerSettings.iOS.buildNumber = _buildCode.ToString();
            UnityEngine.Debug.LogError("设置 IOS buildNumber： " + _buildCode);
#endif
        }
    }

    /**  打包APK */
    public static void buildAPK(string _platName, int _platAddBuildVersion, bool _isUseSdk, string _apkFolder, string _clientLoginType, bool _isShowSDKLog, bool _isDevelopment, bool _isAAB)
    {
#if NP_GAME
        //设置scene
        string[] sceneList = new string[0];

        //开启场景
        Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/lunch.unity", OpenSceneMode.Single);

        //获取主摄像头对象
        GameObject mainCameraGo = GameObject.Find("Main Camera");
        if(null == mainCameraGo)
        {
            UnityEngine.Debug.LogError("Can not find Main Camera!");
            return;
        }
        GameObject ui_cameraGo = GameObject.Find("ui_camera");
        if(null == ui_cameraGo)
        {
            UnityEngine.Debug.LogError("Can not find ui_cameraGo!");
            return;
        }
        
        MainCameraMono mainCameraMono = mainCameraGo.GetComponent<MainCameraMono>();
        if (null == mainCameraMono)
        {
            UnityEngine.Debug.LogError("launch的Main Camera上找不到MGMainCameraMono!");
            return;
        }
        ALSetLocalLoadUIMono localLoadUIMono = ui_cameraGo.GetComponent<ALSetLocalLoadUIMono>();
        if (null == localLoadUIMono)
        {
            UnityEngine.Debug.LogError("launch的ui_camera上找不到ALSetLocalLoadUIMono!");
            return;
        }

        localLoadUIMono.enabled = false;//不使用本地资源
        localLoadUIMono.isShowLocalLoadedTag = true;//显示log
        
        //设置目标平台
        EWCGPlatType platType;
        if(EWCGPlatType.TryParse(_platName, out platType))
        {
            UnityEngine.Debug.LogError($"设置为 固有平台： {platType}");
        }
        else
        {
            UnityEngine.Debug.LogError($"{_platName}解析不了，设置为 自定义平台EXTRA_PLAT 附加版本号： {_platAddBuildVersion}");
            platType = EWCGPlatType.EXTRA_PLAT;
        }
        mainCameraMono.platType = platType;
        
        //找到对应平台的设置，并设置值
        bool hasFindPlat = false;
        foreach (WCGPlatLoginInfo platLoginInfo in mainCameraMono.platInfoList)
        {
            if(platLoginInfo == null)
            {
                continue;
            }
            if(platLoginInfo.platType == mainCameraMono.platType)
            {
                hasFindPlat = true;
                //设置IP，资源下载地址，如果填了cdn，就不会读取这些，以cdn里配置的为准。这些是为了可以连接指定ip，不需要一定走cdn
                setPlatLoginInfo(platLoginInfo, _platAddBuildVersion);
            }
        }

        //设置是否使用SDk
        mainCameraMono.isUseSDK = _isUseSdk;
        UnityEngine.Debug.LogError("设置为 " + (mainCameraMono.isUseSDK ? "使用SDK登录" : "内部登录方式"));

        //是否勾选aab打包
        AssetDeliveryConfig assetDeliveryConfig = AssetDeliveryConfigSerializer.LoadConfig();
        assetDeliveryConfig.Refresh();
        assetDeliveryConfig.SplitBaseModuleAssets = _isAAB;
        AssetDeliveryConfigSerializer.SaveConfig(assetDeliveryConfig);
        UnityEngine.Debug.LogError("设置为 " + (_isAAB ? "打AAB包" : "不打AAB包"));

        //Unity 2021需要保存一下场景，不然对lunch的修改会丢失
        EditorSceneManager.SaveScene(scene);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        //拼凑总路径
        string fullPath = _apkFolder;

        //设置需要符号表文件
        EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;

        //输出Log
        UnityEngine.Debug.LogError("Export Full Path: " + TCSetting.apkUrl);

        BuildOptions buildOptions = BuildOptions.None;
        //是否开启development
        if(_isDevelopment)   
            buildOptions |= BuildOptions.Development;

#if UNITY_IPHONE
        if (Directory.Exists(TCSetting.apkUrl))
        {
            //ios需要设置这个，才不会重新生成xcode
            buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;   
        }
#endif

        //unity打包
        Action buildAction = () =>
        {
            BuildPipeline.BuildPlayer(sceneList, TCSetting.apkUrl,
                //目标平台
#if UNITY_ANDROID
                BuildTarget.Android,
#elif UNITY_IPHONE
            BuildTarget.iOS,
#elif UNITY_STANDALONE_WIN
            BuildTarget.StandaloneWindows64,
#endif
                //打包参数
                buildOptions
            );
        };


        //是否打AAB包，是的话调用谷歌插件的打包接口，该谷歌插件可支持150m以上的AAB游戏包上传谷歌商店
        //不打AAB包的话就走unity的打包接口
        if (_isAAB)
        {
#if UNITY_ANDROID
            EditorUserBuildSettings.buildAppBundle = true;
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = sceneList;
            buildPlayerOptions.locationPathName = TCSetting.apkUrl;
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.targetGroup = BuildTargetGroup.Android;
            buildPlayerOptions.options = buildOptions;

            AppBundlePublisher.Build(buildPlayerOptions, AssetPackConfigSerializer.LoadConfig(), false);
#else
            EditorUserBuildSettings.buildAppBundle = false;
            buildAction();
#endif
        }
        else
        {
            EditorUserBuildSettings.buildAppBundle = false;
            buildAction();
        }


#endif
    }

#if NP_GAME
    static void setPlatLoginInfo(WCGPlatLoginInfo _platLoginInfo, int _platAddBuildVersion)
    {
        if(string.IsNullOrEmpty(TCSetting.connectIp) == false)
            _platLoginInfo.connectIp = TCSetting.connectIp;
        if(TCSetting.connectPort != 0)
            _platLoginInfo.port = TCSetting.connectPort;
        if(TCSetting.serverId != -1)
            _platLoginInfo.serverId = TCSetting.serverId;
        if(string.IsNullOrEmpty(TCSetting.areaResURL) == false)
            _platLoginInfo.areaResURL = TCSetting.areaResURL;
        if(string.IsNullOrEmpty(TCSetting.gameResURL) == false)
            _platLoginInfo.gameResURL = TCSetting.gameResURL;
        if (string.IsNullOrEmpty(TCSetting.hotfixURL) == false)
            _platLoginInfo.hotfixURL = TCSetting.hotfixURL;
        if (string.IsNullOrEmpty(TCSetting.injectFixURL) == false)
            _platLoginInfo.injectFixURL = TCSetting.injectFixURL;
        if (string.IsNullOrEmpty(TCSetting.refdataURL) == false)
            _platLoginInfo.refdataResURL = TCSetting.refdataURL;
        if (string.IsNullOrEmpty(TCSetting.videoResURL) == false)
            _platLoginInfo.videoResURL = TCSetting.videoResURL;
        if (string.IsNullOrEmpty(TCSetting.audioURL) == false)
            _platLoginInfo.audioResURL = TCSetting.audioURL;

        //设置CDN信息
        if (string.IsNullOrEmpty(TCSetting.phpUrlForLoginList) == false)
        {
            if (null == _platLoginInfo.phpUrlForLoginList)
                _platLoginInfo.phpUrlForLoginList = new List<string>();
            _platLoginInfo.phpUrlForLoginList.Clear();
            string[] cdnUrlList = TCSetting.phpUrlForLoginList.Split(';');
            int length = cdnUrlList != null ? cdnUrlList.Length : 0;
            for (int i = 0; i < length; i++)
            {
                string cdnUrl = cdnUrlList[i];
                if (string.IsNullOrEmpty(cdnUrl))
                {
                    continue;//传空的话，跳过
                }
                //检查CDN地址是否合法
                Uri uriResult;
                bool result = Uri.TryCreate(cdnUrl, UriKind.Absolute, out uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (result)
                {
                    _platLoginInfo.phpUrlForLoginList.Add(cdnUrl);
                }
                else
                {
                    Debug.LogError($"CDN地址不是合法的Url，不添加：{cdnUrl}");
                }
            }
        }

        if (string.IsNullOrEmpty(TCSetting.phpADDefaultURL) == false)//设置ADURL
        {
            //拆分
            string[] phpAddURL = TCSetting.phpADDefaultURL.Split(';');

            //创建队列
            if(phpAddURL.Length > 0)
                _platLoginInfo.phpADDefaultURL = phpAddURL[0];
        }

        //设置扩展版本号
        _platLoginInfo.channelId = _platAddBuildVersion;
    }
#endif
}
