package NPUSServer.NPUSUserMgr.UserComp.MuseumComp.Gift;

import Common.MuseumObj.Museum_ItemInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Museum.RefMuseumItem;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.UserComp.MuseumComp.MuseumComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_035_MuseumOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMuseumItemBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 旅店珍宝管理器
 */
public class MuseumItemMgr implements _IUserItemBasicDealer
{
    private final MuseumComponent _m_comp;
    private final List<MuseumItemInfo> _m_itemList;

    public MuseumItemMgr(MuseumComponent innComp)
    {
        _m_comp = innComp;
        _m_itemList = new ArrayList<>();
    }

    /**
     * 获取旅店组件
     */
    public MuseumComponent getComp()
    {
        return _m_comp;
    }

    protected void _lock()
    {
        _m_comp.getUserData().lockUser();
    }

    protected void _unlock()
    {
        _m_comp.getUserData().unlockUser();
    }

    /**
     * 初始化珍宝数据
     */
    public void init(final _ICallBackBool _callBack)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerMuseumItemBO.class).findAll("cid", _m_comp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerMuseumItemBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerMuseumItemBO> boList)
                    {
                        for (PlayerMuseumItemBO bo : boList)
                        {
                            if (bo != null)
                            {
                                // 检查配表是否存在
                                RefMuseumItem refItem = RefMuseumItem.getMgr().get(bo.getItemId());
                                if (refItem == null)
                                {
                                    USLog.error(_m_comp.getUSServer(), "Player {} museum item {} config not found, skip loading",
                                            _m_comp.getUserData().getCid(), bo.getItemId());
                                    continue;
                                }

                                MuseumItemInfo info = new MuseumItemInfo(MuseumItemMgr.this, bo, refItem);
                                _m_itemList.add(info);
                            }
                        }
                        _callBack.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(_m_comp.getUSServer(), "Player {} museum item data load failed", _m_comp.getUserData().getCid());
                        _callBack.onRunOver(false);
                    }
                });
    }

    /**
     * 根据珍宝ID获取珍宝信息
     */
    public MuseumItemInfo lookupItem(long _itemId)
    {
        _lock();
        try
        {
            for (MuseumItemInfo info : _m_itemList)
            {
                if (info.getItemId() == _itemId)
                {
                    return info;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取珍宝
     * @param _itemId
     * @param _isInitGain
     * @param _isNotMerge
     * @param _context
     */
    public void gainItem(long _itemId, boolean _isInitGain, boolean _isNotMerge, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (lookupItem(_itemId) != null)
            {
                USLog.error(_m_comp.getUSServer(), "Player {} museum item {} already exists, cannot gain again",
                        _m_comp.getUserData().getCid(), _itemId);
                return;
            }

            RefMuseumItem refItem = RefMuseumItem.getMgr().get(_itemId);
            if (refItem == null)
            {
                USLog.error(_m_comp.getUSServer(), "Player {} museum item {} config not found, cannot gain",
                        _m_comp.getUserData().getCid(), _itemId);
                return;
            }

            // 创建新的珍宝数据
            PlayerMuseumItemBO bo = new PlayerMuseumItemBO();
            bo.setCid(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getCid());
            bo.setItemId(_m_comp.getUSServer().getBM(), _itemId);
            bo.setLevel(_m_comp.getUSServer().getBM(), 1);
            bo.setGainTimeMs(_m_comp.getUSServer().getBM(), CommonFunc.getNowTimeMS());
            bo.insert(_m_comp.getUSServer().getBM());

            // 创建珍宝信息
            MuseumItemInfo itemInfo = new MuseumItemInfo(this, bo, refItem);
            _m_itemList.add(itemInfo);

            _context.collectItem(ENPItemType.MUSEUM_ITEM, _itemId, 1, _isNotMerge);

            // 推送到客户端
            if (!_isInitGain)
                getComp().getUserData().sendMsgToGC(US2GCWriter_035_MuseumOp.make_050_OnMuseumItemAdd(itemInfo.makeProto()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 填充珍宝列表到指定集合
     * @param _itemList
     */
    public void fillItemList(List<Museum_ItemInfo> _itemList)
    {
        _lock();
        try
        {
            for (MuseumItemInfo itemInfo : _m_itemList)
            {
                _itemList.add(itemInfo.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.MUSEUM_ITEM;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        return lookupItem(_itemId) == null ? 0 : 1;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return lookupItem(_itemId) != null;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        gainItem(_itemId, true, false, _context);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        gainItem(_itemId, false, _isNotMerge, _context);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    public long getItemNum()
    {
        _lock();
        try{
            return _m_itemList.size();
        }finally
        {
            _unlock();
        }
    }
}
