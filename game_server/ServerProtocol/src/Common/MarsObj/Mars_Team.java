package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-队伍数据
 **/
public class Mars_Team implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍ID */
private long teamId;
/** 队伍名称 */
private String name;
/** 入驻大臣ID列表 */
private java.util.ArrayList<Long> heroIdList;
/** 队伍状态 */
private Common.MarsObj.Mars_TeamState state;
/** 队伍损耗数量 */
private long lossValue;


public Mars_Team() {
	teamId = (long)0;
	name = "";
	heroIdList = new java.util.ArrayList<Long>();
	state = new Common.MarsObj.Mars_TeamState();
	lossValue = (long)0;
}

public Mars_Team(
	 long _teamId
	, String _name
	, java.util.ArrayList<Long> _heroIdList
	, Common.MarsObj.Mars_TeamState _state
	, long _lossValue
) {	teamId = _teamId;
	name = _name;
	heroIdList = _heroIdList;
	state = _state;
	lossValue = _lossValue;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 队伍名称 */
public String getName() { return name; }
/** 队伍名称 */
public void setName(String _name) { name = _name; }
/** 入驻大臣ID列表 */
public java.util.ArrayList<Long> getHeroIdList() { return heroIdList; }
/** 入驻大臣ID列表 */
public void addHeroIdList(long _heroIdList) { heroIdList.add(_heroIdList); }
/** 队伍状态 */
public Common.MarsObj.Mars_TeamState getState() { return state; }
/** 队伍状态 */
public void setState(Common.MarsObj.Mars_TeamState _state) { state = _state; }
/** 队伍损耗数量 */
public long getLossValue() { return lossValue; }
/** 队伍损耗数量 */
public void setLossValue(long _lossValue) { lossValue = _lossValue; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (heroIdList.size() * 8);
	_size += 4 + state.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (heroIdList.size() * 8);
	_size += 4 + state.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroIdListCount = _buf.getShort();
	for(int _i = 0; _i < _heroIdListCount; _i++) { 
		long _heroIdList = (long)0;
		if(_buf.remaining() > 0) _heroIdList = _buf.getLong();
		heroIdList.add(_heroIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stateCustLen = _buf.getInt();
	int _stateCurPos = _buf.position();
	state.ReadUnzipBuf(_buf, _stateCurPos + _stateCustLen);
	_buf.position(_stateCurPos + _stateCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lossValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putShort((short)heroIdList.size());
	for(int _i = 0; _i < heroIdList.size(); _i++) { 
		_buf.putLong(heroIdList.get(_i));
	}
	_buf.putInt(state.GetBufSize());
	state.PutUnzipBuf(_buf);
	_buf.putLong(lossValue);
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

