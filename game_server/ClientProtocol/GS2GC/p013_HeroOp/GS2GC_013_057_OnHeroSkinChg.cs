using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣皮肤变化推送
/// </summary>
public class GS2GC_013_057_OnHeroSkinChg : ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private Common.HeroObj.Hero_SkinInfo skinInfo;


public GS2GC_013_057_OnHeroSkinChg() {
	heroId = (long)0;
	skinInfo = new Common.HeroObj.Hero_SkinInfo();
}

public GS2GC_013_057_OnHeroSkinChg(
	long _heroId
	, Common.HeroObj.Hero_SkinInfo _skinInfo
) {	heroId = _heroId;
	skinInfo = _skinInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)57; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public Common.HeroObj.Hero_SkinInfo getSkinInfo() { return skinInfo; }
public void setSkinInfo(Common.HeroObj.Hero_SkinInfo _skinInfo) { skinInfo = _skinInfo; }


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
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _skinInfoCustLen = _buf.getInt();
	int _skinInfoCurPos = _buf.getCurPos();
	skinInfo.ReadUnzipBuf(_buf, _skinInfoCurPos + _skinInfoCustLen);
	_buf.setPosition(_skinInfoCurPos + _skinInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putInt(skinInfo.GetBufSize());
	skinInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)57);
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
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("skinInfo").Append(":").Append(skinInfo == null ? "null" : skinInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

