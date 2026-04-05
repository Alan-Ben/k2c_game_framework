package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.AStar.DLAStarRes;
import ResCommon.Allocator._IResAllocator;
import WCGCommon.Enum.NPEnum.EWCGMoveType;


public class DLPathLineCheckCommonFun
{
    /*********************
     * 根据线段的起始坐标与结束坐标，计算整个线段经过的坐标点队列
     *
     * @author alzq.z
     * @time Jul 9, 2013 10:29:05 PM
     */
    public static void getLinePath(DLMapPos _startPoint, DLMapPos _endPoint, DLAStarRes _resObj, WCGFloatValue _reachDis, _IResAllocator _alloc)
    {
        _alloc.beginStack();
        try
        {
            //计算朝向
            WCGVector direction = _alloc.newVector(_endPoint.x - _startPoint.x, _endPoint.z - _startPoint.z);
            WCGVector normalizeDirection = direction.normalized(_alloc);

            //起点终点一致则直接返回
            if (_startPoint.x == _endPoint.x && _startPoint.z == _endPoint.z)
            {
                _resObj.AddResPoint(_startPoint);
                return;
            }

            //横向在一个区域内平移
            if (_startPoint.x == _endPoint.x)
            {
                //方向垂直
                int mapX = _startPoint.x;

                //逐点进行处理
                if (_endPoint.z > _startPoint.z)
                {
                    //向上
                    for (int mapZ = _startPoint.z; mapZ <= _endPoint.z; mapZ++)
                    {
                        _resObj.AddResPoint(mapX, mapZ);

                        //计算距离是否在范围内
                        if (_endPoint.z - mapZ <= _reachDis.iV())
                            return;
                    }
                } else
                {
                    //向下
                    for (int mapZ = _startPoint.z; mapZ >= _endPoint.z; mapZ--)
                    {
                        _resObj.AddResPoint(mapX, mapZ);

                        //计算距离是否在范围内
                        if (mapZ - _endPoint.z <= _reachDis.iV())
                            return;
                    }
                }
            } else if (_startPoint.z == _endPoint.z)
            {
                //方向水平
                int mapZ = _startPoint.z;

                //逐点进行处理
                if (_endPoint.x > _startPoint.x)
                {
                    //向上
                    for (int mapX = _startPoint.x; mapX <= _endPoint.x; mapX++)
                    {
                        _resObj.AddResPoint(mapX, mapZ);

                        //计算距离是否在范围内
                        if (_endPoint.x - mapX <= _reachDis.iV())
                            return;
                    }
                } else
                {
                    //向下
                    for (int mapX = _startPoint.x; mapX >= _endPoint.x; mapX--)
                    {
                        _resObj.AddResPoint(mapX, mapZ);

                        //计算距离是否在范围内
                        if (mapX - _endPoint.x <= _reachDis.iV())
                            return;
                    }
                }
            } else
            {
                //方向倾斜
                //计算直线的倍数系数a,d
                //公式a * x + d = z
                // x = (z - d) / a
                WCGFloatValue a = _alloc.newFloatValue();
                if (_endPoint.x != _startPoint.x)
                    a = _alloc.newFloatValue(_endPoint.z - _startPoint.z).div(_endPoint.x - _startPoint.x, _alloc);
                WCGFloatValue d = a.mul(_startPoint.x, _alloc).subed(_startPoint.z, _alloc);

                //根据方向的单位叠加
                //获取当前的起始坐标位置信息
                WCGFloatValue curX = _alloc.newFloatValue(_startPoint.x);
                WCGFloatValue curZ = _alloc.newFloatValue(_startPoint.z);
                int curMapX = _startPoint.x;
                int curMapZ = _startPoint.z;

                //先记录开始点
                _resObj.AddResPoint(curMapX, curMapZ);

                WCGFloatValue sqrRDis = _reachDis.pow2(_alloc);

                int preSqrDis = ((curMapX - _endPoint.x) * (curMapX - _endPoint.x)) + ((curMapZ - _endPoint.z) * (curMapZ - _endPoint.z));
                int curSqrDis = preSqrDis;

                int tempBoradZ;
                //逐个叠加到目标位置，超出判断距离则退出判断，并返回endpoint
                while (curSqrDis > sqrRDis.iV() && curSqrDis <= preSqrDis)
                {

                    preSqrDis = curSqrDis;
                    //计算下一个单位节点位置
                    curX = curX.add(normalizeDirection.x(), _alloc);
                    curZ = curZ.add(normalizeDirection.z(), _alloc);
                    int nextMapX = curX.roundIV();
                    int nextMapZ = curZ.roundIV();
                    //判断x和Z是否都变换了
                    if (curMapX != nextMapX && curMapZ != nextMapZ)
                    {
                        //X超过一个单位，计算临界点位置的z坐标
                        //当x由小到大，则需要取目标位置的X计算Z坐标
                        //当X由大到小，则需要取原始位置的X计算Z坐标
                        WCGFloatValue boardZ;
                        if (normalizeDirection.x().v() > 0)
                            boardZ = a.mul(nextMapX, _alloc).add(d, _alloc);
                        else
                            boardZ = a.mul(curMapX, _alloc).add(d, _alloc);
                        tempBoradZ = boardZ.roundIV();
                        //根据临界点处Z坐标是大于原先的z还是小于原先的z
                        //得出线段是穿越上半部分还是穿越下半部分
                        //比较时根据Z方向取不同的值
                        //当z由小到大，则需要取目标位置的Z
                        //当Z由大到小，则需要取原始位置的Z
                        if (normalizeDirection.z().v() > 0)
                        {
                            //Z由小到大，取目标位置Z进行判断,交叉点为中间点时先切换Z
                            if (tempBoradZ >= nextMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                _resObj.AddResPoint(curMapX, nextMapZ);
                                if (curMapX == _endPoint.x && nextMapZ == _endPoint.z)
                                    break;

                                _resObj.AddResPoint(nextMapX, nextMapZ);
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                _resObj.AddResPoint(nextMapX, curMapZ);
                                if (nextMapX == _endPoint.x && curMapZ == _endPoint.z)
                                    break;

                                _resObj.AddResPoint(nextMapX, nextMapZ);
                            }
                        } else
                        {
                            //Z由大到小，取原始位置Z进行判断,交叉点为中间点时先切换X
                            if (tempBoradZ >= curMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                _resObj.AddResPoint(nextMapX, curMapZ);
                                if (nextMapX == _endPoint.x && curMapZ == _endPoint.z)
                                    break;

                                _resObj.AddResPoint(nextMapX, nextMapZ);
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                _resObj.AddResPoint(curMapX, nextMapZ);
                                if (curMapX == _endPoint.x && nextMapZ == _endPoint.z)
                                    break;

                                _resObj.AddResPoint(nextMapX, nextMapZ);
                            }
                        }
                    }
                    //X和Z没有同时变换，则只需要在有坐标变换的时候简单针对前后两个点进行处理就可以
                    else if (curMapX != nextMapX || curMapZ != nextMapZ)
                    {
                        //当Z没更改，而X更改则只需要针对Z的变换进行处理
                        _resObj.AddResPoint(nextMapX, nextMapZ);
                    }

                    //通过判断则设置当前节点为下一节点
                    curMapX = nextMapX;
                    curMapZ = nextMapZ;

                    //计算当前点的距离
                    curSqrDis = ((curMapX - _endPoint.x) * (curMapX - _endPoint.x)) + ((curMapZ - _endPoint.z) * (curMapZ - _endPoint.z));
                }//while
            }
        } finally
        {
            _alloc.endStack();
        }

    }

