using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动基金-分数变化推送
/// </summary>
public class GS2GC_017_064_OnActivityFundScoreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基金ID
/// </summary>
private long fundId;
/// <summary>
/// 公式分数
/// </summary>
private long formulaScore;
/// <summary>
/// 任务分数
/// </summary>
private long taskScore;


public GS2GC_017_064_OnActivityFundScoreChg() {
	fundId = (long)0;
	formulaScore = (long)0;
	taskScore = (long)0;
}

public GS2GC_017_064_OnActivityFundScoreChg(
	long _fundId
	, long _formulaScore
	, long _taskScore
) {	fundId = _fundId;
	formulaScore = _formulaScore;
	taskScore = _taskScore;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)64; }

/// <summary>
/// 基金ID
/// </summary>
public long getFundId() { return fundId; }
/// <summary>
/// 基金ID
/// </summary>
public void setFundId(long _fundId) { fundId = _fundId; }
/// <summary>
/// 公式分数
/// </summary>
public long getFormulaScore() { return formulaScore; }
/// <summary>
/// 公式分数
/// </summary>
public void setFormulaScore(long _formulaScore) { formulaScore = _formulaScore; }
/// <summary>
/// 任务分数
/// </summary>
public long getTaskScore() { return taskScore; }
/// <summary>
/// 任务分数
/// </summary>
public void setTaskScore(long _taskScore) { taskScore = _taskScore; }


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
	fundId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	formulaScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskScore = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(fundId);
	_buf.putLong(formulaScore);
	_buf.putLong(taskScore);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)64);
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
	builder.Append("fundId").Append(":").Append(fundId.ToString()).Append(", ");
	builder.Append("formulaScore").Append(":").Append(formulaScore.ToString()).Append(", ");
	builder.Append("taskScore").Append(":").Append(taskScore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

