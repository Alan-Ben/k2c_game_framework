using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 午间副本_Boss信息
/// </summary>
public class MiddayDungeon_BossInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 波数
/// </summary>
private int wave;
/// <summary>
/// 扣除血量
/// </summary>
private long deductedHp;


public MiddayDungeon_BossInfo() {
	wave = 0;
	deductedHp = (long)0;
}

public MiddayDungeon_BossInfo(
	int _wave
	, long _deductedHp
) {	wave = _wave;
	deductedHp = _deductedHp;
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
	wave = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	deductedHp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(wave);
	_buf.putLong(deductedHp);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

