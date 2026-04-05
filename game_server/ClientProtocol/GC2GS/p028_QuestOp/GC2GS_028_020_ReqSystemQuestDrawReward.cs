using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p028_QuestOp
{

/// <summary>
/// 系统任务领取奖励
/// </summary>
public class GC2GS_028_020_ReqSystemQuestDrawReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组id
/// </summary>
private long groupId;
/// <summary>
/// 步骤
/// </summary>
private int step;


public GC2GS_028_020_ReqSystemQuestDrawReward() {
	groupId = (long)0;
	step = 0;
}

public GC2GS_028_020_ReqSystemQuestDrawReward(
	long _groupId
	, int _step
) {	groupId = _groupId;
	step = _step;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)20; }

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
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putInt(step);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)20);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

