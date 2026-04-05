using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p028_QuestOp
{

public class GS2GC_028_051_OnPlayerQuestStepCountChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务ID
/// </summary>
private long questId;
/// <summary>
/// 任务步骤
/// </summary>
private long questStep;
/// <summary>
/// 任务步骤下的目标ID
/// </summary>
private long questTargetId;
/// <summary>
/// 任务步骤的计数
/// </summary>
private long questTargetCount;


public GS2GC_028_051_OnPlayerQuestStepCountChg() {
	questId = (long)0;
	questStep = (long)0;
	questTargetId = (long)0;
	questTargetCount = (long)0;
}

public GS2GC_028_051_OnPlayerQuestStepCountChg(
	long _questId
	, long _questStep
	, long _questTargetId
	, long _questTargetCount
) {	questId = _questId;
	questStep = _questStep;
	questTargetId = _questTargetId;
	questTargetCount = _questTargetCount;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 任务ID
/// </summary>
public long getQuestId() { return questId; }
/// <summary>
/// 任务ID
/// </summary>
public void setQuestId(long _questId) { questId = _questId; }
/// <summary>
/// 任务步骤
/// </summary>
public long getQuestStep() { return questStep; }
/// <summary>
/// 任务步骤
/// </summary>
public void setQuestStep(long _questStep) { questStep = _questStep; }
/// <summary>
/// 任务步骤下的目标ID
/// </summary>
public long getQuestTargetId() { return questTargetId; }
/// <summary>
/// 任务步骤下的目标ID
/// </summary>
public void setQuestTargetId(long _questTargetId) { questTargetId = _questTargetId; }
/// <summary>
/// 任务步骤的计数
/// </summary>
public long getQuestTargetCount() { return questTargetCount; }
/// <summary>
/// 任务步骤的计数
/// </summary>
public void setQuestTargetCount(long _questTargetCount) { questTargetCount = _questTargetCount; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questStep = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questTargetId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questTargetCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questId);
	_buf.putLong(questStep);
	_buf.putLong(questTargetId);
	_buf.putLong(questTargetCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)51);
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
	builder.Append("questStep").Append(":").Append(questStep.ToString()).Append(", ");
	builder.Append("questTargetId").Append(":").Append(questTargetId.ToString()).Append(", ");
	builder.Append("questTargetCount").Append(":").Append(questTargetCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

