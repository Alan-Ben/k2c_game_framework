using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人-随机邀约
/// </summary>
public class GS2GC_015_004_RetCallRand : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邀约结果
/// </summary>
private Common.ConsortObj.Consort_CallRes res;


public GS2GC_015_004_RetCallRand() {
	res = new Common.ConsortObj.Consort_CallRes();
}

public GS2GC_015_004_RetCallRand(
	Common.ConsortObj.Consort_CallRes _res
) {	res = _res;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 邀约结果
/// </summary>
public Common.ConsortObj.Consort_CallRes getRes() { return res; }
/// <summary>
/// 邀约结果
/// </summary>
public void setRes(Common.ConsortObj.Consort_CallRes _res) { res = _res; }


public int GetBufSize() {
	int _size = 37;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _resCustLen = _buf.getInt();
	int _resCurPos = _buf.getCurPos();
	res.ReadUnzipBuf(_buf, _resCurPos + _resCustLen);
	_buf.setPosition(_resCurPos + _resCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(res.GetBufSize());
	res.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
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
	builder.Append("res").Append(":").Append(res == null ? "null" : res.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

