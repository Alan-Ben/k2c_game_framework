package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-建造/升级队列数据
 **/
public class Mars_BuildingUpQueue implements ALBasicProtocolPack._IALProtocolStructure {
/** 队列ID */
private long id;
/** 建筑ID */
private long buildingId;
/** 建筑开始升级时间（毫秒） */
private long startUpgradeLvlMs;
/** 建筑结束升级时间（毫秒） */
private long endUpgradeLvlMs;
/** 公会求助数据ID */
private long guildHelpId;
/** 公会求助时长（秒） */
private int guildHelpSecs;
/** 道具求助时长（秒） */
private int itemHelpSecs;


public Mars_BuildingUpQueue() {
	id = (long)0;
	buildingId = (long)0;
	startUpgradeLvlMs = (long)0;
	endUpgradeLvlMs = (long)0;
	guildHelpId = (long)0;
	guildHelpSecs = 0;
	itemHelpSecs = 0;
}

public Mars_BuildingUpQueue(
	 long _id
	, long _buildingId
	, long _startUpgradeLvlMs
	, long _endUpgradeLvlMs
	, long _guildHelpId
	, int _guildHelpSecs
	, int _itemHelpSecs
) {	id = _id;
	buildingId = _buildingId;
	startUpgradeLvlMs = _startUpgradeLvlMs;
	endUpgradeLvlMs = _endUpgradeLvlMs;
	guildHelpId = _guildHelpId;
	guildHelpSecs = _guildHelpSecs;
	itemHelpSecs = _itemHelpSecs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队列ID */
public long getId() { return id; }
/** 队列ID */
public void setId(long _id) { id = _id; }
/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 建筑开始升级时间（毫秒） */
public long getStartUpgradeLvlMs() { return startUpgradeLvlMs; }
/** 建筑开始升级时间（毫秒） */
public void setStartUpgradeLvlMs(long _startUpgradeLvlMs) { startUpgradeLvlMs = _startUpgradeLvlMs; }
/** 建筑结束升级时间（毫秒） */
public long getEndUpgradeLvlMs() { return endUpgradeLvlMs; }
/** 建筑结束升级时间（毫秒） */
public void setEndUpgradeLvlMs(long _endUpgradeLvlMs) { endUpgradeLvlMs = _endUpgradeLvlMs; }
/** 公会求助数据ID */
public long getGuildHelpId() { return guildHelpId; }
/** 公会求助数据ID */
public void setGuildHelpId(long _guildHelpId) { guildHelpId = _guildHelpId; }
/** 公会求助时长（秒） */
public int getGuildHelpSecs() { return guildHelpSecs; }
/** 公会求助时长（秒） */
public void setGuildHelpSecs(int _guildHelpSecs) { guildHelpSecs = _guildHelpSecs; }
/** 道具求助时长（秒） */
public int getItemHelpSecs() { return itemHelpSecs; }
/** 道具求助时长（秒） */
public void setItemHelpSecs(int _itemHelpSecs) { itemHelpSecs = _itemHelpSecs; }


public final int GetBufSize() {
	int _size = 48;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startUpgradeLvlMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endUpgradeLvlMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildHelpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemHelpSecs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(buildingId);
	_buf.putLong(startUpgradeLvlMs);
	_buf.putLong(endUpgradeLvlMs);
	_buf.putLong(guildHelpId);
	_buf.putInt(guildHelpSecs);
	_buf.putInt(itemHelpSecs);
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

