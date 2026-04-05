package NPGameRes.GameObjs.Battle.AStar;

import java.util.ArrayList;

/**
 * 一个X坐标上的所有点的容器
 */
public class DLAStarNodeXContainer
{
    private int _m_iX;
    private ArrayList<DLAStarNode> _m_lNodeList;

    public DLAStarNodeXContainer()
    {
        _m_iX = 0;
        _m_lNodeList = new ArrayList<DLAStarNode>();
    }

    public int getX()
    {
        return _m_iX;
    }

    //检索对应位置的坐标
    public DLAStarNode getNode(int _z)
    {
        for (int i = 0; i < _m_lNodeList.size(); i++)
        {
            if (_m_lNodeList.get(i).mapPosZ == _z)
                return _m_lNodeList.get(i);
        }

        return null;
    }

    //设置X
    public void setX(int _x)
    {
        _m_iX = _x;
    }

    //添加节点
    public void addNode(DLAStarNode _node)
    {
        _m_lNodeList.add(_node);
    }

    public void reset()
    {
        _m_iX = 0;
        _m_lNodeList.clear();
    }
}
