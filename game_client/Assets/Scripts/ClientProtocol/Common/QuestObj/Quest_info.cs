using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.QuestObj
{

/// <summary>
/// 任务数据
/// </summary>
public class Quest_info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务配置ID
/// </summary>
private long questId;
/// <summary>
/// 任务状态
/// </summary>
private Common.QuestEnum.EQuestStatus questStatus;
/// <summary>
/// 任务步骤数据
/// </summary>
private Common.QuestObj.Quest_Step questStep;


public Quest_info() {
	questId = (long)0;
	questStatus = 0;
	questStep = new Common.QuestObj.Quest_Step();
}

public Quest_info(
	long _questId
	, Common.QuestEnum.EQuestStatus _questStatus
	, Common.QuestObj.Quest_Step _questStep
) {	questId = _questId;
	questStatus = _questStatus;
	questStep = _questStep;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 任务配置ID
/// </summary>
public long getQuestId() { return questId; }
/// <summary>
/// 任务配置ID
/// </summary>
public void setQuestId(long _questId) { questId = _questId; }
/// <summary>
/// 任务状态
/// </summary>
public Common.QuestEnum.EQuestStatus getQuestStatus() { return questStatus; }
/// <summary>
/// 任务状态
/// </summary>
public void setQuestStatus(Common.QuestEnum.EQuestStatus _questStatus) { questStatus = _questStatus; }
/// <summary>
/// 任务步骤数据
/// </summary>
public Common.QuestObj.Quest_Step getQuestStep() { return questStep; }
/// <summary>
/// 任务步骤数据
/// </summary>
public void setQuestStep(Common.QuestObj.Quest_Step _questStep) { questStep = _questStep; }


public int GetBufSize() {
	int _size = 12;
	_size += 4 + questStep.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + questStep.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questStatus = (Common.QuestEnum.EQuestStatus)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _questStepCustLen = _buf.getInt();
	int _questStepCurPos = _buf.getCurPos();
	questStep.ReadUnzipBuf(_buf, _questStepCurPos + _questStepCustLen);
	_buf.setPosition(_questStepCurPos + _questStepCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questId);
	_buf.putInt((int)questStatus);

	_buf.putInt(questStep.GetBufSize());
	questStep.PutUnzipBuf(_buf);
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
	builder.Append("questId").Append(":").Append(questId.ToString()).Append(", ");
	builder.Append("questStatus").Append(":").Append(questStatus.ToString()).Append(", ");
	builder.Append("questStep").Append(":").Append(questStep == null ? "null" : questStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

