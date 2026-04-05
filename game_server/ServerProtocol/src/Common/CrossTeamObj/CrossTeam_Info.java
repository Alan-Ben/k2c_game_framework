package Common.CrossTeamObj;

import java.nio.ByteBuffer;
/*********
 * 组队数据
 **/
public class CrossTeam_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 组队基础数据 */
private Common.CrossTeamObj.CrossTeam_BaseInfo teamBase;
/** 加入方式 */
private Common.CrossTeamEnum.ENPCrossTeamJoinType joinType;
/** 加入条件 */
private Common.CrossTeamObj.CrossTeam_SetInfo_Join joinCond;
/** 成员列表 */
private java.util.ArrayList<Common.CrossTeamObj.CrossTeamMember_Info> memberList;


public CrossTeam_Info() {
	teamBase = new Common.CrossTeamObj.CrossTeam_BaseInfo();
	joinType = Common.CrossTeamEnum.ENPCrossTeamJoinType.values()[0];
	joinCond = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
	memberList = new java.util.ArrayList<Common.CrossTeamObj.CrossTeamMember_Info>();
}

public CrossTeam_Info(
	 Common.CrossTeamObj.CrossTeam_BaseInfo _teamBase
	, Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond
	, java.util.ArrayList<Common.CrossTeamObj.CrossTeamMember_Info> _memberList
) {	teamBase = _teamBase;
	joinType = _joinType;
	joinCond = _joinCond;
	memberList = _memberList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 组队基础数据 */
public Common.CrossTeamObj.CrossTeam_BaseInfo getTeamBase() { return teamBase; }
/** 组队基础数据 */
public void setTeamBase(Common.CrossTeamObj.CrossTeam_BaseInfo _teamBase) { teamBase = _teamBase; }
/** 加入方式 */
public Common.CrossTeamEnum.ENPCrossTeamJoinType getJoinType() { return joinType; }
/** 加入方式 */
public void setJoinType(Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType) { joinType = _joinType; }
/** 加入条件 */
public Common.CrossTeamObj.CrossTeam_SetInfo_Join getJoinCond() { return joinCond; }
/** 加入条件 */
public void setJoinCond(Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond) { joinCond = _joinCond; }
/** 成员列表 */
public java.util.ArrayList<Common.CrossTeamObj.CrossTeamMember_Info> getMemberList() { return memberList; }
/** 成员列表 */
public void addMemberList(Common.CrossTeamObj.CrossTeamMember_Info _memberList) { memberList.add(_memberList); }


public final int GetBufSize() {
	int _size = 20;
	_size += 4 + teamBase.GetBufSize();
	_size += 2 + (memberList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + teamBase.GetBufSize();
	_size += 2 + (memberList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _teamBaseCustLen = _buf.getInt();
	int _teamBaseCurPos = _buf.position();
	teamBase.ReadUnzipBuf(_buf, _teamBaseCurPos + _teamBaseCustLen);
	_buf.position(_teamBaseCurPos + _teamBaseCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinType = Common.CrossTeamEnum.ENPCrossTeamJoinType.ENPCrossTeamJoinType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _joinCondCustLen = _buf.getInt();
	int _joinCondCurPos = _buf.position();
	joinCond.ReadUnzipBuf(_buf, _joinCondCurPos + _joinCondCustLen);
	_buf.position(_joinCondCurPos + _joinCondCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.CrossTeamObj.CrossTeamMember_Info _memberList = new Common.CrossTeamObj.CrossTeamMember_Info();
		if(_buf.remaining() <= 0) return;
	int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.position();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.position(__memberListCurPos + __memberListCustLen);

		memberList.add(_memberList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(teamBase.GetBufSize());
	teamBase.PutUnzipBuf(_buf);
	_buf.putInt(joinType.ordinal());

	_buf.putInt(joinCond.GetBufSize());
	joinCond.PutUnzipBuf(_buf);
	_buf.putShort((short)memberList.size());
	for(int _i = 0; _i < memberList.size(); _i++) { 
		_buf.putInt(memberList.get(_i).GetBufSize());
	memberList.get(_i).PutUnzipBuf(_buf);
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

