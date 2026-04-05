using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p008_TravelOp
{

/// <summary>
/// 处理妃子酒馆游历事件
/// </summary>
public class GC2GS_008_004_ReqDealConsortBarTravel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 游历事件ID
/// </summary>
private long instanceId;
/// <summary>
/// 妃子ID
/// </summary>
private long consortId;
/// <summary>
/// 消耗配置ID
/// </summary>
private long costId;


public GC2GS_008_004_ReqDealConsortBarTravel() {
	instanceId = (long)0;
	consortId = (long)0;
	costId = (long)0;
}

public GC2GS_008_004_ReqDealConsortBarTravel(
	long _instanceId
	, long _consortId
	, long _costId
) {	instanceId = _instanceId;
	consortId = _consortId;
	costId = _costId;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 游历事件ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 游历事件ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 妃子ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 妃子ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 消耗配置ID
/// </summary>
public long getCostId() { return costId; }
/// <summary>
/// 消耗配置ID
/// </summary>
public void setCostId(long _costId) { costId = _costId; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(consortId);
	_buf.putLong(costId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("costId").Append(":").Append(costId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

