using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣套系变更推送
/// </summary>
public class GS2GC_013_060_OnHeroSuitChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 套系信息
/// </summary>
private Common.HeroObj.Hero_SuitInfo suitInfo;


public GS2GC_013_060_OnHeroSuitChg() {
	suitInfo = new Common.HeroObj.Hero_SuitInfo();
}

public GS2GC_013_060_OnHeroSuitChg(
	Common.HeroObj.Hero_SuitInfo _suitInfo
) {	suitInfo = _suitInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 套系信息
/// </summary>
public Common.HeroObj.Hero_SuitInfo getSuitInfo() { return suitInfo; }
/// <summary>
/// 套系信息
/// </summary>
public void setSuitInfo(Common.HeroObj.Hero_SuitInfo _suitInfo) { suitInfo = _suitInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + suitInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + suitInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _suitInfoCustLen = _buf.getInt();
	int _suitInfoCurPos = _buf.getCurPos();
	suitInfo.ReadUnzipBuf(_buf, _suitInfoCurPos + _suitInfoCustLen);
	_buf.setPosition(_suitInfoCurPos + _suitInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(suitInfo.GetBufSize());
	suitInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)60);
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
	builder.Append("suitInfo").Append(":").Append(suitInfo == null ? "null" : suitInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

