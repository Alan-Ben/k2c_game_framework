using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 请求急速兑换
/// </summary>
public class GC2GS_004_101_ReqRushExchange : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否使用钻石补充不足道具
/// </summary>
private bool useGemSupplement;


public GC2GS_004_101_ReqRushExchange() {
	useGemSupplement = false;
}

public GC2GS_004_101_ReqRushExchange(
	bool _useGemSupplement
) {	useGemSupplement = _useGemSupplement;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)101; }

/// <summary>
/// 是否使用钻石补充不足道具
/// </summary>
public bool getUseGemSupplement() { return useGemSupplement; }
/// <summary>
/// 是否使用钻石补充不足道具
/// </summary>
public void setUseGemSupplement(bool _useGemSupplement) { useGemSupplement = _useGemSupplement; }


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
	useGemSupplement = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(useGemSupplement?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)101);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)101);
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
	builder.Append("useGemSupplement").Append(":").Append(useGemSupplement.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

