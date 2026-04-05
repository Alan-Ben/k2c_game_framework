package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGMoveType;

public interface _IDLBasicMapData
{
    int mapWidth();

    int mapLength();

    //获取对应点的高度
    short getPointHeight(DLMapPos _pos);

    short getPointHeight(int _x, int _y);

    //获取高度
    float getPosHeight(float _x, float _y, int _endX, int _endZ);

    short getShowPointHeight(DLMapPos _pos);

    short getShowPointHeight(int _x, int _y);

    /**
     * 点是否可走 _weight - 权重(只会搜索权重更低的点)
     */
    boolean walkable(int _x, int _y, short _srcHeight, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide);

    /**
     * 点是否可视
     */
    boolean viewable(int _x, int _y, short _srcHeight, EWCGMoveType _moveType);
}
