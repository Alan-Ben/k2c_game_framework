package NPGameRes.GameObjs.Battle.AStar;

import NPCommon.Util.CommonFunc;
import NPCommon.Util.Mathf;
import NPCommon.Util.StringFunc;
import NPGameRes.GameObjs.Battle.DLMapPos;
import NPGameRes.GameObjs.Battle.DLPathLineCheckCommonFun;
import NPGameRes.GameObjs.Battle._IDLBasicMapData;
import WCGCommon.Enum.NPEnum.EWCGMoveType;

import java.util.ArrayList;
import java.util.List;

public class DLAStarRes
{
    /**
     * 路径是否查找到
     */
    public boolean pathFound = false;
    public List<DLAstarPos> pathPointList = new ArrayList<DLAstarPos>();

    /**
     * 是否路径被剪切
     */
    private boolean _m_bIsPathCut = false;
    /**
     * 当前路径索引的下标节点
     */
    private short _m_iPathIdx = 0;

    public boolean isPathCut()
    {
        return _m_bIsPathCut;
    }

    public short _pathIdx()
    {
        return _m_iPathIdx;
    }

    public int PointCount()
    {
        return pathPointList == null ? 0 : pathPointList.size();
    }

    public DLMapPos getPosByIndex(int _index)
    {
        if (pathPointList == null || pathPointList.size() - 1 < _index)
            return null;

        //Copy一份返回而不是直接把源数据返回，防止数据被修改
        return new DLMapPos(pathPointList.get(_index));
    }

    public DLAstarPos curPos()
    {
        if (pathPointList.size() <= _m_iPathIdx)
            return null;

        return pathPointList.get(_m_iPathIdx);
    }

    public DLAstarPos getPos(int _addIdx)
    {
        if (pathPointList.size() <= _m_iPathIdx + _addIdx)
            return null;

        return pathPointList.get(_m_iPathIdx + _addIdx);
    }


    /***************
     * 设置结果
     **/
    public void setData(boolean _isPathFound, boolean _isPathCut, short _pathIdx)
    {
        pathFound = _isPathFound;
        _m_bIsPathCut = _isPathCut;
        _m_iPathIdx = _pathIdx;
    }

    /******************
     * 路径是否有效
     **/
    public boolean pathDataEnable()
    {
        if (!pathFound)
            return false;

        //判断是否剪切，是则判断剩余长度是否超过4，如不超过4则无效
        if (_m_bIsPathCut && _m_iPathIdx > 16)
            return false;
        return true;
    }

    /*****************
     * 检测路径长度，如路径超出长度则进行裁剪，避免路径过长导致的同步数据过大
     **/
    public void checkPathLength()
    {
        //没寻找到则不需要裁剪，因为下一次会重新寻路
        if (!pathFound)
            return;

        //超出路径点上限则裁剪
        if (pathPointList.size() > 20)
        {
            _m_bIsPathCut = true;
        } else
        {
            _m_bIsPathCut = false;
        }
    }

    /***************
     * 判断路径是否可走，只要有一个点可走则都可走
     **/
    public boolean isPathEnable(short _srcHeight, _IDLBasicMapData _map, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide)
    {
        if (pathPointList.size() <= 1 + _m_iPathIdx)
            return false;

        DLAstarPos fuPos1 = getPos(1);

        if (null == fuPos1)
            return false;

        //只要一个可走则可走
        if (DLPathLineCheckCommonFun.judgePointWalkable(fuPos1.x, fuPos1.z, _srcHeight, _map, _weight, _moveType, _ignoreCollide))
            return true;

        return false;
    }

    public DLMapPos getNextPos()
    {
        if (pathPointList.size() <= 1 + _m_iPathIdx)
        {
            return null;
        }
        //判断是否只有一个可走点
        else if (pathPointList.size() == 2 + _m_iPathIdx)
        {
            //直接返回第一个点，之前已经判断过第一个点可走
            return _moveToNextPoint((short) 1);
        } else
        {
            //获取对应点
            DLAstarPos tmpPos = getPos(2);

            //判断第二个路径点是否可走
            if (Mathf.Abs(tmpPos.x - pathPointList.get(_m_iPathIdx).x) <= 1
                    && Mathf.Abs(tmpPos.z - pathPointList.get(_m_iPathIdx).z) <= 1)
            {
                return _moveToNextPoint((short) 2);
            } else
            {
                return _moveToNextPoint((short) 1);
            }
        }

    }

    public DLMapPos getNextPos(short _srcHeight, _IDLBasicMapData _map, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide)
    {
        if (pathPointList.size() <= 1 + _m_iPathIdx)
        {
            return null;
        }
        //判断是否只有一个可走点
        else if (pathPointList.size() == 2 + _m_iPathIdx)
        {
            //直接返回第一个点，之前已经判断过第一个点可走
            return _moveToNextPoint((short) 1);
        } else
        {
            //获取对应点
            DLAstarPos tmpPos = getPos(2);

            //判断第二个路径点是否可走
            if (DLPathLineCheckCommonFun.judgePointWalkable(tmpPos.x, tmpPos.z, _srcHeight, _map, _weight, _moveType, _ignoreCollide)
                    && Mathf.Abs(tmpPos.x - pathPointList.get(_m_iPathIdx).x) <= 1
                    && Mathf.Abs(tmpPos.z - pathPointList.get(_m_iPathIdx).z) <= 1)
            {
                return _moveToNextPoint((short) 2);
            } else
            {
                return _moveToNextPoint((short) 1);
            }
        }
    }


