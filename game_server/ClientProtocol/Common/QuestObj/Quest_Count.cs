using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.QuestObj
{

/// <summary>
/// 任务计数
/// </summary>
public class Quest_Count : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务配置ID
/// </summary>
private long questId;
/// <summary>
/// 完成次数
/// </summary>
private int doneCount;


public Quest_Count() {
	questId = (long)0;
	doneCount = 0;
}

public Quest_Count(
	long _questId
	, int _doneCount
) {	questId = _questId;
	doneCount = _doneCount;
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
/// 完成次数
/// </summary>
public int getDoneCount() { return doneCount; }
/// <summary>
/// 完成次数
/// </summary>
public void setDoneCount(int _doneCount) { doneCount = _doneCount; }


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
	questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	doneCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questId);
	_buf.putInt(doneCount);
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
	builder.Append("doneCount").Append(":").Append(doneCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

