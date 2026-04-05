package NP2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_001_005_TriggerPlayerEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标玩家CID */
private long cid;
/** 事件名称，与@EventDesc.name一致 */
private String eventName;
/** 事件参数列表 */
private java.util.ArrayList<Long> paramList;


public NP2US_001_005_TriggerPlayerEvent() {
	cid = (long)0;
	eventName = "";
	paramList = new java.util.ArrayList<Long>();
}

public NP2US_001_005_TriggerPlayerEvent(
	 long _cid
	, String _eventName
	, java.util.ArrayList<Long> _paramList
) {	cid = _cid;
	eventName = _eventName;
	paramList = _paramList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)5; }

/** 目标玩家CID */
public long getCid() { return cid; }
/** 目标玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 事件名称，与@EventDesc.name一致 */
public String getEventName() { return eventName; }
/** 事件名称，与@EventDesc.name一致 */
public void setEventName(String _eventName) { eventName = _eventName; }
/** 事件参数列表 */
public java.util.ArrayList<Long> getParamList() { return paramList; }
/** 事件参数列表 */
public void addParamList(long _paramList) { paramList.add(_paramList); }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(eventName);
	_size += 2 + (paramList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(eventName);
	_size += 2 + (paramList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _paramListCount = _buf.getShort();
	for(int _i = 0; _i < _paramListCount; _i++) { 
		long _paramList = (long)0;
		if(_buf.remaining() > 0) _paramList = _buf.getLong();
		paramList.add(_paramList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, eventName);
	_buf.putShort((short)paramList.size());
	for(int _i = 0; _i < paramList.size(); _i++) { 
		_buf.putLong(paramList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

