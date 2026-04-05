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
    public class _ABasicAdditionTDSceneAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_ABasicAdditionTDScene);
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

        public class Adapter : _ABasicAdditionTDScene, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo m_onEnterScene_0 = new CrossBindingMethodInfo("_onEnterScene");
            CrossBindingFunctionInfo<ALPackage._AALResourceCore> mget__resourceCore_1 = new CrossBindingFunctionInfo<ALPackage._AALResourceCore>("get__resourceCore");
            CrossBindingMethodInfo m_onQuitTDScene_2 = new CrossBindingMethodInfo("_onQuitTDScene");
            CrossBindingMethodInfo<UnityEngine.GameObject> m_onRootGOLoaded_3 = new CrossBindingMethodInfo<UnityEngine.GameObject>("_onRootGOLoaded");
            CrossBindingFunctionInfo<global::BasicResIndexInfo> m_getDefaultSceneIndex_4 = new CrossBindingFunctionInfo<global::BasicResIndexInfo>("_getDefaultSceneIndex");
            CrossBindingMethodInfo<System.Action> m_enterSceneAdditionLoad_5 = new CrossBindingMethodInfo<System.Action>("_enterSceneAdditionLoad");
            CrossBindingMethodInfo m_onQuitScene_6 = new CrossBindingMethodInfo("_onQuitScene");
            CrossBindingMethodInfo mhideScene_7 = new CrossBindingMethodInfo("hideScene");
            CrossBindingMethodInfo<System.Action> mhideScene_8 = new CrossBindingMethodInfo<System.Action>("hideScene");
            CrossBindingMethodInfo mshowScene_9 = new CrossBindingMethodInfo("showScene");
            CrossBindingMethodInfo<System.Action> mshowScene_10 = new CrossBindingMethodInfo<System.Action>("showScene");
            CrossBindingMethodInfo menterAndShowScene_11 = new CrossBindingMethodInfo("enterAndShowScene");
            CrossBindingMethodInfo<System.Action> menterAndShowScene_12 = new CrossBindingMethodInfo<System.Action>("enterAndShowScene");
            CrossBindingFunctionInfo<System.Boolean> mget_needDiscardOnSwitch_13 = new CrossBindingFunctionInfo<System.Boolean>("get_needDiscardOnSwitch");
            CrossBindingMethodInfo<System.Action> m_dealShowScene_14 = new CrossBindingMethodInfo<System.Action>("_dealShowScene");
            CrossBindingMethodInfo<System.Action> m_dealHideScene_15 = new CrossBindingMethodInfo<System.Action>("_dealHideScene");
            CrossBindingMethodInfo monSwitchHideScene_16 = new CrossBindingMethodInfo("onSwitchHideScene");
            CrossBindingMethodInfo m_onSceneInited_17 = new CrossBindingMethodInfo("_onSceneInited");

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

            protected override void _onEnterScene()
            {
                if (m_onEnterScene_0.CheckShouldInvokeBase(this.instance))
                    base._onEnterScene();
                else
                    m_onEnterScene_0.Invoke(this.instance);
            }

            protected override void _onQuitTDScene()
            {
                m_onQuitTDScene_2.Invoke(this.instance);
            }

            protected override void _onRootGOLoaded(UnityEngine.GameObject _g0)
            {
                m_onRootGOLoaded_3.Invoke(this.instance, _g0);
            }

            protected override global::BasicResIndexInfo _getDefaultSceneIndex()
            {
                if (m_getDefaultSceneIndex_4.CheckShouldInvokeBase(this.instance))
                    return base._getDefaultSceneIndex();
                else
                    return m_getDefaultSceneIndex_4.Invoke(this.instance);
            }

            protected override void _enterSceneAdditionLoad(System.Action _complete)
            {
                if (m_enterSceneAdditionLoad_5.CheckShouldInvokeBase(this.instance))
                    base._enterSceneAdditionLoad(_complete);
                else
                    m_enterSceneAdditionLoad_5.Invoke(this.instance, _complete);
            }

            protected override void _onQuitScene()
            {
                if (m_onQuitScene_6.CheckShouldInvokeBase(this.instance))
                    base._onQuitScene();
                else
                    m_onQuitScene_6.Invoke(this.instance);
            }

            public override void hideScene()
            {
                if (mhideScene_7.CheckShouldInvokeBase(this.instance))
                    base.hideScene();
                else
                    mhideScene_7.Invoke(this.instance);
            }

            public override void hideScene(System.Action _delegate)
            {
                if (mhideScene_8.CheckShouldInvokeBase(this.instance))
                    base.hideScene(_delegate);
                else
                    mhideScene_8.Invoke(this.instance, _delegate);
            }

            public override void showScene()
            {
                if (mshowScene_9.CheckShouldInvokeBase(this.instance))
                    base.showScene();
                else
                    mshowScene_9.Invoke(this.instance);
            }

            public override void showScene(System.Action _delegate)
            {
                if (mshowScene_10.CheckShouldInvokeBase(this.instance))
                    base.showScene(_delegate);
                else
                    mshowScene_10.Invoke(this.instance, _delegate);
            }

            public override void enterAndShowScene()
            {
                if (menterAndShowScene_11.CheckShouldInvokeBase(this.instance))
                    base.enterAndShowScene();
                else
                    menterAndShowScene_11.Invoke(this.instance);
            }

            public override void enterAndShowScene(System.Action _delegate)
            {
                if (menterAndShowScene_12.CheckShouldInvokeBase(this.instance))
                    base.enterAndShowScene(_delegate);
                else
                    menterAndShowScene_12.Invoke(this.instance, _delegate);
            }

            public override void _dealShowScene(System.Action _delegate)
            {
                m_dealShowScene_14.Invoke(this.instance, _delegate);
            }

            public override void _dealHideScene(System.Action _delegate)
            {
                m_dealHideScene_15.Invoke(this.instance, _delegate);
            }

            public override void onSwitchHideScene()
            {
                monSwitchHideScene_16.Invoke(this.instance);
            }

            protected override void _onSceneInited()
            {
                m_onSceneInited_17.Invoke(this.instance);
            }

            protected override ALPackage._AALResourceCore _resourceCore
            {
            get
            {
                return mget__resourceCore_1.Invoke(this.instance);

            }
            }

            public override System.Boolean needDiscardOnSwitch
            {
            get
            {
                return mget_needDiscardOnSwitch_13.Invoke(this.instance);

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

