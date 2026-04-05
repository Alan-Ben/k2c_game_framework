using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif

namespace GOE
{   
    public class _ABasicAdditionMainTDSceneAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_ABasicAdditionMainTDScene);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : _ABasicAdditionMainTDScene, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<ALPackage._AALResourceCore> mget__resourceCore_0 = new CrossBindingFunctionInfo<ALPackage._AALResourceCore>("get__resourceCore");
            CrossBindingFunctionInfo<global::BasicResIndexInfo> m_getDefaultSceneIndex_1 = new CrossBindingFunctionInfo<global::BasicResIndexInfo>("_getDefaultSceneIndex");
            CrossBindingMethodInfo m_showRender_2 = new CrossBindingMethodInfo("_showRender");
            CrossBindingMethodInfo m_hideRender_3 = new CrossBindingMethodInfo("_hideRender");
            CrossBindingMethodInfo<System.Int32, global::SceneInfoRefObj, System.Action> m_loadAllSceneSO_4 = new CrossBindingMethodInfo<System.Int32, global::SceneInfoRefObj, System.Action>("_loadAllSceneSO");
            CrossBindingFunctionInfo<global::SceneInfoRefObj> mget_sceneRefObj_5 = new CrossBindingFunctionInfo<global::SceneInfoRefObj>("get_sceneRefObj");
            CrossBindingFunctionInfo<global::SceneInfoRefObj, _IGameInputDealer> m_getSceneInputerDealer_6 = new CrossBindingFunctionInfo<global::SceneInfoRefObj, _IGameInputDealer>("_getSceneInputerDealer");
            CrossBindingMethodInfo<System.Action> m_dealShowSceneNP_7 = new CrossBindingMethodInfo<System.Action>("_dealShowSceneNP");
            CrossBindingMethodInfo<System.Action> m_dealHideSceneNP_8 = new CrossBindingMethodInfo<System.Action>("_dealHideSceneNP");
            CrossBindingMethodInfo m_onEnterScene_9 = new CrossBindingMethodInfo("_onEnterScene");
            CrossBindingMethodInfo m_onQuitTDScene_10 = new CrossBindingMethodInfo("_onQuitTDScene");
            CrossBindingMethodInfo<UnityEngine.GameObject> m_onRootGOLoaded_11 = new CrossBindingMethodInfo<UnityEngine.GameObject>("_onRootGOLoaded");
            CrossBindingMethodInfo<System.Action> m_enterSceneAdditionLoad_12 = new CrossBindingMethodInfo<System.Action>("_enterSceneAdditionLoad");
            CrossBindingMethodInfo m_onQuitScene_13 = new CrossBindingMethodInfo("_onQuitScene");
            CrossBindingMethodInfo mhideScene_14 = new CrossBindingMethodInfo("hideScene");
            CrossBindingMethodInfo<System.Action> mhideScene_15 = new CrossBindingMethodInfo<System.Action>("hideScene");
            CrossBindingMethodInfo mshowScene_16 = new CrossBindingMethodInfo("showScene");
            CrossBindingMethodInfo<System.Action> mshowScene_17 = new CrossBindingMethodInfo<System.Action>("showScene");
            CrossBindingMethodInfo menterAndShowScene_18 = new CrossBindingMethodInfo("enterAndShowScene");
            CrossBindingMethodInfo<System.Action> menterAndShowScene_19 = new CrossBindingMethodInfo<System.Action>("enterAndShowScene");
            CrossBindingFunctionInfo<System.Boolean> mget_needDiscardOnSwitch_20 = new CrossBindingFunctionInfo<System.Boolean>("get_needDiscardOnSwitch");
            CrossBindingMethodInfo monSwitchHideScene_21 = new CrossBindingMethodInfo("onSwitchHideScene");
            CrossBindingMethodInfo m_onSceneInited_22 = new CrossBindingMethodInfo("_onSceneInited");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override global::BasicResIndexInfo _getDefaultSceneIndex()
            {
                if (m_getDefaultSceneIndex_1.CheckShouldInvokeBase(this.instance))
                    return base._getDefaultSceneIndex();
                else
                    return m_getDefaultSceneIndex_1.Invoke(this.instance);
            }

            protected override void _showRender()
            {
                if (m_showRender_2.CheckShouldInvokeBase(this.instance))
                    base._showRender();
                else
                    m_showRender_2.Invoke(this.instance);
            }

            protected override void _hideRender()
            {
                if (m_hideRender_3.CheckShouldInvokeBase(this.instance))
                    base._hideRender();
                else
                    m_hideRender_3.Invoke(this.instance);
            }

            protected override void _loadAllSceneSO(System.Int32 _opSerialize, global::SceneInfoRefObj _sceneRef, System.Action _onLoaded)
            {
                if (m_loadAllSceneSO_4.CheckShouldInvokeBase(this.instance))
                    base._loadAllSceneSO(_opSerialize, _sceneRef, _onLoaded);
                else
                    m_loadAllSceneSO_4.Invoke(this.instance, _opSerialize, _sceneRef, _onLoaded);
            }

            protected override _IGameInputDealer _getSceneInputerDealer(global::SceneInfoRefObj _sceneRefObj)
            {
                return m_getSceneInputerDealer_6.Invoke(this.instance, _sceneRefObj);
            }

            protected override void _dealShowSceneNP(System.Action _delegate)
            {
                m_dealShowSceneNP_7.Invoke(this.instance, _delegate);
            }

            protected override void _dealHideSceneNP(System.Action _delegate)
            {
                m_dealHideSceneNP_8.Invoke(this.instance, _delegate);
            }

            protected override void _onEnterScene()
            {
                if (m_onEnterScene_9.CheckShouldInvokeBase(this.instance))
                    base._onEnterScene();
                else
                    m_onEnterScene_9.Invoke(this.instance);
            }

            protected override void _onQuitTDScene()
            {
                m_onQuitTDScene_10.Invoke(this.instance);
            }

            protected override void _onRootGOLoaded(UnityEngine.GameObject _g0)
            {
                m_onRootGOLoaded_11.Invoke(this.instance, _g0);
            }

            protected override void _enterSceneAdditionLoad(System.Action _complete)
            {
                if (m_enterSceneAdditionLoad_12.CheckShouldInvokeBase(this.instance))
                    base._enterSceneAdditionLoad(_complete);
                else
                    m_enterSceneAdditionLoad_12.Invoke(this.instance, _complete);
            }

            protected override void _onQuitScene()
            {
                if (m_onQuitScene_13.CheckShouldInvokeBase(this.instance))
                    base._onQuitScene();
                else
                    m_onQuitScene_13.Invoke(this.instance);
            }

            public override void hideScene()
            {
                if (mhideScene_14.CheckShouldInvokeBase(this.instance))
                    base.hideScene();
                else
                    mhideScene_14.Invoke(this.instance);
            }

            public override void hideScene(System.Action _delegate)
            {
                if (mhideScene_15.CheckShouldInvokeBase(this.instance))
                    base.hideScene(_delegate);
                else
                    mhideScene_15.Invoke(this.instance, _delegate);
            }

            public override void showScene()
            {
                if (mshowScene_16.CheckShouldInvokeBase(this.instance))
                    base.showScene();
                else
                    mshowScene_16.Invoke(this.instance);
            }

            public override void showScene(System.Action _delegate)
            {
                if (mshowScene_17.CheckShouldInvokeBase(this.instance))
                    base.showScene(_delegate);
                else
                    mshowScene_17.Invoke(this.instance, _delegate);
            }

            public override void enterAndShowScene()
            {
                if (menterAndShowScene_18.CheckShouldInvokeBase(this.instance))
                    base.enterAndShowScene();
                else
                    menterAndShowScene_18.Invoke(this.instance);
            }

            public override void enterAndShowScene(System.Action _delegate)
            {
                if (menterAndShowScene_19.CheckShouldInvokeBase(this.instance))
                    base.enterAndShowScene(_delegate);
                else
                    menterAndShowScene_19.Invoke(this.instance, _delegate);
            }

            public override void onSwitchHideScene()
            {
                monSwitchHideScene_21.Invoke(this.instance);
            }

            protected override void _onSceneInited()
            {
                m_onSceneInited_22.Invoke(this.instance);
            }

            protected override ALPackage._AALResourceCore _resourceCore
            {
            get
            {
                if (mget__resourceCore_0.CheckShouldInvokeBase(this.instance))
                    return base._resourceCore;
                else
                    return mget__resourceCore_0.Invoke(this.instance);

            }
            }

            public override global::SceneInfoRefObj sceneRefObj
            {
            get
            {
                return mget_sceneRefObj_5.Invoke(this.instance);

            }
            }

            public override System.Boolean needDiscardOnSwitch
            {
            get
            {
                return mget_needDiscardOnSwitch_20.Invoke(this.instance);

            }
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

