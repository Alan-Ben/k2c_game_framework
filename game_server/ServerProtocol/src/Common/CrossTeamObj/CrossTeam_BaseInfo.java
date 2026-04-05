package Common.CrossTeamObj;

import java.nio.ByteBuffer;
/*********
 * 组队基础数据
 **/
public class CrossTeam_BaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 队伍成员上限 */
private int memberLimit;
/** 队伍名称 */
private String teamName;
/** 队伍宣言 */
private String teamDec;


public CrossTeam_BaseInfo() {
	teamId = (long)0;
	memberLimit = 0;
	teamName = "";
	teamDec = "";
}

public CrossTeam_BaseInfo(
	 long _teamId
	, int _memberLimit
	, String _teamName
	, String _teamDec
) {	teamId = _teamId;
	memberLimit = _memberLimit;
	teamName = _teamName;
	teamDec = _teamDec;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 队伍成员上限 */
public int getMemberLimit() { return memberLimit; }
/** 队伍成员上限 */
public void setMemberLimit(int _memberLimit) { memberLimit = _memberLimit; }
/** 队伍名称 */
public String getTeamName() { return teamName; }
/** 队伍名称 */
public void setTeamName(String _teamName) { teamName = _teamName; }
/** 队伍宣言 */
public String getTeamDec() { return teamDec; }
/** 队伍宣言 */
public void setTeamDec(String _teamDec) { teamDec = _teamDec; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) memberLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamDec = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putInt(memberLimit);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, teamName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, teamDec);
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

