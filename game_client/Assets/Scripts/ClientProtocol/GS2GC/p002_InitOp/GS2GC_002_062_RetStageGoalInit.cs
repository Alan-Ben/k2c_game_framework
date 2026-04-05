using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 阶段任务数据组件初始化
/// </summary>
public class GS2GC_002_062_RetStageGoalInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前阶段任务数据
/// </summary>
private Common.StageGoalObj.StageGoal_Info curStageGoal;
/// <summary>
/// 已领取的大阶段奖励列表
/// </summary>
private List<long> hadDrawBigStepList;
/// <summary>
/// 已领取的大阶段首达奖励列表
/// </summary>
private List<long> hadDrawBigStepFirstReachList;
/// <summary>
/// 可领取的大阶段首达奖励列表
/// </summary>
private List<long> canDrawBigStepFirstReachList;


public GS2GC_002_062_RetStageGoalInit() {
	curStageGoal = new Common.StageGoalObj.StageGoal_Info();
	hadDrawBigStepList = new List<long>();
	hadDrawBigStepFirstReachList = new List<long>();
	canDrawBigStepFirstReachList = new List<long>();
}

public GS2GC_002_062_RetStageGoalInit(
	Common.StageGoalObj.StageGoal_Info _curStageGoal
	, List<long> _hadDrawBigStepList
	, List<long> _hadDrawBigStepFirstReachList
	, List<long> _canDrawBigStepFirstReachList
) {	curStageGoal = _curStageGoal;
	hadDrawBigStepList = _hadDrawBigStepList;
	hadDrawBigStepFirstReachList = _hadDrawBigStepFirstReachList;
	canDrawBigStepFirstReachList = _canDrawBigStepFirstReachList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)62; }

/// <summary>
/// 当前阶段任务数据
/// </summary>
public Common.StageGoalObj.StageGoal_Info getCurStageGoal() { return curStageGoal; }
/// <summary>
/// 当前阶段任务数据
/// </summary>
public void setCurStageGoal(Common.StageGoalObj.StageGoal_Info _curStageGoal) { curStageGoal = _curStageGoal; }
/// <summary>
/// 已领取的大阶段奖励列表
/// </summary>
public List<long> getHadDrawBigStepList() { return hadDrawBigStepList; }
/// <summary>
/// 已领取的大阶段奖励列表
/// </summary>
public void addHadDrawBigStepList(long _hadDrawBigStepList) { hadDrawBigStepList.Add(_hadDrawBigStepList); }
/// <summary>
/// 已领取的大阶段首达奖励列表
/// </summary>
public List<long> getHadDrawBigStepFirstReachList() { return hadDrawBigStepFirstReachList; }
/// <summary>
/// 已领取的大阶段首达奖励列表
/// </summary>
public void addHadDrawBigStepFirstReachList(long _hadDrawBigStepFirstReachList) { hadDrawBigStepFirstReachList.Add(_hadDrawBigStepFirstReachList); }
/// <summary>
/// 可领取的大阶段首达奖励列表
/// </summary>
public List<long> getCanDrawBigStepFirstReachList() { return canDrawBigStepFirstReachList; }
/// <summary>
/// 可领取的大阶段首达奖励列表
/// </summary>
public void addCanDrawBigStepFirstReachList(long _canDrawBigStepFirstReachList) { canDrawBigStepFirstReachList.Add(_canDrawBigStepFirstReachList); }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + curStageGoal.GetBufSize();
	_size += 2 + (hadDrawBigStepList.Count * 8);
	_size += 2 + (hadDrawBigStepFirstReachList.Count * 8);
	_size += 2 + (canDrawBigStepFirstReachList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + curStageGoal.GetBufSize();
	_size += 2 + (hadDrawBigStepList.Count * 8);
	_size += 2 + (hadDrawBigStepFirstReachList.Count * 8);
	_size += 2 + (canDrawBigStepFirstReachList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _curStageGoalCustLen = _buf.getInt();
	int _curStageGoalCurPos = _buf.getCurPos();
	curStageGoal.ReadUnzipBuf(_buf, _curStageGoalCurPos + _curStageGoalCustLen);
	_buf.setPosition(_curStageGoalCurPos + _curStageGoalCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawBigStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawBigStepListCount; _i++) { 
		long _hadDrawBigStepList = (long)0;
		_hadDrawBigStepList = _buf.getLong();
		hadDrawBigStepList.Add(_hadDrawBigStepList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawBigStepFirstReachListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawBigStepFirstReachListCount; _i++) { 
		long _hadDrawBigStepFirstReachList = (long)0;
		_hadDrawBigStepFirstReachList = _buf.getLong();
		hadDrawBigStepFirstReachList.Add(_hadDrawBigStepFirstReachList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _canDrawBigStepFirstReachListCount = _buf.getShort();
	for(int _i = 0; _i < _canDrawBigStepFirstReachListCount; _i++) { 
		long _canDrawBigStepFirstReachList = (long)0;
		_canDrawBigStepFirstReachList = _buf.getLong();
		canDrawBigStepFirstReachList.Add(_canDrawBigStepFirstReachList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(curStageGoal.GetBufSize());
	curStageGoal.PutUnzipBuf(_buf);
	_buf.putShort((short)hadDrawBigStepList.Count);
	for(int _i = 0; _i < hadDrawBigStepList.Count; _i++) { 
		_buf.putLong(hadDrawBigStepList[_i]);
	}
	_buf.putShort((short)hadDrawBigStepFirstReachList.Count);
	for(int _i = 0; _i < hadDrawBigStepFirstReachList.Count; _i++) { 
		_buf.putLong(hadDrawBigStepFirstReachList[_i]);
	}
	_buf.putShort((short)canDrawBigStepFirstReachList.Count);
	for(int _i = 0; _i < canDrawBigStepFirstReachList.Count; _i++) { 
		_buf.putLong(canDrawBigStepFirstReachList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)62);
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
	builder.Append("curStageGoal").Append(":").Append(curStageGoal == null ? "null" : curStageGoal.ToString()).Append(", ");
	builder.Append("hadDrawBigStepList").Append(":").Append(hadDrawBigStepList.ToString()).Append(", ");
	builder.Append("hadDrawBigStepFirstReachList").Append(":").Append(hadDrawBigStepFirstReachList.ToString()).Append(", ");
	builder.Append("canDrawBigStepFirstReachList").Append(":").Append(canDrawBigStepFirstReachList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

