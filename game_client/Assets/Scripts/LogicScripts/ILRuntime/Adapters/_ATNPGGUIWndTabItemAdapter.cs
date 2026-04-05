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
    public class _ATNPGGUIWndTabItemAdapter<T> : CrossBindingAdaptor where T : NPGGUIMonoCommonTab
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_ATNPGGUIWndTabItem<T>);//这是你想继承的那个类
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

        public class Adapter : _ATNPGGUIWndTabItem<T>, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo m_onShowWnd_0 = new CrossBindingMethodInfo("_onShowWnd");
            CrossBindingMethodInfo m_onHideWnd_1 = new CrossBindingMethodInfo("_onHideWnd");
            CrossBindingMethodInfo m_onReset_2 = new CrossBindingMethodInfo("_onReset");
            CrossBindingMethodInfo m_onDiscard_3 = new CrossBindingMethodInfo("_onDiscard");
            CrossBindingMethodInfo m_onWndInitDone_4 = new CrossBindingMethodInfo("_onWndInitDone");
            CrossBindingMethodInfo<System.Boolean> msetSelected_5 = new CrossBindingMethodInfo<System.Boolean>("setSelected");
            CrossBindingMethodInfo<UnityEngine.GameObject> m_onClickSelectButton_6 = new CrossBindingMethodInfo<UnityEngine.GameObject>("_onClickSelectButton");
            CrossBindingMethodInfo mshowWnd_7 = new CrossBindingMethodInfo("showWnd");
            CrossBindingMethodInfo mshowWndWithoutAni_8 = new CrossBindingMethodInfo("showWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mshowWnd_9 = new CrossBindingMethodInfo<System.Action>("showWnd");
            CrossBindingMethodInfo mhideWnd_10 = new CrossBindingMethodInfo("hideWnd");
            CrossBindingMethodInfo mhideWndWithoutAni_11 = new CrossBindingMethodInfo("hideWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mhideWnd_12 = new CrossBindingMethodInfo<System.Action>("hideWnd");
            // CrossBindingMethodInfo m_initWnd_13 = new CrossBindingMethodInfo("_initWnd");
            CrossBindingMethodInfo mresetWnd_14 = new CrossBindingMethodInfo("resetWnd");
            CrossBindingMethodInfo mdiscard_15 = new CrossBindingMethodInfo("discard");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter() : base(null)
            {
                initWnd();
            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance) : base(null)
            {
                this.appdomain = appdomain;
                this.instance = instance;
                initWnd();
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override void _onShowWnd()
            {
                if (m_onShowWnd_0.CheckShouldInvokeBase(this.instance))
                    base._onShowWnd();
                else
                    m_onShowWnd_0.Invoke(this.instance);
            }

            protected override void _onHideWnd()
            {
                if (m_onHideWnd_1.CheckShouldInvokeBase(this.instance))
                    base._onHideWnd();
                else
                    m_onHideWnd_1.Invoke(this.instance);
            }

            protected override void _onReset()
            {
                if (m_onReset_2.CheckShouldInvokeBase(this.instance))
                    base._onReset();
                else
                    m_onReset_2.Invoke(this.instance);
            }

            protected override void _onDiscard()
            {
                if (m_onDiscard_3.CheckShouldInvokeBase(this.instance))
                    base._onDiscard();
                else
                    m_onDiscard_3.Invoke(this.instance);
            }

            protected override void _onWndInitDone()
            {
                if (m_onWndInitDone_4.CheckShouldInvokeBase(this.instance))
                    base._onWndInitDone();
                else
                    m_onWndInitDone_4.Invoke(this.instance);
            }

            public override void setSelected(System.Boolean _isSelect)
            {
                if (msetSelected_5.CheckShouldInvokeBase(this.instance))
                    base.setSelected(_isSelect);
                else
                    msetSelected_5.Invoke(this.instance, _isSelect);
            }

            protected override void _onClickSelectButton(UnityEngine.GameObject _go)
            {
                if (m_onClickSelectButton_6.CheckShouldInvokeBase(this.instance))
                    base._onClickSelectButton(_go);
                else
                    m_onClickSelectButton_6.Invoke(this.instance, _go);
            }

            public override void showWnd()
            {
                if (mshowWnd_7.CheckShouldInvokeBase(this.instance))
                    base.showWnd();
                else
                    mshowWnd_7.Invoke(this.instance);
            }

            public override void showWndWithoutAni()
            {
                if (mshowWndWithoutAni_8.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni();
                else
                    mshowWndWithoutAni_8.Invoke(this.instance);
            }

            public override void showWnd(System.Action _doneAction)
            {
                if (mshowWnd_9.CheckShouldInvokeBase(this.instance))
                    base.showWnd(_doneAction);
                else
                    mshowWnd_9.Invoke(this.instance, _doneAction);
            }

            public override void hideWnd()
            {
                if (mhideWnd_10.CheckShouldInvokeBase(this.instance))
                    base.hideWnd();
                else
                    mhideWnd_10.Invoke(this.instance);
            }

            public override void hideWndWithoutAni()
            {
                if (mhideWndWithoutAni_11.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni();
                else
                    mhideWndWithoutAni_11.Invoke(this.instance);
            }

            public override void hideWnd(System.Action _doneAction)
            {
                if (mhideWnd_12.CheckShouldInvokeBase(this.instance))
                    base.hideWnd(_doneAction);
                else
                    mhideWnd_12.Invoke(this.instance, _doneAction);
            }

            // 这里不需要重写，否则instance还未被赋值就会被调用，导致报错
            // protected override void _initWnd()
            // {
            //     if (m_initWnd_13.CheckShouldInvokeBase(this.instance))
            //         base._initWnd();
            //     else
            //         m_initWnd_13.Invoke(this.instance);
            // }

            public override void resetWnd()
            {
                if (mresetWnd_14.CheckShouldInvokeBase(this.instance))
                    base.resetWnd();
                else
                    mresetWnd_14.Invoke(this.instance);
            }

            public override void discard()
            {
                if (mdiscard_15.CheckShouldInvokeBase(this.instance))
                    base.discard();
                else
                    mdiscard_15.Invoke(this.instance);
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

