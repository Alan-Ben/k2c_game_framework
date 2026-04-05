package ActivitiesV01.Activities.TileMatchActivity.Game;

import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LinkType;

import java.util.*;
import java.util.stream.Collectors;

/**
 * @description: 三消游戏工具类
 */
public class TileMatchGameUtil
{
    //连接类型优先级
    public static Integer[] linkTypePriority = new Integer[ETileMatch_LinkType.ETileMatch_LinkType_Length];
    static
    {
        linkTypePriority[ETileMatch_LinkType.LINK_3.ordinal()] = 1;
        linkTypePriority[ETileMatch_LinkType.LINK_4.ordinal()] = 2;
        linkTypePriority[ETileMatch_LinkType.LINK_T.ordinal()] = 3;
        linkTypePriority[ETileMatch_LinkType.LINK_L.ordinal()] = 3;
        linkTypePriority[ETileMatch_LinkType.LINK_5.ordinal()] = 4;
    }
    /**
     * 计算触发点位的消除结果
     * @param _triggerIndex
     * @param _isPlayerTrigger
     * @param _blockList
     * @param _block
     * @return
     */
    public static TileMatchCombineResult calIndexCombineResult(int _triggerIndex, boolean _isPlayerTrigger, TileMatchBlockList _blockList, TileMatch_BlockBaseInfo _block)
    {
        //计算行消列表
        List<Integer> rowLink = linkCheck(_blockList, _triggerIndex, _block, ETileMatchAroundType.ROW);
        //计算列消列表
        List<Integer> columnLink = linkCheck(_blockList, _triggerIndex, _block, ETileMatchAroundType.COLUMN);

        //判断连接形状并且标记相关位置
        return calLinkTypeAndMark(_triggerIndex, _block, _isPlayerTrigger, rowLink, columnLink);
    }

    /**
     * 进行可连性检测
     * @param _blockList  数据源
     * @param _index      检查起点
     * @param _block
     * @param _aroundType 检测方向是按行检查还是按列检测
     * @return Set<Integer> 可连接列表
     */
    public static List<Integer> linkCheck(TileMatchBlockList _blockList, int _index, TileMatch_BlockBaseInfo _block, ETileMatchAroundType _aroundType)
    {
        //计算可连队列
        Set<Integer> linkSet = TileMatchBlockList.calLinkSet(_blockList, _index, _block, _aroundType);

        //返回排序列表
        List<Integer> link = new ArrayList<>(linkSet);
        link.sort(Comparator.comparingInt(Integer::intValue));
        return link;
    }

    /**
     * 判断连接形状并且标记相关位置
     * @param _block
     * @param _rowLink    指定位点的行消列表
     * @param _columnLink 指定位点的列消列表
     */
    public static TileMatchCombineResult calLinkTypeAndMark(int _triggerIndex, TileMatch_BlockBaseInfo _block, boolean _isPlayerTrigger, List<Integer> _rowLink, List<Integer> _columnLink)
    {
        int playerTriggerIndex = _isPlayerTrigger ? _triggerIndex : -1;

        //行 5 型 最左边标记生成
        if (_rowLink.size() >= 5)
        {
            //计算生成位置
            int markIndex = _rowLink.contains(playerTriggerIndex) ? playerTriggerIndex : _rowLink.get(0);
            //计算需要移除的点位
            Set<Integer> blockSet = new HashSet<>(_rowLink);
            if (_columnLink.size() >= 3)
                blockSet.addAll(_columnLink);

            return new TileMatchCombineResult(markIndex, new ArrayList<>(blockSet), ETileMatch_LinkType.LINK_5, _block.getBlockId());
        }
        //列 5 型 最上边标记生成
        if (_columnLink.size() >= 5)
        {
            //计算生成位置
            int markIndex = _columnLink.contains(playerTriggerIndex) ? playerTriggerIndex : _columnLink.get(_columnLink.size() - 1);
            //计算需要移除的点位
            Set<Integer> blockSet = new HashSet<>(_columnLink);
            if (_rowLink.size() >= 3)
                blockSet.addAll(_rowLink);

            return new TileMatchCombineResult(markIndex, new ArrayList<>(blockSet), ETileMatch_LinkType.LINK_5, _block.getBlockId());
        }
        // T 型 或 L 型,在交叉处标记
        if (_rowLink.size() >= 3 && _columnLink.size() >= 3)
        {
            //交集查询
            List<Integer> intersection = intersectionTwoList(_rowLink, _columnLink);
            if (!intersection.isEmpty())
            {
                //计算生成位置
                int markIndex = _rowLink.contains(playerTriggerIndex) ? playerTriggerIndex : intersection.get(0);
                //计算需要移除的点位
                Set<Integer> blockSet = new HashSet<>();
                blockSet.addAll(_rowLink);
                blockSet.addAll(_columnLink);

                return new TileMatchCombineResult(markIndex, new ArrayList<>(blockSet), ETileMatch_LinkType.LINK_T, _block.getBlockId());
            }
        }
        //行 4 型
        if (_rowLink.size() >= 4)
        {
            //计算生成位置
            int markIndex = _rowLink.contains(playerTriggerIndex) ? playerTriggerIndex : _rowLink.get(0);

            return new TileMatchCombineResult(markIndex, _rowLink, ETileMatch_LinkType.LINK_4, _block.getBlockId());
        }
        //列 4 型
        if (_columnLink.size() >= 4)
        {
            //计算生成位置
            int markIndex = _columnLink.contains(playerTriggerIndex) ? playerTriggerIndex : _columnLink.get(_columnLink.size() - 1);

            return new TileMatchCombineResult(markIndex, _columnLink, ETileMatch_LinkType.LINK_4, _block.getBlockId());
        }
        //行 3 型
        if (_rowLink.size() == 3)
        {
            //计算生成位置
            int markIndex = _rowLink.contains(playerTriggerIndex) ? playerTriggerIndex : _rowLink.get(0);

            return new TileMatchCombineResult(markIndex, _rowLink, ETileMatch_LinkType.LINK_3, _block.getBlockId());
        }
        //列 3 型
        if (_columnLink.size() == 3)
        {
            //计算生成位置
            int markIndex = _columnLink.contains(playerTriggerIndex) ? playerTriggerIndex : _columnLink.get(_columnLink.size() - 1);

            return new TileMatchCombineResult(markIndex, _columnLink, ETileMatch_LinkType.LINK_3, _block.getBlockId());
        }

        return null;
    }


    /**
     * 两集合交集
     * @param _rowLink    行列表
     * @param _columnLink 列列表
     * @return _columnLink 交集列表
     */
    public static List<Integer> intersectionTwoList(List<Integer> _rowLink, List<Integer> _columnLink)
    {
        return _rowLink.stream().filter(_columnLink::contains).collect(Collectors.toList());
    }
}
