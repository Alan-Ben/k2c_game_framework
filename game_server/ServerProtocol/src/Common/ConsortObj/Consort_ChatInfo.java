package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人聊天数据
 **/
public class Consort_ChatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 聊天对话列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_ChatDialogue> dialogueList;
/** 是否已添加好友 */
private boolean hasAdd;


public Consort_ChatInfo() {
	consortId = (long)0;
	dialogueList = new java.util.ArrayList<Common.ConsortObj.Consort_ChatDialogue>();
	hasAdd = false;
}

public Consort_ChatInfo(
	 long _consortId
	, java.util.ArrayList<Common.ConsortObj.Consort_ChatDialogue> _dialogueList
	, boolean _hasAdd
) {	consortId = _consortId;
	dialogueList = _dialogueList;
	hasAdd = _hasAdd;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 聊天对话列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_ChatDialogue> getDialogueList() { return dialogueList; }
/** 聊天对话列表 */
public void addDialogueList(Common.ConsortObj.Consort_ChatDialogue _dialogueList) { dialogueList.add(_dialogueList); }
/** 是否已添加好友 */
public boolean getHasAdd() { return hasAdd; }
/** 是否已添加好友 */
public void setHasAdd(boolean _hasAdd) { hasAdd = _hasAdd; }


public final int GetBufSize() {
	int _size = 9;
	_size += 2;
	for(int _i = 0; _i < dialogueList.size(); _i++) {
	_size += 4 + dialogueList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
	for(int _i = 0; _i < dialogueList.size(); _i++) {
	_size += 4 + dialogueList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dialogueListCount = _buf.getShort();
	for(int _i = 0; _i < _dialogueListCount; _i++) { 
		Common.ConsortObj.Consort_ChatDialogue _dialogueList = new Common.ConsortObj.Consort_ChatDialogue();
		if(_buf.remaining() <= 0) return;
	int __dialogueListCustLen = _buf.getInt();
	int __dialogueListCurPos = _buf.position();
	_dialogueList.ReadUnzipBuf(_buf, __dialogueListCurPos + __dialogueListCustLen);
	_buf.position(__dialogueListCurPos + __dialogueListCustLen);

		dialogueList.add(_dialogueList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasAdd = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putShort((short)dialogueList.size());
	for(int _i = 0; _i < dialogueList.size(); _i++) { 
		_buf.putInt(dialogueList.get(_i).GetBufSize());
	dialogueList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(hasAdd?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

