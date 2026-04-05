using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

public class GC2GS_021_040_ReqDealAnecdoteChoiceEvent : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 政务实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 选项ID
/// </summary>
private long optionId;


public GC2GS_021_040_ReqDealAnecdoteChoiceEvent() {
	instanceId = (long)0;
	optionId = (long)0;
}

public GC2GS_021_040_ReqDealAnecdoteChoiceEvent(
	long _instanceId
	, long _optionId
) {	instanceId = _instanceId;
	optionId = _optionId;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)40; }

/// <summary>
/// 政务实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 政务实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 选项ID
/// </summary>
public long getOptionId() { return optionId; }
/// <summary>
/// 选项ID
/// </summary>
public void setOptionId(long _optionId) { optionId = _optionId; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	optionId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(optionId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)40);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)40);
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
	builder.Append("optionId").Append(":").Append(optionId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

