using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p201_TileMatchOp
{

/// <summary>
/// 三消逻辑处理
/// </summary>
public class GS2GC_201_051_OnTileMatchLogicProcess : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 模式类型
/// </summary>
private Hotfix.TileMatchEnum.ETileMatch_ModeType modeType;
/// <summary>
/// 逻辑信息列表
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_LogicInfo> logicList;


public GS2GC_201_051_OnTileMatchLogicProcess() {
	modeType = 0;
	logicList = new List<Hotfix.Common.TileMatchObj.TileMatch_LogicInfo>();
}

public GS2GC_201_051_OnTileMatchLogicProcess(
	Hotfix.TileMatchEnum.ETileMatch_ModeType _modeType
	, List<Hotfix.Common.TileMatchObj.TileMatch_LogicInfo> _logicList
) {	modeType = _modeType;
	logicList = _logicList;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 模式类型
/// </summary>
public Hotfix.TileMatchEnum.ETileMatch_ModeType getModeType() { return modeType; }
/// <summary>
/// 模式类型
/// </summary>
public void setModeType(Hotfix.TileMatchEnum.ETileMatch_ModeType _modeType) { modeType = _modeType; }
/// <summary>
/// 逻辑信息列表
/// </summary>
public List<Hotfix.Common.TileMatchObj.TileMatch_LogicInfo> getLogicList() { return logicList; }
/// <summary>
/// 逻辑信息列表
/// </summary>
public void addLogicList(Hotfix.Common.TileMatchObj.TileMatch_LogicInfo _logicList) { logicList.Add(_logicList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2;
for(int _i = 0; _i < logicList.Count; _i++) {
	_size += 4 + logicList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
for(int _i = 0; _i < logicList.Count; _i++) {
	_size += 4 + logicList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	modeType = (Hotfix.TileMatchEnum.ETileMatch_ModeType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _logicListCount = _buf.getShort();
	for(int _i = 0; _i < _logicListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_LogicInfo _logicList = new Hotfix.Common.TileMatchObj.TileMatch_LogicInfo();
		int __logicListCustLen = _buf.getInt();
	int __logicListCurPos = _buf.getCurPos();
	_logicList.ReadUnzipBuf(_buf, __logicListCurPos + __logicListCustLen);
	_buf.setPosition(__logicListCurPos + __logicListCustLen);

		logicList.Add(_logicList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)modeType);

	_buf.putShort((short)logicList.Count);
	for(int _i = 0; _i < logicList.Count; _i++) { 
		_buf.putInt(logicList[_i].GetBufSize());
	logicList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)51);
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
	builder.Append("logicList").Append(":").Append(logicList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

