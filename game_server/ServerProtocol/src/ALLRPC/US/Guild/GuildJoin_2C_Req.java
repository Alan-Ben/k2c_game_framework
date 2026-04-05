package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildJoin_2C_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
/** 是否申请审批的加入 */
private boolean isRequestJoin;
private String guildName;
private String simpleName;
private int guildLvl;
private java.util.ArrayList<Integer> buildingAddPerArr;


public GuildJoin_2C_Req() {
	cid = (long)0;
	guildId = (long)0;
	isRequestJoin = false;
	guildName = "";
	simpleName = "";
	guildLvl = 0;
	buildingAddPerArr = new java.util.ArrayList<Integer>();
}

public GuildJoin_2C_Req(
	 long _cid
	, long _guildId
	, boolean _isRequestJoin
	, String _guildName
	, String _simpleName
	, int _guildLvl
	, java.util.ArrayList<Integer> _buildingAddPerArr
) {	cid = _cid;
	guildId = _guildId;
	isRequestJoin = _isRequestJoin;
	guildName = _guildName;
	simpleName = _simpleName;
	guildLvl = _guildLvl;
	buildingAddPerArr = _buildingAddPerArr;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
/** 是否申请审批的加入 */
public boolean getIsRequestJoin() { return isRequestJoin; }
/** 是否申请审批的加入 */
public void setIsRequestJoin(boolean _isRequestJoin) { isRequestJoin = _isRequestJoin; }
public String getGuildName() { return guildName; }
public void setGuildName(String _guildName) { guildName = _guildName; }
public String getSimpleName() { return simpleName; }
public void setSimpleName(String _simpleName) { simpleName = _simpleName; }
public int getGuildLvl() { return guildLvl; }
public void setGuildLvl(int _guildLvl) { guildLvl = _guildLvl; }
public java.util.ArrayList<Integer> getBuildingAddPerArr() { return buildingAddPerArr; }
public void addBuildingAddPerArr(int _buildingAddPerArr) { buildingAddPerArr.add(_buildingAddPerArr); }


public final int GetBufSize() {
	int _size = 21;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += 2 + (buildingAddPerArr.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += 2 + (buildingAddPerArr.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isRequestJoin = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) simpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildLvl = _buf.getInt();
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
	_buf.putLong(cid);
	_buf.putLong(guildId);
	_buf.put(isRequestJoin?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, simpleName);
	_buf.putInt(guildLvl);
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

