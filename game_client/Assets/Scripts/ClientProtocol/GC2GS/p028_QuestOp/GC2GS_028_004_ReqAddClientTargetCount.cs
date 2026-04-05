using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p028_QuestOp
{

public class GC2GS_028_004_ReqAddClientTargetCount : ALBasicProtocolPack._IALProtocolStructure {
private long questId;
private long questStepId;
private long questStepTargetId;
/// <summary>
/// 修改的进度值
/// </summary>
private int changeCount;


public GC2GS_028_004_ReqAddClientTargetCount() {
	questId = (long)0;
	questStepId = (long)0;
	questStepTargetId = (long)0;
	changeCount = 0;
}

public GC2GS_028_004_ReqAddClientTargetCount(
	long _questId
	, long _questStepId
	, long _questStepTargetId
	, int _changeCount
) {	questId = _questId;
	questStepId = _questStepId;
	questStepTargetId = _questStepTargetId;
	changeCount = _changeCount;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)4; }

public long getQuestId() { return questId; }
public void setQuestId(long _questId) { questId = _questId; }
public long getQuestStepId() { return questStepId; }
public void setQuestStepId(long _questStepId) { questStepId = _questStepId; }
public long getQuestStepTargetId() { return questStepTargetId; }
public void setQuestStepTargetId(long _questStepTargetId) { questStepTargetId = _questStepTargetId; }
/// <summary>
/// 修改的进度值
/// </summary>
public int getChangeCount() { return changeCount; }
/// <summary>
/// 修改的进度值
/// </summary>
public void setChangeCount(int _changeCount) { changeCount = _changeCount; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questStepId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questStepTargetId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	changeCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questId);
	_buf.putLong(questStepId);
	_buf.putLong(questStepTargetId);
	_buf.putInt(changeCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)4);
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
	builder.Append("questStepId").Append(":").Append(questStepId.ToString()).Append(", ");
	builder.Append("questStepTargetId").Append(":").Append(questStepTargetId.ToString()).Append(", ");
	builder.Append("changeCount").Append(":").Append(changeCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

