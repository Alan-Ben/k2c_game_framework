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
    public class _AActivityMainCityPushNoticeAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE._AActivityMainCityPushNotice);
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

        public class Adapter : GOE._AActivityMainCityPushNotice, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.Boolean> mget__isEnable_0 = new CrossBindingFunctionInfo<System.Boolean>("get__isEnable");
            CrossBindingMethodInfo m__onDealerDone_1 = new CrossBindingMethodInfo("__onDealerDone");
            CrossBindingMethodInfo mshowNotice_2 = new CrossBindingMethodInfo("showNotice");
            CrossBindingMethodInfo m_onGotoOtherMainViewNode_3 = new CrossBindingMethodInfo("_onGotoOtherMainViewNode");
            CrossBindingFunctionInfo<System.Boolean> mget_isPriorityDealer_4 = new CrossBindingFunctionInfo<System.Boolean>("get_isPriorityDealer");
            CrossBindingFunctionInfo<System.Boolean> mget_canPlayPriority_5 = new CrossBindingFunctionInfo<System.Boolean>("get_canPlayPriority");
            CrossBindingFunctionInfo<GOE.ENoticeType[]> mget_noticeType_6 = new CrossBindingFunctionInfo<GOE.ENoticeType[]>("get_noticeType");
            CrossBindingFunctionInfo<System.Boolean> mget__canCurShow_7 = new CrossBindingFunctionInfo<System.Boolean>("get__canCurShow");
            CrossBindingFunctionInfo<System.String> mget__noticeTag_8 = new CrossBindingFunctionInfo<System.String>("get__noticeTag");
            CrossBindingFunctionInfo<System.Boolean> mget_noticeCanDoESC_9 = new CrossBindingFunctionInfo<System.Boolean>("get_noticeCanDoESC");
            CrossBindingFunctionInfo<System.Boolean> mget_isNoticeFullScreen_10 = new CrossBindingFunctionInfo<System.Boolean>("get_isNoticeFullScreen");
            CrossBindingFunctionInfo<System.Boolean> mget_needAutoRemove_11 = new CrossBindingFunctionInfo<System.Boolean>("get_needAutoRemove");
            CrossBindingFunctionInfo<System.Boolean> mget_isOnlyUINode_12 = new CrossBindingFunctionInfo<System.Boolean>("get_isOnlyUINode");
            CrossBindingMethodInfo mclickBkAction_13 = new CrossBindingMethodInfo("clickBkAction");
            CrossBindingMethodInfo monCannotEscBack_14 = new CrossBindingMethodInfo("onCannotEscBack");
            CrossBindingFunctionInfo<System.Boolean> mget_needTransBk_15 = new CrossBindingFunctionInfo<System.Boolean>("get_needTransBk");
            CrossBindingFunctionInfo<ALPackage._AALBasicLoadUIWndBasicClass> mget_uiObj_16 = new CrossBindingFunctionInfo<ALPackage._AALBasicLoadUIWndBasicClass>("get_uiObj");
            CrossBindingFunctionInfo<System.String> mget_nodeTag_17 = new CrossBindingFunctionInfo<System.String>("get_nodeTag");
            CrossBindingMethodInfo mdealShowNotice_18 = new CrossBindingMethodInfo("dealShowNotice");
            CrossBindingMethodInfo mdealHideNotice_19 = new CrossBindingMethodInfo("dealHideNotice");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter():base(0)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance):base(0)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override void __onDealerDone()
            {
                if (m__onDealerDone_1.CheckShouldInvokeBase(this.instance))
                    base.__onDealerDone();
                else
                    m__onDealerDone_1.Invoke(this.instance);
            }

            public override void showNotice()
            {
                if (mshowNotice_2.CheckShouldInvokeBase(this.instance))
                    base.showNotice();
                else
                    mshowNotice_2.Invoke(this.instance);
            }

            protected override void _onGotoOtherMainViewNode()
            {
                if (m_onGotoOtherMainViewNode_3.CheckShouldInvokeBase(this.instance))
                    base._onGotoOtherMainViewNode();
                else
                    m_onGotoOtherMainViewNode_3.Invoke(this.instance);
            }

            public override void clickBkAction()
            {
                if (mclickBkAction_13.CheckShouldInvokeBase(this.instance))
                    base.clickBkAction();
                else
                    mclickBkAction_13.Invoke(this.instance);
            }

            public override void onCannotEscBack()
            {
                if (monCannotEscBack_14.CheckShouldInvokeBase(this.instance))
                    base.onCannotEscBack();
                else
                    monCannotEscBack_14.Invoke(this.instance);
            }

            public override void dealShowNotice()
            {
                mdealShowNotice_18.Invoke(this.instance);
            }

            public override void dealHideNotice()
            {
                mdealHideNotice_19.Invoke(this.instance);
            }

            protected override System.Boolean _isEnable
            {
            get
            {
                if (mget__isEnable_0.CheckShouldInvokeBase(this.instance))
                    return base._isEnable;
                else
                    return mget__isEnable_0.Invoke(this.instance);

            }
            }

            public override System.Boolean isPriorityDealer
            {
            get
            {
                if (mget_isPriorityDealer_4.CheckShouldInvokeBase(this.instance))
                    return base.isPriorityDealer;
                else
                    return mget_isPriorityDealer_4.Invoke(this.instance);

            }
            }

            public override System.Boolean canPlayPriority
            {
            get
            {
                if (mget_canPlayPriority_5.CheckShouldInvokeBase(this.instance))
                    return base.canPlayPriority;
                else
                    return mget_canPlayPriority_5.Invoke(this.instance);

            }
            }

            public override GOE.ENoticeType[] noticeType
            {
            get
            {
                if (mget_noticeType_6.CheckShouldInvokeBase(this.instance))
                    return base.noticeType;
                else
                    return mget_noticeType_6.Invoke(this.instance);

            }
            }

            protected override System.Boolean _canCurShow
            {
            get
            {
                return mget__canCurShow_7.Invoke(this.instance);

            }
            }

            protected override System.String _noticeTag
            {
            get
            {
                return mget__noticeTag_8.Invoke(this.instance);

            }
            }

            public override System.Boolean noticeCanDoESC
            {
            get
            {
                if (mget_noticeCanDoESC_9.CheckShouldInvokeBase(this.instance))
                    return base.noticeCanDoESC;
                else
                    return mget_noticeCanDoESC_9.Invoke(this.instance);

            }
            }

            public override System.Boolean isNoticeFullScreen
            {
            get
            {
                if (mget_isNoticeFullScreen_10.CheckShouldInvokeBase(this.instance))
                    return base.isNoticeFullScreen;
                else
                    return mget_isNoticeFullScreen_10.Invoke(this.instance);

            }
            }

            public override System.Boolean needAutoRemove
            {
            get
            {
                if (mget_needAutoRemove_11.CheckShouldInvokeBase(this.instance))
                    return base.needAutoRemove;
                else
                    return mget_needAutoRemove_11.Invoke(this.instance);

            }
            }

            public override System.Boolean isOnlyUINode
            {
            get
            {
                if (mget_isOnlyUINode_12.CheckShouldInvokeBase(this.instance))
                    return base.isOnlyUINode;
                else
                    return mget_isOnlyUINode_12.Invoke(this.instance);

            }
            }

            public override System.Boolean needTransBk
            {
            get
            {
                if (mget_needTransBk_15.CheckShouldInvokeBase(this.instance))
                    return base.needTransBk;
                else
                    return mget_needTransBk_15.Invoke(this.instance);

            }
            }

            public override ALPackage._AALBasicLoadUIWndBasicClass uiObj
            {
            get
            {
                if (mget_uiObj_16.CheckShouldInvokeBase(this.instance))
                    return base.uiObj;
                else
                    return mget_uiObj_16.Invoke(this.instance);

            }
            }

            public override System.String nodeTag
            {
            get
            {
                if (mget_nodeTag_17.CheckShouldInvokeBase(this.instance))
                    return base.nodeTag;
                else
                    return mget_nodeTag_17.Invoke(this.instance);

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

