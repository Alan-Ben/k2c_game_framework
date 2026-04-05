using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 处理联盟加入请求
/// </summary>
public class GC2GS_032_013_ReqProcessGuildJoinRequest : ALBasicProtocolPack._IALProtocolStructure {
private long requestId;
private bool isAccept;


public GC2GS_032_013_ReqProcessGuildJoinRequest() {
	requestId = (long)0;
	isAccept = false;
}

public GC2GS_032_013_ReqProcessGuildJoinRequest(
	long _requestId
	, bool _isAccept
) {	requestId = _requestId;
	isAccept = _isAccept;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)13; }

public long getRequestId() { return requestId; }
public void setRequestId(long _requestId) { requestId = _requestId; }
public bool getIsAccept() { return isAccept; }
public void setIsAccept(bool _isAccept) { isAccept = _isAccept; }


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
	requestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAccept = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(requestId);
	_buf.put(isAccept?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)13);
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
	builder.Append("requestId").Append(":").Append(requestId.ToString()).Append(", ");
	builder.Append("isAccept").Append(":").Append(isAccept.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

