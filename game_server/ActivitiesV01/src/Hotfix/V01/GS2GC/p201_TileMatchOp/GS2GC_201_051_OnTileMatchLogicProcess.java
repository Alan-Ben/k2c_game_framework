package Hotfix.V01.GS2GC.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消逻辑处理
 **/
public class GS2GC_201_051_OnTileMatchLogicProcess implements ALBasicProtocolPack._IALProtocolStructure {
/** 模式类型 */
private Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType modeType;
/** 逻辑信息列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo> logicList;


public GS2GC_201_051_OnTileMatchLogicProcess() {
	modeType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType.values()[0];
	logicList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo>();
}

public GS2GC_201_051_OnTileMatchLogicProcess(
	 Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType _modeType
	, java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo> _logicList
) {	modeType = _modeType;
	logicList = _logicList;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)51; }

/** 模式类型 */
public Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType getModeType() { return modeType; }
/** 模式类型 */
public void setModeType(Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType _modeType) { modeType = _modeType; }
/** 逻辑信息列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo> getLogicList() { return logicList; }
/** 逻辑信息列表 */
public void addLogicList(Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo _logicList) { logicList.add(_logicList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2;
	for(int _i = 0; _i < logicList.size(); _i++) {
	_size += 4 + logicList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
	for(int _i = 0; _i < logicList.size(); _i++) {
	_size += 4 + logicList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) modeType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType.ETileMatch_ModeType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _logicListCount = _buf.getShort();
	for(int _i = 0; _i < _logicListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo _logicList = new Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo();
		if(_buf.remaining() <= 0) return;
	int __logicListCustLen = _buf.getInt();
	int __logicListCurPos = _buf.position();
	_logicList.ReadUnzipBuf(_buf, __logicListCurPos + __logicListCustLen);
	_buf.position(__logicListCurPos + __logicListCustLen);

		logicList.add(_logicList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(modeType.ordinal());

	_buf.putShort((short)logicList.size());
	for(int _i = 0; _i < logicList.size(); _i++) { 
		_buf.putInt(logicList.get(_i).GetBufSize());
	logicList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)51);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

