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
    public class UIQueueBaseNodeAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE.UIQueueBaseNode);
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

        public class Adapter : GOE.UIQueueBaseNode, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo monEnterQueue_0 = new CrossBindingMethodInfo("onEnterQueue");
            CrossBindingMethodInfo monClose_1 = new CrossBindingMethodInfo("onClose");
            CrossBindingFunctionInfo<GOE.ENoticeType> mget_enableNoticeType_2 = new CrossBindingFunctionInfo<GOE.ENoticeType>("get_enableNoticeType");
            CrossBindingMethodInfo<System.Action> mdoEnterNode_3 = new CrossBindingMethodInfo<System.Action>("doEnterNode");
            CrossBindingMethodInfo<System.Action> mdoQuitNode_4 = new CrossBindingMethodInfo<System.Action>("doQuitNode");
            CrossBindingMethodInfo monRollBackQuit_5 = new CrossBindingMethodInfo("onRollBackQuit");
            CrossBindingMethodInfo monCloseQuit_6 = new CrossBindingMethodInfo("onCloseQuit");
            CrossBindingFunctionInfo<System.Boolean> mget_isOnlyUINode_7 = new CrossBindingFunctionInfo<System.Boolean>("get_isOnlyUINode");
            CrossBindingFunctionInfo<System.Boolean> mget_IsCanRollBackQuit_8 = new CrossBindingFunctionInfo<System.Boolean>("get_IsCanRollBackQuit");
            CrossBindingFunctionInfo<System.Boolean> mget_isEnable_9 = new CrossBindingFunctionInfo<System.Boolean>("get_isEnable");
            CrossBindingFunctionInfo<System.Boolean> mget_canShowNotice_10 = new CrossBindingFunctionInfo<System.Boolean>("get_canShowNotice");
            CrossBindingMethodInfo mEnterNode_11 = new CrossBindingMethodInfo("EnterNode");
            CrossBindingMethodInfo mQuitNode_12 = new CrossBindingMethodInfo("QuitNode");
            CrossBindingMethodInfo mPreEnterNode_13 = new CrossBindingMethodInfo("PreEnterNode");
            CrossBindingMethodInfo mAfterEnterNode_14 = new CrossBindingMethodInfo("AfterEnterNode");
            CrossBindingFunctionInfo<System.Boolean> mget_isAllNodeOpQuitNode_15 = new CrossBindingFunctionInfo<System.Boolean>("get_isAllNodeOpQuitNode");
            CrossBindingFunctionInfo<System.Boolean> mget_isRootNode_16 = new CrossBindingFunctionInfo<System.Boolean>("get_isRootNode");
            CrossBindingFunctionInfo<System.Boolean> mget_NeedRemovePreAutoRemove_17 = new CrossBindingFunctionInfo<System.Boolean>("get_NeedRemovePreAutoRemove");
            CrossBindingFunctionInfo<System.Boolean> mget_canAddWhenLastNodeIsTheSameType_18 = new CrossBindingFunctionInfo<System.Boolean>("get_canAddWhenLastNodeIsTheSameType");
            CrossBindingFunctionInfo<System.String> mget_NodeShowName_19 = new CrossBindingFunctionInfo<System.String>("get_NodeShowName");
            CrossBindingFunctionInfo<System.Boolean> mget_NeedAutoRemove_20 = new CrossBindingFunctionInfo<System.Boolean>("get_NeedAutoRemove");
            CrossBindingFunctionInfo<System.Boolean> mget_IsMainViewNode_21 = new CrossBindingFunctionInfo<System.Boolean>("get_IsMainViewNode");
            CrossBindingMethodInfo monCannotEscBack_22 = new CrossBindingMethodInfo("onCannotEscBack");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter():base(EUIQueueStageType.MAIN)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance):base(EUIQueueStageType.MAIN)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void onEnterQueue()
            {
                if (monEnterQueue_0.CheckShouldInvokeBase(this.instance))
                    base.onEnterQueue();
                else
                    monEnterQueue_0.Invoke(this.instance);
            }

            public override void onClose()
            {
                if (monClose_1.CheckShouldInvokeBase(this.instance))
                    base.onClose();
                else
                    monClose_1.Invoke(this.instance);
            }

            public override void doEnterNode(System.Action _triggerEnterDone)
            {
                if (mdoEnterNode_3.CheckShouldInvokeBase(this.instance))
                    base.doEnterNode(_triggerEnterDone);
                else
                    mdoEnterNode_3.Invoke(this.instance, _triggerEnterDone);
            }

            public override void doQuitNode(System.Action _dealOnQuitDone)
            {
                if (mdoQuitNode_4.CheckShouldInvokeBase(this.instance))
                    base.doQuitNode(_dealOnQuitDone);
                else
                    mdoQuitNode_4.Invoke(this.instance, _dealOnQuitDone);
            }

            public override void onRollBackQuit()
            {
                if (monRollBackQuit_5.CheckShouldInvokeBase(this.instance))
                    base.onRollBackQuit();
                else
                    monRollBackQuit_5.Invoke(this.instance);
            }

            public override void onCloseQuit()
            {
                if (monCloseQuit_6.CheckShouldInvokeBase(this.instance))
                    base.onCloseQuit();
                else
                    monCloseQuit_6.Invoke(this.instance);
            }

            public override void EnterNode()
            {
                mEnterNode_11.Invoke(this.instance);
            }

            public override void QuitNode()
            {
                mQuitNode_12.Invoke(this.instance);
            }

            public override void PreEnterNode()
            {
                if (mPreEnterNode_13.CheckShouldInvokeBase(this.instance))
                    base.PreEnterNode();
                else
                    mPreEnterNode_13.Invoke(this.instance);
            }

            public override void AfterEnterNode()
            {
                if (mAfterEnterNode_14.CheckShouldInvokeBase(this.instance))
                    base.AfterEnterNode();
                else
                    mAfterEnterNode_14.Invoke(this.instance);
            }

            public override void onCannotEscBack()
            {
                if (monCannotEscBack_22.CheckShouldInvokeBase(this.instance))
                    base.onCannotEscBack();
                else
                    monCannotEscBack_22.Invoke(this.instance);
            }

            public override GOE.ENoticeType enableNoticeType
            {
            get
            {
                if (mget_enableNoticeType_2.CheckShouldInvokeBase(this.instance))
                    return base.enableNoticeType;
                else
                    return mget_enableNoticeType_2.Invoke(this.instance);

            }
            }

            public override System.Boolean isOnlyUINode
            {
            get
            {
                if (mget_isOnlyUINode_7.CheckShouldInvokeBase(this.instance))
                    return base.isOnlyUINode;
                else
                    return mget_isOnlyUINode_7.Invoke(this.instance);

            }
            }

            public override System.Boolean IsCanRollBackQuit
            {
            get
            {
                if (mget_IsCanRollBackQuit_8.CheckShouldInvokeBase(this.instance))
                    return base.IsCanRollBackQuit;
                else
                    return mget_IsCanRollBackQuit_8.Invoke(this.instance);

            }
            }

            public override System.Boolean isEnable
            {
            get
            {
                if (mget_isEnable_9.CheckShouldInvokeBase(this.instance))
                    return base.isEnable;
                else
                    return mget_isEnable_9.Invoke(this.instance);

            }
            }

            public override System.Boolean canShowNotice
            {
            get
            {
                if (mget_canShowNotice_10.CheckShouldInvokeBase(this.instance))
                    return base.canShowNotice;
                else
                    return mget_canShowNotice_10.Invoke(this.instance);

            }
            }

            public override System.Boolean isAllNodeOpQuitNode
            {
            get
            {
                if (mget_isAllNodeOpQuitNode_15.CheckShouldInvokeBase(this.instance))
                    return base.isAllNodeOpQuitNode;
                else
                    return mget_isAllNodeOpQuitNode_15.Invoke(this.instance);

            }
            }

            public override System.Boolean isRootNode
            {
            get
            {
                if (mget_isRootNode_16.CheckShouldInvokeBase(this.instance))
                    return base.isRootNode;
                else
                    return mget_isRootNode_16.Invoke(this.instance);

            }
            }

            public override System.Boolean NeedRemovePreAutoRemove
            {
            get
            {
                if (mget_NeedRemovePreAutoRemove_17.CheckShouldInvokeBase(this.instance))
                    return base.NeedRemovePreAutoRemove;
                else
                    return mget_NeedRemovePreAutoRemove_17.Invoke(this.instance);

            }
            }

            public override System.Boolean canAddWhenLastNodeIsTheSameType
            {
            get
            {
                if (mget_canAddWhenLastNodeIsTheSameType_18.CheckShouldInvokeBase(this.instance))
                    return base.canAddWhenLastNodeIsTheSameType;
                else
                    return mget_canAddWhenLastNodeIsTheSameType_18.Invoke(this.instance);

            }
            }

            public override System.String NodeShowName
            {
            get
            {
                if (mget_NodeShowName_19.CheckShouldInvokeBase(this.instance))
                    return base.NodeShowName;
                else
                    return mget_NodeShowName_19.Invoke(this.instance);

            }
            }

            public override System.Boolean NeedAutoRemove
            {
            get
            {
                return mget_NeedAutoRemove_20.Invoke(this.instance);

            }
            }

            public override System.Boolean IsMainViewNode
            {
            get
            {
                return mget_IsMainViewNode_21.Invoke(this.instance);

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

