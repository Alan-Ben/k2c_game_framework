using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p201_TileMatchOp
{

/// <summary>
/// 三消任务数据变更
/// </summary>
public class GS2GC_201_052_OnTileMatchTaskChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 模式类型
/// </summary>
private int modeType;
/// <summary>
/// 任务数据
/// </summary>
private Hotfix.Common.TileMatchObj.TileMatch_TaskInfo taskInfo;


public GS2GC_201_052_OnTileMatchTaskChg() {
	modeType = 0;
	taskInfo = new Hotfix.Common.TileMatchObj.TileMatch_TaskInfo();
}

public GS2GC_201_052_OnTileMatchTaskChg(
	int _modeType
	, Hotfix.Common.TileMatchObj.TileMatch_TaskInfo _taskInfo
) {	modeType = _modeType;
	taskInfo = _taskInfo;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 模式类型
/// </summary>
public int getModeType() { return modeType; }
/// <summary>
/// 模式类型
/// </summary>
public void setModeType(int _modeType) { modeType = _modeType; }
/// <summary>
/// 任务数据
/// </summary>
public Hotfix.Common.TileMatchObj.TileMatch_TaskInfo getTaskInfo() { return taskInfo; }
/// <summary>
/// 任务数据
/// </summary>
public void setTaskInfo(Hotfix.Common.TileMatchObj.TileMatch_TaskInfo _taskInfo) { taskInfo = _taskInfo; }


public int GetBufSize() {
	int _size = 4;
	_size += 4 + taskInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + taskInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	modeType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _taskInfoCustLen = _buf.getInt();
	int _taskInfoCurPos = _buf.getCurPos();
	taskInfo.ReadUnzipBuf(_buf, _taskInfoCurPos + _taskInfoCustLen);
	_buf.setPosition(_taskInfoCurPos + _taskInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(modeType);
	_buf.putInt(taskInfo.GetBufSize());
	taskInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)52);
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
	builder.Append("modeType").Append(":").Append(modeType.ToString()).Append(", ");
	builder.Append("taskInfo").Append(":").Append(taskInfo == null ? "null" : taskInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

