using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

public class GS2GC_042_004_RetMarsHelpDealedList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已互助的数量
/// </summary>
private int dealedCount;
/// <summary>
/// 允许互助的上限
/// </summary>
private int dealLimit;


public GS2GC_042_004_RetMarsHelpDealedList() {
	dealedCount = 0;
	dealLimit = 0;
}

public GS2GC_042_004_RetMarsHelpDealedList(
	int _dealedCount
	, int _dealLimit
) {	dealedCount = _dealedCount;
	dealLimit = _dealLimit;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 已互助的数量
/// </summary>
public int getDealedCount() { return dealedCount; }
/// <summary>
/// 已互助的数量
/// </summary>
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }
/// <summary>
/// 允许互助的上限
/// </summary>
public int getDealLimit() { return dealLimit; }
/// <summary>
/// 允许互助的上限
/// </summary>
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }


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
	dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealLimit = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(dealedCount);
	_buf.putInt(dealLimit);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
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
	builder.Append("dealedCount").Append(":").Append(dealedCount.ToString()).Append(", ");
	builder.Append("dealLimit").Append(":").Append(dealLimit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

