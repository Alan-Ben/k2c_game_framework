using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店人气值变更
/// </summary>
public class GS2GC_034_058_OnInnPopularityChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 新人气值
/// </summary>
private long newPopularity;


public GS2GC_034_058_OnInnPopularityChg() {
	newPopularity = (long)0;
}

public GS2GC_034_058_OnInnPopularityChg(
	long _newPopularity
) {	newPopularity = _newPopularity;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 新人气值
/// </summary>
public long getNewPopularity() { return newPopularity; }
/// <summary>
/// 新人气值
/// </summary>
public void setNewPopularity(long _newPopularity) { newPopularity = _newPopularity; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	newPopularity = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(newPopularity);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)58);
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
	builder.Append("newPopularity").Append(":").Append(newPopularity.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

