package Hotfix.V02.GC2GS.p202_NumMergeOp;

import java.nio.ByteBuffer;
/*********
 * 数字合并-移动棋子操作
 **/
public class GC2GS_202_003_ReqNumMergeMove implements ALBasicProtocolPack._IALProtocolStructure {
/** 模式类型 */
private Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType modeType;
/** 移动方向 */
private Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir moveDir;


public GC2GS_202_003_ReqNumMergeMove() {
	modeType = Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType.values()[0];
	moveDir = Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir.values()[0];
}

public GC2GS_202_003_ReqNumMergeMove(
	 Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType _modeType
	, Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir _moveDir
) {	modeType = _modeType;
	moveDir = _moveDir;
}

public final byte getMainOrder() { return (byte)202; }

public final byte getSubOrder() { return (byte)3; }

/** 模式类型 */
public Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType getModeType() { return modeType; }
/** 模式类型 */
public void setModeType(Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType _modeType) { modeType = _modeType; }
/** 移动方向 */
public Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir getMoveDir() { return moveDir; }
/** 移动方向 */
public void setMoveDir(Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir _moveDir) { moveDir = _moveDir; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) modeType = Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType.ENumMerge_ModeType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) moveDir = Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir.ENumMerge_MoveDir_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(modeType.ordinal());

	_buf.putInt(moveDir.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)3);
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