    /*********************
     * 根据线段的起始坐标与结束坐标，从起始点到结束点线段中是否有凸出部分无法穿透
     * 带入允许跨越的高度，本身算法中将忽略2米的高度差，同时加上高度部分的高度差
     *
     * @author alzq.z
     * @time Jul 9, 2013 10:29:05 PM
     */
    public static boolean lineVisable(int _startX, int _startZ, int _endX, int _endZ, _IDLBasicMapData _map, EWCGMoveType _moveType, _IResAllocator _alloc)
    {
        _alloc.beginStack();
        try
        {
            //计算朝向
            WCGVector direction = _alloc.newVector(_endX - _startX, _endZ - _startZ);
            WCGVector normalizeDirection = direction.normalized(_alloc);

            //获取初始点高度
            short srcHeight = _map.getPointHeight(_startX, _startZ);

            if (_startX == _endX && _startZ == _endZ)
                return true;

            if (_startX == _endX)
            {
                //方向垂直
                int mapX = _startX;

                //逐点进行处理
                int mapZ = _startZ;
                if (_endZ > _startZ)
                {
                    //向上
                    for (; mapZ <= _endZ; mapZ++)
                    {
                        //计算高度
                        if (!judgePointVisable(mapX, mapZ, srcHeight, _map, _moveType))
                            return false;
                    }
                } else
                {
                    //向下
                    for (; mapZ >= _endZ; mapZ--)
                    {
                        //计算高度
                        if (!judgePointVisable(mapX, mapZ, srcHeight, _map, _moveType))
                            return false;
                    }
                }

                //都通过则成功
                return true;
            } else if (_startZ == _endZ)
            {
                //方向水平
                int mapZ = _startZ;

                //逐点进行处理
                int mapX = _startX;
                if (_endX > _startX)
                {
                    //向上
                    for (; mapX <= _endX; mapX++)
                    {
                        if (!judgePointVisable(mapX, mapZ, srcHeight, _map, _moveType))
                            return false;
                    }
                } else
                {
                    //向下
                    for (; mapX >= _endX; mapX--)
                    {
                        if (!judgePointVisable(mapX, mapZ, srcHeight, _map, _moveType))
                            return false;
                    }
                }

                //都通过则取结尾点
                return true;
            } else
            {
                //方向倾斜
                //计算直线的倍数系数a,d
                //公式a * x + d = z
                // x = (z - d) / a
                WCGFloatValue a = WCGFloatValue.makeFromValue(0, _alloc);
                if (_endX != _startX)
                    a = _alloc.newFloatValue(_endZ - _startZ).div(_endX - _startX, _alloc);
                WCGFloatValue d = a.mul(_startX, _alloc).subed(_startZ, _alloc);

                //根据方向的单位叠加
                //获取当前的起始坐标位置信息
                WCGFloatValue curX = _alloc.newFloatValue(_startX);
                WCGFloatValue curZ = _alloc.newFloatValue(_startZ);
                int curMapX = _startX;
                int curMapZ = _startZ;

                int preSqrDis = ((curMapX - _endX) * (curMapX - _endX)) + ((curMapZ - _endZ) * (curMapZ - _endZ));
                int curSqrDis = preSqrDis;

                int tempBoradZ;
                //逐个叠加到目标位置，超出判断距离则退出判断，并返回endpoint
                while (curSqrDis > 0 && curSqrDis <= preSqrDis)
                {
                    preSqrDis = curSqrDis;
                    //计算下一个单位节点位置
                    curX = curX.add(normalizeDirection.x(), _alloc);
                    curZ = curZ.add(normalizeDirection.z(), _alloc);
                    int nextMapX = curX.roundIV();
                    int nextMapZ = curZ.roundIV();

                    //判断x和Z是否都变换了
                    if (curMapX != nextMapX && curMapZ != nextMapZ)
                    {
                        //X超过一个单位，计算临界点位置的z坐标
                        //当x由小到大，则需要取目标位置的X计算Z坐标
                        //当X由大到小，则需要取原始位置的X计算Z坐标
                        WCGFloatValue boardZ;
                        if (normalizeDirection.x().v() > 0)
                            boardZ = a.mul(nextMapX, _alloc).add(d, _alloc);
                        else
                            boardZ = a.mul(curMapX, _alloc).add(d, _alloc);
                        tempBoradZ = boardZ.roundIV();
                        //根据临界点处Z坐标是大于原先的z还是小于原先的z
                        //得出线段是穿越上半部分还是穿越下半部分
                        //比较时根据Z方向取不同的值
                        //当z由小到大，则需要取目标位置的Z
                        //当Z由大到小，则需要取原始位置的Z
                        if (normalizeDirection.z().v() > 0)
                        {
                            //Z由小到大，取目标位置Z进行判断,交叉点为中间点时先切换Z
                            if (tempBoradZ >= nextMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointVisable(curMapX, nextMapZ, srcHeight, _map, _moveType))
                                    return false;

                                if (curMapX == _endX && nextMapZ == _endZ)
                                    return true;

                                if (!judgePointVisable(nextMapX, nextMapZ, srcHeight, _map, _moveType))
                                    return false;
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointVisable(nextMapX, curMapZ, srcHeight, _map, _moveType))
                                    return false;

                                if (nextMapX == _endX && curMapZ == _endZ)
                                    break;

                                if (!judgePointVisable(nextMapX, nextMapZ, srcHeight, _map, _moveType))
                                    return false;
                            }
                        } else
                        {
                            //Z由大到小，取原始位置Z进行判断,交叉点为中间点时先切换X
                            if (tempBoradZ >= curMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointVisable(nextMapX, curMapZ, srcHeight, _map, _moveType))
                                    return false;

                                if (nextMapX == _endX && curMapZ == _endZ)
                                    return true;

                                if (!judgePointVisable(nextMapX, nextMapZ, srcHeight, _map, _moveType))
                                    return false;
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointVisable(curMapX, nextMapZ, srcHeight, _map, _moveType))
                                    return false;

                                if (curMapX == _endX && nextMapZ == _endZ)
                                    break;

                                if (!judgePointVisable(nextMapX, nextMapZ, srcHeight, _map, _moveType))
                                    return false;
                            }
                        }
                    }
                    //X和Z没有同时变换，则只需要在有坐标变换的时候简单针对前后两个点进行处理就可以
                    else if (curMapX != nextMapX || curMapZ != nextMapZ)
                    {
                        //当Z没更改，而X更改则只需要针对Z的变换进行处理
                        if (!judgePointVisable(nextMapX, nextMapZ, srcHeight, _map, _moveType))
                            return false;
                    }

