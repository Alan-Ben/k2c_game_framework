using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p019_DinnerOp
{

/// <summary>
/// 取指定宴会的下一条宴会数据
/// </summary>
public class GC2GS_019_012_ReqGetNextDinnerInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 当前排序
/// </summary>
private int idx;


public GC2GS_019_012_ReqGetNextDinnerInfo() {
	instanceId = (long)0;
	idx = 0;
}

public GC2GS_019_012_ReqGetNextDinnerInfo(
	long _instanceId
	, int _idx
) {	instanceId = _instanceId;
	idx = _idx;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// 宴会实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 宴会实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 当前排序
/// </summary>
public int getIdx() { return idx; }
/// <summary>
/// 当前排序
/// </summary>
public void setIdx(int _idx) { idx = _idx; }


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
	idx = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(idx);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)12);
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
	builder.Append("idx").Append(":").Append(idx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

