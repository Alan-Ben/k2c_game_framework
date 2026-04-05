using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p028_QuestOp
{

/// <summary>
/// 日常任务一键领取完成奖励
/// </summary>
public class GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务刷新序列号
/// </summary>
private long refreshSerial;
/// <summary>
/// 日常任务类型
/// </summary>
private Common.QuestEnum.EDailyQuestType type;


public GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward() {
	refreshSerial = (long)0;
	type = 0;
}

public GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward(
	long _refreshSerial
	, Common.QuestEnum.EDailyQuestType _type
) {	refreshSerial = _refreshSerial;
	type = _type;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 任务刷新序列号
/// </summary>
public long getRefreshSerial() { return refreshSerial; }
/// <summary>
/// 任务刷新序列号
/// </summary>
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/// <summary>
/// 日常任务类型
/// </summary>
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/// <summary>
/// 日常任务类型
/// </summary>
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.QuestEnum.EDailyQuestType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refreshSerial);
	_buf.putInt((int)type);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)13);
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
	builder.Append("refreshSerial").Append(":").Append(refreshSerial.ToString()).Append(", ");
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

