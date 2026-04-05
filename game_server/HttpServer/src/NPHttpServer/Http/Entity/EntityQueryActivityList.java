package NPHttpServer.Http.Entity;

import NPHttpServer.Http.HttpService.AutoDecoder.JsonField;

/**
 * 查询活动列表请求实体（协议5-2）
 *
 * 功能：根据服务器ID查询该服务器的所有活动（包括待激活排期和已激活活动）
 */
public class EntityQueryActivityList
{
    @JsonField(value = "us_type_id", required = true, description = "服务器ID")
    private int usTypeId;

    public int getUsTypeId()
    {
        return usTypeId;
    }

    public void setUsTypeId(int usTypeId)
    {
        this.usTypeId = usTypeId;
    }
}
