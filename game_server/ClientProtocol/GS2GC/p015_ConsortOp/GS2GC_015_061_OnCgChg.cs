using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人CG数据变更
/// </summary>
public class GS2GC_015_061_OnCgChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人CG数据
/// </summary>
private Common.ConsortObj.Consort_CGInfo cg;


public GS2GC_015_061_OnCgChg() {
	cg = new Common.ConsortObj.Consort_CGInfo();
}

public GS2GC_015_061_OnCgChg(
	Common.ConsortObj.Consort_CGInfo _cg
) {	cg = _cg;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 家人CG数据
/// </summary>
public Common.ConsortObj.Consort_CGInfo getCg() { return cg; }
/// <summary>
/// 家人CG数据
/// </summary>
public void setCg(Common.ConsortObj.Consort_CGInfo _cg) { cg = _cg; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _cgCustLen = _buf.getInt();
	int _cgCurPos = _buf.getCurPos();
	cg.ReadUnzipBuf(_buf, _cgCurPos + _cgCustLen);
	_buf.setPosition(_cgCurPos + _cgCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(cg.GetBufSize());
	cg.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)61);
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
	builder.Append("cg").Append(":").Append(cg == null ? "null" : cg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

