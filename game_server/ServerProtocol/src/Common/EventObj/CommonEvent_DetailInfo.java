package Common.EventObj;

import java.nio.ByteBuffer;
/*********
 * 事件详情信息
 **/
public class CommonEvent_DetailInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 通用事件id */
private long commonEventId;
/** 展示信息 */
private byte[] showInfo;


public CommonEvent_DetailInfo() {
	commonEventId = (long)0;
	showInfo = null;
}

public CommonEvent_DetailInfo(
	 long _commonEventId
	, byte[] _showInfo
) {	commonEventId = _commonEventId;
	showInfo = _showInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 通用事件id */
public long getCommonEventId() { return commonEventId; }
/** 通用事件id */
public void setCommonEventId(long _commonEventId) { commonEventId = _commonEventId; }
/** 展示信息 */
public byte[] getShowInfo() { return showInfo; }
public java.nio.ByteBuffer get_buffer_ShowInfo() { if(null == showInfo)return null; else return ByteBuffer.wrap(showInfo); }

/** 展示信息 */
public void setShowInfo(byte[] _showInfo) { showInfo = _showInfo; }
public void setShowInfo(java.nio.ByteBuffer _showInfo) 
{
	if(null == _showInfo){return;}
	int _oldPos = _showInfo.position();
	int _bufLength = _showInfo.remaining();
	showInfo = new byte[_bufLength];
	_showInfo.get(showInfo);
	_showInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 8;
	_size += 4 + (showInfo == null ? 0 : showInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (showInfo == null ? 0 : showInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) commonEventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _showInfoCount = _buf.getInt();
	if(0 < _showInfoCount){
		showInfo = new byte[_showInfoCount];
		_buf.get(showInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(commonEventId);
	_buf.putInt((showInfo == null ? 0 : showInfo.length));
	if(null != showInfo){_buf.put(showInfo);}

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

