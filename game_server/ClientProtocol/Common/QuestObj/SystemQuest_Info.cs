using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.QuestObj
{

/// <summary>
/// 系统任务数据
/// </summary>
public class SystemQuest_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组ID
/// </summary>
private long groupId;
/// <summary>
/// 步骤
/// </summary>
private int step;
/// <summary>
/// 计数
/// </summary>
private long count;


public SystemQuest_Info() {
	groupId = (long)0;
	step = 0;
	count = (long)0;
}

public SystemQuest_Info(
	long _groupId
	, int _step
	, long _count
) {	groupId = _groupId;
	step = _step;
	count = _count;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 组ID
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 组ID
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
/// 计数
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 计数
/// </summary>
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putInt(step);
	_buf.putLong(count);
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("step").Append(":").Append(step.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

