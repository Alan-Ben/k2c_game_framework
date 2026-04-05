using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 晚间副本_Boss信息
/// </summary>
public class EveningDungeon_BossInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 波数
/// </summary>
private int wave;
/// <summary>
/// 扣除血量
/// </summary>
private long deductedHp;
/// <summary>
/// 总血量
/// </summary>
private long totalHp;
/// <summary>
/// 被击败时间戳
/// </summary>
private long beDefeatTimeMs;
/// <summary>
/// 击杀玩家ID
/// </summary>
private long defeatCid;


public EveningDungeon_BossInfo() {
	wave = 0;
	deductedHp = (long)0;
	totalHp = (long)0;
	beDefeatTimeMs = (long)0;
	defeatCid = (long)0;
}

public EveningDungeon_BossInfo(
	int _wave
	, long _deductedHp
	, long _totalHp
	, long _beDefeatTimeMs
	, long _defeatCid
) {	wave = _wave;
	deductedHp = _deductedHp;
	totalHp = _totalHp;
	beDefeatTimeMs = _beDefeatTimeMs;
	defeatCid = _defeatCid;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 波数
/// </summary>
public int getWave() { return wave; }
/// <summary>
/// 波数
/// </summary>
public void setWave(int _wave) { wave = _wave; }
/// <summary>
/// 扣除血量
/// </summary>
public long getDeductedHp() { return deductedHp; }
/// <summary>
/// 扣除血量
/// </summary>
public void setDeductedHp(long _deductedHp) { deductedHp = _deductedHp; }
/// <summary>
/// 总血量
/// </summary>
public long getTotalHp() { return totalHp; }
/// <summary>
/// 总血量
/// </summary>
public void setTotalHp(long _totalHp) { totalHp = _totalHp; }
/// <summary>
/// 被击败时间戳
/// </summary>
public long getBeDefeatTimeMs() { return beDefeatTimeMs; }
/// <summary>
/// 被击败时间戳
/// </summary>
public void setBeDefeatTimeMs(long _beDefeatTimeMs) { beDefeatTimeMs = _beDefeatTimeMs; }
/// <summary>
/// 击杀玩家ID
/// </summary>
public long getDefeatCid() { return defeatCid; }
/// <summary>
/// 击杀玩家ID
/// </summary>
public void setDefeatCid(long _defeatCid) { defeatCid = _defeatCid; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	wave = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	deductedHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	beDefeatTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	defeatCid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(wave);
	_buf.putLong(deductedHp);
	_buf.putLong(totalHp);
	_buf.putLong(beDefeatTimeMs);
	_buf.putLong(defeatCid);
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
	builder.Append("wave").Append(":").Append(wave.ToString()).Append(", ");
	builder.Append("deductedHp").Append(":").Append(deductedHp.ToString()).Append(", ");
	builder.Append("totalHp").Append(":").Append(totalHp.ToString()).Append(", ");
	builder.Append("beDefeatTimeMs").Append(":").Append(beDefeatTimeMs.ToString()).Append(", ");
	builder.Append("defeatCid").Append(":").Append(defeatCid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

