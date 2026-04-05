using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_071_OnStageGoalChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段任务数据
/// </summary>
private Common.StageGoalObj.StageGoal_Info stageGoal;


public GS2GC_007_071_OnStageGoalChg() {
	stageGoal = new Common.StageGoalObj.StageGoal_Info();
}

public GS2GC_007_071_OnStageGoalChg(
	Common.StageGoalObj.StageGoal_Info _stageGoal
) {	stageGoal = _stageGoal;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)71; }

/// <summary>
/// 阶段任务数据
/// </summary>
public Common.StageGoalObj.StageGoal_Info getStageGoal() { return stageGoal; }
/// <summary>
/// 阶段任务数据
/// </summary>
public void setStageGoal(Common.StageGoalObj.StageGoal_Info _stageGoal) { stageGoal = _stageGoal; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + stageGoal.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + stageGoal.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stageGoalCustLen = _buf.getInt();
	int _stageGoalCurPos = _buf.getCurPos();
	stageGoal.ReadUnzipBuf(_buf, _stageGoalCurPos + _stageGoalCustLen);
	_buf.setPosition(_stageGoalCurPos + _stageGoalCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stageGoal.GetBufSize());
	stageGoal.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)71);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)71);
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
	builder.Append("stageGoal").Append(":").Append(stageGoal == null ? "null" : stageGoal.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

