using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 午间副本_出战大臣信息
/// </summary>
public class MiddayDungeon_FightHero : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣ID
/// </summary>
private long heroId;
/// <summary>
/// 出战次数
/// </summary>
private short num;


public MiddayDungeon_FightHero() {
	heroId = (long)0;
	num = (short)0;
}

public MiddayDungeon_FightHero(
	long _heroId
	, short _num
) {	heroId = _heroId;
	num = _num;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大臣ID
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣ID
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 出战次数
/// </summary>
public short getNum() { return num; }
/// <summary>
/// 出战次数
/// </summary>
public void setNum(short _num) { num = _num; }


public int GetBufSize() {
	int _size = 10;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 12;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getShort();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putShort(num);
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
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

