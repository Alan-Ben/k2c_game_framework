using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-领取矿石记录奖励
/// </summary>
public class GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 记录档位索引
/// </summary>
private int recordIndex;


public GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward() {
	oreId = (long)0;
	recordIndex = 0;
}

public GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward(
	long _oreId
	, int _recordIndex
) {	oreId = _oreId;
	recordIndex = _recordIndex;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// 记录档位索引
/// </summary>
public int getRecordIndex() { return recordIndex; }
/// <summary>
/// 记录档位索引
/// </summary>
public void setRecordIndex(int _recordIndex) { recordIndex = _recordIndex; }


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
	recordIndex = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.putInt(recordIndex);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)7);
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
	builder.Append("recordIndex").Append(":").Append(recordIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

