using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动阶段奖励分数变更
/// </summary>
public class GS2GC_017_057_OnActivityStepRewardScoreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 阶段奖励ID
/// </summary>
private long stepRewardId;
private long score;


public GS2GC_017_057_OnActivityStepRewardScoreChg() {
	instanceId = (long)0;
	stepRewardId = (long)0;
	score = (long)0;
}

public GS2GC_017_057_OnActivityStepRewardScoreChg(
	long _instanceId
	, long _stepRewardId
	, long _score
) {	instanceId = _instanceId;
	stepRewardId = _stepRewardId;
	score = _score;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 阶段奖励ID
/// </summary>
public long getStepRewardId() { return stepRewardId; }
/// <summary>
/// 阶段奖励ID
/// </summary>
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(stepRewardId);
	_buf.putLong(score);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)57);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("stepRewardId").Append(":").Append(stepRewardId.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

