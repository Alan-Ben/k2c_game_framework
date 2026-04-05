using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 政务事件数据变更
/// </summary>
public class GS2GC_021_070_OnAnecdoteChg : ALBasicProtocolPack._IALProtocolStructure {
private long instanceId;
private byte[] extraData;


public GS2GC_021_070_OnAnecdoteChg() {
	instanceId = (long)0;
	extraData = null;
}

public GS2GC_021_070_OnAnecdoteChg(
	long _instanceId
	, byte[] _extraData
) {	instanceId = _instanceId;
	extraData = _extraData;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)70; }

public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public byte[] getExtraData() { return extraData; }

public void setExtraData(byte[] _extraData) { extraData = _extraData; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extraData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putByteBuffer(extraData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)70);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)70);
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
	builder.Append("extraData").Append(":").Append(extraData == null ? "null" : extraData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