                    //通过判断则设置当前节点为下一节点
                    curMapX = nextMapX;
                    curMapZ = nextMapZ;

                    //计算当前点的距离
                    curSqrDis = ((curMapX - _endX) * (curMapX - _endX)) + ((curMapZ - _endZ) * (curMapZ - _endZ));
                }

                return true;
            }
        } finally
        {
            _alloc.endStack();
        }

    }

    /*********************
     * 根据线段的起始坐标与结束坐标，从起始点到结束点线段中是否有凸出部分无法行走
     * 可行走高度按照2米高低差进行计算
     *
     * @author alzq.z
     * @time Jul 9, 2013 10:29:05 PM
     */
    public static boolean lineWalkable(DLMapPos _startPoint, DLMapPos _endPoint, _IDLBasicMapData _map, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide, _IResAllocator _alloc)
    {
        //计算朝向
        _alloc.beginStack();
        try
        {
            WCGVector direction = _alloc.newVector(_endPoint.x - _startPoint.x, _endPoint.z - _startPoint.z);
            WCGVector normalizeDirection = direction.normalized(_alloc);

            if (_startPoint.x == _endPoint.x && _startPoint.z == _endPoint.z)
                return true;

            //获取初始点高度
            short srcHeight = _map.getPointHeight(_startPoint);

            if (_startPoint.x == _endPoint.x)
            {
                //方向垂直
                int mapX = _startPoint.x;

                //逐点进行处理
                int mapZ = _startPoint.z;
                if (_endPoint.z > _startPoint.z)
                {
                    //向上
                    for (; mapZ <= _endPoint.z; mapZ++)
                    {
                        //计算高度
                        if (!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                            return false;

                        //计算新点
                        srcHeight = _map.getPointHeight(mapX, mapZ);
                    }
                } else
                {
                    //向下
                    for (; mapZ >= _endPoint.z; mapZ--)
                    {
                        //计算高度
                        if (!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                            return false;

                        //计算新点
                        srcHeight = _map.getPointHeight(mapX, mapZ);
                    }
                }

                //都通过则取结尾点
                return true;
            } else if (_startPoint.z == _endPoint.z)
            {
                //方向水平
                int mapZ = _startPoint.z;

                //逐点进行处理
                int mapX = _startPoint.x;
                if (_endPoint.x > _startPoint.x)
                {
                    //向上
                    for (; mapX <= _endPoint.x; mapX++)
                    {
                        if (!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                            return false;

                        //计算新点
                        srcHeight = _map.getPointHeight(mapX, mapZ);
                    }
                } else
                {
                    //向下
                    for (; mapX >= _endPoint.x; mapX--)
                    {
                        if (!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                            return false;

                        //计算新点
                        srcHeight = _map.getPointHeight(mapX, mapZ);
                    }
                }

                //都通过则取结尾点
                return true;
            } else
            {
                //方向倾斜
                //计算直线的倍数系数a,d
                //公式a * x + d = z
                // x = (z - d) / a

                WCGFloatValue a = _alloc.newFloatValue();
                if (_endPoint.x != _startPoint.x)
                    a = _alloc.newFloatValue(_endPoint.z - _startPoint.z).div(_endPoint.x - _startPoint.x, _alloc);
                WCGFloatValue d = a.mul(_startPoint.x, _alloc).subed(_startPoint.z, _alloc);

                //根据方向的单位叠加
                //获取当前的起始坐标位置信息
                WCGFloatValue curX = _alloc.newFloatValue(_startPoint.x);
                WCGFloatValue curZ = _alloc.newFloatValue(_startPoint.z);
                int curMapX = _startPoint.x;
                int curMapZ = _startPoint.z;

                int preSqrDis = ((curMapX - _endPoint.x) * (curMapX - _endPoint.x)) + ((curMapZ - _endPoint.z) * (curMapZ - _endPoint.z));
                int curSqrDis = preSqrDis;

                int tempBoradZ;
                //逐个叠加到目标位置，超出判断距离则退出判断，并返回endpoint
                while (curSqrDis > 0 && curSqrDis <= preSqrDis)
                {
                    preSqrDis = curSqrDis;
                    //计算下一个单位节点位置
                    curX = curX.add(normalizeDirection.x(), _alloc);
                    curZ = curZ.add(normalizeDirection.z(), _alloc);
                    int nextMapX = curX.roundIV();
                    int nextMapZ = curZ.roundIV();

                    //判断x和Z是否都变换了
                    if (curMapX != nextMapX && curMapZ != nextMapZ)
                    {
                        //X超过一个单位，计算临界点位置的z坐标
                        //当x由小到大，则需要取目标位置的X计算Z坐标
                        //当X由大到小，则需要取原始位置的X计算Z坐标
                        WCGFloatValue boardZ;
                        if (normalizeDirection.x().v() > 0)
                            boardZ = a.mul(nextMapX, _alloc).add(d, _alloc);
                        else
                            boardZ = a.mul(curMapX, _alloc).add(d, _alloc);
                        tempBoradZ = boardZ.roundIV();
                        //根据临界点处Z坐标是大于原先的z还是小于原先的z
                        //得出线段是穿越上半部分还是穿越下半部分
                        //比较时根据Z方向取不同的值
                        //当z由小到大，则需要取目标位置的Z
                        //当Z由大到小，则需要取原始位置的Z
                        if (normalizeDirection.z().v() > 0)
                        {
                            //Z由小到大，取目标位置Z进行判断,交叉点为中间点时先切换Z
                            if (tempBoradZ >= nextMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointWalkable(curMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;

                                if (curMapX == _endPoint.x && nextMapZ == _endPoint.z)
                                    return true;

                                if (!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointWalkable(nextMapX, curMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;

                                if (nextMapX == _endPoint.x && curMapZ == _endPoint.z)
                                    break;

                                if (!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;
                            }
                        } else
                        {
                            //Z由大到小，取原始位置Z进行判断,交叉点为中间点时先切换X
                            if (tempBoradZ >= curMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointWalkable(nextMapX, curMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;

                                if (nextMapX == _endPoint.x && curMapZ == _endPoint.z)
                                    return true;

                                if (!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!judgePointWalkable(curMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;

                                if (curMapX == _endPoint.x && nextMapZ == _endPoint.z)
                                    break;

                                if (!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                                    return false;
                            }
                        }
                    }
                    //X和Z没有同时变换，则只需要在有坐标变换的时候简单针对前后两个点进行处理就可以
                    else if (curMapX != nextMapX || curMapZ != nextMapZ)
                    {
                        //当Z没更改，而X更改则只需要针对Z的变换进行处理
                        if (!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide))
                        {
                            return false;
                        }
                    }

                    //通过判断则设置当前节点为下一节点
                    curMapX = nextMapX;
                    curMapZ = nextMapZ;

                    //计算新点
                    srcHeight = _map.getPointHeight(curMapX, curMapZ);
                }//while
                return true;
            }
        } finally
        {
            _alloc.endStack();
        }

    }

    /*********************
     * 根据线段的起始坐标与结束坐标，从起始点到结束点线段中是否有凸出部分无法行走
     * 可行走高度按照2米高低差进行计算
     * 判断的时候带入的_judgeWalkCount表示需要判断是否可走的格子数，在部分情况下不需要全部判断
     *
     * @author alzq.z
     * @time Jul 9, 2013 10:29:05 PM
     */
    public static boolean lineWalkable(DLMapPos _startPoint, DLMapPos _endPoint, _IDLBasicMapData _map
            , WCGFloatValue _reachDis, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide, int _judgeWalkCount, _IResAllocator _alloc)
    {
        //计算朝向
        _alloc.beginStack();
        try
        {
            WCGVector direction = _alloc.newVector(_endPoint.x - _startPoint.x, _endPoint.z - _startPoint.z);
            WCGVector normalizeDirection = direction.normalized(_alloc);

            if (_startPoint.x == _endPoint.x && _startPoint.z == _endPoint.z)
                return true;

            //获取初始点高度
            short srcHeight = _map.getPointHeight(_startPoint);

            //是否在可触及区域内
            boolean inRange = false;
            if (_startPoint.x == _endPoint.x)
            {
                //方向垂直
                int mapX = _startPoint.x;

                //逐点进行处理
                int mapZ = _startPoint.z;
                if (_endPoint.z > _startPoint.z)
                {
                    //向上
                    for (; mapZ <= _endPoint.z; mapZ++)
                    {
                        //先判断此过程是否有效
                        if (!inRange && _judgeWalkCount >= 0)
                        {
                            //计算高度
                            if ((!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                    || !judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, true))
                                return false;

                            _judgeWalkCount--;
                            //计算新点
                            srcHeight = _map.getPointHeight(mapX, mapZ);
                        }

                        //计算距离是否在范围内
                        if (!inRange)
                        {
                            if (_endPoint.z - mapZ <= _reachDis.iV())
                                break;
                        }
                    }
                } else
                {
                    //向下
                    for (; mapZ >= _endPoint.z; mapZ--)
                    {
                        //先判断此过程是否有效
                        if (!inRange && _judgeWalkCount >= 0)
                        {
                            //计算高度
                            if ((!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                    || !judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, true))
                                return false;

                            _judgeWalkCount--;
                            //计算新点
                            srcHeight = _map.getPointHeight(mapX, mapZ);
                        }

                        //计算距离是否在范围内
                        if (!inRange)
                        {
                            if (mapZ - _endPoint.z <= _reachDis.iV())
                                break;
                        }
                    }
                }

                //都通过则成功
                return true;
            } else if (_startPoint.z == _endPoint.z)
            {
                //方向水平
                int mapZ = _startPoint.z;

                //逐点进行处理
                int mapX = _startPoint.x;
                if (_endPoint.x > _startPoint.x)
                {
                    //向上
                    for (; mapX <= _endPoint.x; mapX++)
                    {
                        //先判断此过程是否有效
                        if (!inRange && _judgeWalkCount >= 0)
                        {
                            //计算高度
                            if ((!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                    || !judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, true))
                                return false;

                            _judgeWalkCount--;
                            //计算新点
                            srcHeight = _map.getPointHeight(mapX, mapZ);
                        }

                        //计算距离是否在范围内
                        if (!inRange)
                        {
                            if (_endPoint.x - mapX <= _reachDis.iV())
                                break;
                        }
                    }
                } else
                {
                    //向下
                    for (; mapX >= _endPoint.x; mapX--)
                    {
                        //先判断此过程是否有效
                        if (!inRange)
                        {
                            //计算高度
                            if ((!judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                    || !judgePointWalkable(mapX, mapZ, srcHeight, _map, _weight, _moveType, true))
                                return false;

                            _judgeWalkCount--;
                            //计算新点
                            srcHeight = _map.getPointHeight(mapX, mapZ);
                        }

                        //计算距离是否在范围内
                        if (!inRange)
                        {
                            if (mapX - _endPoint.x <= _reachDis.iV())
                                break;
                        }
                    }
                }

                //都通过则取结尾点
                return true;
            } else
            {
                //方向倾斜
                //计算直线的倍数系数a,d
                //公式a * x + d = z
                // x = (z - d) / a

                WCGFloatValue a = _alloc.newFloatValue();
                if (_endPoint.x != _startPoint.x)
                    a = _alloc.newFloatValue(_endPoint.z - _startPoint.z).div(_endPoint.x - _startPoint.x, _alloc);
                WCGFloatValue d = a.mul(_startPoint.x, _alloc).subed(_startPoint.z, _alloc);

                //根据方向的单位叠加
                //获取当前的起始坐标位置信息
                WCGFloatValue curX = _alloc.newFloatValue(_startPoint.x);
                WCGFloatValue curZ = _alloc.newFloatValue(_startPoint.z);
                int curMapX = _startPoint.x;
                int curMapZ = _startPoint.z;

                WCGFloatValue sqrRDis = _reachDis.pow2(_alloc);

                int preSqrDis = ((curMapX - _endPoint.x) * (curMapX - _endPoint.x)) + ((curMapZ - _endPoint.z) * (curMapZ - _endPoint.z));
                int curSqrDis = preSqrDis;

                int tempBoradZ;
                //逐个叠加到目标位置，超出判断距离则退出判断，并返回endpoint
                while (curSqrDis > sqrRDis.iV() && curSqrDis <= preSqrDis)
                {
                    preSqrDis = curSqrDis;
                    //计算下一个单位节点位置
                    WCGFloatValue nextX = curX.add(normalizeDirection.x(), _alloc);
                    WCGFloatValue nextZ = curZ.add(normalizeDirection.z(), _alloc);
                    int nextMapX = nextX.roundIV();
                    int nextMapZ = nextZ.roundIV();

                    //判断x和Z是否都变换了
                    if (curMapX != nextMapX && curMapZ != nextMapZ)
                    {
                        //X超过一个单位，计算临界点位置的z坐标
                        //当x由小到大，则需要取目标位置的X计算Z坐标
                        //当X由大到小，则需要取原始位置的X计算Z坐标
                        WCGFloatValue boardZ;
                        if (normalizeDirection.x().v() > 0)
                            boardZ = a.mul(nextMapX, _alloc).add(d, _alloc);
                        else
                            boardZ = a.mul(curMapX, _alloc).add(d, _alloc);
                        tempBoradZ = boardZ.roundIV();
                        //根据临界点处Z坐标是大于原先的z还是小于原先的z
                        //得出线段是穿越上半部分还是穿越下半部分
                        //比较时根据Z方向取不同的值
                        //当z由小到大，则需要取目标位置的Z
                        //当Z由大到小，则需要取原始位置的Z
                        if (normalizeDirection.z().v() > 0)
                        {
                            //Z由小到大，取目标位置Z进行判断,交叉点为中间点时先切换Z
                            if (tempBoradZ >= nextMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!inRange)
                                {
                                    //计算高度
                                    if ((!judgePointWalkable(curMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(curMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;

                                    if (curMapX == _endPoint.x && nextMapZ == _endPoint.z)
                                        return true;

                                    //进行可视判断
                                    if ((!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;
                                }

                                _judgeWalkCount--;
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!inRange)
                                {
                                    //计算高度
                                    if ((!judgePointWalkable(nextMapX, curMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(nextMapX, curMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;

                                    if (nextMapX == _endPoint.x && curMapZ == _endPoint.z)
                                        return true;

                                    //进行可视判断
                                    if ((!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;
                                }

                                _judgeWalkCount--;
                            }
                        } else
                        {
                            //Z由大到小，取原始位置Z进行判断,交叉点为中间点时先切换X
                            if (tempBoradZ >= curMapZ)
                            {
                                //在目标位置Z以上，表示需要先移动X
                                //即先判断nextMapX, curMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!inRange)
                                {
                                    //计算高度
                                    if ((!judgePointWalkable(nextMapX, curMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(nextMapX, curMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;

                                    if (nextMapX == _endPoint.x && curMapZ == _endPoint.z)
                                        return true;

                                    //进行可视判断
                                    if ((!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;
                                }

                                _judgeWalkCount--;
                            } else
                            {
                                //在目标位置Z以下，表示需要先移动Z
                                //即先判断curMapX, nextMapZ的跳转
                                //之后判断下一个节点nextMapX, nextMapZ
                                if (!inRange)
                                {
                                    //计算高度
                                    if ((!judgePointWalkable(curMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(curMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;

                                    if (curMapX == _endPoint.x && nextMapZ == _endPoint.z)
                                        return true;

                                    if ((!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                            || !judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                        return false;
                                }

                                _judgeWalkCount--;
                            }
                        }
                    }
                    //X和Z没有同时变换，则只需要在有坐标变换的时候简单针对前后两个点进行处理就可以
                    else if (curMapX != nextMapX || curMapZ != nextMapZ)
                    {
                        //当Z没更改，而X更改则只需要针对Z的变换进行处理
                        if (!inRange)
                            //计算高度
                            if ((!judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, _ignoreCollide) && _judgeWalkCount >= 0)
                                    || !judgePointWalkable(nextMapX, nextMapZ, srcHeight, _map, _weight, _moveType, true))
                                return false;

                        _judgeWalkCount--;
                    }

                    //通过判断则设置当前节点为下一节点
                    curX = nextX;
                    curZ = nextZ;
                    curMapX = nextMapX;
                    curMapZ = nextMapZ;

                    srcHeight = _map.getPointHeight(curMapX, curMapZ);

                    //计算当前点的距离
                    curSqrDis = ((curMapX - _endPoint.x) * (curMapX - _endPoint.x)) + ((curMapZ - _endPoint.z) * (curMapZ - _endPoint.z));

                    //判断是否已经到达可触及区域
                    if (!inRange)
                    {
                        if (curSqrDis <= sqrRDis.iV())
                            break;
                    }
                }
                return true;
            }
        } finally
        {
            _alloc.endStack();
        }

    }

    /*********************
     * 判断对应点是否可视
     *
     * @author alzq.z
     * @time Jul 10, 2013 1:24:36 AM
     */
    public static boolean judgePointVisable(int _x, int _z, short _srcHeight, _IDLBasicMapData _map, EWCGMoveType _moveType)
    {
        if (!judgeInMap(_map, _x, _z))
            return false;

        return _map.viewable(_x, _z, _srcHeight, _moveType);
    }

    /*********************
     * 判断对应点是否可走
     *
     * @author alzq.z
     * @time Jul 10, 2013 1:24:36 AM
     */
    public static boolean judgePointWalkable(int _x, int _y, short _srcHeight, _IDLBasicMapData _map, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide)
    {
        if (!judgeInMap(_map, _x, _y))
            return false;

        return _map.walkable(_x, _y, _srcHeight, _weight, _moveType, _ignoreCollide);
    }

    /**********************
     * 判断坐标位置是否在地图内
     *
     * @author alzq.z
     * @time Jul 11, 2013 1:35:27 AM
     */
    public static boolean judgeInMap(_IDLBasicMapData _map, int _x, int _y)
    {
        if (_x >= 0 && _x < _map.mapWidth() && _y >= 0 && _y < _map.mapLength())
        {
            return true;
        }

        return false;
    }
}

