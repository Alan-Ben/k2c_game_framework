using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p008_TravelOp
{

/// <summary>
/// 处理兑换游历事件
/// </summary>
public class GC2GS_008_005_ReqDealChangeTravel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 游历事件ID
/// </summary>
private long instanceId;
/// <summary>
/// 是否兑换 true-兑换
/// </summary>
private bool isChange;


public GC2GS_008_005_ReqDealChangeTravel() {
	instanceId = (long)0;
	isChange = false;
}

public GC2GS_008_005_ReqDealChangeTravel(
	long _instanceId
	, bool _isChange
) {	instanceId = _instanceId;
	isChange = _isChange;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 游历事件ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 游历事件ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 是否兑换 true-兑换
/// </summary>
public bool getIsChange() { return isChange; }
/// <summary>
/// 是否兑换 true-兑换
/// </summary>
public void setIsChange(bool _isChange) { isChange = _isChange; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isChange = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.put(isChange?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)5);
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
	builder.Append("isChange").Append(":").Append(isChange.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

