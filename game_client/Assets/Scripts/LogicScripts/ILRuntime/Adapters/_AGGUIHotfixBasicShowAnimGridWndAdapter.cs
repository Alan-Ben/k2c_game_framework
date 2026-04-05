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
    public class _AGGUIHotfixBasicShowAnimGridWndAdapter<T> : CrossBindingAdaptor where T : _AGGUIHotfixBasicGridItemWndAdapter.Adapter
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_AGGUIHotfixBasicShowAnimGridWnd<T>);
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

        public class Adapter : _AGGUIHotfixBasicShowAnimGridWnd<T>, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo<System.Action> mshowWnd_0 = new CrossBindingMethodInfo<System.Action>("showWnd");
            CrossBindingMethodInfo<System.Action> mhideWnd_1 = new CrossBindingMethodInfo<System.Action>("hideWnd");
            CrossBindingMethodInfo m_onFrameRefresh_2 = new CrossBindingMethodInfo("_onFrameRefresh");
            // CrossBindingMethodInfo<T, System.Int32> m_onRefreshItemWnd_3 = new CrossBindingMethodInfo<T, System.Int32>("_onRefreshItemWnd");
            CrossBindingMethodInfo mshowWnd_4 = new CrossBindingMethodInfo("showWnd");
            CrossBindingMethodInfo mhideWnd_5 = new CrossBindingMethodInfo("hideWnd");
            CrossBindingMethodInfo m_initWnd_6 = new CrossBindingMethodInfo("_initWnd");
            CrossBindingMethodInfo mresetWnd_7 = new CrossBindingMethodInfo("resetWnd");
            CrossBindingMethodInfo mdiscard_8 = new CrossBindingMethodInfo("discard");
            // CrossBindingFunctionInfo<NP.NPGGUIHotfixCommonMono, T> m_createItemWnd_9 = new CrossBindingFunctionInfo<NP.NPGGUIHotfixCommonMono, T>("_createItemWnd");
            CrossBindingMethodInfo mshowWndWithoutAni_10 = new CrossBindingMethodInfo("showWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mshowWndWithoutAni_11 = new CrossBindingMethodInfo<System.Action>("showWndWithoutAni");
            CrossBindingMethodInfo mhideWndWithoutAni_12 = new CrossBindingMethodInfo("hideWndWithoutAni");
            CrossBindingMethodInfo<System.Action> mhideWndWithoutAni_13 = new CrossBindingMethodInfo<System.Action>("hideWndWithoutAni");
            CrossBindingMethodInfo m_onShowWnd_14 = new CrossBindingMethodInfo("_onShowWnd");
            CrossBindingMethodInfo m_onHideWnd_15 = new CrossBindingMethodInfo("_onHideWnd");
            CrossBindingMethodInfo m_onReset_16 = new CrossBindingMethodInfo("_onReset");
            CrossBindingMethodInfo m_onDiscard_17 = new CrossBindingMethodInfo("_onDiscard");
            CrossBindingMethodInfo m_onWndInitDone_18 = new CrossBindingMethodInfo("_onWndInitDone");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;
            //缓存这个数组来避免调用时的GC Alloc
            object[] param1 = new object[1];
            object[] param2 = new object[2];

            public Adapter() : base(null)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance) : base(null)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void showWnd(System.Action _delegate)
            {
                if (mshowWnd_0.CheckShouldInvokeBase(this.instance))
                    base.showWnd(_delegate);
                else
                    mshowWnd_0.Invoke(this.instance, _delegate);
            }

            public override void hideWnd(System.Action _delegate)
            {
                if (mhideWnd_1.CheckShouldInvokeBase(this.instance))
                    base.hideWnd(_delegate);
                else
                    mhideWnd_1.Invoke(this.instance, _delegate);
            }

            protected override void _onFrameRefresh()
            {
                if (m_onFrameRefresh_2.CheckShouldInvokeBase(this.instance))
                    base._onFrameRefresh();
                else
                    m_onFrameRefresh_2.Invoke(this.instance);
            }


            bool _m_onRefreshItemWndGot;
            IMethod _m_onRefreshItemWnd;
            bool onRefreshItemWndInvoking;
            protected override void _onRefreshItemWnd(T _itemMono, System.Int32 _itemIdx)
            {
                if (!_m_onRefreshItemWndGot)
                {
                    _m_onRefreshItemWnd = instance.Type.GetMethod("_onRefreshItemWnd", 2);
                    _m_onRefreshItemWndGot = true;
                }
                //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
                if (_m_onRefreshItemWnd != null && !onRefreshItemWndInvoking)
                {
                    onRefreshItemWndInvoking = true;
                    param2[0] = _itemMono;
                    param2[1] = _itemIdx;
                    appdomain.Invoke(_m_onRefreshItemWnd, instance, param2);
                    param2[0] = null;
                    param2[1] = null;
                    onRefreshItemWndInvoking = false;
                }
            }

            public override void showWnd()
            {
                if (mshowWnd_4.CheckShouldInvokeBase(this.instance))
                    base.showWnd();
                else
                    mshowWnd_4.Invoke(this.instance);
            }

            public override void hideWnd()
            {
                if (mhideWnd_5.CheckShouldInvokeBase(this.instance))
                    base.hideWnd();
                else
                    mhideWnd_5.Invoke(this.instance);
            }

            protected override void _initWnd()
            {
                if (m_initWnd_6.CheckShouldInvokeBase(this.instance))
                    base._initWnd();
                else
                    m_initWnd_6.Invoke(this.instance);
            }

            public override void resetWnd()
            {
                if (mresetWnd_7.CheckShouldInvokeBase(this.instance))
                    base.resetWnd();
                else
                    mresetWnd_7.Invoke(this.instance);
            }

            public override void discard()
            {
                if (mdiscard_8.CheckShouldInvokeBase(this.instance))
                    base.discard();
                else
                    mdiscard_8.Invoke(this.instance);
            }

            bool _m_createItemWndGot;
            IMethod _m_createItemWnd;
            bool createItemWndInvoking;
            protected override T _createItemWnd(GGUIHotfixCommonMono _itemMono)
            {
                if (!_m_createItemWndGot)
                {
                    _m_createItemWnd = instance.Type.GetMethod("_createItemWnd", 1);
                    _m_createItemWndGot = true;
                }
                //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
                if (_m_createItemWnd != null && !createItemWndInvoking)
                {
                    createItemWndInvoking = true;
                    param1[0] = _itemMono;
                    object obj = appdomain.Invoke(_m_createItemWnd, instance, param1);
                    param1[0] = null;
                    createItemWndInvoking = false;
                    return (T)obj;
                }

                return default(T);
            }

            public override void showWndWithoutAni()
            {
                if (mshowWndWithoutAni_10.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni();
                else
                    mshowWndWithoutAni_10.Invoke(this.instance);
            }

            public override void showWndWithoutAni(System.Action _doneAction)
            {
                if (mshowWndWithoutAni_11.CheckShouldInvokeBase(this.instance))
                    base.showWndWithoutAni(_doneAction);
                else
                    mshowWndWithoutAni_11.Invoke(this.instance, _doneAction);
            }

            public override void hideWndWithoutAni()
            {
                if (mhideWndWithoutAni_12.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni();
                else
                    mhideWndWithoutAni_12.Invoke(this.instance);
            }

            public override void hideWndWithoutAni(System.Action _doneAction)
            {
                if (mhideWndWithoutAni_13.CheckShouldInvokeBase(this.instance))
                    base.hideWndWithoutAni(_doneAction);
                else
                    mhideWndWithoutAni_13.Invoke(this.instance, _doneAction);
            }

            protected override void _onShowWnd()
            {
                m_onShowWnd_14.Invoke(this.instance);
            }

            protected override void _onHideWnd()
            {
                m_onHideWnd_15.Invoke(this.instance);
            }

            protected override void _onReset()
            {
                m_onReset_16.Invoke(this.instance);
            }

            protected override void _onDiscard()
            {
                m_onDiscard_17.Invoke(this.instance);
            }

            protected override void _onWndInitDone()
            {
                m_onWndInitDone_18.Invoke(this.instance);
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

