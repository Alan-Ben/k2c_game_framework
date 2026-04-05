using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p028_QuestOp
{

/// <summary>
/// 尝试刷新任务数据
/// </summary>
public class GS2GC_028_012_RetDailyQuestTryFresh : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日常任务数据
/// </summary>
private Common.QuestObj.DailyQuest_Group dailyquestInfo;


public GS2GC_028_012_RetDailyQuestTryFresh() {
	dailyquestInfo = new Common.QuestObj.DailyQuest_Group();
}

public GS2GC_028_012_RetDailyQuestTryFresh(
	Common.QuestObj.DailyQuest_Group _dailyquestInfo
) {	dailyquestInfo = _dailyquestInfo;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// 日常任务数据
/// </summary>
public Common.QuestObj.DailyQuest_Group getDailyquestInfo() { return dailyquestInfo; }
/// <summary>
/// 日常任务数据
/// </summary>
public void setDailyquestInfo(Common.QuestObj.DailyQuest_Group _dailyquestInfo) { dailyquestInfo = _dailyquestInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + dailyquestInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + dailyquestInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _dailyquestInfoCustLen = _buf.getInt();
	int _dailyquestInfoCurPos = _buf.getCurPos();
	dailyquestInfo.ReadUnzipBuf(_buf, _dailyquestInfoCurPos + _dailyquestInfoCustLen);
	_buf.setPosition(_dailyquestInfoCurPos + _dailyquestInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(dailyquestInfo.GetBufSize());
	dailyquestInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)12);
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
	builder.Append("dailyquestInfo").Append(":").Append(dailyquestInfo == null ? "null" : dailyquestInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

