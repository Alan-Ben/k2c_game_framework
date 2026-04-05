package NP2CGS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2CGS_R_001_001_SendCrossGameRequest implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服游戏类型 */
private NPEnum.ENPCrossGameCategoryEnum category;
/** 实例ID */
private long instanceId;
/** 玩家CID，0-无玩家操作 */
private long cid;
/** 消息 */
private byte[] reqMsg;


public NP2CGS_R_001_001_SendCrossGameRequest() {
	category = NPEnum.ENPCrossGameCategoryEnum.values()[0];
	instanceId = (long)0;
	cid = (long)0;
	reqMsg = null;
}

public NP2CGS_R_001_001_SendCrossGameRequest(
	 NPEnum.ENPCrossGameCategoryEnum _category
	, long _instanceId
	, long _cid
	, byte[] _reqMsg
) {	category = _category;
	instanceId = _instanceId;
	cid = _cid;
	reqMsg = _reqMsg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

/** 跨服游戏类型 */
public NPEnum.ENPCrossGameCategoryEnum getCategory() { return category; }
/** 跨服游戏类型 */
public void setCategory(NPEnum.ENPCrossGameCategoryEnum _category) { category = _category; }
/** 实例ID */
public long getInstanceId() { return instanceId; }
/** 实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 玩家CID，0-无玩家操作 */
public long getCid() { return cid; }
/** 玩家CID，0-无玩家操作 */
public void setCid(long _cid) { cid = _cid; }
/** 消息 */
public byte[] getReqMsg() { return reqMsg; }
public java.nio.ByteBuffer get_buffer_ReqMsg() { if(null == reqMsg)return null; else return ByteBuffer.wrap(reqMsg); }

/** 消息 */
public void setReqMsg(byte[] _reqMsg) { reqMsg = _reqMsg; }
public void setReqMsg(java.nio.ByteBuffer _reqMsg) 
{
	if(null == _reqMsg){return;}
	int _oldPos = _reqMsg.position();
	int _bufLength = _reqMsg.remaining();
	reqMsg = new byte[_bufLength];
	_reqMsg.get(reqMsg);
	_reqMsg.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 20;
	_size += 4 + (reqMsg == null ? 0 : reqMsg.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + (reqMsg == null ? 0 : reqMsg.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) category = NPEnum.ENPCrossGameCategoryEnum.ENPCrossGameCategoryEnum_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _reqMsgCount = _buf.getInt();
	if(0 < _reqMsgCount){
		reqMsg = new byte[_reqMsgCount];
		_buf.get(reqMsg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(category.ordinal());

	_buf.putLong(instanceId);
	_buf.putLong(cid);
	_buf.putInt((reqMsg == null ? 0 : reqMsg.length));
	if(null != reqMsg){_buf.put(reqMsg);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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

