package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 玩家加入联盟
 **/
public class Offline_PlayerJoinGuild implements ALBasicProtocolPack._IALProtocolStructure {
private long newGuildId;
private String guildName;
private String simpleName;
private java.util.ArrayList<Integer> buildingAddPerArr;


public Offline_PlayerJoinGuild() {
	newGuildId = (long)0;
	guildName = "";
	simpleName = "";
	buildingAddPerArr = new java.util.ArrayList<Integer>();
}

public Offline_PlayerJoinGuild(
	 long _newGuildId
	, String _guildName
	, String _simpleName
	, java.util.ArrayList<Integer> _buildingAddPerArr
) {	newGuildId = _newGuildId;
	guildName = _guildName;
	simpleName = _simpleName;
	buildingAddPerArr = _buildingAddPerArr;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getNewGuildId() { return newGuildId; }
public void setNewGuildId(long _newGuildId) { newGuildId = _newGuildId; }
public String getGuildName() { return guildName; }
public void setGuildName(String _guildName) { guildName = _guildName; }
public String getSimpleName() { return simpleName; }
public void setSimpleName(String _simpleName) { simpleName = _simpleName; }
public java.util.ArrayList<Integer> getBuildingAddPerArr() { return buildingAddPerArr; }
public void addBuildingAddPerArr(int _buildingAddPerArr) { buildingAddPerArr.add(_buildingAddPerArr); }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += 2 + (buildingAddPerArr.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += 2 + (buildingAddPerArr.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) newGuildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) simpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buildingAddPerArrCount = _buf.getShort();
	for(int _i = 0; _i < _buildingAddPerArrCount; _i++) { 
		int _buildingAddPerArr = 0;
		if(_buf.remaining() > 0) _buildingAddPerArr = _buf.getInt();
		buildingAddPerArr.add(_buildingAddPerArr);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(newGuildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, simpleName);
	_buf.putShort((short)buildingAddPerArr.size());
	for(int _i = 0; _i < buildingAddPerArr.size(); _i++) { 
		_buf.putInt(buildingAddPerArr.get(_i));
	}
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

