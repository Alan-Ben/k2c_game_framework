using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 一键处理入盟请求
/// </summary>
public class GC2GS_032_023_ReqGuildJoinRequestAKeyDeal : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否同意
/// </summary>
private bool isAgree;


public GC2GS_032_023_ReqGuildJoinRequestAKeyDeal() {
	isAgree = false;
}

public GC2GS_032_023_ReqGuildJoinRequestAKeyDeal(
	bool _isAgree
) {	isAgree = _isAgree;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)23; }

/// <summary>
/// 是否同意
/// </summary>
public bool getIsAgree() { return isAgree; }
/// <summary>
/// 是否同意
/// </summary>
public void setIsAgree(bool _isAgree) { isAgree = _isAgree; }


public int GetBufSize() {
	int _size = 1;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAgree = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isAgree?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)23);
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
	builder.Append("isAgree").Append(":").Append(isAgree.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

