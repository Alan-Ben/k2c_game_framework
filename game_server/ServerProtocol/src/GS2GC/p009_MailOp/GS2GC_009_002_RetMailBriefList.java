package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_002_RetMailBriefList implements ALBasicProtocolPack._IALProtocolStructure {
/** 全部邮件排序列表 */
private java.util.ArrayList<Common.MailObj.Mail_BriefInfo> briefList;


public GS2GC_009_002_RetMailBriefList() {
	briefList = new java.util.ArrayList<Common.MailObj.Mail_BriefInfo>();
}

public GS2GC_009_002_RetMailBriefList(
	 java.util.ArrayList<Common.MailObj.Mail_BriefInfo> _briefList
) {	briefList = _briefList;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)2; }

/** 全部邮件排序列表 */
public java.util.ArrayList<Common.MailObj.Mail_BriefInfo> getBriefList() { return briefList; }
/** 全部邮件排序列表 */
public void addBriefList(Common.MailObj.Mail_BriefInfo _briefList) { briefList.add(_briefList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < briefList.size(); _i++) {
	_size += 4 + briefList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < briefList.size(); _i++) {
	_size += 4 + briefList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _briefListCount = _buf.getShort();
	for(int _i = 0; _i < _briefListCount; _i++) { 
		Common.MailObj.Mail_BriefInfo _briefList = new Common.MailObj.Mail_BriefInfo();
		if(_buf.remaining() <= 0) return;
	int __briefListCustLen = _buf.getInt();
	int __briefListCurPos = _buf.position();
	_briefList.ReadUnzipBuf(_buf, __briefListCurPos + __briefListCustLen);
	_buf.position(__briefListCurPos + __briefListCustLen);

		briefList.add(_briefList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)briefList.size());
	for(int _i = 0; _i < briefList.size(); _i++) { 
		_buf.putInt(briefList.get(_i).GetBufSize());
	briefList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
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

