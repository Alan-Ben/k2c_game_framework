package NPGameRes.GameObjs.Battle.AStar;

import NPGameRes.GameObjs.Battle.DLMapPos;

public class DLAStarNode
{
    /**
     * 指向本节点的上一节点对象
     */
    public DLAStarNode parentNode;

    public int mapPosX;
    public int mapPosZ;

    /**
     * 对应点高度
     */
    public short height;

    /**
     * A星算法中权重比对对象
     */
    public int f;
    /**
     * 已经行进的距离系数
     */
    public int g;
    /**
     * 附加计算本节点距离目标权重的附加参数
     */
    public int h;

    /**
     * 是否已经经过处理
     */
    public boolean dealed;

    public DLAStarNode()
    {
        mapPosX = 0;
        mapPosZ = 0;
        height = 0;
        parentNode = null;
        g = 0;
        h = 0;
        f = 0;
        dealed = false;
    }
    //public DLAStarNode(DLMapPos _pos, DLAStarNode _parentNode, DLMapPos _destination)
    //{
    //    mapPos = _pos.clone();

    //    //创建对应位置标记
    //    positionTag = mapPos.getPositionTag();

    //    parentNode = _parentNode;
    //    g = 0;

    //    //当有父节点时，行进距离系数需要计算
    //    if (null != parentNode)
    //        g = parentNode.g + 1;

    //    //计算距离目标的权重系数
    //    h = DLAStarCommonFun.manhattan(mapPos, _destination);

    //    //将两个参数混合后得出最终权重系数
    //    f = g + h;

    //    dealed = false;
    //}

    /**************
     * 设置节点信息
     **/
    public void setNodeInfo(int _x, int _z, short _height, DLAStarNode _parentNode, DLMapPos _destination)
    {
        mapPosX = _x;
        mapPosZ = _z;

        //创建对应位置标记
        height = _height;
        parentNode = _parentNode;
        g = 0;

        //当有父节点时，行进距离系数需要计算
        if (null != parentNode)
            g = parentNode.g + 1;

        //计算距离目标的权重系数
        h = DLAStarCommonFun.manhattan(mapPosX, mapPosZ, _destination);

        //将两个参数混合后得出最终权重系数
        f = g + h;

        dealed = false;
    }

    /*************
     * 重置信息
     **/
    public void reset()
    {
        mapPosX = 0;
        mapPosZ = 0;
        parentNode = null;
        g = 0;
        h = 0;
        f = 0;
        dealed = false;
    }

    /**********************
     * 尝试修改父节点，当父节点的行进距离更短时需要修改
     *
     * @author alzq.z
     * @time Jul 11, 2013 12:34:22 AM
     */
    public boolean tryChgParentNode(DLAStarNode _parentNode)
    {
        int newG = _parentNode.g + 1;
        if (g > newG)
        {
            //此时修改成功
            parentNode = _parentNode;
            g = newG;
            f = g + h;

            return true;
        } else
        {
            return false;
        }
    }

    /******************
     * 设置节点未处理
     *
     * @author alzq.z
     * @time Aug 1, 2013 12:46:46 AM
     */
    public void setUndeal()
    {
        dealed = false;
    }
}
