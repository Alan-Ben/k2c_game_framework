using ALBasicProtocolPack;
using GOE;

namespace Hotfix
{
    public partial class HotfixMain
    {
        //是否初始化过
        private static bool _m_hasInit = false;

        //初始化注册
        private static void _initPatchTableDealer()
        {
            //注册协议处理
            if (_m_hasInit)
            {
                return;//协议只注册一次
            }
            _m_hasInit = true;
            
            CommonHotRefPatchDealerMgr.instance.registerPatchDealer(new TileMatchJackpotGroupRefObjPathDealer());
        }
    }
}