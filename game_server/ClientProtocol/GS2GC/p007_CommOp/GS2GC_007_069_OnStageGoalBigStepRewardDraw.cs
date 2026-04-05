using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

/// <summary>
/// 阶段目标大阶段奖励领取推送
/// </summary>
public class GS2GC_007_069_OnStageGoalBigStepRewardDraw : ALBasicProtocolPack._IALProtocolStructure {
private long bigStep;


public GS2GC_007_069_OnStageGoalBigStepRewardDraw() {
	bigStep = (long)0;
}

public GS2GC_007_069_OnStageGoalBigStepRewardDraw(
	long _bigStep
) {	bigStep = _bigStep;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)69; }

public long getBigStep() { return bigStep; }
public void setBigStep(long _bigStep) { bigStep = _bigStep; }


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
	bigStep = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bigStep);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)69);
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
	builder.Append("bigStep").Append(":").Append(bigStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

