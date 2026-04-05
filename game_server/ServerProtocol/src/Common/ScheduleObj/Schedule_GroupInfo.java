package Common.ScheduleObj;

import java.nio.ByteBuffer;
/*********
 * 排期分组信息
 **/
public class Schedule_GroupInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** us分组列表 */
private java.util.ArrayList<Common.Common_IntList> usGroupList;
/** 资源文件名 */
private String resFile;
/** 资源文件MD5 */
private String resFileMd5;
/** 资源文件目录 */
private String resFileDir;


public Schedule_GroupInfo() {
	usGroupList = new java.util.ArrayList<Common.Common_IntList>();
	resFile = "";
	resFileMd5 = "";
	resFileDir = "";
}

public Schedule_GroupInfo(
	 java.util.ArrayList<Common.Common_IntList> _usGroupList
	, String _resFile
	, String _resFileMd5
	, String _resFileDir
) {	usGroupList = _usGroupList;
	resFile = _resFile;
	resFileMd5 = _resFileMd5;
	resFileDir = _resFileDir;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** us分组列表 */
public java.util.ArrayList<Common.Common_IntList> getUsGroupList() { return usGroupList; }
/** us分组列表 */
public void addUsGroupList(Common.Common_IntList _usGroupList) { usGroupList.add(_usGroupList); }
/** 资源文件名 */
public String getResFile() { return resFile; }
/** 资源文件名 */
public void setResFile(String _resFile) { resFile = _resFile; }
/** 资源文件MD5 */
public String getResFileMd5() { return resFileMd5; }
/** 资源文件MD5 */
public void setResFileMd5(String _resFileMd5) { resFileMd5 = _resFileMd5; }
/** 资源文件目录 */
public String getResFileDir() { return resFileDir; }
/** 资源文件目录 */
public void setResFileDir(String _resFileDir) { resFileDir = _resFileDir; }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < usGroupList.size(); _i++) {
	_size += 4 + usGroupList.get(_i).GetBufSize();
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFile);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileDir);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < usGroupList.size(); _i++) {
	_size += 4 + usGroupList.get(_i).GetBufSize();
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFile);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileDir);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usGroupListCount = _buf.getShort();
	for(int _i = 0; _i < _usGroupListCount; _i++) { 
		Common.Common_IntList _usGroupList = new Common.Common_IntList();
		if(_buf.remaining() <= 0) return;
	int __usGroupListCustLen = _buf.getInt();
	int __usGroupListCurPos = _buf.position();
	_usGroupList.ReadUnzipBuf(_buf, __usGroupListCurPos + __usGroupListCustLen);
	_buf.position(__usGroupListCurPos + __usGroupListCustLen);

		usGroupList.add(_usGroupList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resFile = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resFileMd5 = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resFileDir = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)usGroupList.size());
	for(int _i = 0; _i < usGroupList.size(); _i++) { 
		_buf.putInt(usGroupList.get(_i).GetBufSize());
	usGroupList.get(_i).PutUnzipBuf(_buf);
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resFile);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resFileMd5);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resFileDir);
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

