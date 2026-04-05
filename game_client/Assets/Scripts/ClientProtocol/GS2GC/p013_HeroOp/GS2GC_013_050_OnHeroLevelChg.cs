using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣升级
/// </summary>
public class GS2GC_013_050_OnHeroLevelChg : ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
/// <summary>
/// 当前等级
/// </summary>
private int level;


public GS2GC_013_050_OnHeroLevelChg() {
	heroId = (long)0;
	level = 0;
}

public GS2GC_013_050_OnHeroLevelChg(
	long _heroId
	, int _level
) {	heroId = _heroId;
	level = _level;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)50; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 当前等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 当前等级
/// </summary>
public void setLevel(int _level) { level = _level; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putInt(level);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)50);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