    public DLMapPos getNextFlyPos()
    {
        if (pathPointList.size() <= 1 + _m_iPathIdx)
        {
            return null;
        }
        //判断是否只有一个可走点
        else if (pathPointList.size() == 2 + _m_iPathIdx)
        {
            //直接返回第一个点，之前已经判断过第一个点可走
            return _moveToNextPoint((short) 1);
        } else
        {
            if (pathPointList.size() >= 3 + _m_iPathIdx)
            {
                //获取对应点
                DLAstarPos tmpPos = getPos(2);

                //判断第二个路径点是否可走
                if (Mathf.Abs(tmpPos.x - pathPointList.get(_m_iPathIdx).x) <= 1
                        && Mathf.Abs(tmpPos.z - pathPointList.get(_m_iPathIdx).z) <= 1)
                {
                    return _moveToNextPoint((short) 2);
                }
            }
            if (pathPointList.size() >= 4 + _m_iPathIdx)
            {
                //获取对应点
                DLAstarPos tmpPos = getPos(3);

                //判断第三个路径点是否可走
                if (Mathf.Abs(tmpPos.x - pathPointList.get(_m_iPathIdx).x) <= 2
                        && Mathf.Abs(tmpPos.z - pathPointList.get(_m_iPathIdx).z) <= 2)
                {
                    return _moveToNextPoint((short) 3);
                }
            }

            return _moveToNextPoint((short) 1);
        }
    }

    public DLMapPos getNextFlyPos(short _srcHeight, _IDLBasicMapData _map, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide)
    {
        if (pathPointList.size() <= 1 + _m_iPathIdx)
        {
            return null;
        }
        //判断是否只有一个可走点
        else if (pathPointList.size() == 2 + _m_iPathIdx)
        {
            //直接返回第一个点，之前已经判断过第一个点可走
            return _moveToNextPoint((short) 1);
        } else
        {
            if (pathPointList.size() >= 3 + _m_iPathIdx)
            {
                //获取对应点
                DLAstarPos tmpPos = getPos(2);
                //判断是否可走，不可走则直接返回第一个点
                if (!DLPathLineCheckCommonFun.judgePointWalkable(tmpPos.x, tmpPos.z, _srcHeight, _map, _weight, _moveType, _ignoreCollide))
                {
                    //直接返回第一个点，之前已经判断过第一个点可走
                    return _moveToNextPoint((short) 1);
                }

                //判断第二个路径点是否可走
                if (Mathf.Abs(tmpPos.x - pathPointList.get(_m_iPathIdx).x) <= 1
                        && Mathf.Abs(tmpPos.z - pathPointList.get(_m_iPathIdx).z) <= 1)
                {
                    return _moveToNextPoint((short) 2);
                }
            }
            if (pathPointList.size() >= 4 + _m_iPathIdx)
            {
                //获取对应点
                DLAstarPos tmpPos = getPos(3);
                //判断是否可走，不可走则直接返回第一个点
                if (!DLPathLineCheckCommonFun.judgePointWalkable(tmpPos.x, tmpPos.z, _srcHeight, _map, _weight, _moveType, _ignoreCollide))
                {
                    //直接返回第一个点，之前已经判断过第一个点可走
                    return _moveToNextPoint((short) 1);
                }

                //判断第三个路径点是否可走
                if (Mathf.Abs(tmpPos.x - pathPointList.get(_m_iPathIdx).x) <= 2
                        && Mathf.Abs(tmpPos.z - pathPointList.get(_m_iPathIdx).z) <= 2)
                {
                    return _moveToNextPoint((short) 3);

                }
            }

            return _moveToNextPoint((short) 1);
        }
    }

    //便宜并返回新索引坐标
    protected DLMapPos _moveToNextPoint(short _jumpIdx)
    {
        _m_iPathIdx += _jumpIdx;
        return new DLMapPos(pathPointList.get(_m_iPathIdx));
    }

    /**
     * 插入新结果点到开始
     */
    public void insertNewResPoint(int _x, int _z)
    {
        DLAstarPos pos = new DLAstarPos();
        pos.set(_x, _z);

        pathPointList.add(0, pos);
    }

    /**
     * 插入新结果点到结尾
     */
    public void AddResPoint(DLMapPos _pos)
    {
        DLAstarPos pos = new DLAstarPos();
        pos.set(_pos.x, _pos.z);

        pathPointList.add(pos);
    }

    public void AddResPoint(int _x, int _z)
    {
        DLAstarPos pos = new DLAstarPos();
        pos.set(_x, _z);

        pathPointList.add(pos);
    }

    /*************
     * 重置内容
     **/
    public void reset()
    {
        pathFound = false;
        pathPointList.clear();
        _m_bIsPathCut = false;
        _m_iPathIdx = 0;

    }

    @Override
    public String toString()
    {
        return String.format("寻路结果：%s", StringFunc.joinString(",", CommonFunc.toStringList(this.pathPointList)));
    }


}
