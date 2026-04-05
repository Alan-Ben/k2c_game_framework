using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p201_TileMatchOp
{

/// <summary>
/// 三消总分数变更
/// </summary>
public class GS2GC_201_055_OnTileMatchTotalScoreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 总分数
/// </summary>
private long totalScore;


public GS2GC_201_055_OnTileMatchTotalScoreChg() {
	totalScore = (long)0;
}

public GS2GC_201_055_OnTileMatchTotalScoreChg(
	long _totalScore
) {	totalScore = _totalScore;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 总分数
/// </summary>
public long getTotalScore() { return totalScore; }
/// <summary>
/// 总分数
/// </summary>
public void setTotalScore(long _totalScore) { totalScore = _totalScore; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalScore = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(totalScore);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)55);
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
	builder.Append("totalScore").Append(":").Append(totalScore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

