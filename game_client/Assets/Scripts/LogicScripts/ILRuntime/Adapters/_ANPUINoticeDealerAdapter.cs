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
    public class _ANPUINoticeDealerAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(NPUINoticeMgr._ANPUINoticeDealer);
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

        public class Adapter : NPUINoticeMgr._ANPUINoticeDealer, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.Boolean> mget_noticeCanDoESC_0 = new CrossBindingFunctionInfo<System.Boolean>("get_noticeCanDoESC");
            CrossBindingFunctionInfo<System.Boolean> mget_isNoticeFullScreen_1 = new CrossBindingFunctionInfo<System.Boolean>("get_isNoticeFullScreen");
            CrossBindingFunctionInfo<System.Boolean> mget_isOnlyUINode_2 = new CrossBindingFunctionInfo<System.Boolean>("get_isOnlyUINode");
            CrossBindingMethodInfo mshowNotice_3 = new CrossBindingMethodInfo("showNotice");
            CrossBindingFunctionInfo<ENoticeType[]> mget_noticeType_4 = new CrossBindingFunctionInfo<ENoticeType[]>("get_noticeType");
            CrossBindingFunctionInfo<System.Boolean> mget_needTransBk_5 = new CrossBindingFunctionInfo<System.Boolean>("get_needTransBk");
            CrossBindingFunctionInfo<System.String> mget_strTag_6 = new CrossBindingFunctionInfo<System.String>("get_strTag");
            CrossBindingFunctionInfo<System.Boolean> mget_isPriorityDealer_7 = new CrossBindingFunctionInfo<System.Boolean>("get_isPriorityDealer");
            CrossBindingFunctionInfo<System.Boolean> mget_canPlayPriority_8 = new CrossBindingFunctionInfo<System.Boolean>("get_canPlayPriority");
            CrossBindingFunctionInfo<System.Boolean> mget_canCurShow_9 = new CrossBindingFunctionInfo<System.Boolean>("get_canCurShow");
            CrossBindingMethodInfo m_onDealerDone_10 = new CrossBindingMethodInfo("_onDealerDone");
            CrossBindingMethodInfo mdealShowNotice_11 = new CrossBindingMethodInfo("dealShowNotice");
            CrossBindingMethodInfo mdealHideNotice_12 = new CrossBindingMethodInfo("dealHideNotice");
            CrossBindingFunctionInfo<ENoticeType> mget_noticeTypeSingle_13 = new CrossBindingFunctionInfo<ENoticeType>("get_noticeTypeSingle");

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

            public override void showNotice()
            {
                if (mshowNotice_3.CheckShouldInvokeBase(this.instance))
                    base.showNotice();
                else
                    mshowNotice_3.Invoke(this.instance);
            }

            protected override void _onDealerDone()
            {
                m_onDealerDone_10.Invoke(this.instance);
            }

            public override void dealShowNotice()
            {
                mdealShowNotice_11.Invoke(this.instance);
            }

            public override void dealHideNotice()
            {
                mdealHideNotice_12.Invoke(this.instance);
            }

            public override System.Boolean noticeCanDoESC
            {
            get
            {
                if (mget_noticeCanDoESC_0.CheckShouldInvokeBase(this.instance))
                    return base.noticeCanDoESC;
                else
                    return mget_noticeCanDoESC_0.Invoke(this.instance);

            }
            }

            public override System.Boolean isNoticeFullScreen
            {
            get
            {
                if (mget_isNoticeFullScreen_1.CheckShouldInvokeBase(this.instance))
                    return base.isNoticeFullScreen;
                else
                    return mget_isNoticeFullScreen_1.Invoke(this.instance);

            }
            }

            public override System.Boolean isOnlyUINode
            {
            get
            {
                if (mget_isOnlyUINode_2.CheckShouldInvokeBase(this.instance))
                    return base.isOnlyUINode;
                else
                    return mget_isOnlyUINode_2.Invoke(this.instance);

            }
            }

            public override ENoticeType[] noticeType
            {
            get
            {
                if (mget_noticeType_4.CheckShouldInvokeBase(this.instance))
                    return base.noticeType;
                else
                    return mget_noticeType_4.Invoke(this.instance);

            }
            }

            public override ENoticeType noticeTypeSingle
            {
            get
            {
                if (mget_noticeTypeSingle_13.CheckShouldInvokeBase(this.instance))
                    return base.noticeTypeSingle;
                else
                    return mget_noticeTypeSingle_13.Invoke(this.instance);

            }
            }

            public override System.Boolean needTransBk
            {
            get
            {
                if (mget_needTransBk_5.CheckShouldInvokeBase(this.instance))
                    return base.needTransBk;
                else
                    return mget_needTransBk_5.Invoke(this.instance);

            }
            }

            public override System.String noticeTag
            {
            get
            {
                if (mget_strTag_6.CheckShouldInvokeBase(this.instance))
                    return base.noticeTag;
                else
                    return mget_strTag_6.Invoke(this.instance);

            }
            }

            public override System.Boolean isPriorityDealer
            {
            get
            {
                return mget_isPriorityDealer_7.Invoke(this.instance);

            }
            }

            public override System.Boolean canPlayPriority
            {
            get
            {
                return mget_canPlayPriority_8.Invoke(this.instance);

            }
            }

            public override System.Boolean canCurShow
            {
            get
            {
                return mget_canCurShow_9.Invoke(this.instance);

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

