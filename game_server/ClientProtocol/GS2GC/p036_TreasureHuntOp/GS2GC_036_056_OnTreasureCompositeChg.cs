using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-矿石组合变更
/// </summary>
public class GS2GC_036_056_OnTreasureCompositeChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组合信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_CompositeInfo compositeInfo;


public GS2GC_036_056_OnTreasureCompositeChg() {
	compositeInfo = new Common.TreasureHuntObj.TreasureHunt_CompositeInfo();
}

public GS2GC_036_056_OnTreasureCompositeChg(
	Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeInfo
) {	compositeInfo = _compositeInfo;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 组合信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_CompositeInfo getCompositeInfo() { return compositeInfo; }
/// <summary>
/// 组合信息
/// </summary>
public void setCompositeInfo(Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeInfo) { compositeInfo = _compositeInfo; }


public int GetBufSize() {
	int _size = 22;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 24;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _compositeInfoCustLen = _buf.getInt();
	int _compositeInfoCurPos = _buf.getCurPos();
	compositeInfo.ReadUnzipBuf(_buf, _compositeInfoCurPos + _compositeInfoCustLen);
	_buf.setPosition(_compositeInfoCurPos + _compositeInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(compositeInfo.GetBufSize());
	compositeInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)56);
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
	builder.Append("compositeInfo").Append(":").Append(compositeInfo == null ? "null" : compositeInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

