package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索队伍-队伍名称变更
 **/
public class GS2GC_041_056_OnExploreTeamNameChg implements ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
/** 队伍名称 */
private String name;


public GS2GC_041_056_OnExploreTeamNameChg() {
	teamId = (long)0;
	name = "";
}

public GS2GC_041_056_OnExploreTeamNameChg(
	 long _teamId
	, String _name
) {	teamId = _teamId;
	name = _name;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)56; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 队伍名称 */
public String getName() { return name; }
/** 队伍名称 */
public void setName(String _name) { name = _name; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
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

