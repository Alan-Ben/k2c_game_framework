package NPUSServer.NPUSUserMgr.UserComp.PlayerComp.Param;

import GS2GC.p004_PlayerOp.GS2GC_004_054_OnPlayerParamUpdated;
import NPCommon.Param._ATNPParamBase;
import NPEnum.ENPPlayerParam;
import USDB.Bo.PlayerBO;

/*********************
 * 玩家信息中的参数数据对象基类，会重载几个不需要处理的函数
 * @author mj
 *
 */
public abstract class _ANPPlayerInfoParamObj extends _ATNPParamBase<PlayerBO>
{
    public _ANPPlayerInfoParamObj(ENPPlayerParam _index, PlayerBO _data)
    {
        super(_index.ordinal(), _data);
    }

    /*****************
     * 将参数同步到客户端
     * @param param
     */
    public GS2GC_004_054_OnPlayerParamUpdated makeSyncProto()
    {
        GS2GC_004_054_OnPlayerParamUpdated proto = new GS2GC_004_054_OnPlayerParamUpdated();
        proto.setIndex(getIndex());
        proto.setParamValue(GetValue());

        return proto;
    }
}
