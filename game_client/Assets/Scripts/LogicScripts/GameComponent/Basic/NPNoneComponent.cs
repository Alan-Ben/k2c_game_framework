using System;
using System.Collections.Generic;

namespace GOE
{
    public class NPNoneComponent : _ANPBasicPlayerComponent
    {
        public NPNoneComponent(NPPlayerComponentMgr _compMgr)
            : base(_compMgr)
        {

        }

        //是否必要型初始化组件
        public override bool isMustInit { get { return false; } }
        //获取组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.NONE; } }
        //依赖的组件队列
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
        }

        //处理初始化操作
        protected override void _dealInit()
        {
            //加载所有必要模块
            _initMustComp();
        }
        //初始化结果操作
        protected override void _onInitDone()
        {

        }
        protected override void _onInitFail()
        {

        }
        //释放资源函数
        protected override void _discard()
        {

        }

        //初始化必要组件
        protected void _initMustComp()
        {
            //进行组件管理器的加载
            bool res = compMgr.__initMustComp(_initMustComp);

            //执行preinit处理
            compMgr.__preInitAllComp();

            //判断是否加载成功了
            if(res)
            {
                setInitDone();
            }
        }
    }
}
