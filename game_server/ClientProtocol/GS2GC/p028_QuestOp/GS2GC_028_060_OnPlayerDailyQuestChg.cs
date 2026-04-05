using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p028_QuestOp
{

/// <summary>
/// 日常任务信息变更
/// </summary>
public class GS2GC_028_060_OnPlayerDailyQuestChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日常任务类型
/// </summary>
private Common.QuestEnum.EDailyQuestType type;
/// <summary>
/// 任务数据更新
/// </summary>
private Common.QuestObj.DailyQuest_Info quest;


public GS2GC_028_060_OnPlayerDailyQuestChg() {
	type = 0;
	quest = new Common.QuestObj.DailyQuest_Info();
}

public GS2GC_028_060_OnPlayerDailyQuestChg(
	Common.QuestEnum.EDailyQuestType _type
	, Common.QuestObj.DailyQuest_Info _quest
) {	type = _type;
	quest = _quest;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 日常任务类型
/// </summary>
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/// <summary>
/// 日常任务类型
/// </summary>
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }
/// <summary>
/// 任务数据更新
/// </summary>
public Common.QuestObj.DailyQuest_Info getQuest() { return quest; }
/// <summary>
/// 任务数据更新
/// </summary>
public void setQuest(Common.QuestObj.DailyQuest_Info _quest) { quest = _quest; }


public int GetBufSize() {
	int _size = 25;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.QuestEnum.EDailyQuestType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _questCustLen = _buf.getInt();
	int _questCurPos = _buf.getCurPos();
	quest.ReadUnzipBuf(_buf, _questCurPos + _questCustLen);
	_buf.setPosition(_questCurPos + _questCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putInt(quest.GetBufSize());
	quest.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)60);
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
	builder.Append("quest").Append(":").Append(quest == null ? "null" : quest.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

