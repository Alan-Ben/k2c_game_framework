using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-阶段奖励信息
/// </summary>
public class TileMatch_StepRewardInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段
/// </summary>
private int step;
/// <summary>
/// 分数
/// </summary>
private long curScore;
/// <summary>
/// 序列号
/// </summary>
private int serialId;


public TileMatch_StepRewardInfo() {
	step = 0;
	curScore = (long)0;
	serialId = 0;
}

public TileMatch_StepRewardInfo(
	int _step
	, long _curScore
	, int _serialId
) {	step = _step;
	curScore = _curScore;
	serialId = _serialId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 阶段
/// </summary>
public int getStep() { return step; }
/// <summary>
/// 阶段
/// </summary>
public void setStep(int _step) { step = _step; }
/// <summary>
/// 分数
/// </summary>
public long getCurScore() { return curScore; }
/// <summary>
/// 分数
/// </summary>
public void setCurScore(long _curScore) { curScore = _curScore; }
/// <summary>
/// 序列号
/// </summary>
public int getSerialId() { return serialId; }
/// <summary>
/// 序列号
/// </summary>
public void setSerialId(int _serialId) { serialId = _serialId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialId = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(step);
	_buf.putLong(curScore);
	_buf.putInt(serialId);
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
	builder.Append("step").Append(":").Append(step.ToString()).Append(", ");
	builder.Append("curScore").Append(":").Append(curScore.ToString()).Append(", ");
	builder.Append("serialId").Append(":").Append(serialId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

