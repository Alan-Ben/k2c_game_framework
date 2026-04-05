using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GC2GS.p202_NumMergeOp
{

/// <summary>
/// 数字合并-移动棋子操作
/// </summary>
public class GC2GS_202_003_ReqNumMergeMove : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 模式类型
/// </summary>
private Hotfix.NumMergeEnum.ENumMerge_ModeType modeType;
/// <summary>
/// 移动方向
/// </summary>
private Hotfix.NumMergeEnum.ENumMerge_MoveDir moveDir;


public GC2GS_202_003_ReqNumMergeMove() {
	modeType = 0;
	moveDir = 0;
}

public GC2GS_202_003_ReqNumMergeMove(
	Hotfix.NumMergeEnum.ENumMerge_ModeType _modeType
	, Hotfix.NumMergeEnum.ENumMerge_MoveDir _moveDir
) {	modeType = _modeType;
	moveDir = _moveDir;
}

public byte getMainOrder() { return (byte)202; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 模式类型
/// </summary>
public Hotfix.NumMergeEnum.ENumMerge_ModeType getModeType() { return modeType; }
/// <summary>
/// 模式类型
/// </summary>
public void setModeType(Hotfix.NumMergeEnum.ENumMerge_ModeType _modeType) { modeType = _modeType; }
/// <summary>
/// 移动方向
/// </summary>
public Hotfix.NumMergeEnum.ENumMerge_MoveDir getMoveDir() { return moveDir; }
/// <summary>
/// 移动方向
/// </summary>
public void setMoveDir(Hotfix.NumMergeEnum.ENumMerge_MoveDir _moveDir) { moveDir = _moveDir; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	modeType = (Hotfix.NumMergeEnum.ENumMerge_ModeType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	moveDir = (Hotfix.NumMergeEnum.ENumMerge_MoveDir)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)modeType);

	_buf.putInt((int)moveDir);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)3);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("modeType").Append(":").Append(modeType.ToString()).Append(", ");
	builder.Append("moveDir").Append(":").Append(moveDir.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

