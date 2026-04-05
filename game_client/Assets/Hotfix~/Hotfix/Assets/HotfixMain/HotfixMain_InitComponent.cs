using System;
using ALPackage;
using GOE;

namespace Hotfix
{
    public partial class HotfixMain
    {
        /// <summary>
        /// 主程序域调用，初始化热更数据组件
        /// </summary>
        public static void initDataComponent(Action<bool> _onDataComponentInitDone)
        {
            try
            {
                // 先初始化账号存档
                HotfixAccountSettingMgr.instance.init();

                HotfixNPPlayer.instance.init((_isSucc)=>
                {
                    _onDataComponentInitDone?.Invoke(_isSucc);
                });
                
#if UNITY_EDITOR
                //3秒后服务器初始化数据未回包弹窗提示未初始化的组件
                long npplayerSerialize = HotfixNPPlayer.instance.npplayerSerialize;
                CommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (npplayerSerialize != HotfixNPPlayer.instance.npplayerSerialize)
                        return;
                    
                    string mes = HotfixNPPlayer.instance.getAllUnInitComp();

                    if (!string.IsNullOrEmpty(mes))
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_init_hotfix_component_failed, mes), 
                            TextTranslate.instance.getLanguage(TransKeyConst.confirm), HotfixNPPlayer.instance.forceInitDone);
                }, 3f);
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"调用Hotfix函数HotfixMain.initDataComponent()时发生Exception\n{e.ToString_ILRuntime()}");
            }
        }

        /// <summary>
        /// 主程序域调用，销毁热更数据组件
        /// </summary>
        public static void discardDataComponent()
        {
            HotfixAccountSettingMgr.instance.discard();
            
            HotfixNPPlayer.instance.discard();
        }
    }
}