using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-奇物产出信息
/// </summary>
public class TreasureHunt_TreasureOutputInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奇物ID
/// </summary>
private long treasureId;
/// <summary>
/// 下次可领取钻石时间 ms
/// </summary>
private long nextCanDrawGemTimeMs;
/// <summary>
/// 下次可领取钻石数量
/// </summary>
private int nextCanDrawGemNum;


public TreasureHunt_TreasureOutputInfo() {
	treasureId = (long)0;
	nextCanDrawGemTimeMs = (long)0;
	nextCanDrawGemNum = 0;
}

public TreasureHunt_TreasureOutputInfo(
	long _treasureId
	, long _nextCanDrawGemTimeMs
	, int _nextCanDrawGemNum
) {	treasureId = _treasureId;
	nextCanDrawGemTimeMs = _nextCanDrawGemTimeMs;
	nextCanDrawGemNum = _nextCanDrawGemNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 奇物ID
/// </summary>
public long getTreasureId() { return treasureId; }
/// <summary>
/// 奇物ID
/// </summary>
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }
/// <summary>
/// 下次可领取钻石时间 ms
/// </summary>
public long getNextCanDrawGemTimeMs() { return nextCanDrawGemTimeMs; }
/// <summary>
/// 下次可领取钻石时间 ms
/// </summary>
public void setNextCanDrawGemTimeMs(long _nextCanDrawGemTimeMs) { nextCanDrawGemTimeMs = _nextCanDrawGemTimeMs; }
/// <summary>
/// 下次可领取钻石数量
/// </summary>
public int getNextCanDrawGemNum() { return nextCanDrawGemNum; }
/// <summary>
/// 下次可领取钻石数量
/// </summary>
public void setNextCanDrawGemNum(int _nextCanDrawGemNum) { nextCanDrawGemNum = _nextCanDrawGemNum; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	treasureId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextCanDrawGemTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextCanDrawGemNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(treasureId);
	_buf.putLong(nextCanDrawGemTimeMs);
	_buf.putInt(nextCanDrawGemNum);
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
	builder.Append("treasureId").Append(":").Append(treasureId.ToString()).Append(", ");
	builder.Append("nextCanDrawGemTimeMs").Append(":").Append(nextCanDrawGemTimeMs.ToString()).Append(", ");
	builder.Append("nextCanDrawGemNum").Append(":").Append(nextCanDrawGemNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

