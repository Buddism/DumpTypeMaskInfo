schedule(0, 0, talk, " %object.dumpTypeInfo(%client, %listAll (BOOLEAN))");
function zeroStartLength(%value, %length)
{
    %len = strlen(%value);
    if(%len >= %length)
        return %value;

    %dif = %length - %len;
    for(%i = 0; %i < %dif; %i++)
        %repeat = "0" @ %repeat;

    return %repeat @ %value;
}

function intToBinary(%value)
{
	while(%value > 0)
	{
		%binary = (%value % 2) @ %binary;
		%value = %value / 2;

		if(%value < 1)
			break;
	}
	return zeroStartLength(%binary, 32);
}

//$TypeMasks::All  "-1";
$TMIC = 0;

$TMI[$TMIC++ - 1] = "StaticObjectType"			TAB intToBinary(1)			TAB	1;
$TMI[$TMIC++ - 1] = "EnvironmentObjectType"		TAB intToBinary(2)			TAB	2;
$TMI[$TMIC++ - 1] = "TerrainObjectType"			TAB intToBinary(4)			TAB	4;
$TMI[$TMIC++ - 1] = "WaterObjectType"			TAB intToBinary(16)			TAB	16;
$TMI[$TMIC++ - 1] = "TriggerObjectType"			TAB intToBinary(32)			TAB	32;
$TMI[$TMIC++ - 1] = "MarkerObjectType"			TAB intToBinary(64)			TAB	64;
$TMI[$TMIC++ - 1] = "GameBaseObjectType"		TAB intToBinary(1024)		TAB	1024;
$TMI[$TMIC++ - 1] = "ShapeBaseObjectType"		TAB intToBinary(2048)		TAB	2048;
$TMI[$TMIC++ - 1] = "CameraObjectType"			TAB intToBinary(4096)		TAB	4096;
$TMI[$TMIC++ - 1] = "StaticShapeObjectType"		TAB intToBinary(8192)		TAB	8192;
$TMI[$TMIC++ - 1] = "PlayerObjectType"			TAB intToBinary(16384)		TAB	16384;
$TMI[$TMIC++ - 1] = "ItemObjectType"			TAB intToBinary(32768)		TAB	32768;
$TMI[$TMIC++ - 1] = "VehicleObjectType"			TAB intToBinary(65536)		TAB	65536;
$TMI[$TMIC++ - 1] = "VehicleBlockerObjectType"	TAB intToBinary(131072)		TAB	131072;
$TMI[$TMIC++ - 1] = "ProjectileObjectType"		TAB intToBinary(262144)		TAB	262144;
$TMI[$TMIC++ - 1] = "ExplosionObjectType"		TAB intToBinary(524288)		TAB	524288;
$TMI[$TMIC++ - 1] = "CorpseObjectType"			TAB intToBinary(1048576)	TAB	1048576;
$TMI[$TMIC++ - 1] = "DebrisObjectType"			TAB intToBinary(4194304)	TAB	4194304;
$TMI[$TMIC++ - 1] = "PhysicalZoneObjectType"	TAB intToBinary(8388608)	TAB	8388608;
$TMI[$TMIC++ - 1] = "StaticTSObjectType"		TAB intToBinary(16777216)	TAB	16777216;
$TMI[$TMIC++ - 1] = "FxBrickObjectType"			TAB intToBinary(33554432)	TAB	33554432;
$TMI[$TMIC++ - 1] = "FxBrickAlwaysObjectType"	TAB intToBinary(67108864)	TAB	67108864;
$TMI[$TMIC++ - 1] = "StaticRenderedObjectType"	TAB intToBinary(134217728)	TAB	134217728;
$TMI[$TMIC++ - 1] = "DamagableItemObjectType"	TAB intToBinary(268435456)	TAB	268435456;
$TMI[$TMIC++ - 1] = "PlayerObjectTypeHidden"    TAB intToBinary(536870912)    TAB 536870912; // 1<<29 // selective player collision dll
function dumpTypeInfo(%dumpTypeMask, %client)
{
	%message = "<just:left><tab:215>";
	for(%I = 0; %I < $TMIC; %I++)
	{
		%typeMask = $TMI[%I];
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
            %displayMessage = %message @ "<br>" @ %typeName TAB "<color:AAAAAA>" @ %typeBinary SPC "<color:dd0000>" SPC %typeMask;
            messageClient(%client, '', %displayMessage);
        }
	}

	%binaryDump = intToBinary(%dumpTypeMask);
    %binaryDump = strReplace(%binaryDump, "1", "\c21<color:777777>");

	%displayMessage = %message @ "<br>\c5" TAB "<color:777777>" @ %binaryDump SPC "<color:ffcc00>" SPC %dumpTypeMask;
	messageClient(%client, '', %displayMessage);
}
function SimObject::dumpTypeInfo(%object, %client, %listAll)
{
	if(!isObject(%client))
	{
		error(" FUNCTION REQUIRES CLIENT ARGUMENT %object.dumpTypeInfo(%client, %listAll (BOOLEAN))");
		return;
	}

	%dumpTypeMask = %object.getType();
	%message = "<just:left><tab:215>";
	for(%I = 0; %I < $TMIC; %I++)
	{
		%typeMask = $TMI[%I];
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
            %displayMessage = %message @ "<br>" @ %typeName TAB "<color:AAAAAA>" @ %typeBinary SPC "<color:dd0000>" SPC %typeMask;
            messageClient(%client, '', %displayMessage);
        }
	}

	%binaryDump = intToBinary(%dumpTypeMask);
    %binaryDump = strReplace(%binaryDump, "1", "\c21<color:777777>");

	%displayMessage = %message @ "<br>\c5" @ %object.getClassName() TAB "<color:777777>" @ %binaryDump SPC "<color:ffcc00>" SPC %dumpTypeMask;
	messageClient(%client, '', %displayMessage);
}
