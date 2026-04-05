package NPHttpServer.Http.Entity;

import NPHttpServer.Http.HttpService.AutoDecoder.JsonField;

import java.util.List;

/**
 * 开启活动请求实体（协议5-1）
 *
 * 功能：接收后台推送的活动排期配置，向ScheduleServer发送排期请求
 *
 * 请求参数映射关系：
 * - serial: 唯一序列号，用于追踪请求
 * - launch_id: 主表ID，对应phpScheduleId
 * - us_type_id_list: 目标服务器ID列表
 * - start_time: 活动开启时间（格式：yyyy-MM-dd HH:mm:ss）
 * - end_time: 活动结算时间（格式：yyyy-MM-dd HH:mm:ss）
 * - show_end_time: 活动展示结束时间（格式：yyyy-MM-dd HH:mm:ss）
 * - json_data: 扩展字段（JSON字符串）
 */
public class EntityAddActivitySchedule
{
    @JsonField(value = "serial", required = true, description = "唯一序列号")
    private String serial;

    @JsonField(value = "launch_id", required = true, description = "主表ID")
    private long launchId;

    @JsonField(value = "us_type_id_list", required = true, description = "服务器ID列表")
    private List<Integer> usTypeIdList;

    @JsonField(value = "start_time", required = true, description = "开始时间")
    private String startTime;

    @JsonField(value = "end_time", required = true, description = "结束时间")
    private String endTime;

    @JsonField(value = "show_end_time", required = true, description = "展示结束时间")
    private String showEndTime;

    @JsonField(value = "json_data", required = false, description = "扩展字段")
    private String jsonData;

    public String getSerial()
    {
        return serial;
    }

    public void setSerial(String serial)
    {
        this.serial = serial;
    }

    public long getLaunchId()
    {
        return launchId;
    }

    public void setLaunchId(long launchId)
    {
        this.launchId = launchId;
    }

    public List<Integer> getUsTypeIdList()
    {
        return usTypeIdList;
    }

    public void setUsTypeIdList(List<Integer> usTypeIdList)
    {
        this.usTypeIdList = usTypeIdList;
    }

    public String getStartTime()
    {
        return startTime;
    }

    public void setStartTime(String startTime)
    {
        this.startTime = startTime;
    }

    public String getEndTime()
    {
        return endTime;
    }

    public void setEndTime(String endTime)
    {
        this.endTime = endTime;
    }

    public String getShowEndTime()
    {
        return showEndTime;
    }

    public void setShowEndTime(String showEndTime)
    {
        this.showEndTime = showEndTime;
    }

    public String getJsonData()
    {
        return jsonData;
    }

    public void setJsonData(String jsonData)
    {
        this.jsonData = jsonData;
    }
}