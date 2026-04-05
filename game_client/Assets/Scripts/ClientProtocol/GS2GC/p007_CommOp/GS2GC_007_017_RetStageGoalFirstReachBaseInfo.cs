using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_017_RetStageGoalFirstReachBaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 顶级玩家信息
/// </summary>
private Common.StageGoalObj.StageGoal_TopPlayerInfo topInfo;
/// <summary>
/// 大阶段首达信息列表
/// </summary>
private List<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo> infoList;


public GS2GC_007_017_RetStageGoalFirstReachBaseInfo() {
	topInfo = new Common.StageGoalObj.StageGoal_TopPlayerInfo();
	infoList = new List<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo>();
}

public GS2GC_007_017_RetStageGoalFirstReachBaseInfo(
	Common.StageGoalObj.StageGoal_TopPlayerInfo _topInfo
	, List<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo> _infoList
) {	topInfo = _topInfo;
	infoList = _infoList;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)17; }

/// <summary>
/// 顶级玩家信息
/// </summary>
public Common.StageGoalObj.StageGoal_TopPlayerInfo getTopInfo() { return topInfo; }
/// <summary>
/// 顶级玩家信息
/// </summary>
public void setTopInfo(Common.StageGoalObj.StageGoal_TopPlayerInfo _topInfo) { topInfo = _topInfo; }
/// <summary>
/// 大阶段首达信息列表
/// </summary>
public List<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo> getInfoList() { return infoList; }
/// <summary>
/// 大阶段首达信息列表
/// </summary>
public void addInfoList(Common.StageGoalObj.StageGoal_BigStepFirstReachInfo _infoList) { infoList.Add(_infoList); }


public int GetBufSize() {
	int _size = 20;
	_size += 2 + (infoList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (infoList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _topInfoCustLen = _buf.getInt();
	int _topInfoCurPos = _buf.getCurPos();
	topInfo.ReadUnzipBuf(_buf, _topInfoCurPos + _topInfoCustLen);
	_buf.setPosition(_topInfoCurPos + _topInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.StageGoalObj.StageGoal_BigStepFirstReachInfo _infoList = new Common.StageGoalObj.StageGoal_BigStepFirstReachInfo();
		int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.getCurPos();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.setPosition(__infoListCurPos + __infoListCustLen);

		infoList.Add(_infoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(topInfo.GetBufSize());
	topInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)infoList.Count);
	for(int _i = 0; _i < infoList.Count; _i++) { 
		_buf.putInt(infoList[_i].GetBufSize());
	infoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)17);
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
	builder.Append("topInfo").Append(":").Append(topInfo == null ? "null" : topInfo.ToString()).Append(", ");
	builder.Append("infoList").Append(":").Append(infoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

