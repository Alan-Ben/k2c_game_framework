using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.QuestObj
{

/// <summary>
/// 日常任务刷新组
/// </summary>
public class DailyQuest_Group : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日常任务类型
/// </summary>
private Common.QuestEnum.EDailyQuestType type;
/// <summary>
/// 刷新序列号
/// </summary>
private long refreshSerial;
/// <summary>
/// 下一次刷新时间
/// </summary>
private long nextFreshTimeMs;
/// <summary>
/// 任务目标信息
/// </summary>
private List<Common.QuestObj.DailyQuest_Info> questInfoList;
/// <summary>
/// 随机任务目标信息
/// </summary>
private List<Common.QuestObj.DailyQuest_Info> randomQuestInfoList;
/// <summary>
/// 已领取活跃度奖励id
/// </summary>
private List<long> hasTakenActiveRewardRefIdList;


public DailyQuest_Group() {
	type = 0;
	refreshSerial = (long)0;
	nextFreshTimeMs = (long)0;
	questInfoList = new List<Common.QuestObj.DailyQuest_Info>();
	randomQuestInfoList = new List<Common.QuestObj.DailyQuest_Info>();
	hasTakenActiveRewardRefIdList = new List<long>();
}

public DailyQuest_Group(
	Common.QuestEnum.EDailyQuestType _type
	, long _refreshSerial
	, long _nextFreshTimeMs
	, List<Common.QuestObj.DailyQuest_Info> _questInfoList
	, List<Common.QuestObj.DailyQuest_Info> _randomQuestInfoList
	, List<long> _hasTakenActiveRewardRefIdList
) {	type = _type;
	refreshSerial = _refreshSerial;
	nextFreshTimeMs = _nextFreshTimeMs;
	questInfoList = _questInfoList;
	randomQuestInfoList = _randomQuestInfoList;
	hasTakenActiveRewardRefIdList = _hasTakenActiveRewardRefIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日常任务类型
/// </summary>
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/// <summary>
/// 日常任务类型
/// </summary>
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }
/// <summary>
/// 刷新序列号
/// </summary>
public long getRefreshSerial() { return refreshSerial; }
/// <summary>
/// 刷新序列号
/// </summary>
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/// <summary>
/// 下一次刷新时间
/// </summary>
public long getNextFreshTimeMs() { return nextFreshTimeMs; }
/// <summary>
/// 下一次刷新时间
/// </summary>
public void setNextFreshTimeMs(long _nextFreshTimeMs) { nextFreshTimeMs = _nextFreshTimeMs; }
/// <summary>
/// 任务目标信息
/// </summary>
public List<Common.QuestObj.DailyQuest_Info> getQuestInfoList() { return questInfoList; }
/// <summary>
/// 任务目标信息
/// </summary>
public void addQuestInfoList(Common.QuestObj.DailyQuest_Info _questInfoList) { questInfoList.Add(_questInfoList); }
/// <summary>
/// 随机任务目标信息
/// </summary>
public List<Common.QuestObj.DailyQuest_Info> getRandomQuestInfoList() { return randomQuestInfoList; }
/// <summary>
/// 随机任务目标信息
/// </summary>
public void addRandomQuestInfoList(Common.QuestObj.DailyQuest_Info _randomQuestInfoList) { randomQuestInfoList.Add(_randomQuestInfoList); }
/// <summary>
/// 已领取活跃度奖励id
/// </summary>
public List<long> getHasTakenActiveRewardRefIdList() { return hasTakenActiveRewardRefIdList; }
/// <summary>
/// 已领取活跃度奖励id
/// </summary>
public void addHasTakenActiveRewardRefIdList(long _hasTakenActiveRewardRefIdList) { hasTakenActiveRewardRefIdList.Add(_hasTakenActiveRewardRefIdList); }


public int GetBufSize() {
	int _size = 20;
	_size += 2 + (questInfoList.Count * 21);
	_size += 2 + (randomQuestInfoList.Count * 21);
	_size += 2 + (hasTakenActiveRewardRefIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (questInfoList.Count * 21);
	_size += 2 + (randomQuestInfoList.Count * 21);
	_size += 2 + (hasTakenActiveRewardRefIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.QuestEnum.EDailyQuestType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextFreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _questInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _questInfoListCount; _i++) { 
		Common.QuestObj.DailyQuest_Info _questInfoList = new Common.QuestObj.DailyQuest_Info();
		int __questInfoListCustLen = _buf.getInt();
	int __questInfoListCurPos = _buf.getCurPos();
	_questInfoList.ReadUnzipBuf(_buf, __questInfoListCurPos + __questInfoListCustLen);
	_buf.setPosition(__questInfoListCurPos + __questInfoListCustLen);

		questInfoList.Add(_questInfoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _randomQuestInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _randomQuestInfoListCount; _i++) { 
		Common.QuestObj.DailyQuest_Info _randomQuestInfoList = new Common.QuestObj.DailyQuest_Info();
		int __randomQuestInfoListCustLen = _buf.getInt();
	int __randomQuestInfoListCurPos = _buf.getCurPos();
	_randomQuestInfoList.ReadUnzipBuf(_buf, __randomQuestInfoListCurPos + __randomQuestInfoListCustLen);
	_buf.setPosition(__randomQuestInfoListCurPos + __randomQuestInfoListCustLen);

		randomQuestInfoList.Add(_randomQuestInfoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hasTakenActiveRewardRefIdListCount = _buf.getShort();
	for(int _i = 0; _i < _hasTakenActiveRewardRefIdListCount; _i++) { 
		long _hasTakenActiveRewardRefIdList = (long)0;
		_hasTakenActiveRewardRefIdList = _buf.getLong();
		hasTakenActiveRewardRefIdList.Add(_hasTakenActiveRewardRefIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putLong(refreshSerial);
	_buf.putLong(nextFreshTimeMs);
	_buf.putShort((short)questInfoList.Count);
	for(int _i = 0; _i < questInfoList.Count; _i++) { 
		_buf.putInt(questInfoList[_i].GetBufSize());
	questInfoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)randomQuestInfoList.Count);
	for(int _i = 0; _i < randomQuestInfoList.Count; _i++) { 
		_buf.putInt(randomQuestInfoList[_i].GetBufSize());
	randomQuestInfoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)hasTakenActiveRewardRefIdList.Count);
	for(int _i = 0; _i < hasTakenActiveRewardRefIdList.Count; _i++) { 
		_buf.putLong(hasTakenActiveRewardRefIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("refreshSerial").Append(":").Append(refreshSerial.ToString()).Append(", ");
	builder.Append("nextFreshTimeMs").Append(":").Append(nextFreshTimeMs.ToString()).Append(", ");
	builder.Append("questInfoList").Append(":").Append(questInfoList.ToString()).Append(", ");
	builder.Append("randomQuestInfoList").Append(":").Append(randomQuestInfoList.ToString()).Append(", ");
	builder.Append("hasTakenActiveRewardRefIdList").Append(":").Append(hasTakenActiveRewardRefIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

