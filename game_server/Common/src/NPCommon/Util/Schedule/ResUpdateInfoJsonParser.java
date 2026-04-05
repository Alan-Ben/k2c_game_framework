package NPCommon.Util.Schedule;

import Common.Common_IntList;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_ResUpdateInfo;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.SSActivityScheduleErr;
import NPCommon.Log.CommLog;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.util.HashSet;
import java.util.Set;

/**
 * 资源更新信息JSON数据校验器 - 专门用于校验传入的JSON结构数据
 *
 * 主要功能：
 * 1. 校验JSON格式的正确性
 * 2. 验证必要字段的存在性
 * 3. 检查数据类型和格式
 * 4. 检查分组数据结构的完整性
 * 5. 验证UserServer ID不重复
 *
 * JSON格式示例：
 * {
 *     "scheduleId": 9,
 *     "submitCount": 1,
 *     "list": [{
 *         "usIdList": [
 *             [1, 2, 5, 6]
 *         ],
 *         "resFile": "102000_11_1_20250924163916.txt",
 *         "resMd5": "769084cb4ce5a67ab54fca499d84b27b",
 *         "resDir": "hot_refdata/20251022/"
 *     }]
 * }
 */
public class ResUpdateInfoJsonParser
{

    /**
     * 校验并解析资源更新JSON数据 - 先校验格式，再解析数据
     *
     * 执行流程：
     * 1. 解析JSON格式并校验结构完整性
     * 2. 校验所有必要字段和数据格式
     * 3. 解析并构建数据对象
     *
     * @param jsonStr 待校验的JSON字符串
     * @return 校验结果，成功返回解析好的数据对象，失败返回具体错误码
     */
    public static ResultOne<Schedule_ResUpdateInfo> validateAndParseResUpdateJson(String jsonStr)
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
                CommLog.error("ResUpdateInfoJsonParser.validateAndParseResUpdateJson - JSON parse failed: format error, error={}",
                             e.getMessage());
                return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
            }

            // 2. 先快速提取scheduleId用于日志跟踪（JSON字段名为scheduleId，映射到phpScheduleId）
            long scheduleId = 0;
            if (jsonObj.has("scheduleId"))
            {
                try
                {
                    scheduleId = jsonObj.get("scheduleId").getAsLong();
                } catch (Exception e)
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateAndParseResUpdateJson - field validation failed: scheduleId format error, error={}",
                                 e.getMessage());
                    return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
                }
            } else
            {
                CommLog.error("ResUpdateInfoJsonParser.validateAndParseResUpdateJson - field validation failed: missing required field scheduleId");
                return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
            }

            // 3. 进行完整的格式校验（传入scheduleId用于日志）
            Result validateResult = validateJsonStructure(jsonObj, scheduleId);
            if (!validateResult.isSucc())
            {
                return ResultOne.failed(validateResult);
            }

            // 4. 格式校验通过后，进行数据解析
            Schedule_ResUpdateInfo parsedData = parseJsonToData(jsonObj);
            if (parsedData == null)
            {
                CommLog.error("ResUpdateInfoJsonParser.validateAndParseResUpdateJson - data parse failed: parse process failed, scheduleId={}",
                             scheduleId);
                return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
            }

            CommLog.info("ResUpdateInfoJsonParser.validateAndParseResUpdateJson - validation and parse success: scheduleId={}",
                        parsedData.getPhpScheduleId());
            return ResultOne.succ(parsedData);

        } catch (Exception e)
        {
            CommLog.error("ResUpdateInfoJsonParser.validateAndParseResUpdateJson - validation process exception: error={}",
                         e.getMessage());
            return ResultOne.failed(SSActivityScheduleErr.PARSE_ERR);
        }
    }

    /**
     * 校验JSON结构完整性 - 只进行格式和字段存在性校验，不解析数据
     *
     * 执行流程：
     * 1. 校验根级必要字段存在性
     * 2. 校验分组数据结构
     * 3. 验证UserServer ID不重复
     *
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

            // 2. 校验分组数据结构（JSON字段名为list）
            JsonArray listArray = jsonObj.getAsJsonArray("list");
            Result listResult = validateGroupDataStructure(listArray, scheduleId);
            if (!listResult.isSucc())
            {
                return listResult;
            }

            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("ResUpdateInfoJsonParser.validateJsonStructure - structure validation exception: scheduleId={}, error={}",
                         scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }

    /**
     * 解析JSON数据为数据对象 - 格式校验通过后进行数据解析
     *
     * @param jsonObj 已校验的JSON对象
     * @return 解析后的数据对象，失败返回null
     */
    private static Schedule_ResUpdateInfo parseJsonToData(JsonObject jsonObj)
    {
        try
        {
            Schedule_ResUpdateInfo parsedData = new Schedule_ResUpdateInfo();

            // 1. 解析根级字段
            parseRootFieldsToData(jsonObj, parsedData);

            // 2. 解析分组数据（JSON字段名为list）
            parseGroupDataToData(jsonObj.getAsJsonArray("list"), parsedData);

            return parsedData;

        } catch (Exception e)
        {
            CommLog.error("ResUpdateInfoJsonParser.parseJsonToData - parse data failed: exception occurred, error={}",
                         e.getMessage());
            return null;
        }
    }

    /**
     * 校验根级字段存在性和格式
     *
     * @param jsonObj    JSON对象
     * @param scheduleId 排期ID，用于日志跟踪
     * @return 校验结果
     */
    private static Result validateRootFields(JsonObject jsonObj, long scheduleId)
    {
        try
        {
            // scheduleId已在上层校验过，这里跳过

            // 校验submitCount存在性
            if (!jsonObj.has("submitCount"))
            {
                CommLog.error("ResUpdateInfoJsonParser.validateRootFields - field validation failed: missing required field submitCount, scheduleId={}",
                             scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }

            try
            {
                jsonObj.get("submitCount").getAsInt();
            } catch (Exception e)
            {
                CommLog.error("ResUpdateInfoJsonParser.validateRootFields - field validation failed: submitCount format error, scheduleId={}, error={}",
                             scheduleId, e.getMessage());
                return SSActivityScheduleErr.PARSE_ERR;
            }

            // 校验list字段存在性（JSON字段名为list）
            if (!jsonObj.has("list"))
            {
                CommLog.error("ResUpdateInfoJsonParser.validateRootFields - field validation failed: missing required field list, scheduleId={}",
                             scheduleId);
                return SSActivityScheduleErr.PARSE_ERR;
            }

            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("ResUpdateInfoJsonParser.validateRootFields - root field validation failed: scheduleId={}, error={}",
                         scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }

    /**
     * 校验分组数据结构
     *
     * @param listArray  分组数据数组
     * @param scheduleId 排期ID，用于日志跟踪
     * @return 校验结果
     */
    private static Result validateGroupDataStructure(JsonArray listArray, long scheduleId)
    {
        if (listArray == null || listArray.size() == 0)
        {
            CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - list validation failed: list is empty, scheduleId={}",
                         scheduleId);
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
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: item is not valid object, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                JsonObject groupObj = element.getAsJsonObject();

                // 校验必要字段（JSON字段名为usIdList）
                if (!groupObj.has("usIdList"))
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: missing required field usIdList, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
                if (!groupObj.has("resFile"))
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: missing required field resFile, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
                if (!groupObj.has("resMd5"))
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: missing required field resMd5, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
                if (!groupObj.has("resDir"))
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: missing required field resDir, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 校验usIdList结构（二维数组，JSON字段名为usIdList）
                JsonArray usIdListArray = groupObj.getAsJsonArray("usIdList");
                if (usIdListArray == null || usIdListArray.size() == 0)
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: usIdList is empty, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 验证usIdList是二维数组结构，同时收集UserServer ID检查重复
                for (int j = 0; j < usIdListArray.size(); j++)
                {
                    JsonElement usGroupElement = usIdListArray.get(j);
                    if (!usGroupElement.isJsonArray())
                    {
                        CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - usIdList validation failed: item is not array, groupIndex={}, usGroupIndex={}, scheduleId={}",
                                     i, j, scheduleId);
                        return SSActivityScheduleErr.PARSE_ERR;
                    }

                    // 检查当前分组中的每个UserServer ID
                    JsonArray usIdArray = usGroupElement.getAsJsonArray();
                    for (int k = 0; k < usIdArray.size(); k++)
                    {
                        try
                        {
                            int usId = usIdArray.get(k).getAsInt();

                            // 检查UserServer ID是否已在其他分组中出现过
                            if (allUsIds.contains(usId))
                            {
                                CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - usId duplicate validation failed: usId appears in multiple groups, usId={}, groupIndex={}, scheduleId={}",
                                             usId, i, scheduleId);
                                return SSActivityScheduleErr.PARSE_ERR;
                            }

                            // 记录当前UserServer ID
                            allUsIds.add(usId);

                        } catch (Exception e)
                        {
                            CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - usId format validation failed: invalid usId format, groupIndex={}, usGroupIndex={}, usIdIndex={}, scheduleId={}, error={}",
                                         i, j, k, scheduleId, e.getMessage());
                            return SSActivityScheduleErr.PARSE_ERR;
                        }
                    }
                }

                // 校验resFile字段格式（字符串非空）
                String resFile = groupObj.get("resFile").getAsString();
                if (resFile == null || resFile.trim().isEmpty())
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: resFile is empty, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 校验resMd5字段格式（字符串非空）
                String resMd5 = groupObj.get("resMd5").getAsString();
                if (resMd5 == null || resMd5.trim().isEmpty())
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: resMd5 is empty, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }

                // 校验resDir字段格式（字符串非空）
                String resDir = groupObj.get("resDir").getAsString();
                if (resDir == null || resDir.trim().isEmpty())
                {
                    CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group item validation failed: resDir is empty, groupIndex={}, scheduleId={}",
                                 i, scheduleId);
                    return SSActivityScheduleErr.PARSE_ERR;
                }
            }

            CommLog.info("ResUpdateInfoJsonParser.validateGroupDataStructure - usId duplicate validation success: unique usId count={}, scheduleId={}",
                        allUsIds.size(), scheduleId);
            return Result.SUCC;

        } catch (Exception e)
        {
            CommLog.error("ResUpdateInfoJsonParser.validateGroupDataStructure - group data validation failed: exception occurred, scheduleId={}, error={}",
                         scheduleId, e.getMessage());
            return SSActivityScheduleErr.PARSE_ERR;
        }
    }

    /**
     * 解析根级字段到数据对象
     *
     * @param jsonObj    JSON对象
     * @param parsedData 目标数据对象
     */
    private static void parseRootFieldsToData(JsonObject jsonObj, Schedule_ResUpdateInfo parsedData)
    {
        // 解析scheduleId（JSON字段名为scheduleId，映射到phpScheduleId）
        parsedData.setPhpScheduleId(jsonObj.get("scheduleId").getAsLong());

        // 解析submitCount
        parsedData.setSubmitCount(jsonObj.get("submitCount").getAsInt());
    }

    /**
     * 解析分组数据到数据对象
     *
     * @param listArray  分组数据数组
     * @param parsedData 目标数据对象
     */
    private static void parseGroupDataToData(JsonArray listArray, Schedule_ResUpdateInfo parsedData)
    {
        for (int i = 0; i < listArray.size(); i++)
        {
            JsonObject groupObj = listArray.get(i).getAsJsonObject();

            // 创建分组数据信息对象
            Schedule_GroupInfo groupInfo = new Schedule_GroupInfo();

            // 解析UserServer ID分组列表（二维数组，JSON字段名为usIdList）
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

            parsedData.addGroupList(groupInfo);
        }
    }
}