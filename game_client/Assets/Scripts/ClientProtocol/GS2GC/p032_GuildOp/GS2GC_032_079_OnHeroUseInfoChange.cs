using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 大臣使用信息变更推送
/// </summary>
public class GS2GC_032_079_OnHeroUseInfoChange : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣使用信息
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_HeroUseInfo heroUseInfo;


public GS2GC_032_079_OnHeroUseInfoChange() {
	heroUseInfo = new Common.GuildCooperateObj.GuildCooperate_HeroUseInfo();
}

public GS2GC_032_079_OnHeroUseInfoChange(
	Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfo
) {	heroUseInfo = _heroUseInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)79; }

/// <summary>
/// 大臣使用信息
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_HeroUseInfo getHeroUseInfo() { return heroUseInfo; }
/// <summary>
/// 大臣使用信息
/// </summary>
public void setHeroUseInfo(Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfo) { heroUseInfo = _heroUseInfo; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _heroUseInfoCustLen = _buf.getInt();
	int _heroUseInfoCurPos = _buf.getCurPos();
	heroUseInfo.ReadUnzipBuf(_buf, _heroUseInfoCurPos + _heroUseInfoCustLen);
	_buf.setPosition(_heroUseInfoCurPos + _heroUseInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(heroUseInfo.GetBufSize());
	heroUseInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)79);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)79);
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
	builder.Append("heroUseInfo").Append(":").Append(heroUseInfo == null ? "null" : heroUseInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

