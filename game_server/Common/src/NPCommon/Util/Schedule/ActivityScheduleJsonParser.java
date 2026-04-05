package NPCommon.Util.Schedule;

import Common.Common_IntList;
import Common.ScheduleObj.Schedule_ActivityInfo;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_PhpInfo;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.SSActivityScheduleErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.util.HashSet;
import java.util.Set;

/**
 * 活动排期JSON数据校验器 - 专门用于校验传入的JSON结构数据
 * <p>
 * 主要功能：
 * 1. 校验JSON格式的正确性
 * 2. 验证必要字段的存在性
 * 3. 检查数据类型和格式
 * 4. 验证时间格式的有效性
 * 5. 检查分组数据结构的完整性
 *
 * {
 * 	"scheduleId": 9,
 * 	"submitCount": 1,
 * 	"prePushTime": "2025-09-24 20:00:00",
 * 	"activity": {
 * 		"activityId": 102000,
 * 		"startTime": "2025-09-25 00:00:00",
 * 		"endTime": "2025-09-28 00:00:00",
 * 		"closeTime": "2025-09-28 00:00:00"
 * 	    },
 * 	"list": [{
 * 		"usIdList": [
 * 			[1, 2, 5, 6]
 * 		],
 * 		"resFile": "102000_11_1_20250924163916.txt",
 *      "resMd5":"769084cb4ce5a67ab54fca499d84b27b",
 *      "resDir":"hot_refdata/20251022/"
 *    }]
 * }
 */
public class ActivityScheduleJsonParser
{

    /**
     * 校验并解析排期JSON数据 - 先校验格式，再解析数据
     * <p>
     * 执行流程：
     * 1. 解析JSON格式并校验结构完整性
     * 2. 校验所有必要字段和数据格式
     * 3. 解析并构建数据对象
     * @param jsonStr 待校验的JSON字符串
     * @return 校验结果，成功返回解析好的数据对象，失败返回具体错误码
     */
    public static ResultOne<Schedule_PhpInfo> validateAndParseScheduleJson(String jsonStr)
    {
        try
        {
            // 1. 基本JSON格式校验
            JsonObject jsonObj;
            try
            {
                JsonParser parser = new JsonParser();
                jsonObj = parser.parse(jsonStr).getAsJsonObject();
            } catch (Exception e)
            {
                CommLog.error("JSON格式解析失败: {}", e.getMessage());
                return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
            }

            // 2. 先快速提取scheduleId用于日志跟踪
            long scheduleId = 0;
            if (jsonObj.has("scheduleId"))
            {
                try
                {
                    scheduleId = jsonObj.get("scheduleId").getAsLong();
                } catch (Exception e)
                {
                    CommLog.error("scheduleId格式错误: {}", e.getMessage());
                    return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
                }
            } else
            {
                CommLog.error("缺少必要字段: scheduleId");
                return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
            }

            // 3. 进行完整的格式校验（传入scheduleId用于日志）
            Result validateResult = validateJsonStructure(jsonObj, scheduleId);
            if (!validateResult.isSucc())
            {
                return ResultOne.failed(validateResult);
            }

            // 4. 格式校验通过后，进行数据解析
            Schedule_PhpInfo parsedData = parseJsonToData(jsonObj);
            if (parsedData == null)
            {
                CommLog.error("数据解析过程失败: phpScheduleId:{}", scheduleId);
                return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
            }

            CommLog.info("JSON数据校验和解析完成: phpScheduleId:{}", parsedData.getPhpScheduleId());
            return ResultOne.succ(parsedData);

        } catch (Exception e)
        {
            CommLog.error("JSON校验过程异常: {}", e.getMessage());
            return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
        }
    }

