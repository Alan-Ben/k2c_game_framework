using ALBasicProtocolPack;
using GOE;

namespace Hotfix
{
    public partial class HotfixMain
    {
        //是否初始化过协议
        private static bool _m_hasInitProtocol = false;

        //初始化注册协议
        private static void _initProtocolDealer()
        {
            //注册协议处理
            if (_m_hasInitProtocol)
            {
                Debug.LogError("重复initProtocol");
                return;//协议只注册一次
            }
            _m_hasInitProtocol = true;

			ALBasicProtocolMainOrderDealer mainDealer200_HotSimpleActivityOp = new ALBasicProtocolMainOrderDealer(200, 110);
			mainDealer200_HotSimpleActivityOp.regDealer(new GSSubDealer_200_101_OnRegularActivityShopItemBuy());
			mainDealer200_HotSimpleActivityOp.regDealer(new GSSubDealer_200_102_OnRegularActivityShopRefresh());
			GSProtocolDispather.instance.RegProtocol(mainDealer200_HotSimpleActivityOp);
            
            ALBasicProtocolMainOrderDealer mainDealer201_TileMatchOp = new ALBasicProtocolMainOrderDealer(201, 100);
            mainDealer201_TileMatchOp.regDealer(new GSSubDealer_201_051_OnTileMatchLogicProcess());
            mainDealer201_TileMatchOp.regDealer(new GSSubDealer_201_052_OnTileMatchTaskChg());
            mainDealer201_TileMatchOp.regDealer(new GSSubDealer_201_053_OnTileMatchStepRewardChg());
            mainDealer201_TileMatchOp.regDealer(new GSSubDealer_201_054_OnTileMatchCanDrawStepRewardChg());
            mainDealer201_TileMatchOp.regDealer(new GSSubDealer_201_055_OnTileMatchTotalScoreChg());
            GSProtocolDispather.instance.RegProtocol(mainDealer201_TileMatchOp);
            
            ALBasicProtocolMainOrderDealer mainDealer202_NumMergeOp = new ALBasicProtocolMainOrderDealer(202, 100);
            mainDealer202_NumMergeOp.regDealer(new GSSubDealer_202_050_OnNumMergeBoardChg());
            mainDealer202_NumMergeOp.regDealer(new GSSubDealer_202_051_OnNumMergeBoxChg());
            GSProtocolDispather.instance.RegProtocol(mainDealer202_NumMergeOp);
        }
    }
}