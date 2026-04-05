using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-获得奇物
/// </summary>
public class GS2GC_036_051_OnTreasureHuntTreasureAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奇物信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_TreasureInfo treasureInfo;


public GS2GC_036_051_OnTreasureHuntTreasureAdd() {
	treasureInfo = new Common.TreasureHuntObj.TreasureHunt_TreasureInfo();
}

public GS2GC_036_051_OnTreasureHuntTreasureAdd(
	Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureInfo
) {	treasureInfo = _treasureInfo;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 奇物信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_TreasureInfo getTreasureInfo() { return treasureInfo; }
/// <summary>
/// 奇物信息
/// </summary>
public void setTreasureInfo(Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureInfo) { treasureInfo = _treasureInfo; }


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
	int _treasureInfoCustLen = _buf.getInt();
	int _treasureInfoCurPos = _buf.getCurPos();
	treasureInfo.ReadUnzipBuf(_buf, _treasureInfoCurPos + _treasureInfoCustLen);
	_buf.setPosition(_treasureInfoCurPos + _treasureInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(treasureInfo.GetBufSize());
	treasureInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)51);
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
	builder.Append("treasureInfo").Append(":").Append(treasureInfo == null ? "null" : treasureInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

