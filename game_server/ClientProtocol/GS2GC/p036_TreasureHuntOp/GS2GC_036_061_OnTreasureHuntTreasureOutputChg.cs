using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-奇物产出变更
/// </summary>
public class GS2GC_036_061_OnTreasureHuntTreasureOutputChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奇物产出信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo treasureOutputInfo;


public GS2GC_036_061_OnTreasureHuntTreasureOutputChg() {
	treasureOutputInfo = new Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo();
}

public GS2GC_036_061_OnTreasureHuntTreasureOutputChg(
	Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo _treasureOutputInfo
) {	treasureOutputInfo = _treasureOutputInfo;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 奇物产出信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo getTreasureOutputInfo() { return treasureOutputInfo; }
/// <summary>
/// 奇物产出信息
/// </summary>
public void setTreasureOutputInfo(Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo _treasureOutputInfo) { treasureOutputInfo = _treasureOutputInfo; }


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
	int _treasureOutputInfoCustLen = _buf.getInt();
	int _treasureOutputInfoCurPos = _buf.getCurPos();
	treasureOutputInfo.ReadUnzipBuf(_buf, _treasureOutputInfoCurPos + _treasureOutputInfoCustLen);
	_buf.setPosition(_treasureOutputInfoCurPos + _treasureOutputInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(treasureOutputInfo.GetBufSize());
	treasureOutputInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)61);
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
	builder.Append("treasureOutputInfo").Append(":").Append(treasureOutputInfo == null ? "null" : treasureOutputInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

