function DTMI_zeroStartLength(%value, %length)
{
	%len = strlen(%value);
	if(%len >= %length)
		return %value;

	%dif = %length - %len;
	for(%i = 0; %i < %dif; %i++)
		%repeat = "0" @ %repeat;

	return %repeat @ %value;
}

function DTMI_intToBinary(%value)
{
	while(%value > 0)
	{
		%binary = (%value % 2) @ %binary;
		%value = %value / 2;

		if(%value < 1)
			break;
	}
	return DTMI_zeroStartLength(%binary, 32);
}

//$TypeMasks::All  "-1";
$DumpMaskInfoCount = 0;

$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "StaticObjectType"			TAB DTMI_intToBinary(1)			TAB	1;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "EnvironmentObjectType"		TAB DTMI_intToBinary(2)			TAB	2;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "TerrainObjectType"			TAB DTMI_intToBinary(4)			TAB	4;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "WaterObjectType"			    TAB DTMI_intToBinary(16)		TAB	16;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "TriggerObjectType"			TAB DTMI_intToBinary(32)		TAB	32;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "MarkerObjectType"			TAB DTMI_intToBinary(64)		TAB	64;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "GameBaseObjectType"		    TAB DTMI_intToBinary(1024)		TAB	1024;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "ShapeBaseObjectType"		    TAB DTMI_intToBinary(2048)		TAB	2048;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "CameraObjectType"			TAB DTMI_intToBinary(4096)		TAB	4096;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "StaticShapeObjectType"		TAB DTMI_intToBinary(8192)		TAB	8192;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "PlayerObjectType"			TAB DTMI_intToBinary(16384)		TAB	16384;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "ItemObjectType"			    TAB DTMI_intToBinary(32768)		TAB	32768;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "VehicleObjectType"			TAB DTMI_intToBinary(65536)		TAB	65536;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "VehicleBlockerObjectType"	TAB DTMI_intToBinary(131072)	TAB	131072;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "ProjectileObjectType"		TAB DTMI_intToBinary(262144)	TAB	262144;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "ExplosionObjectType"		    TAB DTMI_intToBinary(524288)	TAB	524288;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "CorpseObjectType"			TAB DTMI_intToBinary(1048576)	TAB	1048576;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "DebrisObjectType"			TAB DTMI_intToBinary(4194304)	TAB	4194304;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "PhysicalZoneObjectType"	    TAB DTMI_intToBinary(8388608)	TAB	8388608;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "StaticTSObjectType"		    TAB DTMI_intToBinary(16777216)	TAB	16777216;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "FxBrickObjectType"			TAB DTMI_intToBinary(33554432)	TAB	33554432;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "FxBrickAlwaysObjectType"	    TAB DTMI_intToBinary(67108864)	TAB	67108864;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "StaticRenderedObjectType"	TAB DTMI_intToBinary(134217728)	TAB	134217728;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "DamagableItemObjectType"	    TAB DTMI_intToBinary(268435456)	TAB	268435456;
$DumpMaskInfoType[$DumpMaskInfoCount++ - 1] = "PlayerObjectTypeHidden"      TAB DTMI_intToBinary(536870912) TAB 536870912; // 1<<29 // selective player collision dll

function dumpTypeInfo(%dumpTypeMask, %client, %listAll, %displayName)
{
	%message = "<just:left><tab:215>";
	for(%I = 0; %I < $DumpMaskInfoCount; %I++)
	{
		%typeMask = $DumpMaskInfoType[%I];
		%typeName   = getField(%typeMask, 0);
		%typeBinary = getField(%typeMask, 1);
		%typeMask   = getField(%typeMask, 2);

		if(%dumpTypeMask & %typeMask)
		{
			%typeBinary = strReplace(%typeBinary, "1", "\c31<color:AAAAAA>");
			%typeName = "\c4" @ %typeName;
		} else {
			%typeName = "\c6" @ %typeName;
		}

		if(%listAll || (!%listAll && %dumpTypeMask & %typeMask))
		{
			%displayMessage = %message @ %typeName TAB "<color:AAAAAA>" @ %typeBinary SPC "<color:dd0000>" SPC %typeMask;
			if(isObject(%client))
				messageClient(%client, '', %displayMessage);
			else
				announce(%displayMessage);

		}
	}

	%binaryDump = DTMI_intToBinary(%dumpTypeMask);
	%binaryDump = strReplace(%binaryDump, "1", "\c21<color:777777>");

	%displayMessage = %message @ "\c5" @ %displayName TAB "<color:777777>" @ %binaryDump SPC "<color:ffcc00>" SPC %dumpTypeMask;

	if(isObject(%client))
		messageClient(%client, '', %displayMessage);
	else
		announce(%displayMessage);
}
function SimObject::dumpTypeInfo(%object, %client, %listAll)
{
	%dumpTypeMask = %object.getType();
	dumpTypeInfo(%dumpTypeMask, %client, %listAll, %object.getClassName());
}
