package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;

public class TileMatchLogic_ItemGen extends _ATileMatchGameLogic
{
    private int _m_genIndexId;
    private int _m_genBlockId;
    private int _m_fromBlockId;

    public TileMatchLogic_ItemGen(int _genIndexId, int _genBlockId, int _fromBlockId)
    {
        _m_genIndexId = _genIndexId;
        _m_genBlockId = _genBlockId;
        _m_fromBlockId = _fromBlockId;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.ITEM_GEN;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        _blockList.set(_m_genIndexId, new TileMatch_BlockBaseInfo(_m_genBlockId, _m_fromBlockId));
    }
}
