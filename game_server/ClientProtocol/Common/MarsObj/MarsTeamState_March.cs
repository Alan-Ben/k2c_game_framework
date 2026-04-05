using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星队伍-行军状态数据
/// </summary>
public class MarsTeamState_March : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对象实例ID，对象类型根据进入的状态确认
/// </summary>
private long instanceId;
/// <summary>
/// 目标状态类型
/// </summary>
private int targetState;


public MarsTeamState_March() {
	instanceId = (long)0;
	targetState = 0;
}

public MarsTeamState_March(
	long _instanceId
	, int _targetState
) {	instanceId = _instanceId;
	targetState = _targetState;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 对象实例ID，对象类型根据进入的状态确认
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 对象实例ID，对象类型根据进入的状态确认
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 目标状态类型
/// </summary>
public int getTargetState() { return targetState; }
/// <summary>
/// 目标状态类型
/// </summary>
public void setTargetState(int _targetState) { targetState = _targetState; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetState = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(targetState);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("targetState").Append(":").Append(targetState.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

