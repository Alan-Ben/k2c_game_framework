using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.QuestObj
{

/// <summary>
/// 任务步骤数据
/// </summary>
public class Quest_Step : ALBasicProtocolPack._IALProtocolStructure {
private long questStep;
/// <summary>
/// 任务目标数据列表
/// </summary>
private List<Common.QuestObj.Quest_Target> targetList;
/// <summary>
/// 任务超时时间标记，单位秒。0表示永久
/// </summary>
private int expireTimeTagS;


public Quest_Step() {
	questStep = (long)0;
	targetList = new List<Common.QuestObj.Quest_Target>();
	expireTimeTagS = 0;
}

public Quest_Step(
	long _questStep
	, List<Common.QuestObj.Quest_Target> _targetList
	, int _expireTimeTagS
) {	questStep = _questStep;
	targetList = _targetList;
	expireTimeTagS = _expireTimeTagS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getQuestStep() { return questStep; }
public void setQuestStep(long _questStep) { questStep = _questStep; }
/// <summary>
/// 任务目标数据列表
/// </summary>
public List<Common.QuestObj.Quest_Target> getTargetList() { return targetList; }
/// <summary>
/// 任务目标数据列表
/// </summary>
public void addTargetList(Common.QuestObj.Quest_Target _targetList) { targetList.Add(_targetList); }
/// <summary>
/// 任务超时时间标记，单位秒。0表示永久
/// </summary>
public int getExpireTimeTagS() { return expireTimeTagS; }
/// <summary>
/// 任务超时时间标记，单位秒。0表示永久
/// </summary>
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (targetList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (targetList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questStep = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _targetListCount = _buf.getShort();
	for(int _i = 0; _i < _targetListCount; _i++) { 
		Common.QuestObj.Quest_Target _targetList = new Common.QuestObj.Quest_Target();
		int __targetListCustLen = _buf.getInt();
	int __targetListCurPos = _buf.getCurPos();
	_targetList.ReadUnzipBuf(_buf, __targetListCurPos + __targetListCustLen);
	_buf.setPosition(__targetListCurPos + __targetListCustLen);

		targetList.Add(_targetList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expireTimeTagS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questStep);
	_buf.putShort((short)targetList.Count);
	for(int _i = 0; _i < targetList.Count; _i++) { 
		_buf.putInt(targetList[_i].GetBufSize());
	targetList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(expireTimeTagS);
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
	builder.Append("questStep").Append(":").Append(questStep.ToString()).Append(", ");
	builder.Append("targetList").Append(":").Append(targetList.ToString()).Append(", ");
	builder.Append("expireTimeTagS").Append(":").Append(expireTimeTagS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

