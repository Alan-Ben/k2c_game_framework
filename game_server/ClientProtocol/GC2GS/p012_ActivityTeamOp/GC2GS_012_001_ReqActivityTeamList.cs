using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p012_ActivityTeamOp
{

/// <summary>
/// 队伍列表
/// </summary>
public class GC2GS_012_001_ReqActivityTeamList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 请求页码，从1开始
/// </summary>
private int page;


public GC2GS_012_001_ReqActivityTeamList() {
	instanceId = (long)0;
	page = 0;
}

public GC2GS_012_001_ReqActivityTeamList(
	long _instanceId
	, int _page
) {	instanceId = _instanceId;
	page = _page;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 请求页码，从1开始
/// </summary>
public int getPage() { return page; }
/// <summary>
/// 请求页码，从1开始
/// </summary>
public void setPage(int _page) { page = _page; }


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
	page = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(page);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)1);
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
	builder.Append("page").Append(":").Append(page.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

