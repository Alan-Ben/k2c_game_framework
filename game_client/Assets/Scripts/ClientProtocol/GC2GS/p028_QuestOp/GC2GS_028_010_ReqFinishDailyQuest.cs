using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p028_QuestOp
{

/// <summary>
/// 完成日常任务
/// </summary>
public class GC2GS_028_010_ReqFinishDailyQuest : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务刷新序列号
/// </summary>
private long refreshSerial;
/// <summary>
/// 任务id
/// </summary>
private long questId;


public GC2GS_028_010_ReqFinishDailyQuest() {
	refreshSerial = (long)0;
	questId = (long)0;
}

public GC2GS_028_010_ReqFinishDailyQuest(
	long _refreshSerial
	, long _questId
) {	refreshSerial = _refreshSerial;
	questId = _questId;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 任务刷新序列号
/// </summary>
public long getRefreshSerial() { return refreshSerial; }
/// <summary>
/// 任务刷新序列号
/// </summary>
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/// <summary>
/// 任务id
/// </summary>
public long getQuestId() { return questId; }
/// <summary>
/// 任务id
/// </summary>
public void setQuestId(long _questId) { questId = _questId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refreshSerial);
	_buf.putLong(questId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)10);
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
	builder.Append("questId").Append(":").Append(questId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

