package NPScheduleServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleData;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleUsGroup;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleUsInfo;
import SSDB.Bo.ActivityScheduleUsGroupBO;
import SSDB.Bo.ActivityScheduleUsInfoBO;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.List;

/**
 * 排期修复相关GM命令
 *
 * 用于处理排期数据异常情况的修复操作
 */
@ACommander(comment = "排期修复相关GM命令", name = "schedulefix")
public class CmdScheduleFix extends CmdClassBase
{
	/**
	 * 移除错配的UsInfo记录（用于清理不存在的usId配置）
	 *
	 * 适用场景：
	 * - 运营错误配置了不存在的usId
	 * - 推送失败超过5次后被标记为已推送
	 * - 需要清理错误数据
	 *
	 * 安全检查：
	 * - 只能移除单个us的本服分组（避免破坏跨服实例）
	 * - 只能移除已推送状态的us（避免影响正常流程）
	 *
	 * 线程安全：使用ActivityScheduleData的锁保护并发访问
	 */
	@ACommand(comment = "移除错配的UsInfo记录[数据库排期ID][usId]")
	public String removeErrorUsInfo(long scheduleId, int usId)
	{
		ActivityScheduleData scheduleData = ActivityScheduleDataMgr.getInstance().lookupSchedule(scheduleId);
		if (scheduleData == null)
		{
			return "未找到数据库排期ID为 " + scheduleId + " 的排期";
		}

		// 使用反射调用ActivityScheduleData的私有锁方法
		Method lockMethod;
		Method unlockMethod;
		try
		{
			lockMethod = scheduleData.getClass().getDeclaredMethod("_lock");
			unlockMethod = scheduleData.getClass().getDeclaredMethod("_unlock");
			lockMethod.setAccessible(true);
			unlockMethod.setAccessible(true);
		}
		catch (Exception e)
		{
			return "获取锁方法失败: " + e.getMessage();
		}

		try
		{
			// 加锁
			lockMethod.invoke(scheduleData);

			try
			{
				// 1. 查找对应的usInfo
				ActivityScheduleUsInfo usInfo = scheduleData.lookupUsInfo(usId);
				if (usInfo == null)
				{
					return "排期ID " + scheduleId + " 中未找到usId=" + usId + " 的记录";
				}

				ActivityScheduleUsGroup usGroup = usInfo.getUsGroup();

				// 2. 安全检查：只能移除单个us的本服分组
				if (usGroup.getAllUsInfoList().size() != 1)
				{
					return "安全检查失败：该分组包含 " + usGroup.getAllUsInfoList().size() + " 个US，只能移除单个US的本服分组";
				}

				// 3. 安全检查：只能移除已推送状态的us（推送失败超过5次后会标记为已推送）
				if (!usInfo.hadPush())
				{
					return "安全检查失败：该US未标记为已推送状态，请等待推送流程完成后再移除";
				}

				// 4. 获取数据库ID（通过反射访问私有字段）
				Field usInfoBoField = usInfo.getClass().getDeclaredField("_m_bo");
				usInfoBoField.setAccessible(true);
				ActivityScheduleUsInfoBO usInfoBO = (ActivityScheduleUsInfoBO) usInfoBoField.get(usInfo);
				long usInfoDbId = usInfoBO.getId();
				long usGroupDbId = usGroup.getGroupDbId();

				// 5. 删除数据库记录
				scheduleData.getBM().getBM(ActivityScheduleUsInfoBO.class).delAll("id", usInfoDbId);
				scheduleData.getBM().getBM(ActivityScheduleUsGroupBO.class).delAll("id", usGroupDbId);

				// 6. 从内存中移除（使用反射访问私有字段）
				Field scheduleDataListField = scheduleData.getClass().getDeclaredField("_m_userGroupList");
				scheduleDataListField.setAccessible(true);
				@SuppressWarnings("unchecked")
				List<ActivityScheduleUsGroup> userGroupList = (List<ActivityScheduleUsGroup>) scheduleDataListField.get(scheduleData);
				userGroupList.remove(usGroup);

				return String.format("成功移除错配的UsInfo记录:\n" +
								"- 排期ID: %d\n" +
								"- UsId: %d\n" +
								"- UsInfo数据库ID: %d\n" +
								"- UsGroup数据库ID: %d\n" +
								"- 当前排期剩余分组数: %d",
						scheduleId, usId, usInfoDbId, usGroupDbId, userGroupList.size());
			}
			finally
			{
				// 释放锁
				unlockMethod.invoke(scheduleData);
			}
		}
		catch (Exception e)
		{
			return "移除失败: " + e.getMessage();
		}
	}
}
