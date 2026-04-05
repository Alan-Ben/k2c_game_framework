package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Battle.*;

import java.util.ArrayList;
import java.util.List;

/**
 * @author scott 地图 信 息
 */
public class WCGMapRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return mapId;
    }

    public long mapId; // 地图ID
    public String name;// 地图名字
    public String desc;// 地图介绍
    public String icon;// 地图图标

    public long scene_id;

    public List<WCGCampObj> camp_list = new ArrayList<>();//阵营势力列表
    public List<WCGMapGroupObj> groupList = new ArrayList<>();//组列表

    public ArrayList<Integer> player_group_list;//玩家组别列表

    public ArrayList<WCGSegmentObj> segmentList;// 地图的所有线段

    public List<NPPointObj> pointList;//地图上所有寻路点

    public ArrayList<NPAreaObj> areaList;// 区域列表

    public long surrender_time;//战局开始多久后可以发起投降.单位(毫秒) 
    public long surrender_cd;//发起投降后多久可以再次投降.单位(毫秒)

    //后续初始化部分
    private RefScene _m_rsSceneRef;

    public RefScene sceneRef()
    {
        if (null == _m_rsSceneRef)
            _m_rsSceneRef = RefScene.getMgr().get(scene_id);

        return _m_rsSceneRef;
    }


    public NPPointObj getPoint(int _pointId)
    {
        for (int i = 0; i < pointList.size(); i++)
        {
            NPPointObj pointObj = pointList.get(i);
            if (pointObj.point_id == _pointId)
                return pointObj;
        }
        return null;
    }

    public WCGCampObj GetCampObj(int _campId)
    {
        for (int i = 0; i < camp_list.size(); i++)
        {
            if (camp_list.get(i).camp_id == _campId)
            {
                return camp_list.get(i);
            }
        }
        return null;
    }

    //获取集合对应的阵营信息
    public WCGCampObj GetGroupCampObj(int _groupId)
    {
        for (int i = 0; i < camp_list.size(); i++)
        {
            if (camp_list.get(i).group_list.contains(_groupId))
            {
                return camp_list.get(i);
            }
        }
        return null;
    }

    public WCGMapGroupObj GetGroupObj(int _groupId)
    {
        for (int i = 0; i < groupList.size(); i++)
        {
            if (groupList.get(i).groupId == _groupId)
            {
                return groupList.get(i);
            }
        }
        return null;
    }


    public void adapt(RefMap ref)
    {
        this.mapId = ref.map_id;
        this.name = ref.Name;
        this.desc = ref.desc;
        this.scene_id = ref.scene_id;
        this.player_group_list = ref.player_group;

        this.surrender_time = ref.surrender_time;
        this.surrender_cd = ref.surrender_cd;


        // this.groupList= ref.groupList;


        // this.pathList = ref.pathList;
        this.segmentList = new ArrayList<>();
        for (RefMapPath refPath : RefMapPath.getMgr().getList())
        {
            if (refPath.map_id == this.mapId)
            {
                WCGSegmentObj segmentObj = new WCGSegmentObj();
                segmentObj.groupId = refPath.group_id;
                segmentObj.point_id = refPath.point_id;
                segmentObj.next_point_id = refPath.next_point_id;
                segmentObj.research_distance = refPath.research_distance;
                segmentObj.area_id = refPath.area_id;
                segmentObj.is_research_enable = refPath.is_research_enable;
                this.segmentList.add(segmentObj);
            }
        }

        //pointList;
        this.pointList = new ArrayList<>();
        for (RefMapPoint refPoint : RefMapPoint.getMgr().getList())
        {
            if (refPoint.map_id == this.mapId)
            {
                NPPointObj point = new NPPointObj();
                point.point_id = refPoint.point_id;
                point.pos = new DLMapPos(refPoint.x, refPoint.y);
                point.radius = refPoint.radius;
                this.pointList.add(point);
            }
        }


        //this.areaList = ref.areaList;
        this.areaList = new ArrayList<>();
        for (RefMapArea refArea : RefMapArea.getMgr().getList())
        {
            if (refArea.map_id == this.mapId)
            {
                NPAreaObj obj = new NPAreaObj();
                obj.adapt(refArea);
                this.areaList.add(obj);
            }
        }


        this.groupList = new ArrayList<>();
        for (RefMapGroup refMapGroup : RefMapGroup.getMgr().getList())
        {
            if (refMapGroup.map_id == this.mapId)
            {
                WCGMapGroupObj obj = new WCGMapGroupObj();
                obj.adapt(refMapGroup);
                this.groupList.add(obj);
            }
        }

        this.camp_list = new ArrayList<WCGCampObj>();
        for (RefCamp refCamp : RefCamp.getMgr().getList())
        {
            if (refCamp.map_id == this.mapId)
            {
                WCGCampObj obj = new WCGCampObj();
                obj.adapt(refCamp);
                this.camp_list.add(obj);
            }
        }
        if (this.camp_list.size() < 2)
        {
            CommLog.error("dungeon id :" + mapId + " has not setup camp list,num:" + this.camp_list.size());
        }

        for (int i = 0; i < this.camp_list.size(); i++)
        {
            WCGCampObj campObj = this.camp_list.get(i);
            if (null == campObj)
                continue;
            for (int j = 0; j < campObj.group_list.size(); j++)
            {
                WCGMapGroupObj groupObj = this.GetGroupObj(campObj.group_list.get(j));
                if (null == groupObj)
                    continue;

//                groupObj.setShareResList(campObj.group_list);
            }
        }
    }
}
