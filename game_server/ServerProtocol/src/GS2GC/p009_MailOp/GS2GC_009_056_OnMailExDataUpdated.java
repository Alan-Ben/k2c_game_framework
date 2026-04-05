package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_056_OnMailExDataUpdated implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 额外信息内容 */
private byte[] exData;


public GS2GC_009_056_OnMailExDataUpdated() {
	mailUid = (long)0;
	exData = null;
}

public GS2GC_009_056_OnMailExDataUpdated(
	 long _mailUid
	, byte[] _exData
) {	mailUid = _mailUid;
	exData = _exData;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)56; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 额外信息内容 */
public byte[] getExData() { return exData; }
public java.nio.ByteBuffer get_buffer_ExData() { if(null == exData)return null; else return ByteBuffer.wrap(exData); }

/** 额外信息内容 */
public void setExData(byte[] _exData) { exData = _exData; }
public void setExData(java.nio.ByteBuffer _exData) 
{
	if(null == _exData){return;}
	int _oldPos = _exData.position();
	int _bufLength = _exData.remaining();
	exData = new byte[_bufLength];
	_exData.get(exData);
	_exData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 8;
	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exDataCount = _buf.getInt();
	if(0 < _exDataCount){
		exData = new byte[_exDataCount];
		_buf.get(exData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	_buf.putInt((exData == null ? 0 : exData.length));
	if(null != exData){_buf.put(exData);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)56);
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

