using System;
using ALPackage;
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
    public class _AGGUIHotfixBasicWndAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_AGGUIHotfixBasicWnd);
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

        public class Adapter : _AGGUIHotfixBasicWnd, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<ALPackage._AALResourceCore> mget__resourceCore_0 = new CrossBindingFunctionInfo<ALPackage._AALResourceCore>("get__resourceCore");
            CrossBindingMethodInfo mhideUI_2 = new CrossBindingMethodInfo("hideUI");
            CrossBindingMethodInfo mshowUI_3 = new CrossBindingMethodInfo("showUI");
            CrossBindingFunctionInfo<UnityEngine.Transform> m_getParentTransForm_4 = new CrossBindingFunctionInfo<UnityEngine.Transform>("_getParentTransForm");
            CrossBindingFunctionInfo<UnityEngine.RectTransform> mget_rectTransform_5 = new CrossBindingFunctionInfo<UnityEngine.RectTransform>("get_rectTransform");
            CrossBindingFunctionInfo<System.Boolean> mget__isWndLoadSyn_6 = new CrossBindingFunctionInfo<System.Boolean>("get__isWndLoadSyn");
            CrossBindingMethodInfo m_loadOp_7 = new CrossBindingMethodInfo("_loadOp");
            CrossBindingFunctionInfo<UnityEngine.GameObject> mgetGameObj_8 = new CrossBindingFunctionInfo<UnityEngine.GameObject>("getGameObj");
            CrossBindingMethodInfo mshowWnd_9 = new CrossBindingMethodInfo("showWnd");
            CrossBindingMethodInfo<System.Action> mshowWnd_10 = new CrossBindingMethodInfo<System.Action>("showWnd");
            CrossBindingMethodInfo mshowWndWithoutAni_11 = new CrossBindingMethodInfo("showWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mshowWndWithoutAni_12 = new CrossBindingMethodInfo<System.Action>("showWndWithoutAni");
            CrossBindingMethodInfo mhideWnd_13 = new CrossBindingMethodInfo("hideWnd");
            CrossBindingMethodInfo<System.Action> mhideWnd_14 = new CrossBindingMethodInfo<System.Action>("hideWnd");
            CrossBindingMethodInfo mhideWndWithoutAni_15 = new CrossBindingMethodInfo("hideWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mhideWndWithoutAni_16 = new CrossBindingMethodInfo<System.Action>("hideWndWithoutAni");
            CrossBindingMethodInfo mresetWnd_17 = new CrossBindingMethodInfo("resetWnd");
            CrossBindingMethodInfo m_discard_18 = new CrossBindingMethodInfo("_discard");
            CrossBindingMethodInfo m_onShowWnd_19 = new CrossBindingMethodInfo("_onShowWnd");
            CrossBindingMethodInfo m_onHideWnd_20 = new CrossBindingMethodInfo("_onHideWnd");
            CrossBindingMethodInfo m_onReset_21 = new CrossBindingMethodInfo("_onReset");
            CrossBindingMethodInfo m_onDiscard_22 = new CrossBindingMethodInfo("_onDiscard");
            CrossBindingMethodInfo m_onWndInitDone_23 = new CrossBindingMethodInfo("_onWndInitDone");
            CrossBindingFunctionInfo<System.String> mget__monoAssetPath_24 = new CrossBindingFunctionInfo<System.String>("get__monoAssetPath");
            CrossBindingFunctionInfo<System.String> mget__monoObjName_25 = new CrossBindingFunctionInfo<System.String>("get__monoObjName");
            CrossBindingFunctionInfo<System.Boolean> mget_needDiscardOnSwitch_26 = new CrossBindingFunctionInfo<System.Boolean>("get_needDiscardOnSwitch");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter():base(EALUIWndLayer.NORMAL)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance):base(EALUIWndLayer.NORMAL)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void hideUI()
            {
                if (mhideUI_2.CheckShouldInvokeBase(this.instance))
                    base.hideUI();
                else
                    mhideUI_2.Invoke(this.instance);
            }

            public override void showUI()
            {
                if (mshowUI_3.CheckShouldInvokeBase(this.instance))
                    base.showUI();
                else
                    mshowUI_3.Invoke(this.instance);
            }

            protected override UnityEngine.Transform _getParentTransForm()
            {
                if (m_getParentTransForm_4.CheckShouldInvokeBase(this.instance))
                    return base._getParentTransForm();
                else
                    return m_getParentTransForm_4.Invoke(this.instance);
            }

            protected override void _loadOp()
            {
                if (m_loadOp_7.CheckShouldInvokeBase(this.instance))
                    base._loadOp();
                else
                    m_loadOp_7.Invoke(this.instance);
            }

            public override UnityEngine.GameObject getGameObj()
            {
                if (mgetGameObj_8.CheckShouldInvokeBase(this.instance))
                    return base.getGameObj();
                else
                    return mgetGameObj_8.Invoke(this.instance);
            }

            public override void showWnd()
            {
                if (mshowWnd_9.CheckShouldInvokeBase(this.instance))
                    base.showWnd();
                else
                    mshowWnd_9.Invoke(this.instance);
            }

            public override void showWnd(System.Action _delayDoneAction)
            {
                if (mshowWnd_10.CheckShouldInvokeBase(this.instance))
                    base.showWnd(_delayDoneAction);
                else
                    mshowWnd_10.Invoke(this.instance, _delayDoneAction);
            }

            public override void showWndWithoutAni()
            {
                if (mshowWndWithoutAni_11.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni();
                else
                    mshowWndWithoutAni_11.Invoke(this.instance);
            }

            public override void showWndWithoutAni(System.Action _doneAction)
            {
                if (mshowWndWithoutAni_12.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni(_doneAction);
                else
                    mshowWndWithoutAni_12.Invoke(this.instance, _doneAction);
            }

            public override void hideWnd()
            {
                if (mhideWnd_13.CheckShouldInvokeBase(this.instance))
                    base.hideWnd();
                else
                    mhideWnd_13.Invoke(this.instance);
            }

            public override void hideWnd(System.Action _delayDoneAction)
            {
                if (mhideWnd_14.CheckShouldInvokeBase(this.instance))
                    base.hideWnd(_delayDoneAction);
                else
                    mhideWnd_14.Invoke(this.instance, _delayDoneAction);
            }

            public override void hideWndWithoutAni()
            {
                if (mhideWndWithoutAni_15.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni();
                else
                    mhideWndWithoutAni_15.Invoke(this.instance);
            }

            public override void hideWndWithoutAni(System.Action _doneAction)
            {
                if (mhideWndWithoutAni_16.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni(_doneAction);
                else
                    mhideWndWithoutAni_16.Invoke(this.instance, _doneAction);
            }

            public override void resetWnd()
            {
                if (mresetWnd_17.CheckShouldInvokeBase(this.instance))
                    base.resetWnd();
                else
                    mresetWnd_17.Invoke(this.instance);
            }

            protected override void _discard()
            {
                if (m_discard_18.CheckShouldInvokeBase(this.instance))
                    base._discard();
                else
                    m_discard_18.Invoke(this.instance);
            }

            protected override void _onShowWnd()
            {
                m_onShowWnd_19.Invoke(this.instance);
            }

            protected override void _onHideWnd()
            {
                m_onHideWnd_20.Invoke(this.instance);
            }

            protected override void _onReset()
            {
                m_onReset_21.Invoke(this.instance);
            }

            protected override void _onDiscard()
            {
                m_onDiscard_22.Invoke(this.instance);
            }

            protected override void _onWndInitDone()
            {
                m_onWndInitDone_23.Invoke(this.instance);
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
                if (mget_rectTransform_5.CheckShouldInvokeBase(this.instance))
                    return base.rectTransform;
                else
                    return mget_rectTransform_5.Invoke(this.instance);

            }
            }

            protected override System.Boolean _isWndLoadSyn
            {
            get
            {
                if (mget__isWndLoadSyn_6.CheckShouldInvokeBase(this.instance))
                    return base._isWndLoadSyn;
                else
                    return mget__isWndLoadSyn_6.Invoke(this.instance);

            }
            }

            protected override System.String _monoAssetPath
            {
            get
            {
                return mget__monoAssetPath_24.Invoke(this.instance);

            }
            }

            protected override System.String _monoObjName
            {
            get
            {
                return mget__monoObjName_25.Invoke(this.instance);

            }
            }

            public override System.Boolean needDiscardOnSwitch
            {
            get
            {
                if (mget_needDiscardOnSwitch_26.CheckShouldInvokeBase(this.instance))
                    return base.needDiscardOnSwitch;
                else
                    return mget_needDiscardOnSwitch_26.Invoke(this.instance);

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

