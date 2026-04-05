package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-队伍战斗结果信息
 **/
public class ServerObj_MarsBattleV2_TeamFightResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍主加成，类似车头加成 */
private java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus> teamMajorBonus;
/** 参与队伍的所有成员信息 */
private java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult> memberList;


public ServerObj_MarsBattleV2_TeamFightResult() {
	teamMajorBonus = new java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus>();
	memberList = new java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult>();
}

public ServerObj_MarsBattleV2_TeamFightResult(
	 java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus> _teamMajorBonus
	, java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult> _memberList
) {	teamMajorBonus = _teamMajorBonus;
	memberList = _memberList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍主加成，类似车头加成 */
public java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus> getTeamMajorBonus() { return teamMajorBonus; }
/** 队伍主加成，类似车头加成 */
public void addTeamMajorBonus(Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus _teamMajorBonus) { teamMajorBonus.add(_teamMajorBonus); }
/** 参与队伍的所有成员信息 */
public java.util.ArrayList<Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult> getMemberList() { return memberList; }
/** 参与队伍的所有成员信息 */
public void addMemberList(Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult _memberList) { memberList.add(_memberList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (teamMajorBonus.size() * 16);
	_size += 2 + (memberList.size() * 40);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (teamMajorBonus.size() * 16);
	_size += 2 + (memberList.size() * 40);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _teamMajorBonusCount = _buf.getShort();
	for(int _i = 0; _i < _teamMajorBonusCount; _i++) { 
		Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus _teamMajorBonus = new Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus();
		if(_buf.remaining() <= 0) return;
	int __teamMajorBonusCustLen = _buf.getInt();
	int __teamMajorBonusCurPos = _buf.position();
	_teamMajorBonus.ReadUnzipBuf(_buf, __teamMajorBonusCurPos + __teamMajorBonusCustLen);
	_buf.position(__teamMajorBonusCurPos + __teamMajorBonusCustLen);

		teamMajorBonus.add(_teamMajorBonus);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult _memberList = new Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult();
		if(_buf.remaining() <= 0) return;
	int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.position();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.position(__memberListCurPos + __memberListCustLen);

		memberList.add(_memberList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)teamMajorBonus.size());
	for(int _i = 0; _i < teamMajorBonus.size(); _i++) { 
		_buf.putInt(teamMajorBonus.get(_i).GetBufSize());
	teamMajorBonus.get(_i).PutUnzipBuf(_buf);
	}
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

