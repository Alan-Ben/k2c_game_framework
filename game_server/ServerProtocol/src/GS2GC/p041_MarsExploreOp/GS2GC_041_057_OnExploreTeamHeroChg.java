package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索队伍-大臣数据变更
 **/
public class GS2GC_041_057_OnExploreTeamHeroChg implements ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
/** 入驻大臣ID列表 */
private java.util.ArrayList<Long> heroIdList;


public GS2GC_041_057_OnExploreTeamHeroChg() {
	teamId = (long)0;
	heroIdList = new java.util.ArrayList<Long>();
}

public GS2GC_041_057_OnExploreTeamHeroChg(
	 long _teamId
	, java.util.ArrayList<Long> _heroIdList
) {	teamId = _teamId;
	heroIdList = _heroIdList;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)57; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 入驻大臣ID列表 */
public java.util.ArrayList<Long> getHeroIdList() { return heroIdList; }
/** 入驻大臣ID列表 */
public void addHeroIdList(long _heroIdList) { heroIdList.add(_heroIdList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (heroIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (heroIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroIdListCount = _buf.getShort();
	for(int _i = 0; _i < _heroIdListCount; _i++) { 
		long _heroIdList = (long)0;
		if(_buf.remaining() > 0) _heroIdList = _buf.getLong();
		heroIdList.add(_heroIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putShort((short)heroIdList.size());
	for(int _i = 0; _i < heroIdList.size(); _i++) { 
		_buf.putLong(heroIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)57);
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

