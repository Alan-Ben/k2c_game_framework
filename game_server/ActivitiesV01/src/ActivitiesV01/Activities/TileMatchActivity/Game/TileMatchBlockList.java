package ActivitiesV01.Activities.TileMatchActivity.Game;

import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import ActivitiesV01.Refs.TileMatch.RefTileMatchOther;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseList;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairInt;

import java.nio.ByteBuffer;
import java.util.*;
import java.util.function.Consumer;
import java.util.function.Supplier;
import java.util.function.ToIntBiFunction;

/**
 * 三消游戏格子逻辑数据，_m_optLinkedList是格子数据的链表，所有的操作都对其进行操作
 * 保存时通过提供的保存方法向数据库保存
 */
public class TileMatchBlockList extends _ATileMatchArrayToList<Integer, TileMatch_BlockBaseInfo>
{

    /**
     * 获得源数据 bytes方法
     */
    protected final Supplier<byte[]> _m_getOriMapBytes;

    /**
     * 保存源数据 bytes 方法
     */
    protected final Consumer<byte[]> _m_saveOriMapBytes;

    /**
     * 源结构体 map
     */
    private final TileMatch_BlockBaseList _m_oriStructureList;

    /**
     * 三消游戏用来操作的连接列表，交换位置的操作比较多，使用linkedList
     */
    private final LinkedList<TileMatch_BlockBaseInfo> _m_optLinkedList;

    /**
     * 数据是否有变更
     */
    private boolean _m_bHasChange;

    /**
     * 需要提供 ByteBuffer 数据源的获取和保存方法
     * @param _getOriMapBytes  获取方法
     * @param _saveOriMapBytes 保存方法
     */
    public TileMatchBlockList(Supplier<byte[]> _getOriMapBytes, Consumer<byte[]> _saveOriMapBytes)
    {
        _m_getOriMapBytes = _getOriMapBytes;
        _m_saveOriMapBytes = _saveOriMapBytes;
        _m_oriStructureList = new TileMatch_BlockBaseList();
        if (_getOriMapBytes.get() != null)
            _m_oriStructureList.readPackage(ByteBuffer.wrap(_getOriMapBytes.get()));
        _m_optLinkedList = new LinkedList<>();
        _m_optLinkedList.addAll(_m_oriStructureList.getBlockList());
    }

    /**
     * 从给定坐标开始，进行深度优先遍历，查找所有可连单位
     * @param _index 指定索引
     * @param _block
     * @return Set<Integer> 所有可连格子索引
     */
    public static Set<Integer> calLinkSet(TileMatchBlockList _blockList, int _index, TileMatch_BlockBaseInfo _block, ETileMatchAroundType _aroundType)
    {
        //可连接记录
        Set<Integer> canLinkSet = new HashSet<>();
        //已检测记录
        Set<Integer> checkedSet = new HashSet<>();
        //检查队列
        Queue<Integer> checkQueue = new ArrayDeque<>();
        //增加起点方格
        checkQueue.add(_index);

        for (Integer poll = checkQueue.poll(); poll != null; poll = checkQueue.poll())
        {
            if (!checkedSet.add(poll))
                continue;

            TileMatch_BlockBaseInfo aroundBlock = _blockList.get(poll);
            if (aroundBlock == null)
                continue;

            //能否可连
            if (aroundBlock.getBlockId() != _block.getBlockId())
                continue;

            canLinkSet.add(poll);
            //周围的坐标
            List<Integer> aroundIndexList = TileMatchBlockList.getAroundIndex(poll, _aroundType);
            for (Integer index : aroundIndexList)
            {
                if (checkedSet.contains(index))
                    continue;

                checkQueue.add(index);
            }
        }
        return canLinkSet;
    }

    public static WCGPairInt getMapSize()
    {
        return RefTileMatchOther.Ref().tilematch_map_size;
    }

    /**
     * 有效下标是0-55
     * @param _index
     * @return
     */
    public static boolean indexIsValid(int _index)
    {
        return _index >= 0 && _index < getMapSize().first() * getMapSize().second();
    }

