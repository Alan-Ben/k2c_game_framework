using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣光环变更推送
/// </summary>
public class GS2GC_013_061_OnHeroHaloChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 光环信息
/// </summary>
private Common.HeroObj.Hero_HaloInfo haloInfo;


public GS2GC_013_061_OnHeroHaloChg() {
	haloInfo = new Common.HeroObj.Hero_HaloInfo();
}

public GS2GC_013_061_OnHeroHaloChg(
	Common.HeroObj.Hero_HaloInfo _haloInfo
) {	haloInfo = _haloInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 光环信息
/// </summary>
public Common.HeroObj.Hero_HaloInfo getHaloInfo() { return haloInfo; }
/// <summary>
/// 光环信息
/// </summary>
public void setHaloInfo(Common.HeroObj.Hero_HaloInfo _haloInfo) { haloInfo = _haloInfo; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _haloInfoCustLen = _buf.getInt();
	int _haloInfoCurPos = _buf.getCurPos();
	haloInfo.ReadUnzipBuf(_buf, _haloInfoCurPos + _haloInfoCustLen);
	_buf.setPosition(_haloInfoCurPos + _haloInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(haloInfo.GetBufSize());
	haloInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
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
	builder.Append("haloInfo").Append(":").Append(haloInfo == null ? "null" : haloInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

