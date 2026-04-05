using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 获得大臣
/// </summary>
public class GS2GC_013_055_OnGainHero : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣信息
/// </summary>
private Common.HeroObj.Hero_Info heroInfo;


public GS2GC_013_055_OnGainHero() {
	heroInfo = new Common.HeroObj.Hero_Info();
}

public GS2GC_013_055_OnGainHero(
	Common.HeroObj.Hero_Info _heroInfo
) {	heroInfo = _heroInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 大臣信息
/// </summary>
public Common.HeroObj.Hero_Info getHeroInfo() { return heroInfo; }
/// <summary>
/// 大臣信息
/// </summary>
public void setHeroInfo(Common.HeroObj.Hero_Info _heroInfo) { heroInfo = _heroInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + heroInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + heroInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _heroInfoCustLen = _buf.getInt();
	int _heroInfoCurPos = _buf.getCurPos();
	heroInfo.ReadUnzipBuf(_buf, _heroInfoCurPos + _heroInfoCustLen);
	_buf.setPosition(_heroInfoCurPos + _heroInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(heroInfo.GetBufSize());
	heroInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)55);
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
	builder.Append("heroInfo").Append(":").Append(heroInfo == null ? "null" : heroInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

