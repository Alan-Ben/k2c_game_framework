using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-矿石数量变更
/// </summary>
public class GS2GC_036_054_OnTreasureHuntOreNumChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 矿石数量信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_OreNumInfo numInfo;


public GS2GC_036_054_OnTreasureHuntOreNumChg() {
	oreId = (long)0;
	numInfo = new Common.TreasureHuntObj.TreasureHunt_OreNumInfo();
}

public GS2GC_036_054_OnTreasureHuntOreNumChg(
	long _oreId
	, Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo
) {	oreId = _oreId;
	numInfo = _numInfo;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// 矿石数量信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_OreNumInfo getNumInfo() { return numInfo; }
/// <summary>
/// 矿石数量信息
/// </summary>
public void setNumInfo(Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo) { numInfo = _numInfo; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _numInfoCustLen = _buf.getInt();
	int _numInfoCurPos = _buf.getCurPos();
	numInfo.ReadUnzipBuf(_buf, _numInfoCurPos + _numInfoCustLen);
	_buf.setPosition(_numInfoCurPos + _numInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.putInt(numInfo.GetBufSize());
	numInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)54);
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
	builder.Append("numInfo").Append(":").Append(numInfo == null ? "null" : numInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

