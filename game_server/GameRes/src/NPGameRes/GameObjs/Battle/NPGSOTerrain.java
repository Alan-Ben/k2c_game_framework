package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommFile;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

/// <summary>
/// 单个地图的路点数据
/// </summary>
public class NPGSOTerrain
{
    public int width;//地形宽度
    public int height;//地形长度
    public List<Short> pointList = new ArrayList<Short>();//地形节点通过标识

    //转成二维地图节点数据
    public short[][] makeMapData()
    {
        short[][] res = new short[width][height];
        for (int idx = 0; idx < pointList.size(); ++idx)
        {
            int x = idx / height;
            int z = idx % height;
            res[x][z] = pointList.get(idx);
        }
        return res;
    }

    public short getHeight(int _x, int _z)
    {
        int idx = (_x * height) + _z;
        return pointList.get(idx);
    }

    public static String GetAssetPath(int _mainId)
    {
        return "terrain/terrain_" + _mainId;
    }

    public static String GetObjName(int _mainId)
    {
        return "terrain_" + _mainId;
    }

    public static NPGSOTerrain read(String filePath)
    {
        List<String> lines = CommFile.getLinesFromFile(filePath);
        if (lines.size() < 3)
        {
            CommLog.error("terrain file [{}] line less than 3", filePath);
            return null;
        }
        try
        {
            int width = Integer.parseInt(lines.get(0).trim());
            int height = Integer.parseInt(lines.get(1).trim());
            String sData = lines.get(2);
            String[] points = CommonFunc.charSplit(sData, '|');
            if (points.length < width * height)
            {
                CommLog.error("WCGSOTerrain len less than width*height :", points.length);
                return null;
            }


            NPGSOTerrain terrain = new NPGSOTerrain();
            terrain.width = width;
            terrain.height = height;
            for (int i = 0; i < width * height; i++)
            {
                terrain.pointList.add(Short.parseShort(points[i]));
            }
            return terrain;
        } catch (Exception e)
        {

            CommLog.error("read terrain  file failed  [{}] ", filePath, e);
            return null;
        }

    }
}
