package NPScheduleServer.GMCommand.Cmds;

import Common.ScheduleObj.Schedule_PhpInfo;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Util.CommFile;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Schedule.ActivityScheduleJsonParser;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleData;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.ActivityScheduleMgr.ActivitySchedulePendingData;

import java.util.List;

@ACommander(comment = "排期相关GM命令", name = "schedule")
public class CmdSchedule extends CmdClassBase
{
	@ACommand(comment = "注册指定排期[排期ZoneId]")
	public String testJson()
	{
		String textFromFile = CommFile.getTextFromFile("./schedule_push_data.json");
		if(textFromFile == null || textFromFile.isEmpty())
			return "读取排期文件失败";

		ResultOne<Schedule_PhpInfo> resultOne = ActivityScheduleJsonParser.validateAndParseScheduleJson(textFromFile);
		if (!resultOne.isSucc())
		{
			return "解析排期文件失败:" + resultOne.getResult().toString();
		}

		return CommonFunc.protoToString(resultOne.getResult());
	}

	/**
	 * 根据后台排期ID查询排期状态（可查询待激活和已激活排期）
	 */
	@ACommand(comment = "根据后台排期ID查询排期状态[后台排期ID]")
	public String showByPhpScheduleId(long phpScheduleId)
	{
		// 先从待激活列表查找
		List<ActivitySchedulePendingData> pendingList = ActivityScheduleDataMgr.getInstance().getPendingDataList();
		for (ActivitySchedulePendingData pendingData : pendingList)
		{
			if (pendingData.getScheduleId() == phpScheduleId)
			{
				StringBuilder sb = new StringBuilder();
				sb.append("========== 排期详情（待激活） ==========\n");
				sb.append(pendingData).append("\n");
				return sb.toString();
			}
		}

		// 再从已激活列表查找
		ActivityScheduleData scheduleData = ActivityScheduleDataMgr.getInstance().lookupScheduleByPhpScheduleId(phpScheduleId);
		if (scheduleData != null)
		{
			StringBuilder sb = new StringBuilder();
			sb.append("========== 排期详情（已激活） ==========\n");
			sb.append(scheduleData).append("\n");
			return sb.toString();
		}

		return "未找到后台排期ID为 " + phpScheduleId + " 的排期";
	}

	/**
	 * 根据数据库排期ID查询已激活排期状态
	 */
	@ACommand(comment = "根据数据库排期ID查询排期状态[数据库排期ID]")
	public String showByScheduleId(long scheduleId)
	{
		ActivityScheduleData scheduleData = ActivityScheduleDataMgr.getInstance().lookupSchedule(scheduleId);
		if (scheduleData != null)
		{
			StringBuilder sb = new StringBuilder();
			sb.append("========== 排期详情（已激活） ==========\n");
			sb.append(scheduleData).append("\n");
			return sb.toString();
		}

		return "未找到数据库排期ID为 " + scheduleId + " 的排期（只能查询已激活排期）";
	}

}
