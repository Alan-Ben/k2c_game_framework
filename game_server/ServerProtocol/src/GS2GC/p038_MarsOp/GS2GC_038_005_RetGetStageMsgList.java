package GS2GC.p038_MarsOp;

import java.nio.ByteBuffer;
/*********
 * 前往火星-阶段留言列表响应
 **/
public class GS2GC_038_005_RetGetStageMsgList implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段 */
private int stage;
/** 留言列表 */
private java.util.ArrayList<Common.MarsObj.Mars_GoRoute_StageMsg> msgList;


public GS2GC_038_005_RetGetStageMsgList() {
	stage = 0;
	msgList = new java.util.ArrayList<Common.MarsObj.Mars_GoRoute_StageMsg>();
}

public GS2GC_038_005_RetGetStageMsgList(
	 int _stage
	, java.util.ArrayList<Common.MarsObj.Mars_GoRoute_StageMsg> _msgList
) {	stage = _stage;
	msgList = _msgList;
}

public final byte getMainOrder() { return (byte)38; }

public final byte getSubOrder() { return (byte)5; }

/** 阶段 */
public int getStage() { return stage; }
/** 阶段 */
public void setStage(int _stage) { stage = _stage; }
/** 留言列表 */
public java.util.ArrayList<Common.MarsObj.Mars_GoRoute_StageMsg> getMsgList() { return msgList; }
/** 留言列表 */
public void addMsgList(Common.MarsObj.Mars_GoRoute_StageMsg _msgList) { msgList.add(_msgList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2;
	for(int _i = 0; _i < msgList.size(); _i++) {
	_size += 4 + msgList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
	for(int _i = 0; _i < msgList.size(); _i++) {
	_size += 4 + msgList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _msgListCount = _buf.getShort();
	for(int _i = 0; _i < _msgListCount; _i++) { 
		Common.MarsObj.Mars_GoRoute_StageMsg _msgList = new Common.MarsObj.Mars_GoRoute_StageMsg();
		if(_buf.remaining() <= 0) return;
	int __msgListCustLen = _buf.getInt();
	int __msgListCurPos = _buf.position();
	_msgList.ReadUnzipBuf(_buf, __msgListCurPos + __msgListCustLen);
	_buf.position(__msgListCurPos + __msgListCustLen);

		msgList.add(_msgList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stage);
	_buf.putShort((short)msgList.size());
	for(int _i = 0; _i < msgList.size(); _i++) { 
		_buf.putInt(msgList.get(_i).GetBufSize());
	msgList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)5);
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

