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
    public class _AGGUIHotfixBasicSubPrefabWndAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_AGGUIHotfixBasicSubPrefabWnd);
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

        public class Adapter : _AGGUIHotfixBasicSubPrefabWnd, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<ALPackage._AALResourceCore> mget__resourceCore_0 = new CrossBindingFunctionInfo<ALPackage._AALResourceCore>("get__resourceCore");
            CrossBindingFunctionInfo<UnityEngine.Transform> m_getParentTransForm_2 = new CrossBindingFunctionInfo<UnityEngine.Transform>("_getParentTransForm");
            CrossBindingFunctionInfo<UnityEngine.RectTransform> mget_rectTransform_3 = new CrossBindingFunctionInfo<UnityEngine.RectTransform>("get_rectTransform");
            CrossBindingFunctionInfo<System.Boolean> mget__isWndLoadSyn_4 = new CrossBindingFunctionInfo<System.Boolean>("get__isWndLoadSyn");
            CrossBindingMethodInfo m_loadOp_5 = new CrossBindingMethodInfo("_loadOp");
            CrossBindingFunctionInfo<UnityEngine.GameObject> mgetGameObj_6 = new CrossBindingFunctionInfo<UnityEngine.GameObject>("getGameObj");
            CrossBindingMethodInfo mshowWnd_7 = new CrossBindingMethodInfo("showWnd");
            CrossBindingMethodInfo<System.Action> mshowWnd_8 = new CrossBindingMethodInfo<System.Action>("showWnd");
            CrossBindingMethodInfo mshowWndWithoutAni_9 = new CrossBindingMethodInfo("showWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mshowWndWithoutAni_10 = new CrossBindingMethodInfo<System.Action>("showWndWithoutAni");
            CrossBindingMethodInfo mhideWnd_11 = new CrossBindingMethodInfo("hideWnd");
            CrossBindingMethodInfo<System.Action> mhideWnd_12 = new CrossBindingMethodInfo<System.Action>("hideWnd");
            CrossBindingMethodInfo mhideWndWithoutAni_13 = new CrossBindingMethodInfo("hideWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mhideWndWithoutAni_14 = new CrossBindingMethodInfo<System.Action>("hideWndWithoutAni");
            CrossBindingMethodInfo mresetWnd_15 = new CrossBindingMethodInfo("resetWnd");
            CrossBindingMethodInfo m_discard_16 = new CrossBindingMethodInfo("_discard");
            CrossBindingMethodInfo m_onShowWnd_17 = new CrossBindingMethodInfo("_onShowWnd");
            CrossBindingMethodInfo m_onHideWnd_18 = new CrossBindingMethodInfo("_onHideWnd");
            CrossBindingMethodInfo m_onReset_19 = new CrossBindingMethodInfo("_onReset");
            CrossBindingMethodInfo m_onDiscard_20 = new CrossBindingMethodInfo("_onDiscard");
            CrossBindingMethodInfo m_onWndInitDone_21 = new CrossBindingMethodInfo("_onWndInitDone");
            CrossBindingFunctionInfo<System.String> mget__monoAssetPath_22 = new CrossBindingFunctionInfo<System.String>("get__monoAssetPath");
            CrossBindingFunctionInfo<System.String> mget__monoObjName_23 = new CrossBindingFunctionInfo<System.String>("get__monoObjName");
            CrossBindingFunctionInfo<System.Boolean> mget_needDiscardOnSwitch_24 = new CrossBindingFunctionInfo<System.Boolean>("get_needDiscardOnSwitch");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter() : base(null)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance): base(null)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override UnityEngine.Transform _getParentTransForm()
            {
                if (m_getParentTransForm_2.CheckShouldInvokeBase(this.instance))
                    return base._getParentTransForm();
                else
                    return m_getParentTransForm_2.Invoke(this.instance);
            }

            protected override void _loadOp()
            {
                if (m_loadOp_5.CheckShouldInvokeBase(this.instance))
                    base._loadOp();
                else
                    m_loadOp_5.Invoke(this.instance);
            }

            public override UnityEngine.GameObject getGameObj()
            {
                if (mgetGameObj_6.CheckShouldInvokeBase(this.instance))
                    return base.getGameObj();
                else
                    return mgetGameObj_6.Invoke(this.instance);
            }

            public override void showWnd()
            {
                if (mshowWnd_7.CheckShouldInvokeBase(this.instance))
                    base.showWnd();
                else
                    mshowWnd_7.Invoke(this.instance);
            }

            public override void showWnd(System.Action _delayDoneAction)
            {
                if (mshowWnd_8.CheckShouldInvokeBase(this.instance))
                    base.showWnd(_delayDoneAction);
                else
                    mshowWnd_8.Invoke(this.instance, _delayDoneAction);
            }

            public override void showWndWithoutAni()
            {
                if (mshowWndWithoutAni_9.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni();
                else
                    mshowWndWithoutAni_9.Invoke(this.instance);
            }

            public override void showWndWithoutAni(System.Action _doneAction)
            {
                if (mshowWndWithoutAni_10.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni(_doneAction);
                else
                    mshowWndWithoutAni_10.Invoke(this.instance, _doneAction);
            }

            public override void hideWnd()
            {
                if (mhideWnd_11.CheckShouldInvokeBase(this.instance))
                    base.hideWnd();
                else
                    mhideWnd_11.Invoke(this.instance);
            }

            public override void hideWnd(System.Action _delayDoneAction)
            {
                if (mhideWnd_12.CheckShouldInvokeBase(this.instance))
                    base.hideWnd(_delayDoneAction);
                else
                    mhideWnd_12.Invoke(this.instance, _delayDoneAction);
            }

            public override void hideWndWithoutAni()
            {
                if (mhideWndWithoutAni_13.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni();
                else
                    mhideWndWithoutAni_13.Invoke(this.instance);
            }

            public override void hideWndWithoutAni(System.Action _doneAction)
            {
                if (mhideWndWithoutAni_14.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni(_doneAction);
                else
                    mhideWndWithoutAni_14.Invoke(this.instance, _doneAction);
            }

            public override void resetWnd()
            {
                if (mresetWnd_15.CheckShouldInvokeBase(this.instance))
                    base.resetWnd();
                else
                    mresetWnd_15.Invoke(this.instance);
            }

            protected override void _discard()
            {
                if (m_discard_16.CheckShouldInvokeBase(this.instance))
                    base._discard();
                else
                    m_discard_16.Invoke(this.instance);
            }

            protected override void _onShowWnd()
            {
                m_onShowWnd_17.Invoke(this.instance);
            }

            protected override void _onHideWnd()
            {
                m_onHideWnd_18.Invoke(this.instance);
            }

            protected override void _onReset()
            {
                m_onReset_19.Invoke(this.instance);
            }

            protected override void _onDiscard()
            {
                m_onDiscard_20.Invoke(this.instance);
            }

            protected override void _onWndInitDone()
            {
                m_onWndInitDone_21.Invoke(this.instance);
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

            public override UnityEngine.RectTransform rectTransform
            {
            get
            {
                if (mget_rectTransform_3.CheckShouldInvokeBase(this.instance))
                    return base.rectTransform;
                else
                    return mget_rectTransform_3.Invoke(this.instance);

            }
            }

            protected override System.Boolean _isWndLoadSyn
            {
            get
            {
                if (mget__isWndLoadSyn_4.CheckShouldInvokeBase(this.instance))
                    return base._isWndLoadSyn;
                else
                    return mget__isWndLoadSyn_4.Invoke(this.instance);

            }
            }

            protected override System.String _monoAssetPath
            {
            get
            {
                return mget__monoAssetPath_22.Invoke(this.instance);

            }
            }

            protected override System.String _monoObjName
            {
            get
            {
                return mget__monoObjName_23.Invoke(this.instance);

            }
            }

            public override System.Boolean needDiscardOnSwitch
            {
            get
            {
                if (mget_needDiscardOnSwitch_24.CheckShouldInvokeBase(this.instance))
                    return base.needDiscardOnSwitch;
                else
                    return mget_needDiscardOnSwitch_24.Invoke(this.instance);

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

