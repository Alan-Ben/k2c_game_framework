using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-组合激活
/// </summary>
public class GC2GS_036_006_ReqTreasureHuntCompositeActive : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组合ID
/// </summary>
private long compositeId;
/// <summary>
/// true普通技能 false高级技能
/// </summary>
private bool isNormal;


public GC2GS_036_006_ReqTreasureHuntCompositeActive() {
	compositeId = (long)0;
	isNormal = false;
}

public GC2GS_036_006_ReqTreasureHuntCompositeActive(
	long _compositeId
	, bool _isNormal
) {	compositeId = _compositeId;
	isNormal = _isNormal;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 组合ID
/// </summary>
public long getCompositeId() { return compositeId; }
/// <summary>
/// 组合ID
/// </summary>
public void setCompositeId(long _compositeId) { compositeId = _compositeId; }
/// <summary>
/// true普通技能 false高级技能
/// </summary>
public bool getIsNormal() { return isNormal; }
/// <summary>
/// true普通技能 false高级技能
/// </summary>
public void setIsNormal(bool _isNormal) { isNormal = _isNormal; }


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
	compositeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNormal = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(compositeId);
	_buf.put(isNormal?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)6);
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
	builder.Append("compositeId").Append(":").Append(compositeId.ToString()).Append(", ");
	builder.Append("isNormal").Append(":").Append(isNormal.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

