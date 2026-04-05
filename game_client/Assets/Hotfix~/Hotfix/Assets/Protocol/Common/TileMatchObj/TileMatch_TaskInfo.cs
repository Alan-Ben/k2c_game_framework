using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-任务信息
/// </summary>
public class TileMatch_TaskInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务序列号
/// </summary>
private int serialId;
/// <summary>
/// 任务id
/// </summary>
private long taskId;
/// <summary>
/// 子任务列表
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo> subList;
/// <summary>
/// 已经走的步数
/// </summary>
private int hadGoStep;


public TileMatch_TaskInfo() {
	serialId = 0;
	taskId = (long)0;
	subList = new List<Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo>();
	hadGoStep = 0;
}

public TileMatch_TaskInfo(
	int _serialId
	, long _taskId
	, List<Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo> _subList
	, int _hadGoStep
) {	serialId = _serialId;
	taskId = _taskId;
	subList = _subList;
	hadGoStep = _hadGoStep;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 任务序列号
/// </summary>
public int getSerialId() { return serialId; }
/// <summary>
/// 任务序列号
/// </summary>
public void setSerialId(int _serialId) { serialId = _serialId; }
/// <summary>
/// 任务id
/// </summary>
public long getTaskId() { return taskId; }
/// <summary>
/// 任务id
/// </summary>
public void setTaskId(long _taskId) { taskId = _taskId; }
/// <summary>
/// 子任务列表
/// </summary>
public List<Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo> getSubList() { return subList; }
/// <summary>
/// 子任务列表
/// </summary>
public void addSubList(Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo _subList) { subList.Add(_subList); }
/// <summary>
/// 已经走的步数
/// </summary>
public int getHadGoStep() { return hadGoStep; }
/// <summary>
/// 已经走的步数
/// </summary>
public void setHadGoStep(int _hadGoStep) { hadGoStep = _hadGoStep; }


public int GetBufSize() {
	int _size = 16;
	_size += 2 + (subList.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (subList.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _subListCount = _buf.getShort();
	for(int _i = 0; _i < _subListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo _subList = new Hotfix.Common.TileMatchObj.TileMatch_TaskSubInfo();
		int __subListCustLen = _buf.getInt();
	int __subListCurPos = _buf.getCurPos();
	_subList.ReadUnzipBuf(_buf, __subListCurPos + __subListCustLen);
	_buf.setPosition(__subListCurPos + __subListCustLen);

		subList.Add(_subList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadGoStep = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(serialId);
	_buf.putLong(taskId);
	_buf.putShort((short)subList.Count);
	for(int _i = 0; _i < subList.Count; _i++) { 
		_buf.putInt(subList[_i].GetBufSize());
	subList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(hadGoStep);
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
	builder.Append("serialId").Append(":").Append(serialId.ToString()).Append(", ");
	builder.Append("taskId").Append(":").Append(taskId.ToString()).Append(", ");
	builder.Append("subList").Append(":").Append(subList.ToString()).Append(", ");
	builder.Append("hadGoStep").Append(":").Append(hadGoStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

