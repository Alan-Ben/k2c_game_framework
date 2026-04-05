package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 服务器信息
 **/
public class NP_SYS_ServerItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器大区 */
private String areaTag;
/** 服务器分组id */
private long groupId;
/** 外部赋予的服务器id */
private int serverLogicId;
/** 服务器typeId */
private int serverTypeId;
/** 服务器名 */
private String serverName;
/** 在线状态 EServerOnlineState */
private int onlineStateTypeId;
/** 对外显示状态 EServerShowState */
private int showStateTypeId;
/** 是否新服 */
private boolean isNew;
/** 开服时间 日期格式：yyyy-mm-dd  */
private String startDate;
/** 预留 */
private String ext;


public NP_SYS_ServerItem() {
	areaTag = "";
	groupId = (long)0;
	serverLogicId = 0;
	serverTypeId = 0;
	serverName = "";
	onlineStateTypeId = 0;
	showStateTypeId = 0;
	isNew = false;
	startDate = "";
	ext = "";
}

public NP_SYS_ServerItem(
	 String _areaTag
	, long _groupId
	, int _serverLogicId
	, int _serverTypeId
	, String _serverName
	, int _onlineStateTypeId
	, int _showStateTypeId
	, boolean _isNew
	, String _startDate
	, String _ext
) {	areaTag = _areaTag;
	groupId = _groupId;
	serverLogicId = _serverLogicId;
	serverTypeId = _serverTypeId;
	serverName = _serverName;
	onlineStateTypeId = _onlineStateTypeId;
	showStateTypeId = _showStateTypeId;
	isNew = _isNew;
	startDate = _startDate;
	ext = _ext;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 服务器大区 */
public String getAreaTag() { return areaTag; }
/** 服务器大区 */
public void setAreaTag(String _areaTag) { areaTag = _areaTag; }
/** 服务器分组id */
public long getGroupId() { return groupId; }
/** 服务器分组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 外部赋予的服务器id */
public int getServerLogicId() { return serverLogicId; }
/** 外部赋予的服务器id */
public void setServerLogicId(int _serverLogicId) { serverLogicId = _serverLogicId; }
/** 服务器typeId */
public int getServerTypeId() { return serverTypeId; }
/** 服务器typeId */
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
/** 服务器名 */
public String getServerName() { return serverName; }
/** 服务器名 */
public void setServerName(String _serverName) { serverName = _serverName; }
/** 在线状态 EServerOnlineState */
public int getOnlineStateTypeId() { return onlineStateTypeId; }
/** 在线状态 EServerOnlineState */
public void setOnlineStateTypeId(int _onlineStateTypeId) { onlineStateTypeId = _onlineStateTypeId; }
/** 对外显示状态 EServerShowState */
public int getShowStateTypeId() { return showStateTypeId; }
/** 对外显示状态 EServerShowState */
public void setShowStateTypeId(int _showStateTypeId) { showStateTypeId = _showStateTypeId; }
/** 是否新服 */
public boolean getIsNew() { return isNew; }
/** 是否新服 */
public void setIsNew(boolean _isNew) { isNew = _isNew; }
/** 开服时间 日期格式：yyyy-mm-dd  */
public String getStartDate() { return startDate; }
/** 开服时间 日期格式：yyyy-mm-dd  */
public void setStartDate(String _startDate) { startDate = _startDate; }
/** 预留 */
public String getExt() { return ext; }
/** 预留 */
public void setExt(String _ext) { ext = _ext; }


public final int GetBufSize() {
	int _size = 25;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(serverName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(startDate);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(serverName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(startDate);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) areaTag = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverLogicId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) onlineStateTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) showStateTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNew = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startDate = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ext = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, areaTag);
	_buf.putLong(groupId);
	_buf.putInt(serverLogicId);
	_buf.putInt(serverTypeId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, serverName);
	_buf.putInt(onlineStateTypeId);
	_buf.putInt(showStateTypeId);
	_buf.put(isNew?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, startDate);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, ext);
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

