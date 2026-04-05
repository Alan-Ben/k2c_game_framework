using System;

namespace Hotfix
{
    public partial class HotfixMain
    {
        //是否初始化过refData
        private static bool _m_hasInitRefdata = false;
        
        /// <summary>
        /// 主程序域调用，初始化dll里的refdata读取类
        /// </summary>
        public static void initRefdata(Action _action)
        {
            if(_m_hasInitRefdata)
            {
                if (_action != null) 
                    _action();
                return;
            }
            _m_hasInitRefdata = true;
            HotfixRefdataCoreMgr.instance.InitAllRefCore(() =>
            {
                if (_action != null)
                    _action();
#if UNITY_EDITOR
                Debug.Log("热更refdata初始化完成");
#endif
            });
        }
    }
}