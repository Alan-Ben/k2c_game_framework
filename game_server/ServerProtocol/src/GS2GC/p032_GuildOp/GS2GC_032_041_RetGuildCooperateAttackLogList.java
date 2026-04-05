package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 返回联盟协作攻击日志列表
 **/
public class GS2GC_032_041_RetGuildCooperateAttackLogList implements ALBasicProtocolPack._IALProtocolStructure {
/** 攻击日志列表 */
private java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_AttackLog> attackLogList;


public GS2GC_032_041_RetGuildCooperateAttackLogList() {
	attackLogList = new java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_AttackLog>();
}

public GS2GC_032_041_RetGuildCooperateAttackLogList(
	 java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_AttackLog> _attackLogList
) {	attackLogList = _attackLogList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)41; }

/** 攻击日志列表 */
public java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_AttackLog> getAttackLogList() { return attackLogList; }
/** 攻击日志列表 */
public void addAttackLogList(Common.GuildCooperateObj.GuildCooperate_AttackLog _attackLogList) { attackLogList.add(_attackLogList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < attackLogList.size(); _i++) {
	_size += 4 + attackLogList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < attackLogList.size(); _i++) {
	_size += 4 + attackLogList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _attackLogListCount = _buf.getShort();
	for(int _i = 0; _i < _attackLogListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_AttackLog _attackLogList = new Common.GuildCooperateObj.GuildCooperate_AttackLog();
		if(_buf.remaining() <= 0) return;
	int __attackLogListCustLen = _buf.getInt();
	int __attackLogListCurPos = _buf.position();
	_attackLogList.ReadUnzipBuf(_buf, __attackLogListCurPos + __attackLogListCustLen);
	_buf.position(__attackLogListCurPos + __attackLogListCustLen);

		attackLogList.add(_attackLogList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)attackLogList.size());
	for(int _i = 0; _i < attackLogList.size(); _i++) { 
		_buf.putInt(attackLogList.get(_i).GetBufSize());
	attackLogList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)41);
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

