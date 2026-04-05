using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-矿石最大记录变更
/// </summary>
public class GS2GC_036_058_OnTreasureHuntOreMaxRecordChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 最大记录
/// </summary>
private int maxRecord;


public GS2GC_036_058_OnTreasureHuntOreMaxRecordChg() {
	oreId = (long)0;
	maxRecord = 0;
}

public GS2GC_036_058_OnTreasureHuntOreMaxRecordChg(
	long _oreId
	, int _maxRecord
) {	oreId = _oreId;
	maxRecord = _maxRecord;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// 最大记录
/// </summary>
public int getMaxRecord() { return maxRecord; }
/// <summary>
/// 最大记录
/// </summary>
public void setMaxRecord(int _maxRecord) { maxRecord = _maxRecord; }


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
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxRecord = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.putInt(maxRecord);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
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
	builder.Append("oreId").Append(":").Append(oreId.ToString()).Append(", ");
	builder.Append("maxRecord").Append(":").Append(maxRecord.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

