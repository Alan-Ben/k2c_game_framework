package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_010_RetMailTitleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件标题列表 */
private java.util.ArrayList<Common.MailObj.Mail_TitleInfo> titleList;


public GS2GC_009_010_RetMailTitleInfo() {
	titleList = new java.util.ArrayList<Common.MailObj.Mail_TitleInfo>();
}

public GS2GC_009_010_RetMailTitleInfo(
	 java.util.ArrayList<Common.MailObj.Mail_TitleInfo> _titleList
) {	titleList = _titleList;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)10; }

/** 邮件标题列表 */
public java.util.ArrayList<Common.MailObj.Mail_TitleInfo> getTitleList() { return titleList; }
/** 邮件标题列表 */
public void addTitleList(Common.MailObj.Mail_TitleInfo _titleList) { titleList.add(_titleList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < titleList.size(); _i++) {
	_size += 4 + titleList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < titleList.size(); _i++) {
	_size += 4 + titleList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _titleListCount = _buf.getShort();
	for(int _i = 0; _i < _titleListCount; _i++) { 
		Common.MailObj.Mail_TitleInfo _titleList = new Common.MailObj.Mail_TitleInfo();
		if(_buf.remaining() <= 0) return;
	int __titleListCustLen = _buf.getInt();
	int __titleListCurPos = _buf.position();
	_titleList.ReadUnzipBuf(_buf, __titleListCurPos + __titleListCustLen);
	_buf.position(__titleListCurPos + __titleListCustLen);

		titleList.add(_titleList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)titleList.size());
	for(int _i = 0; _i < titleList.size(); _i++) { 
		_buf.putInt(titleList.get(_i).GetBufSize());
	titleList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)10);
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

