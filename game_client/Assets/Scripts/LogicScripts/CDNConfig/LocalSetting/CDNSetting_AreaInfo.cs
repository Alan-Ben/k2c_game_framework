using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace GOE
{
    /// <summary>
    /// 登录服务器的相关配置，此数据要求CDNLocalSetting_ClientConfigInfo先下载完成
    /// </summary>
    public class CDNSetting_AreaInfo : _ATCDNConfigSetting<string>
    {
        private static CDNSetting_AreaInfo _g_instance;
        [NotNull]
        public static CDNSetting_AreaInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_AreaInfo();
                return _g_instance;
            }
        }

        public const string INVALID_AREA_ID = "-1";//无效的大区id标识
        public const string DEFAULT_AREA_ID = "1";//默认大区id标识
        private Dictionary<string, string> _m_areaMap;

        protected CDNSetting_AreaInfo() : base("AreaInfo")
        {

        }
        
        /// <summary>
        /// 设置获取分区ID
        /// </summary>
        public string areaId
        {
            get
            {
                return GameSetting.instance.getAreaId();
            }
            set
            {
                GameSetting.instance.setAreaId(value, (int) Game.instance.mainCamera.platType);
            }
        }

        /// <summary>
        /// 当前大区标识
        /// </summary>
        public string areaTag
        {
            get
            {
                return getAreaTagById(areaId);
            }
        }

        /// <summary>
        /// cdn上配置的大区信息，key是tag，值是大区id
        /// </summary>
        public Dictionary<string, string> areaMap
        {
            get
            {
                if (null == data || null == data.config)
                    return null;

                if (null == _m_areaMap)
                {
                    try
                    {
                        _m_areaMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.config);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"【CDNSetting_AreaInfo】解析CDN大区配置错误，{e.ToString()}");
                    }
                }
                return _m_areaMap;
            }
        }

        /// <summary>
        /// 获取大区id根据大区标记
        /// </summary>
        /// <param name="_areaTag"></param>
        /// <returns></returns>
        public string getAreaIdByTag(string _areaTag)
        {
            if (null != areaMap && areaMap.TryGetValue(_areaTag, out string areaId))
            {
                return areaId;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取大区标记根据大区id
        /// </summary>
        /// <param name="_areaId"></param>
        /// <returns></returns>
        public string getAreaTagById(string _areaId)
        {
            if (areaMap != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in areaMap)
                {
                    if (keyValuePair.Value == _areaId)
                        return keyValuePair.Key;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 初始化大区信息
        /// </summary>
        public void initArea(Action _initDelegate)
        {
            //发送埋点-CDN-开始初始化Area
            GCommon.sendStepReport(TraceConst.START_INIT_AREA);

            commonDownload(() =>
            {
                //如果有缓存的大区，只要cdn上有配置就直接使用
                if (areaId != INVALID_AREA_ID && !string.IsNullOrEmpty(areaId))
                {
                    if (areaMap != null)
                    {
                        //如果cdn有配置对应大区直接用，否则重新走下面的初始化逻辑
                        foreach (KeyValuePair<string, string> keyValuePair in areaMap)
                        {
                            if (keyValuePair.Value == areaId)
                            {
                                //发送埋点-CDN-初始化Area成功,{0}
                                GCommon.sendStepReport(TraceConst.INIT_AREA_SUC.setMarkParam(areaId));
                                if (_initDelegate != null)
                                    _initDelegate();
                                return;
                            }
                        }
                    }
                }
                
                //推荐大区
                string recommendArea = "";
                //SDK获取的国家
                string sdkRegion = SDKMgr.instance.netWorkCity;

                //根据配置获取国家对应的大区
                if (PCountryAreaInfo.instance.obj != null && PCountryAreaInfo.instance.obj.refList != null && !string.IsNullOrEmpty(sdkRegion))
                {
                    for (int i = 0; i < PCountryAreaInfo.instance.obj.refList.Count; i++)
                    {
                        if (PCountryAreaInfo.instance.obj.refList[i] != null && 
                            !string.IsNullOrEmpty(PCountryAreaInfo.instance.obj.refList[i].area_code) &&
                            PCountryAreaInfo.instance.obj.refList[i].area_code.Equals(sdkRegion))
                        {
                            recommendArea = PCountryAreaInfo.instance.obj.refList[i].region_code;
                            break;
                        }
                    }
                }

                //发送埋点
                if (!string.IsNullOrEmpty(sdkRegion))
                {
                    //发送埋点-CDN-初始化Area，获取SDK区域：{0}，获取到的配表推荐大区：{1}
                    GCommon.sendStepReport(TraceConst.INIT_AREA_SDK_CITY.setMarkParam(sdkRegion, recommendArea));
                }
                else
                {
                    //发送埋点-CDN-初始化Area，获取SDK区域为空
                    GCommon.sendStepReport(TraceConst.INIT_AREA_SDK_CITY_NULL);
                }

                //cdn没返回数据，直接用默认值
                if (null == data || null == data.config)
                {
                    areaId = DEFAULT_AREA_ID;
                    //发送埋点-CDN-CDN-初始化Area失败，直接使用默认大区配置：{0}
                    GCommon.sendStepReport(TraceConst.INIT_AREA_FAIL.setMarkParam(areaId));
                    if (_initDelegate != null) 
                        _initDelegate();
                    return;
                }
                
                //cdn数据json解析失败，直接用默认值
                if (null == areaMap)
                {
                    areaId = DEFAULT_AREA_ID;
                    //发送埋点-CDN-CDN-初始化Area失败，直接使用默认大区配置：{0}
                    GCommon.sendStepReport(TraceConst.INIT_AREA_FAIL.setMarkParam(areaId));
                    if (_initDelegate != null) 
                        _initDelegate();
                    return;
                }

                //如果cdn有配置对应大区直接用
                if (areaMap.TryGetValue(recommendArea, out string curValue))
                {
                    areaId = curValue;
                    //发送埋点-CDN-初始化Area成功,{0}
                    GCommon.sendStepReport(TraceConst.INIT_AREA_SUC.setMarkParam(areaId));
                    if (_initDelegate != null) 
                        _initDelegate();
                    return;
                }
                else
                {
                    //推荐大区在cdn里面没找到，用cdn的默认大区配置
                    if (areaMap.TryGetValue("default", out string defaultValue))
                    {
                        areaId = defaultValue;
                        //发送埋点-CDN-初始化Area在CDN未找到推荐大区，使用默认大区{0}
                        GCommon.sendStepReport(TraceConst.INIT_AREA_USE_DEFAULT.setMarkParam(areaId));
                        if (_initDelegate != null) 
                            _initDelegate();
                        return;
                    }
                }

                //啥都没有就用默认的吧
                areaId = DEFAULT_AREA_ID;
                //发送埋点-CDN-初始化Area失败，直接使用默认大区配置：{0}
                GCommon.sendStepReport(TraceConst.INIT_AREA_FAIL.setMarkParam(areaId));
                if (_initDelegate != null) 
                    _initDelegate();
            });
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            //先写死服务器Id为1，后续根据需求调整
            return string.Format("/area_map/{0}", CDNSetting_ClientConfigInfo.instance.platformId);
        }

        /// <summary>
        /// 获取下载操作的处理对象
        /// </summary>
        /// <returns></returns>
        protected override _ATALCDNDownloadProvider _getURLDownloaderProvider()
        {
            return CDNURLProvider_Client.instance;
        }

        /// <summary>
        /// 在数据加载完成后的事件
        /// </summary>
        protected override void _onDataLoaded()
        {
            if (!isValid)
            {
                Debug.LogError($"cdn 数据加载失败{this}");
            }
        }
    }
}