    /**
     * 校验JSON结构完整性 - 只进行格式和字段存在性校验，不解析数据
     * <p>
     * 执行流程：
     * 1. 校验根级必要字段存在性
     * 2. 校验活动信息结构
     * 3. 校验分组数据结构
     * 4. 校验时间格式有效性
     * @param jsonObj    已解析的JSON对象
     * @param scheduleId 排期ID，用于日志跟踪
     * @return 校验结果
     */
    private static Result validateJsonStructure(JsonObject jsonObj, long scheduleId)
    {
        try
        {
            // 1. 校验根级必要字段存在性
            Result rootResult = validateRootFields(jsonObj, scheduleId);
            if (!rootResult.isSucc())
            {
                return rootResult;
            }

            // 2. 校验活动信息结构
            JsonObject activityObj = jsonObj.getAsJsonObject("activity");
            Result activityResult = validateActivityStructure(activityObj, scheduleId);
            if (!activityResult.isSucc())
            {
                return activityResult;
            }

            // 3. 校验分组数据结构
            JsonArray listArray = jsonObj.getAsJsonArray("list");
            Result listResult = validateGroupDataStructure(listArray, scheduleId);
            if (!listResult.isSucc())
            {
                return listResult;
            }

            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("JSON结构校验异常: phpScheduleId:{} 错误:{}", scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }

    /**
     * 解析JSON数据为数据对象 - 格式校验通过后进行数据解析
     * @param jsonObj 已校验的JSON对象
     * @return 解析后的数据对象，失败返回null
     */
    private static Schedule_PhpInfo parseJsonToData(JsonObject jsonObj)
    {
        try
        {
            Schedule_PhpInfo parsedData = new Schedule_PhpInfo();

            // 1. 解析根级字段
            parseRootFieldsToData(jsonObj, parsedData);

            // 2. 解析活动信息
            parseActivityInfoToData(jsonObj.getAsJsonObject("activity"), parsedData);

            // 3. 解析分组数据
            parseGroupDataToData(jsonObj.getAsJsonArray("list"), parsedData);

            return parsedData;

        } catch (Exception e)
        {
            CommLog.error("parse schedule data fail", e);
            return null;
        }
    }

    /**
     * 校验根级字段存在性和格式
     */
    private static Result validateRootFields(JsonObject jsonObj, long scheduleId)
    {
        try
        {
            // scheduleId已在上层校验过，这里跳过

            // 校验prePushTime存在性和格式
            if (!jsonObj.has("prePushTime"))
            {
                CommLog.error("缺少必要字段: prePushTime phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }
            String prePushTimeStr = jsonObj.get("prePushTime").getAsString();
            long prePushTimeMs = parseAndValidateTime(prePushTimeStr, "prePushTime", scheduleId);
            if (prePushTimeMs == -1)
            {
                return SSActivityScheduleErr.PARSE_ERR;
            }

            // 校验必要字段存在性
            if (!jsonObj.has("activity"))
            {
                CommLog.error("缺少必要字段: activity phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }
            if (!jsonObj.has("list"))
            {
                CommLog.error("缺少必要字段: list phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }

            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("根级字段校验失败: phpScheduleId:{} 错误:{}", scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }

    /**
     * 校验活动信息结构
     */
    private static Result validateActivityStructure(JsonObject activityObj, long scheduleId)
    {
        if (activityObj == null)
        {
            CommLog.error("activity字段不能为空: phpScheduleId:{}", scheduleId);
            return SSActivityScheduleErr.PARSE_ERR;
        }

        try
        {
            // 校验activityId存在性
            if (!activityObj.has("activityId"))
            {
                CommLog.error("activity缺少必要字段: activityId phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }

            // 校验startTime存在性和格式
            if (!activityObj.has("startTime"))
            {
                CommLog.error("activity缺少必要字段: startTime phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }
            String startTimeStr = activityObj.get("startTime").getAsString();
            long startTimeMs = parseAndValidateTime(startTimeStr, "startTime", scheduleId);
            if (startTimeMs == -1)
            {
                return SSActivityScheduleErr.PARSE_ERR;
            }

            // 校验endTime存在性和格式
            if (!activityObj.has("endTime"))
            {
                CommLog.error("activity缺少必要字段: endTime phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }
            String endTimeStr = activityObj.get("endTime").getAsString();
            long endTimeMs = parseAndValidateTime(endTimeStr, "endTime", scheduleId);
            if (endTimeMs == -1)
            {
                return SSActivityScheduleErr.PARSE_ERR;
            }

            // 校验closeTime存在性和格式
            if (!activityObj.has("closeTime"))
            {
                CommLog.error("activity缺少必要字段: closeTime phpScheduleId:{}", scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }
            String closeTimeStr = activityObj.get("closeTime").getAsString();
            long closeTimeMs = parseAndValidateTime(closeTimeStr, "closeTime", scheduleId);
            if (closeTimeMs == -1)
            {
                return SSActivityScheduleErr.PARSE_ERR;
            }

            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("活动信息结构校验失败: phpScheduleId:{} 错误:{}", scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }

    /**
     * 校验分组数据结构
     */
    private static Result validateGroupDataStructure(JsonArray listArray, long scheduleId)
    {
        if (listArray == null || listArray.size() == 0)
        {
            CommLog.error("list数组不能为空: phpScheduleId:{}", scheduleId);
            return SSActivityScheduleErr.PARSE_ERR;
        }

        try
        {
            // 用于记录所有已出现的UserServer ID，检查重复
            Set<Integer> allUsIds = new HashSet<>();

            // 校验每个分组数据项的结构
            for (int i = 0; i < listArray.size(); i++)
            {
                JsonElement element = listArray.get(i);
                if (!element.isJsonObject())
                {
                    CommLog.error("list数组第{}项不是有效对象: phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                JsonObject groupObj = element.getAsJsonObject();

                // 校验必要字段
                if (!groupObj.has("usIdList"))
                {
                    CommLog.error("list数组第{}项缺少必要字段(usIdList): phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
                if (!groupObj.has("resFile"))
                {
                    CommLog.error("list数组第{}项缺少必要字段(resFile): phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
                if (!groupObj.has("resMd5"))
                {
                    CommLog.error("list数组第{}项缺少必要字段(resMd5): phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 校验usIdList结构（二维数组）
                JsonArray usIdListArray = groupObj.getAsJsonArray("usIdList");
                if (usIdListArray == null || usIdListArray.size() == 0)
                {
                    CommLog.error("list数组第{}项的usIdList不能为空: phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 验证usIdList是二维数组结构，同时收集UserServer ID检查重复
                for (int j = 0; j < usIdListArray.size(); j++)
                {
                    JsonElement usIdElement = usIdListArray.get(j);
                    if (!usIdElement.isJsonArray())
                    {
                        CommLog.error("list数组第{}项的usIdList第{}项不是数组: phpScheduleId:{}", i, j, scheduleId);
                        return SSActivityScheduleErr.PARSE_ERR;
                    }

                    // 检查当前分组中的每个UserServer ID
                    JsonArray usIdArray = usIdElement.getAsJsonArray();
                    for (int k = 0; k < usIdArray.size(); k++)
                    {
                        try
                        {
                            int usId = usIdArray.get(k).getAsInt();

                            // 检查UserServer ID是否已在其他分组中出现过
                            if (allUsIds.contains(usId))
                            {
                                CommLog.error("UserServer ID重复出现在多个分组中: usId:{} 分组:{} phpScheduleId:{}", usId, i, scheduleId);
                                return SSActivityScheduleErr.PARSE_ERR;
                            }

                            // 记录当前UserServer ID
                            allUsIds.add(usId);

                        } catch (Exception e)
                        {
                            CommLog.error("UserServer ID格式错误: 分组{}的usIdList第{}项第{}个元素 phpScheduleId:{}", i, j, k, scheduleId);
                            return SSActivityScheduleErr.PARSE_ERR;
                        }
                    }
                }
                // 校验resFile字段格式（字符串非空）
                String resFile = groupObj.get("resFile").getAsString();
                if (resFile == null || resFile.trim().isEmpty())
                {
                    CommLog.error("list数组第{}项的resFile不能为空: phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 校验resMd5字段格式（字符串非空）
                String resMd5 = groupObj.get("resMd5").getAsString();
                if (resMd5 == null || resMd5.trim().isEmpty())
                {
                    CommLog.error("list数组第{}项的resMd5不能为空: phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 校验resDir字段格式（字符串非空）
                String resDir = groupObj.get("resDir").getAsString();
                if (resDir == null || resDir.trim().isEmpty())
                {
                    CommLog.error("list数组第{}项的resDir不能为空: phpScheduleId:{}", i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
            }

            CommLog.info("UserServer ID重复性校验通过: 共{}个唯一UserServer phpScheduleId:{}", allUsIds.size(), scheduleId);
            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("分组数据结构校验失败: phpScheduleId:{} 错误:{}", scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }


    /**
     * 解析根级字段到数据对象
     */
    private static void parseRootFieldsToData(JsonObject jsonObj, Schedule_PhpInfo parsedData)
    {
        // 解析scheduleId
        parsedData.setPhpScheduleId(jsonObj.get("scheduleId").getAsLong());

        // 解析submitCount
        parsedData.setSubmitCount(jsonObj.get("submitCount").getAsInt());

        // 解析prePushTime
        String prePushTimeStr = jsonObj.get("prePushTime").getAsString();
        long prePushTimeMs = parseAndValidateTime(prePushTimeStr, "prePushTime");
        parsedData.setPrePushTimeMs(prePushTimeMs);
    }

    /**
     * 解析活动信息到数据对象
     */
    private static void parseActivityInfoToData(JsonObject activityObj, Schedule_PhpInfo parsedData)
    {
        // 创建活动信息对象
        Schedule_ActivityInfo activityInfo = new Schedule_ActivityInfo();

        // 解析activityId
        activityInfo.setActivityId(activityObj.get("activityId").getAsLong());

        // 解析startTime
        String startTimeStr = activityObj.get("startTime").getAsString();
        long startTimeMs = parseAndValidateTime(startTimeStr, "startTime");
        activityInfo.setStartTimeMs(startTimeMs);

        // 解析endTime
        String endTimeStr = activityObj.get("endTime").getAsString();
        long endTimeMs = parseAndValidateTime(endTimeStr, "endTime");
        activityInfo.setEndTimeMs(endTimeMs);

        // 解析closeTime
        String closeTimeStr = activityObj.get("closeTime").getAsString();
        long closeTimeMs = parseAndValidateTime(closeTimeStr, "closeTime");
        activityInfo.setCloseTimeMs(closeTimeMs);

        // 设置到parsedData中
        parsedData.setActivity(activityInfo);
    }

    /**
     * 解析分组数据到数据对象
     */
    private static void parseGroupDataToData(JsonArray listArray, Schedule_PhpInfo parsedData)
    {
        for (int i = 0; i < listArray.size(); i++)
        {
            JsonObject groupObj = listArray.get(i).getAsJsonObject();

            // 创建分组数据信息对象
            Schedule_GroupInfo groupInfo = new Schedule_GroupInfo();

            // 解析UserServer ID分组列表（二维数组）
            JsonArray usIdListArray = groupObj.getAsJsonArray("usIdList");

            for (int j = 0; j < usIdListArray.size(); j++)
            {
                JsonArray usIdArray = usIdListArray.get(j).getAsJsonArray();
                Common_IntList usIds = new Common_IntList();
                for (int k = 0; k < usIdArray.size(); k++)
                {
                    usIds.addValueList(usIdArray.get(k).getAsInt());
                }
                groupInfo.addUsGroupList(usIds);
            }

            // 解析资源文件名
            String resFile = groupObj.get("resFile").getAsString();
            groupInfo.setResFile(resFile);

            // 解析资源文件MD5
            String resMd5 = groupObj.get("resMd5").getAsString();
            groupInfo.setResFileMd5(resMd5);

            // 解析资源文件目录
            String resDir = groupObj.get("resDir").getAsString();
            groupInfo.setResFileDir(resDir);

            parsedData.getGroupList().add(groupInfo);
        }
    }

    /**
     * 解析时间字符串为毫秒时间戳，同时进行格式校验
     * @param timeStr    时间字符串
     * @param fieldName  字段名称（用于错误日志）
     * @param scheduleId 排期ID，用于日志跟踪
     * @return 毫秒时间戳，解析失败返回-1
     */
    private static long parseAndValidateTime(String timeStr, String fieldName, long scheduleId)
    {
        try
        {
            return CommonFunc.simpleDateFormatTimeMs(timeStr);
        } catch (Exception e)
        {
            CommLog.error("{}时间格式无效: {} phpScheduleId:{} 错误:{}", fieldName, timeStr, scheduleId, e.getMessage());
            return -1;
        }
    }

    /**
     * 解析时间字符串为毫秒时间戳，同时进行格式校验（用于解析阶段，不需要scheduleId）
     * @param timeStr   时间字符串
     * @param fieldName 字段名称（用于错误日志）
     * @return 毫秒时间戳，解析失败返回-1
     */
    private static long parseAndValidateTime(String timeStr, String fieldName)
    {
        try
        {
            return CommonFunc.simpleDateFormatTimeMs(timeStr);
        } catch (Exception e)
        {
            CommLog.error("{}时间格式无效: {} 错误:{}", fieldName, timeStr, e.getMessage());
            return -1;
        }
    }
}