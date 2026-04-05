package MGClient.Cmd.Cmds;

import GC2GS.p010_BuildingOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "建筑", name = "Building")
public class CmdBuilding extends CmdBase {
	@Command(comment = "请求建造建筑(建筑ID)")
	public void BuildingBuild(long _param0)
	{
		GC2GS_010_001_ReqBuildingBuild msg = new GC2GS_010_001_ReqBuildingBuild();
		msg.setBuildingId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "请求升级农田建筑等级(建筑ID)")
	public void FarmUpgradeLvl(long _param0)
	{
		GC2GS_010_002_ReqFarmUpgradeLvl msg = new GC2GS_010_002_ReqFarmUpgradeLvl();
		msg.setBuildingId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "请求点击农田产出(建筑ID)")
	public void FarmClickOutput(long _param0)
	{
		GC2GS_010_003_ReqFarmClickOutput msg = new GC2GS_010_003_ReqFarmClickOutput();
		msg.setBuildingId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "请求升级经营建筑等级(建筑ID)")
	public void BusinessUpgradeLvl(long _param0)
	{
		GC2GS_010_004_ReqBusinessUpgradeLvl msg = new GC2GS_010_004_ReqBusinessUpgradeLvl();
		msg.setBuildingId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "请求招聘经营建筑雇员(建筑ID)")
	public void BusinessHireEmployee(long _param0)
	{
		GC2GS_010_005_ReqBusinessHireEmployee msg = new GC2GS_010_005_ReqBusinessHireEmployee();
		msg.setBuildingId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "请求招聘经营10个建筑雇员(建筑ID)")
	public void BusinessHireTenEmployees(long _param0)
	{
		GC2GS_010_006_ReqBusinessHireTenEmployees msg = new GC2GS_010_006_ReqBusinessHireTenEmployees();
		msg.setBuildingId(_param0);
		getOwner().sendGameMsg(msg);

	}
}
