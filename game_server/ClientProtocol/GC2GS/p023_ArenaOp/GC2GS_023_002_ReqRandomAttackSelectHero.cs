using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 随机攻击选择出战大臣
/// </summary>
public class GC2GS_023_002_ReqRandomAttackSelectHero : ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
/// <summary>
/// 对手机器人名字
/// </summary>
private string opponentBotName;
private long buffId;


public GC2GS_023_002_ReqRandomAttackSelectHero() {
	heroId = (long)0;
	opponentBotName = "";
	buffId = (long)0;
}

public GC2GS_023_002_ReqRandomAttackSelectHero(
	long _heroId
	, string _opponentBotName
	, long _buffId
) {	heroId = _heroId;
	opponentBotName = _opponentBotName;
	buffId = _buffId;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)2; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 对手机器人名字
/// </summary>
public string getOpponentBotName() { return opponentBotName; }
/// <summary>
/// 对手机器人名字
/// </summary>
public void setOpponentBotName(string _opponentBotName) { opponentBotName = _opponentBotName; }
public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(opponentBotName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(opponentBotName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentBotName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buffId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putString(opponentBotName);
	_buf.putLong(buffId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)2);
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
	builder.Append("opponentBotName").Append(":").Append(opponentBotName.ToString()).Append(", ");
	builder.Append("buffId").Append(":").Append(buffId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

