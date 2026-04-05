package NPHttpServer.Http.Entity;

import NPHttpServer.Http.HttpService.AutoDecoder.JsonField;

import java.util.List;

/**
 * 查询多服务器活动状态请求实体（协议5-4）
 *
 * 功能：查询指定活动在多个服务器中的状态
 */
public class EntityQueryMultiServerActivity
{
    @JsonField(value = "activity_id", required = true, description = "活动ID")
    private long activityId;

    @JsonField(value = "us_type_id_list", required = true, description = "服务器ID列表")
    private List<Integer> usTypeIdList;

    public long getActivityId()
    {
        return activityId;
    }

    public void setActivityId(long activityId)
    {
        this.activityId = activityId;
    }

    public List<Integer> getUsTypeIdList()
    {
        return usTypeIdList;
    }

    public void setUsTypeIdList(List<Integer> usTypeIdList)
    {
        this.usTypeIdList = usTypeIdList;
    }
}
