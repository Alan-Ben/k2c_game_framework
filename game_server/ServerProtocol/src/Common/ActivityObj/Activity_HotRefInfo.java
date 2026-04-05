package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动热更配置信息
 **/
public class Activity_HotRefInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 活动ID */
private long activityId;
/** 热更文件名 */
private String fileName;
/** 热更文件MD5 */
private String fileMd5;
/** 热更文件目录 */
private String fileDir;


public Activity_HotRefInfo() {
	activityInstanceId = (long)0;
	activityId = (long)0;
	fileName = "";
	fileMd5 = "";
	fileDir = "";
}

public Activity_HotRefInfo(
	 long _activityInstanceId
	, long _activityId
	, String _fileName
	, String _fileMd5
	, String _fileDir
) {	activityInstanceId = _activityInstanceId;
	activityId = _activityId;
	fileName = _fileName;
	fileMd5 = _fileMd5;
	fileDir = _fileDir;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 活动ID */
public long getActivityId() { return activityId; }
/** 活动ID */
public void setActivityId(long _activityId) { activityId = _activityId; }
/** 热更文件名 */
public String getFileName() { return fileName; }
/** 热更文件名 */
public void setFileName(String _fileName) { fileName = _fileName; }
/** 热更文件MD5 */
public String getFileMd5() { return fileMd5; }
/** 热更文件MD5 */
public void setFileMd5(String _fileMd5) { fileMd5 = _fileMd5; }
/** 热更文件目录 */
public String getFileDir() { return fileDir; }
/** 热更文件目录 */
public void setFileDir(String _fileDir) { fileDir = _fileDir; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileDir);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileDir);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fileName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fileMd5 = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fileDir = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(activityId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, fileName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, fileMd5);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, fileDir);
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