    /**
     * 计算给定方块所在位置
     * @param _index 下标
     * @return WCGPairInt 高，宽
     */
    public static WCGPairInt getPosXY(int _index)
    {
        //获得高、宽
        WCGPairInt mapSize = getMapSize();
        //列数
        int columnNum = mapSize.second();
        return new WCGPairInt(_index / columnNum, _index % columnNum);
    }

    /**
     * 计算给定位置所在下标
     * @param _posXY WCGPairInt 高，宽
     * @return int 下标
     */
    public static int getIndexByPosXY(WCGPairInt _posXY)
    {
        return getIndexByPosXY(_posXY.first(), _posXY.second());
    }

    public static int getIndexByPosXY(int row, int column)
    {
        //获得高、宽
        WCGPairInt mapSize = getMapSize();
        if (row < 0 || row >= mapSize.first())
        {
            return -1;
        }
        if (column < 0 || column >= mapSize.second())
        {
            return -1;
        }
        return row * mapSize.second() + column;
    }

    /**
     * 获取方向相连坐标
     * @param _index      起始点
     * @param _aroundType 方向
     * @return List<Integer>
     */
    public static List<Integer> getAroundIndex(int _index, ETileMatchAroundType _aroundType)
    {
        WCGPairInt map_size = getMapSize();
        int columnNum = map_size.second();
        return getAroundIndex(_index / columnNum, _index % columnNum, _aroundType);
    }

    /**
     * 计算给定位置 周围的坐标
     * @param _row        行
     * @param _column     列
     * @param _aroundType 检测方向
     * @return List<Integer> 周围的坐标列表
     */
    public static List<Integer> getAroundIndex(int _row, int _column, ETileMatchAroundType _aroundType)
    {
        //获得高、宽
        WCGPairInt map_size = getMapSize();

        List<Integer> list = new ArrayList<>();
        switch (_aroundType)
        {
            case ROW:
                //左
                if (_column - 1 >= 0)
                {
                    list.add(getIndexByPosXY(_row, _column - 1));
                }
                //右
                if (_column + 1 < map_size.second())
                {
                    list.add(getIndexByPosXY(_row, _column + 1));
                }
                break;
            case COLUMN:
                //上
                if (_row + 1 < map_size.first())
                {
                    list.add(getIndexByPosXY(_row + 1, _column));
                }
                //下
                if (_row - 1 >= 0)
                {
                    list.add(getIndexByPosXY(_row - 1, _column));
                }
                break;
            case ROW_AND_COLUMN:
                //上
                if (_row + 1 < map_size.first())
                {
                    list.add(getIndexByPosXY(_row + 1, _column));
                }
                //下
                if (_row - 1 >= 0)
                {
                    list.add(getIndexByPosXY(_row - 1, _column));
                }
                //左
                if (_column - 1 >= 0)
                {
                    list.add(getIndexByPosXY(_row, _column - 1));
                }
                //右
                if (_column + 1 < map_size.second())
                {
                    list.add(getIndexByPosXY(_row, _column + 1));
                }
            default:
                break;
        }

        return list;
    }

    public static void columnRowForeach(ToIntBiFunction<Integer, Integer> _dealFunc)
    {
        //正常顺序下标遍历
        columnRowForeach(false, true, _dealFunc);
    }

