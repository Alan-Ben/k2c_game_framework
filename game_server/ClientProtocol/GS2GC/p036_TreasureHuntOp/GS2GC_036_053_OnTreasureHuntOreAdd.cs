using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-新增矿石
/// </summary>
public class GS2GC_036_053_OnTreasureHuntOreAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_OreInfo oreInfo;


public GS2GC_036_053_OnTreasureHuntOreAdd() {
	oreInfo = new Common.TreasureHuntObj.TreasureHunt_OreInfo();
}

public GS2GC_036_053_OnTreasureHuntOreAdd(
	Common.TreasureHuntObj.TreasureHunt_OreInfo _oreInfo
) {	oreInfo = _oreInfo;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 矿石信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_OreInfo getOreInfo() { return oreInfo; }
/// <summary>
/// 矿石信息
/// </summary>
public void setOreInfo(Common.TreasureHuntObj.TreasureHunt_OreInfo _oreInfo) { oreInfo = _oreInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + oreInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + oreInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _oreInfoCustLen = _buf.getInt();
	int _oreInfoCurPos = _buf.getCurPos();
	oreInfo.ReadUnzipBuf(_buf, _oreInfoCurPos + _oreInfoCustLen);
	_buf.setPosition(_oreInfoCurPos + _oreInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(oreInfo.GetBufSize());
	oreInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)53);
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
	builder.Append("oreInfo").Append(":").Append(oreInfo == null ? "null" : oreInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

