package NPCommon.Util;

import NPCommon.DB.BM.BM;
import NPCommon.DB._ISelectDBInterface;
import NPCommon.DDAlert.DDAlert;
import NPCommon.Enum.NPCommonEnum;
import WCGBasicServer._AWCGBasicServer;

/*******
 * 增加部分基础底层对象的服务器基础对象
 */
public abstract class _ABasicServerObj extends _AWCGBasicServer implements _ISelectDBInterface
{
    /**
     * 每个服务器通用的钉钉报警对象
     */
    private DDAlert _m_dlDDAlert;

    /***
     * 数据库访问对象
     */
    private BM _m_bm;

    protected  _ABasicServerObj()
    {
        _m_dlDDAlert = new DDAlert();
        _m_bm = new BM(this);
    }

    public DDAlert getDDAlert() {return _m_dlDDAlert;}
    public BM getBM() {return _m_bm;}

    /**********
     * 允许服务器对数据进行转换
     * @param _srcTag
     * @return
     */
    @Override
    public NPCommonEnum.EDBTag switchDBTag(NPCommonEnum.EDBTag _srcTag)
    {
        //默认直接返回
        return _srcTag;
    }

    /**
     * Bus服务器重新设置的时候，触发的函数，一般可用于一些服务器重新通知其他服务器上线或者重置数据
     * @param _newBusHandleId
     */
    @Override
    public void onBusServerHandleChg(int _newBusHandleId)
    {
        //默认不做特殊处理，特殊服务器可以进行数据同步触发
    }
}
