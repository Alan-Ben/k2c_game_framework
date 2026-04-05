using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 大臣竞技场展示信息
/// </summary>
public class Hero_ArenaShowInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣id
/// </summary>
private long heroId;
/// <summary>
/// 皮肤id
/// </summary>
private long skinId;
/// <summary>
/// 等级
/// </summary>
private int level;
/// <summary>
/// 实力
/// </summary>
private long power;


public Hero_ArenaShowInfo() {
	heroId = (long)0;
	skinId = (long)0;
	level = 0;
	power = (long)0;
}

public Hero_ArenaShowInfo(
	long _heroId
	, long _skinId
	, int _level
	, long _power
) {	heroId = _heroId;
	skinId = _skinId;
	level = _level;
	power = _power;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 皮肤id
/// </summary>
public long getSkinId() { return skinId; }
/// <summary>
/// 皮肤id
/// </summary>
public void setSkinId(long _skinId) { skinId = _skinId; }
/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 实力
/// </summary>
public long getPower() { return power; }
/// <summary>
/// 实力
/// </summary>
public void setPower(long _power) { power = _power; }


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
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	power = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putInt(level);
	_buf.putLong(power);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("power").Append(":").Append(power.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