    /**
     * 给定遍历方向进行遍历
     * @param _top      是否从顶部开始
     * @param _left     是否从左边开始
     * @param _dealFunc 返回值 < 0 跳出循环
     *                  返回值 = 0 跳出本行循环
     *                  返回值 > 0 继续循环
     */
    public static void columnRowForeach(boolean _top, boolean _left, ToIntBiFunction<Integer, Integer> _dealFunc)
    {
        //按列处理 向上冒泡
        int row = getMapSize().first();//行数
        int column = getMapSize().second();//列数

        if (_top)
        {
            //从上到下
            for (int i = row - 1; i >= 0; i--)
            {
                if (_left)
                {
                    //从左到右
                    for (int j = 0; j < column; j++)
                    {
                        int ret = _dealFunc.applyAsInt(i, j);
                        if (ret < 0)
                        {
                            return;
                        }
                    }
                } else
                {
                    //从右到左
                    for (int j = column - 1; j >= 0; j--)
                    {
                        int ret = _dealFunc.applyAsInt(i, j);
                        if (ret < 0)
                        {
                            return;
                        }
                    }
                }
            }
        } else
        {
            //从下到上
            for (int i = 0; i < row; i++)
            {
                if (_left)
                {
                    //从左到右
                    for (int j = 0; j < column; j++)
                    {
                        int ret = _dealFunc.applyAsInt(i, j);
                        if (ret < 0)
                        {
                            return;
                        }
                    }
                } else
                {
                    //从右到左
                    for (int j = column - 1; j >= 0; j--)
                    {
                        int ret = _dealFunc.applyAsInt(i, j);
                        if (ret < 0)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    /**
     * 计算附近接壤位置
     * 八个个方向
     * @param _row    指定位置
     * @param _column 指定位置
     * @param _round  半径
     * @return 接壤位置 包括指定位置
     */
    public static ArrayList<Integer> calPosNearPosList_x(int _row, int _column, int _round)
    {
        ArrayList<Integer> nearPosList = new ArrayList<>();
        for (int i = _row - _round; i <= _row + _round; i++)
        {
            for (int j = _column - _round; j <= _column + _round; j++)
            {
                nearPosList.add(TileMatchBlockList.getIndexByPosXY(i, j));
            }
        }
        return nearPosList;
    }

    public int size()
    {
        return _m_optLinkedList.size();
    }

    /**
     * 获取源数据
     * @return 数据库数据来源
     */
    public byte[] getOriByteBuffer()
    {
        return _m_getOriMapBytes.get();
    }

    /**
     * 获取内存中数据结构
     * @return TileMatch_BlockList
     */
    public TileMatch_BlockBaseList getOriStructureList()
    {
        return _m_oriStructureList;
    }

    public Consumer<byte[]> getSaveOriMapBytes()
    {
        return _m_saveOriMapBytes;
    }

    /**
     * 设置格子列表
     * @param _blockList
     */
    public void setBlockList(List<Integer> _blockList)
    {
        clear();

        for (Integer blockId : _blockList)
        {
            add(new TileMatch_BlockBaseInfo(blockId, 0));
        }
    }

    public void clear()
    {
        _m_optLinkedList.clear();
        _m_bHasChange = true;
    }

    /**
     * 计算给定方块所在位置
     * @param _v 给定方块
     * @return WCGPairInt 高，宽
     */
    public WCGPairInt getPosXY(TileMatch_BlockBaseInfo _v)
    {
        int index = _m_optLinkedList.indexOf(_v);
        return getPosXY(index);
    }

    /**
     * 交换两个数据
     * @param _beginIndex 开始下标
     * @param _endIndex   结束下标
     */
    public void switchBlock(int _beginIndex, int _endIndex)
    {
        _m_bHasChange = true;
        if (!indexIsValid(_beginIndex))
        {
            return;
        }
        if (!indexIsValid(_endIndex))
        {
            return;
        }
        Collections.swap(_m_optLinkedList, _beginIndex, _endIndex);
    }

    /**
     * 计算每种类型方块的数量
     * @return Map<Long, Integer>
     */
    public Map<Integer, Integer> calBlockSize()
    {
        Map<Integer, Integer> blockSize = new HashMap<>();
        for (TileMatch_BlockBaseInfo block : _m_optLinkedList)
        {
            if (block == null)
            {
                continue;
            }
            Integer size = blockSize.computeIfAbsent(block.getBlockId(), k -> 0);
            blockSize.put(block.getBlockId(), size + 1);
        }
        return blockSize;
    }

    @Override
    public void add(TileMatch_BlockBaseInfo _v)
    {
        if (_m_optLinkedList.size() >= getMapSize().first() * getMapSize().second())
        {
            CommLog.error("TileMatchBlockList add err", new Exception());
            return;
        }

        _m_bHasChange = true;
        _m_optLinkedList.add(_v);
    }

    @Override
    public Integer getIndex(TileMatch_BlockBaseInfo _v)
    {
        return _m_optLinkedList.indexOf(_v);
    }

    @Override
    public Collection<TileMatch_BlockBaseInfo> getList()
    {
        return _m_optLinkedList;
    }

    @Override
    public void set(Integer _key, TileMatch_BlockBaseInfo _v)
    {
        //key不合法
        if (!indexIsValid(_key))
        {
            CommLog.error("TileMatchBlockList set err key:{}", _key, new Exception());
            return;
        }
        _m_bHasChange = true;
        _m_optLinkedList.set(_key, _v);
    }

    @Override
    public void saveAllMark()
    {
        if (!_m_bHasChange)
        {
            return;
        }
        _m_bHasChange = false;
        _m_oriStructureList.getBlockList().clear();
        _m_oriStructureList.getBlockList().addAll(_m_optLinkedList);
        getSaveOriMapBytes().accept(CommonFunc.ByteBfferToBytes(_m_oriStructureList.makePackage()));
    }

    @Override
    public String toString()
    {
        //生成一个阵列
        StringBuilder m = new StringBuilder();

        int row = getMapSize().first();//行数
        int column = getMapSize().second();//列数
        for (int i = row - 1; i >= 0; i--)
        {
            m.append("\n");
            for (int j = 0; j < column; j++)
            {
                int indexByPosXY = TileMatchBlockList.getIndexByPosXY(i, j);
                TileMatch_BlockBaseInfo block = _m_optLinkedList.get(indexByPosXY);
                if (block == null)
                {
                    // 为空值添加占位符，保持格式一致
                    m.append(String.format("[%2d]--  ", indexByPosXY));
                } else
                {
                    m.append(String.format("[%2d]%-2d  ", indexByPosXY, block.getBlockId()));
                }
            }
            m.append("\n");
        }
        return "TileMatchBlockList{" +
                " _m_bHasChange=" + _m_bHasChange +
                ", map=" + m +
                '}';
    }

    /**
     * 通过格子类型获取位置列表
     * @param _type
     * @return
     */
    public List<Integer> getIndexListByBlockType(ETileMatch_BlockType _type)
    {
        List<Integer> blockList = new ArrayList<>();
        for (TileMatch_BlockBaseInfo block : _m_optLinkedList)
        {
            RefTileMatchBlock refBlock = RefTileMatchBlock.getMgr().get(block.getBlockId());
            if (refBlock.type == _type)
            {
                blockList.add(_m_optLinkedList.indexOf(block));
            }
        }
        return blockList;
    }

    /**
     * 获取十字下标
     * @param _pos     中心位置坐标
     * @param rowWidth 横向宽度（横向总列数）
     * @param colWidth 纵向宽度（纵向总行数）
     * @return ArrayList<Integer>
     */
    public static ArrayList<Integer> calTenIndex(WCGPairInt _pos, int rowWidth, int colWidth)
    {
        ArrayList<Integer> res = new ArrayList<>();
        int centerRow = _pos.first();
        int centerCol = _pos.second();
        WCGPairInt mapSize = getMapSize();

        // 计算横向范围
        int halfRowWidth = (rowWidth - 1) / 2;
        int startCol = Math.max(0, centerCol - halfRowWidth);
        int endCol = Math.min(mapSize.second() - 1, centerCol + halfRowWidth);

        // 添加横向指定列的所有格子（整列）
        for (int col = startCol; col <= endCol; col++)
        {
            for (int row = 0; row < mapSize.first(); row++)
            {
                res.add(getIndexByPosXY(row, col));
            }
        }

        // 计算纵向范围
        int halfColWidth = (colWidth - 1) / 2;
        int startRow = Math.max(0, centerRow - halfColWidth);
        int endRow = Math.min(mapSize.first() - 1, centerRow + halfColWidth);

        // 添加纵向指定行的所有格子（整行）
        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = 0; col < mapSize.second(); col++)
            {
                // 跳过已经添加过的格子（列和行的交叉点）
                if (col >= startCol && col <= endCol)
                {
                    continue;
                }
                res.add(getIndexByPosXY(row, col));
            }
        }

        return res;
    }
}
