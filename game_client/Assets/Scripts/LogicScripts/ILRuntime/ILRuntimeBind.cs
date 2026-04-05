using ILRuntime.Runtime.Enviorment;
using System;

namespace GOE
{
    /// <summary>
    /// NPILRuntime adaptor,action 
    /// </summary>
    public static class ILRuntimeBind
    {
        public static void bindILRuntimeAdapter(ILRuntime.Runtime.Enviorment.AppDomain _appDomain)
        {
            if (_appDomain == null)
                return;
            
            IlRuntimeLitJson.JsonMapper.RegisterILRuntimeCLRRedirection(_appDomain);
            
            _appDomain.RegisterCrossBindingAdaptor(new _AGGUIHotfixBasicWndAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGGUIHotfixBasicSubWndAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGGUIHotfixBasicSubPrefabWndAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGGUIHotfixBasicSimpleSubWndAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new UIQueueBaseNodeAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _ANPUINoticeDealerAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGGUIHotfixBasicGridItemWndAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGGUIHotfixBasicShowAnimGridWndAdapter<_AGGUIHotfixBasicGridItemWndAdapter.Adapter>());
            _appDomain.RegisterCrossBindingAdaptor(new _AALUGUIGridBarControllerAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _ABasicAdditionMainTDSceneAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _ABasicAdditionTDSceneAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _IBaseHotfixRefObjAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _ATNPGGUIWndTabItemAdapter<NPGGUIMonoCommonTab>());
            _appDomain.RegisterCrossBindingAdaptor(new _AGameDealerAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGameLoadUnitAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGameLogicAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGameTickUnitAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AGameUnitAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AALBasicSettingInfoAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _INPRequestCallbackDealerAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _ACommonListHotRefPatchDealerAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new IEqualityComparer_1_Int32Adapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AMainCityPushNoticeAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _AActivityMainCityPushNoticeAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _ABaseActivityInfoAdapter());

            _appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Object[]>();
            _appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Object>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean, ALPackage.ALAssetBundleObj>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean, ALPackage._IALProtocolStructureAdapter.Adapter>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            _appDomain.DelegateManager.RegisterMethodDelegate<GOE.NPGGUIWndCommonToggleEx>();
            _appDomain.DelegateManager.RegisterMethodDelegate<ALPackage._IALProtocolStructureAdapter.Adapter>(); 
            _appDomain.DelegateManager.RegisterMethodDelegate<GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList>();
            _appDomain.DelegateManager.RegisterMethodDelegate<GS2GC.p017_ActivityOp.GS2GC_017_005_RetActivityRankBaseInfoByKey>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
            _appDomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean, UnityEngine.EventSystems.PointerEventData>();
            _appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Action>();
            _appDomain.DelegateManager.RegisterMethodDelegate<GOE.NPGGUIWndTextTip>();
            _appDomain.DelegateManager.RegisterMethodDelegate<Common.RankObj.Rank_BaseItem>();
            _appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean, System.Action>();
            _appDomain.DelegateManager.RegisterMethodDelegate<GOE.EMainCityPushNoticeTriggerType, System.Action>();
            _appDomain.DelegateManager.RegisterMethodDelegate<GS2GC.p017_ActivityOp.GS2GC_017_004_RetActivityRankBaseInfoByRank>();

            _appDomain.DelegateManager.RegisterFunctionDelegate<GOE.BaseQueueNode, System.Boolean>();
            _appDomain.DelegateManager.RegisterFunctionDelegate<ALPackage._IALProtocolStructureAdapter.Adapter>();
            _appDomain.DelegateManager.RegisterFunctionDelegate<GOE._IBaseHotfixRefObjAdapter.Adapter, GOE._IBaseHotfixRefObjAdapter.Adapter, System.Int32>();
            _appDomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            _appDomain.DelegateManager.RegisterFunctionDelegate<GOE.NPRankCommonShowInfo, GOE.NPRankCommonShowInfo, System.Int32>();
            _appDomain.DelegateManager.RegisterFunctionDelegate<System.String, System.String, System.String>();
            _appDomain.DelegateManager.RegisterFunctionDelegate<Common.ActivityObj.Activity_Info, GOE._ABaseActivityInfo>();



            _appDomain.DelegateManager.RegisterDelegateConvertor<ALPackage._assetDownloadedDelegate>((act) =>
            {
                return new ALPackage._assetDownloadedDelegate((_isSuc, _assetObj) =>
                {
                    ((Action<System.Boolean, ALPackage.ALAssetBundleObj>)act)(_isSuc, _assetObj);
                });
            });
            _appDomain.DelegateManager.RegisterDelegateConvertor<ALPackage.MsgRecAction>((act) =>
            {
                return new ALPackage.MsgRecAction((_objs) =>
                {
                    ((Action<System.Object[]>)act)(_objs);
                });
            });
            _appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<GOE._IBaseHotfixRefObjAdapter.Adapter>>((act) =>
            {
                return new System.Comparison<GOE._IBaseHotfixRefObjAdapter.Adapter>((x, y) =>
                {
                    return ((Func<GOE._IBaseHotfixRefObjAdapter.Adapter, GOE._IBaseHotfixRefObjAdapter.Adapter, System.Int32>)act)(x, y);
                });
            });
            _appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
                });
            });
            _appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<GOE.NPRankCommonShowInfo>>((act) =>
            {
                return new System.Comparison<GOE.NPRankCommonShowInfo>((x, y) =>
                {
                    return ((Func<GOE.NPRankCommonShowInfo, GOE.NPRankCommonShowInfo, System.Int32>)act)(x, y);
                });
            });
            _appDomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() =>
                {
                    try
                    {
                        ((Action) act)();
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("Error in DG.Tweening.TweenCallback: " + e.Message + "\n" + e.StackTrace);
                    }
                });
            });

        }
    }
}