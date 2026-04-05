package NPServerProtocolWriter.NP2US.RequestBack;

import Common.ServerObj.ServerObj_MarsMine;
import NP2US_RB.p010_MarsMineOp.*;

/**
 * 火星矿操作响应协议Writer
 *
 * 提供构造火星矿相关跨服响应协议的便利方法
 */
public class NP2US_RB_Writer_010_MarsMineOp
{
    /**
     * 构造返回火星矿数据响应协议
     *
     * @param _marsMine 矿数据
     * @return 响应协议对象
     */
    public static NP2US_RB_010_001_RetGetMarsMine make_001_RetGetMarsMine(ServerObj_MarsMine _marsMine)
    {
        NP2US_RB_010_001_RetGetMarsMine proto = new NP2US_RB_010_001_RetGetMarsMine();
        proto.setMarsMine(_marsMine);
        return proto;
    }

    /**
     * 构造返回火星矿是否被其他公会攻击过响应协议
     *
     * @param _hasAttacked 是否被其他公会攻击过
     * @return 响应协议对象
     */
    public static NP2US_RB_010_002_RetGetMarsHadAttackByOtherTag make_002_RetGetMarsHadAttackByOtherTag(
            boolean _hasAttacked)
    {
        NP2US_RB_010_002_RetGetMarsHadAttackByOtherTag proto = new NP2US_RB_010_002_RetGetMarsHadAttackByOtherTag();
        proto.setHasAttacked(_hasAttacked);
        return proto;
    }

    /**
     * 构造返回前往占领火星矿结果响应协议
     *
     * @return 响应协议对象
     */
    public static NP2US_RB_010_003_RetGoToOccupyMarsMine make_003_RetGoToOccupyMarsMine()
    {
        NP2US_RB_010_003_RetGoToOccupyMarsMine proto = new NP2US_RB_010_003_RetGoToOccupyMarsMine();
        return proto;
    }

    /**
     * 构造返回离开火星矿结果响应协议
     *
     * @return 响应协议对象
     */
    public static NP2US_RB_010_004_RetLeaveMarsMine make_004_RetLeaveMarsMine()
    {
        NP2US_RB_010_004_RetLeaveMarsMine proto = new NP2US_RB_010_004_RetLeaveMarsMine();
        return proto;
    }

    /**
     * 构造返回设置火星矿剩余资源数量结果响应协议
     *
     * @return 响应协议对象
     */
    public static NP2US_RB_010_005_RetSetMineRemainNum make_005_RetSetMineRemainNum()
    {
        NP2US_RB_010_005_RetSetMineRemainNum proto = new NP2US_RB_010_005_RetSetMineRemainNum();
        return proto;
    }
}
