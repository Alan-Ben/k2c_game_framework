using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p028_QuestOp
{

/// <summary>
/// 已领取阶段奖励变更
/// </summary>
public class GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日常任务类型
/// </summary>
private Common.QuestEnum.EDailyQuestType type;
/// <summary>
/// 刷新序列号
/// </summary>
private long refreshSerial;
/// <summary>
/// 已领取活跃度奖励id
/// </summary>
private List<long> hasTakenActiveRewardRefIdList;


public GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg() {
	type = 0;
	refreshSerial = (long)0;
	hasTakenActiveRewardRefIdList = new List<long>();
}

public GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg(
	Common.QuestEnum.EDailyQuestType _type
	, long _refreshSerial
	, List<long> _hasTakenActiveRewardRefIdList
) {	type = _type;
	refreshSerial = _refreshSerial;
	hasTakenActiveRewardRefIdList = _hasTakenActiveRewardRefIdList;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)61; }

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
/// 已领取活跃度奖励id
/// </summary>
public List<long> getHasTakenActiveRewardRefIdList() { return hasTakenActiveRewardRefIdList; }
/// <summary>
/// 已领取活跃度奖励id
/// </summary>
public void addHasTakenActiveRewardRefIdList(long _hasTakenActiveRewardRefIdList) { hasTakenActiveRewardRefIdList.Add(_hasTakenActiveRewardRefIdList); }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (hasTakenActiveRewardRefIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (hasTakenActiveRewardRefIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.QuestEnum.EDailyQuestType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refreshSerial = _buf.getLong();
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
	_buf.putShort((short)hasTakenActiveRewardRefIdList.Count);
	for(int _i = 0; _i < hasTakenActiveRewardRefIdList.Count; _i++) { 
		_buf.putLong(hasTakenActiveRewardRefIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)61);
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
	builder.Append("hasTakenActiveRewardRefIdList").Append(":").Append(hasTakenActiveRewardRefIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

