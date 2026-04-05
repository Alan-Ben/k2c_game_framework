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
    public class _AMainCityPushNoticeAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE._AMainCityPushNotice);
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

        public class Adapter : GOE._AMainCityPushNotice, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.Boolean> mget_isPriorityDealer_0 = new CrossBindingFunctionInfo<System.Boolean>("get_isPriorityDealer");
            CrossBindingFunctionInfo<System.Boolean> mget_canPlayPriority_1 = new CrossBindingFunctionInfo<System.Boolean>("get_canPlayPriority");
            CrossBindingFunctionInfo<GOE.ENoticeType[]> mget_noticeType_2 = new CrossBindingFunctionInfo<GOE.ENoticeType[]>("get_noticeType");
            CrossBindingFunctionInfo<System.Boolean> mget__isEnable_3 = new CrossBindingFunctionInfo<System.Boolean>("get__isEnable");
            CrossBindingFunctionInfo<System.Boolean> mget__canCurShow_4 = new CrossBindingFunctionInfo<System.Boolean>("get__canCurShow");
            CrossBindingFunctionInfo<System.String> mget__noticeTag_5 = new CrossBindingFunctionInfo<System.String>("get__noticeTag");
            CrossBindingFunctionInfo<System.Boolean> mget_noticeCanDoESC_6 = new CrossBindingFunctionInfo<System.Boolean>("get_noticeCanDoESC");
            CrossBindingFunctionInfo<System.Boolean> mget_isNoticeFullScreen_7 = new CrossBindingFunctionInfo<System.Boolean>("get_isNoticeFullScreen");
            CrossBindingFunctionInfo<System.Boolean> mget_needAutoRemove_8 = new CrossBindingFunctionInfo<System.Boolean>("get_needAutoRemove");
            CrossBindingFunctionInfo<System.Boolean> mget_isOnlyUINode_9 = new CrossBindingFunctionInfo<System.Boolean>("get_isOnlyUINode");
            CrossBindingMethodInfo mshowNotice_10 = new CrossBindingMethodInfo("showNotice");
            CrossBindingMethodInfo mclickBkAction_11 = new CrossBindingMethodInfo("clickBkAction");
            CrossBindingMethodInfo monCannotEscBack_12 = new CrossBindingMethodInfo("onCannotEscBack");
            CrossBindingFunctionInfo<System.Boolean> mget_needTransBk_13 = new CrossBindingFunctionInfo<System.Boolean>("get_needTransBk");
            CrossBindingFunctionInfo<ALPackage._AALBasicLoadUIWndBasicClass> mget_uiObj_14 = new CrossBindingFunctionInfo<ALPackage._AALBasicLoadUIWndBasicClass>("get_uiObj");
            CrossBindingFunctionInfo<System.String> mget_nodeTag_15 = new CrossBindingFunctionInfo<System.String>("get_nodeTag");
            CrossBindingMethodInfo m_onDealerDone_16 = new CrossBindingMethodInfo("_onDealerDone");
            CrossBindingMethodInfo mdealShowNotice_17 = new CrossBindingMethodInfo("dealShowNotice");
            CrossBindingMethodInfo mdealHideNotice_18 = new CrossBindingMethodInfo("dealHideNotice");

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

            public override void showNotice()
            {
                if (mshowNotice_10.CheckShouldInvokeBase(this.instance))
                    base.showNotice();
                else
                    mshowNotice_10.Invoke(this.instance);
            }

            public override void clickBkAction()
            {
                if (mclickBkAction_11.CheckShouldInvokeBase(this.instance))
                    base.clickBkAction();
                else
                    mclickBkAction_11.Invoke(this.instance);
            }

            public override void onCannotEscBack()
            {
                if (monCannotEscBack_12.CheckShouldInvokeBase(this.instance))
                    base.onCannotEscBack();
                else
                    monCannotEscBack_12.Invoke(this.instance);
            }

            protected override void _onDealerDone()
            {
                m_onDealerDone_16.Invoke(this.instance);
            }

            public override void dealShowNotice()
            {
                mdealShowNotice_17.Invoke(this.instance);
            }

            public override void dealHideNotice()
            {
                mdealHideNotice_18.Invoke(this.instance);
            }

            public override System.Boolean isPriorityDealer
            {
            get
            {
                if (mget_isPriorityDealer_0.CheckShouldInvokeBase(this.instance))
                    return base.isPriorityDealer;
                else
                    return mget_isPriorityDealer_0.Invoke(this.instance);

            }
            }

            public override System.Boolean canPlayPriority
            {
            get
            {
                if (mget_canPlayPriority_1.CheckShouldInvokeBase(this.instance))
                    return base.canPlayPriority;
                else
                    return mget_canPlayPriority_1.Invoke(this.instance);

            }
            }

            public override GOE.ENoticeType[] noticeType
            {
            get
            {
                if (mget_noticeType_2.CheckShouldInvokeBase(this.instance))
                    return base.noticeType;
                else
                    return mget_noticeType_2.Invoke(this.instance);

            }
            }

            protected override System.Boolean _isEnable
            {
            get
            {
                return mget__isEnable_3.Invoke(this.instance);

            }
            }

            protected override System.Boolean _canCurShow
            {
            get
            {
                return mget__canCurShow_4.Invoke(this.instance);

            }
            }

            protected override System.String _noticeTag
            {
            get
            {
                return mget__noticeTag_5.Invoke(this.instance);

            }
            }

            public override System.Boolean noticeCanDoESC
            {
            get
            {
                if (mget_noticeCanDoESC_6.CheckShouldInvokeBase(this.instance))
                    return base.noticeCanDoESC;
                else
                    return mget_noticeCanDoESC_6.Invoke(this.instance);

            }
            }

            public override System.Boolean isNoticeFullScreen
            {
            get
            {
                if (mget_isNoticeFullScreen_7.CheckShouldInvokeBase(this.instance))
                    return base.isNoticeFullScreen;
                else
                    return mget_isNoticeFullScreen_7.Invoke(this.instance);

            }
            }

            public override System.Boolean needAutoRemove
            {
            get
            {
                if (mget_needAutoRemove_8.CheckShouldInvokeBase(this.instance))
                    return base.needAutoRemove;
                else
                    return mget_needAutoRemove_8.Invoke(this.instance);

            }
            }

            public override System.Boolean isOnlyUINode
            {
            get
            {
                if (mget_isOnlyUINode_9.CheckShouldInvokeBase(this.instance))
                    return base.isOnlyUINode;
                else
                    return mget_isOnlyUINode_9.Invoke(this.instance);

            }
            }

            public override System.Boolean needTransBk
            {
            get
            {
                if (mget_needTransBk_13.CheckShouldInvokeBase(this.instance))
                    return base.needTransBk;
                else
                    return mget_needTransBk_13.Invoke(this.instance);

            }
            }

            public override ALPackage._AALBasicLoadUIWndBasicClass uiObj
            {
            get
            {
                if (mget_uiObj_14.CheckShouldInvokeBase(this.instance))
                    return base.uiObj;
                else
                    return mget_uiObj_14.Invoke(this.instance);

            }
            }

            public override System.String nodeTag
            {
            get
            {
                if (mget_nodeTag_15.CheckShouldInvokeBase(this.instance))
                    return base.nodeTag;
                else
                    return mget_nodeTag_15.Invoke(this.instance);

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

