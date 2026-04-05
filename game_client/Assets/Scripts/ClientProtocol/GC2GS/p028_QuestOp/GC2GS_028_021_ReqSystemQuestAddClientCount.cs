using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p028_QuestOp
{

public class GC2GS_028_021_ReqSystemQuestAddClientCount : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组id
/// </summary>
private long groupId;
/// <summary>
/// 步骤
/// </summary>
private int step;
/// <summary>
/// 修改的进度值
/// </summary>
private int changeCount;


public GC2GS_028_021_ReqSystemQuestAddClientCount() {
	groupId = (long)0;
	step = 0;
	changeCount = 0;
}

public GC2GS_028_021_ReqSystemQuestAddClientCount(
	long _groupId
	, int _step
	, int _changeCount
) {	groupId = _groupId;
	step = _step;
	changeCount = _changeCount;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 步骤
/// </summary>
public int getStep() { return step; }
/// <summary>
/// 步骤
/// </summary>
public void setStep(int _step) { step = _step; }
/// <summary>
/// 修改的进度值
/// </summary>
public int getChangeCount() { return changeCount; }
/// <summary>
/// 修改的进度值
/// </summary>
public void setChangeCount(int _changeCount) { changeCount = _changeCount; }


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
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	changeCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putInt(step);
	_buf.putInt(changeCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)21);
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("step").Append(":").Append(step.ToString()).Append(", ");
	builder.Append("changeCount").Append(":").Append(changeCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

