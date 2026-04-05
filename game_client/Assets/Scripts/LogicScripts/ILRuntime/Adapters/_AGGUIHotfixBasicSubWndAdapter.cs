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
    public class _AGGUIHotfixBasicSubWndAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_AGGUIHotfixBasicSubWnd);
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

        public class Adapter : _AGGUIHotfixBasicSubWnd, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo mshowWnd_0 = new CrossBindingMethodInfo("showWnd");
            CrossBindingMethodInfo<System.Action> mshowWnd_1 = new CrossBindingMethodInfo<System.Action>("showWnd");
            CrossBindingMethodInfo mshowWndWithoutAni_2 = new CrossBindingMethodInfo("showWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mshowWndWithoutAni_3 = new CrossBindingMethodInfo<System.Action>("showWndWithoutAni");
            CrossBindingMethodInfo mhideWnd_4 = new CrossBindingMethodInfo("hideWnd");
            CrossBindingMethodInfo<System.Action> mhideWnd_5 = new CrossBindingMethodInfo<System.Action>("hideWnd");
            CrossBindingMethodInfo mhideWndWithoutAni_6 = new CrossBindingMethodInfo("hideWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mhideWndWithoutAni_7 = new CrossBindingMethodInfo<System.Action>("hideWndWithoutAni");
            CrossBindingMethodInfo m_initWnd_8 = new CrossBindingMethodInfo("_initWnd");
            CrossBindingMethodInfo mresetWnd_9 = new CrossBindingMethodInfo("resetWnd");
            CrossBindingMethodInfo mdiscard_10 = new CrossBindingMethodInfo("discard");
            CrossBindingMethodInfo m_onShowWnd_11 = new CrossBindingMethodInfo("_onShowWnd");
            CrossBindingMethodInfo m_onHideWnd_12 = new CrossBindingMethodInfo("_onHideWnd");
            CrossBindingMethodInfo m_onReset_13 = new CrossBindingMethodInfo("_onReset");
            CrossBindingMethodInfo m_onDiscard_14 = new CrossBindingMethodInfo("_onDiscard");
            CrossBindingMethodInfo m_onWndInitDone_15 = new CrossBindingMethodInfo("_onWndInitDone");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter(): base(null)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance): base(null)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void showWnd()
            {
                if (mshowWnd_0.CheckShouldInvokeBase(this.instance))
                    base.showWnd();
                else
                    mshowWnd_0.Invoke(this.instance);
            }

            public override void showWnd(System.Action _delayDoneAction)
            {
                if (mshowWnd_1.CheckShouldInvokeBase(this.instance))
                    base.showWnd(_delayDoneAction);
                else
                    mshowWnd_1.Invoke(this.instance, _delayDoneAction);
            }

            public override void showWndWithoutAni()
            {
                if (mshowWndWithoutAni_2.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni();
                else
                    mshowWndWithoutAni_2.Invoke(this.instance);
            }

            public override void showWndWithoutAni(System.Action _doneAction)
            {
                if (mshowWndWithoutAni_3.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni(_doneAction);
                else
                    mshowWndWithoutAni_3.Invoke(this.instance, _doneAction);
            }

            public override void hideWnd()
            {
                if (mhideWnd_4.CheckShouldInvokeBase(this.instance))
                    base.hideWnd();
                else
                    mhideWnd_4.Invoke(this.instance);
            }

            public override void hideWnd(System.Action _delayDoneAction)
            {
                if (mhideWnd_5.CheckShouldInvokeBase(this.instance))
                    base.hideWnd(_delayDoneAction);
                else
                    mhideWnd_5.Invoke(this.instance, _delayDoneAction);
            }

            public override void hideWndWithoutAni()
            {
                if (mhideWndWithoutAni_6.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni();
                else
                    mhideWndWithoutAni_6.Invoke(this.instance);
            }

            public override void hideWndWithoutAni(System.Action _doneAction)
            {
                if (mhideWndWithoutAni_7.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni(_doneAction);
                else
                    mhideWndWithoutAni_7.Invoke(this.instance, _doneAction);
            }

            protected override void _initWnd()
            {
                if (m_initWnd_8.CheckShouldInvokeBase(this.instance))
                    base._initWnd();
                else
                    m_initWnd_8.Invoke(this.instance);
            }

            public override void resetWnd()
            {
                if (mresetWnd_9.CheckShouldInvokeBase(this.instance))
                    base.resetWnd();
                else
                    mresetWnd_9.Invoke(this.instance);
            }

            public override void discard()
            {
                if (mdiscard_10.CheckShouldInvokeBase(this.instance))
                    base.discard();
                else
                    mdiscard_10.Invoke(this.instance);
            }

            protected override void _onShowWnd()
            {
                m_onShowWnd_11.Invoke(this.instance);
            }

            protected override void _onHideWnd()
            {
                m_onHideWnd_12.Invoke(this.instance);
            }

            protected override void _onReset()
            {
                m_onReset_13.Invoke(this.instance);
            }

            protected override void _onDiscard()
            {
                m_onDiscard_14.Invoke(this.instance);
            }

            protected override void _onWndInitDone()
            {
                m_onWndInitDone_15.Invoke(this.instance);
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

