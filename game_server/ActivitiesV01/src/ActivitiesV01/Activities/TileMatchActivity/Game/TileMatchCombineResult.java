package ActivitiesV01.Activities.TileMatchActivity.Game;

import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LinkType;

import java.util.List;

public class TileMatchCombineResult
{
    public int genIndexId;
    public List<Integer> blockList;
    public ETileMatch_LinkType linkType;
    public int fromBlockId;

    public TileMatchCombineResult(int genIndexId, List<Integer> blockList, ETileMatch_LinkType linkType, int fromBlockId)
    {
        this.genIndexId = genIndexId;
        this.blockList = blockList;
        this.linkType = linkType;
        this.fromBlockId = fromBlockId;
    }
}
