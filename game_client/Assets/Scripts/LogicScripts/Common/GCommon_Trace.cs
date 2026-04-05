using ALPackage;
using Common.OfflineRewardObj;
using MJSDK_Package;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace GOE
{
    public static partial class GCommon
    {
        private static long _m_lInitTime;//初始化时间
        private static long _m_lLastEventTime;//最后一次发送埋点的时间

        /// <summary>
        /// 初始化埋点开始时间
        /// </summary>
        public static void initStepReportStartTime()
        {
            _m_lInitTime = ALCommon.getNowTimeMill();
            _m_lLastEventTime = 0;
        }

        /// <summary>
        /// 发送PHP埋点
        /// </summary>
        /// <param name="_stepReportInfo"></param>
        public static void sendStepReport(TraceStepData _stepReportInfo)
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                //查看非报错埋点
                if(_stepReportInfo.ID != TraceConst.LOG_ERROR.ID)
                    Debug.Log($"发送埋点-PHP埋点，ID:{_stepReportInfo.ID} mark:{_stepReportInfo.mark}");
            }

            if (SDKMgr.instance.isUseSDK)
                SDKMgr.instance.trace_gameStep(_stepReportInfo);
            else
                EvenTrackingPostMgr.instance.sendStepReport(_stepReportInfo);
        }

        /// <summary>
        /// 发送第三方埋点
        /// </summary>
        /// <param name="_type"></param>
        public static void sendAllThirdCustomEvent(EThirdCustomEventType _type,string _param = null)
        {
            if (_type == EThirdCustomEventType.NONE)
                return;

            //TODO 不同渠道可能标识不一样，之后再做区分

            string thirdEventTag = "";
            switch (_type)
            {
                case EThirdCustomEventType.LOGIN:
                    thirdEventTag = "login";
                    sendStepReport(TraceConst.THIRD_LOGIN);
                    break;
                case EThirdCustomEventType.ROLE:
                    thirdEventTag = "role";
                    sendStepReport(TraceConst.THIRD_ROLE);
                    break;
                case EThirdCustomEventType.TUTORIAL:
                    thirdEventTag = "tutorial";
                    sendStepReport(TraceConst.THIRD_TUTORIAL);
                    break;
                case EThirdCustomEventType.STAGE_2:
                    thirdEventTag = "stage2";
                    sendStepReport(TraceConst.THIRD_STAGE_2);
                    break;
                case EThirdCustomEventType.STAGE_8:
                    thirdEventTag = "stage8";
                    sendStepReport(TraceConst.THIRD_STAGE_8);
                    break;
            }

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"发送埋点-第三方埋点，{thirdEventTag}");
            }

            //发送firebase、appsflyer、facebook埋点
            if (!string.IsNullOrEmpty(thirdEventTag))
            {
                SDKMgr.instance.firebase_customEvent(thirdEventTag);
                SDKMgr.instance.appsflyer_customEvent(thirdEventTag);
                SDKMgr.instance.facebook_customEvent(thirdEventTag);
            }
        }

        /// <summary>
        /// 发送第三方支付埋点
        /// </summary>
        /// <param name="_info"></param>
        public static void sendAllThirdPurchaseEvent(Offline_OrderDelivery _info)
        {
            //订单类型：1 内购，2 网页充值，3 福利（虚拟充值），95 代金券
            //只发送内购和网页充值的埋点
            if (_info == null || (_info.getOrderType() != 1 && _info.getOrderType() != 2))
                return;

            MJSDK_2SDK_ThirdParty_purchaseEvent_base purchaseData = new MJSDK_2SDK_ThirdParty_purchaseEvent_base();
            purchaseData.mj_order_id = _info.getSdkOrderId();
            purchaseData.app_order_id = _info.getOrderId();
            purchaseData.revenue = _info.getPayMoney().ToString(CultureInfo.InvariantCulture);
            purchaseData.currency = _info.getPayCurrency();
            purchaseData.goods_id = _info.getGiftPackId().ToString();
            purchaseData.server_id = GameInit_SelectServer.instance.loginServerLogicId.ToString();

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"发送埋点-第三方支付埋点，mj_order_id:{purchaseData.mj_order_id},app_order_id:{purchaseData.app_order_id},revenue:{purchaseData.revenue},currency:{purchaseData.currency},goods_id:{purchaseData.goods_id},server_id:{purchaseData.server_id}");
            }

            //先发送php埋点
            sendStepReport(TraceConst.THIRD_PURCHASE.setMarkParam(purchaseData.mj_order_id, purchaseData.app_order_id, purchaseData.revenue, purchaseData.currency, purchaseData.goods_id, purchaseData.server_id));

            //是否是内购
            bool isInGamePurchase = _info.getOrderType() == 1;

            //发送第三方埋点
            SDKMgr.instance.appsflyer_purchaseEvent(purchaseData);
            SDKMgr.instance.facebook_purchaseEvent(purchaseData);
            //游戏内购firebase会自动从google play拉取购买事件，仅需要发送非内购事件
            if (!isInGamePurchase)
                SDKMgr.instance.firebase_purchaseEvent(purchaseData);
        }


        /// <summary>
        /// 获取埋点时间差及累计时间
        /// </summary>
        /// <returns></returns>
        public static string getStepReportMark2String()
        {
            //当前上报时间
            long nowTime = ALCommon.getNowTimeMill();
            if (_m_lLastEventTime == 0)
                _m_lLastEventTime = nowTime;

            //埋点时间差
            long elapsedTime = nowTime - _m_lLastEventTime;
            _m_lLastEventTime = nowTime;

            return $"{elapsedTime}-{nowTime - _m_lInitTime}";
        }

        /// <summary>
        /// 获取SystemInfo信息
        /// </summary>
        /// <returns></returns>
        public static string getSystemInfo()
        {
            StringBuilder builder = new StringBuilder(512);
            bool supportASTC = SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_4x4) && SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_6x6);
            bool supportHDRASTC = SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_4x4);
            builder.Append(supportASTC ? 1 : 0).Append(";");;
            builder.Append(supportHDRASTC ? 1 : 0).Append(";");;
            builder.Append(SystemInfo.batteryLevel).Append(";"); //float
            builder.Append(SystemInfo.batteryStatus).Append(";"); //BatteryStatus
            builder.Append(SystemInfo.computeSubGroupSize).Append(";"); //int
            builder.Append(SystemInfo.constantBufferOffsetAlignment).Append(";"); //int
            builder.Append((int)SystemInfo.copyTextureSupport).Append(";"); //CopyTextureSupport
            builder.Append(SystemInfo.deviceModel).Append(";"); //string
            builder.Append(SystemInfo.deviceName).Append(";"); //string
            builder.Append((int)SystemInfo.deviceType).Append(";"); //DeviceType
            builder.Append(SystemInfo.deviceUniqueIdentifier).Append(";"); //string
            builder.Append(SystemInfo.graphicsDeviceID).Append(";"); //int
            builder.Append(SystemInfo.graphicsDeviceName).Append(";"); //string
            builder.Append((int)SystemInfo.graphicsDeviceType).Append(";"); //GraphicsDeviceType
            builder.Append(SystemInfo.graphicsDeviceVendor).Append(";"); //string
            builder.Append(SystemInfo.graphicsDeviceVendorID).Append(";"); //int
            builder.Append(SystemInfo.graphicsDeviceVersion).Append(";"); //string
            builder.Append(SystemInfo.graphicsMemorySize).Append(";"); //int
            builder.Append(SystemInfo.graphicsMultiThreaded ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.graphicsShaderLevel).Append(";"); //int
            builder.Append(SystemInfo.graphicsUVStartsAtTop ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.hasHiddenSurfaceRemovalOnGPU ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.hasMipMaxLevel ? 1 : 0).Append(";"); //bool
            builder.Append((int)SystemInfo.hdrDisplaySupportFlags).Append(";"); //HDRDisplaySupportFlags
            builder.Append(SystemInfo.maxAnisotropyLevel).Append(";"); //int
            builder.Append(SystemInfo.maxComputeBufferInputsCompute).Append(";"); //int
            builder.Append(SystemInfo.maxComputeBufferInputsDomain).Append(";"); //int
            builder.Append(SystemInfo.maxComputeBufferInputsFragment).Append(";"); //int
            builder.Append(SystemInfo.maxComputeBufferInputsGeometry).Append(";"); //int
            builder.Append(SystemInfo.maxComputeBufferInputsHull).Append(";"); //int
            builder.Append(SystemInfo.maxComputeBufferInputsVertex).Append(";"); //int
            builder.Append(SystemInfo.maxComputeWorkGroupSize).Append(";"); //int
            builder.Append(SystemInfo.maxComputeWorkGroupSizeX).Append(";"); //int
            builder.Append(SystemInfo.maxComputeWorkGroupSizeY).Append(";"); //int
            builder.Append(SystemInfo.maxComputeWorkGroupSizeZ).Append(";"); //int
            builder.Append(SystemInfo.maxCubemapSize).Append(";"); //int
            builder.Append(SystemInfo.maxGraphicsBufferSize).Append(";"); //long
            builder.Append(SystemInfo.maxTexture3DSize).Append(";"); //int
            builder.Append(SystemInfo.maxTextureArraySlices).Append(";"); //int
            builder.Append(SystemInfo.maxTextureSize).Append(";"); //int
            builder.Append((int)SystemInfo.npotSupport).Append(";"); //NPOTSupport
            builder.Append(SystemInfo.operatingSystem).Append(";"); //string
            builder.Append((int)SystemInfo.operatingSystemFamily).Append(";"); //OperatingSystemFamily
            builder.Append(SystemInfo.processorCount).Append(";"); //int
            builder.Append(SystemInfo.processorFrequency).Append(";"); //int
            builder.Append(SystemInfo.processorType).Append(";"); //string
            builder.Append((int)SystemInfo.renderingThreadingMode).Append(";"); //RenderingThreadingMode
            builder.Append(SystemInfo.supportedRandomWriteTargetCount).Append(";"); //int
            builder.Append(SystemInfo.supportedRenderTargetCount).Append(";"); //int
            builder.Append(SystemInfo.supports2DArrayTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supports32bitsIndexBuffer ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supports3DRenderTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supports3DTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsAccelerometer ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsAnisotropicFilter ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsAsyncCompute ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsAsyncGPUReadback ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsAudio ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsCompressed3DTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsComputeShaders ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsConservativeRaster ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsCubemapArrayTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsGeometryShaders ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsGpuRecorder ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsGraphicsFence ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsGyroscope ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsHardwareQuadTopology ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsInstancing ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsLocationService ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsMipStreaming ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsMotionVectors ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsMultisampleAutoResolve ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsMultisampled2DArrayTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsMultisampledTextures).Append(";"); //int
            builder.Append(SystemInfo.supportsMultisampleResolveDepth ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsMultiview ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsRawShadowDepthSampling ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsRayTracing ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsRenderTargetArrayIndexFromVertexShader ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsSeparatedRenderTargetsBlend ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsSetConstantBuffer ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsShadows ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsSparseTextures ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsStoreAndResolveAction ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsTessellationShaders ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.supportsTextureWrapMirrorOnce).Append(";"); //int
            builder.Append(SystemInfo.supportsVibration ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.systemMemorySize).Append(";"); //int
            builder.Append(SystemInfo.unsupportedIdentifier).Append(";"); //string
            builder.Append(SystemInfo.usesLoadStoreActions ? 1 : 0).Append(";"); //bool
            builder.Append(SystemInfo.usesReversedZBuffer ? 1 : 0); //bool
            builder.Append(EmulatorCheck.IsProbablyEmulator() ? 1 : 0).Append(";");

            return builder.ToString();
        }
    }
}