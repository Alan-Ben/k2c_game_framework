package NP2HS_RB.p001_HSOp;

import java.nio.ByteBuffer;
public class NP2HS_RB_001_002_RetAllServerMail implements ALBasicProtocolPack._IALProtocolStructure {
/** 最大邮件dbid */
private long mailMaxId;
/** 平台邮件 */
private java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMail> platformMailList;


public NP2HS_RB_001_002_RetAllServerMail() {
	mailMaxId = (long)0;
	platformMailList = new java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMail>();
}

public NP2HS_RB_001_002_RetAllServerMail(
	 long _mailMaxId
	, java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMail> _platformMailList
) {	mailMaxId = _mailMaxId;
	platformMailList = _platformMailList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

/** 最大邮件dbid */
public long getMailMaxId() { return mailMaxId; }
/** 最大邮件dbid */
public void setMailMaxId(long _mailMaxId) { mailMaxId = _mailMaxId; }
/** 平台邮件 */
public java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMail> getPlatformMailList() { return platformMailList; }
/** 平台邮件 */
public void addPlatformMailList(Common.NpServerObj.NpServerObj_PlatFormMail _platformMailList) { platformMailList.add(_platformMailList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < platformMailList.size(); _i++) {
	_size += 4 + platformMailList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < platformMailList.size(); _i++) {
	_size += 4 + platformMailList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailMaxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _platformMailListCount = _buf.getShort();
	for(int _i = 0; _i < _platformMailListCount; _i++) { 
		Common.NpServerObj.NpServerObj_PlatFormMail _platformMailList = new Common.NpServerObj.NpServerObj_PlatFormMail();
		if(_buf.remaining() <= 0) return;
	int __platformMailListCustLen = _buf.getInt();
	int __platformMailListCurPos = _buf.position();
	_platformMailList.ReadUnzipBuf(_buf, __platformMailListCurPos + __platformMailListCustLen);
	_buf.position(__platformMailListCurPos + __platformMailListCustLen);

		platformMailList.add(_platformMailList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailMaxId);
	_buf.putShort((short)platformMailList.size());
	for(int _i = 0; _i < platformMailList.size(); _i++) { 
		_buf.putInt(platformMailList.get(_i).GetBufSize());
	platformMailList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

