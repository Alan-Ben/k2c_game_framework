package GC2GS.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-Ai对话
 **/
public class GC2GS_015_025_ReqConsortInitiateAiChat implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 消息列表 */
private java.util.ArrayList<Common.Common_AiChatMessage> msgList;
/** 客户端数据ID */
private long clientDataId;


public GC2GS_015_025_ReqConsortInitiateAiChat() {
	consortId = (long)0;
	msgList = new java.util.ArrayList<Common.Common_AiChatMessage>();
	clientDataId = (long)0;
}

public GC2GS_015_025_ReqConsortInitiateAiChat(
	 long _consortId
	, java.util.ArrayList<Common.Common_AiChatMessage> _msgList
	, long _clientDataId
) {	consortId = _consortId;
	msgList = _msgList;
	clientDataId = _clientDataId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)25; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 消息列表 */
public java.util.ArrayList<Common.Common_AiChatMessage> getMsgList() { return msgList; }
/** 消息列表 */
public void addMsgList(Common.Common_AiChatMessage _msgList) { msgList.add(_msgList); }
/** 客户端数据ID */
public long getClientDataId() { return clientDataId; }
/** 客户端数据ID */
public void setClientDataId(long _clientDataId) { clientDataId = _clientDataId; }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < msgList.size(); _i++) {
	_size += 4 + msgList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < msgList.size(); _i++) {
	_size += 4 + msgList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _msgListCount = _buf.getShort();
	for(int _i = 0; _i < _msgListCount; _i++) { 
		Common.Common_AiChatMessage _msgList = new Common.Common_AiChatMessage();
		if(_buf.remaining() <= 0) return;
	int __msgListCustLen = _buf.getInt();
	int __msgListCurPos = _buf.position();
	_msgList.ReadUnzipBuf(_buf, __msgListCurPos + __msgListCustLen);
	_buf.position(__msgListCurPos + __msgListCustLen);

		msgList.add(_msgList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientDataId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putShort((short)msgList.size());
	for(int _i = 0; _i < msgList.size(); _i++) { 
		_buf.putInt(msgList.get(_i).GetBufSize());
	msgList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(clientDataId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)25);
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

