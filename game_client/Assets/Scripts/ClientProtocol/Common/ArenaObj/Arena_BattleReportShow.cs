using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场战报展示
/// </summary>
public class Arena_BattleReportShow : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对手CID
/// </summary>
private long opponentCid;
/// <summary>
/// 击败我方大臣数量
/// </summary>
private int defeatHeroNum;
/// <summary>
/// 扣除影响力
/// </summary>
private int deductinfluence;
/// <summary>
/// 时间戳
/// </summary>
private long timestamp;


public Arena_BattleReportShow() {
	opponentCid = (long)0;
	defeatHeroNum = 0;
	deductinfluence = 0;
	timestamp = (long)0;
}

public Arena_BattleReportShow(
	long _opponentCid
	, int _defeatHeroNum
	, int _deductinfluence
	, long _timestamp
) {	opponentCid = _opponentCid;
	defeatHeroNum = _defeatHeroNum;
	deductinfluence = _deductinfluence;
	timestamp = _timestamp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 对手CID
/// </summary>
public long getOpponentCid() { return opponentCid; }
/// <summary>
/// 对手CID
/// </summary>
public void setOpponentCid(long _opponentCid) { opponentCid = _opponentCid; }
/// <summary>
/// 击败我方大臣数量
/// </summary>
public int getDefeatHeroNum() { return defeatHeroNum; }
/// <summary>
/// 击败我方大臣数量
/// </summary>
public void setDefeatHeroNum(int _defeatHeroNum) { defeatHeroNum = _defeatHeroNum; }
/// <summary>
/// 扣除影响力
/// </summary>
public int getDeductinfluence() { return deductinfluence; }
/// <summary>
/// 扣除影响力
/// </summary>
public void setDeductinfluence(int _deductinfluence) { deductinfluence = _deductinfluence; }
/// <summary>
/// 时间戳
/// </summary>
public long getTimestamp() { return timestamp; }
/// <summary>
/// 时间戳
/// </summary>
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }


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
	opponentCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	defeatHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	deductinfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timestamp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(opponentCid);
	_buf.putInt(defeatHeroNum);
	_buf.putInt(deductinfluence);
	_buf.putLong(timestamp);
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
	builder.Append("opponentCid").Append(":").Append(opponentCid.ToString()).Append(", ");
	builder.Append("defeatHeroNum").Append(":").Append(defeatHeroNum.ToString()).Append(", ");
	builder.Append("deductinfluence").Append(":").Append(deductinfluence.ToString()).Append(", ");
	builder.Append("timestamp").Append(":").Append(timestamp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

