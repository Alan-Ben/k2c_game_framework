using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarketObj
{

/// <summary>
/// 集市一键经营结果数据
/// </summary>
public class Market_AkeyResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 集市ID
/// </summary>
private long marketId;
/// <summary>
/// 倍数
/// </summary>
private int multiple;
/// <summary>
/// 是否暴击
/// </summary>
private bool isCrit;


public Market_AkeyResult() {
	marketId = (long)0;
	multiple = 0;
	isCrit = false;
}

public Market_AkeyResult(
	long _marketId
	, int _multiple
	, bool _isCrit
) {	marketId = _marketId;
	multiple = _multiple;
	isCrit = _isCrit;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 集市ID
/// </summary>
public long getMarketId() { return marketId; }
/// <summary>
/// 集市ID
/// </summary>
public void setMarketId(long _marketId) { marketId = _marketId; }
/// <summary>
/// 倍数
/// </summary>
public int getMultiple() { return multiple; }
/// <summary>
/// 倍数
/// </summary>
public void setMultiple(int _multiple) { multiple = _multiple; }
/// <summary>
/// 是否暴击
/// </summary>
public bool getIsCrit() { return isCrit; }
/// <summary>
/// 是否暴击
/// </summary>
public void setIsCrit(bool _isCrit) { isCrit = _isCrit; }


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
	marketId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	multiple = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCrit = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(marketId);
	_buf.putInt(multiple);
	_buf.put(isCrit?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("marketId").Append(":").Append(marketId.ToString()).Append(", ");
	builder.Append("multiple").Append(":").Append(multiple.ToString()).Append(", ");
	builder.Append("isCrit").Append(":").Append(isCrit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

