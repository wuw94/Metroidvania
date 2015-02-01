using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ResourceDirectory
{
	public static Dictionary<System.Type, string> directory = new Dictionary<System.Type, string>()
	{
		{typeof(Player), "Prefabs/Mobiles/Player/Player"},
		{typeof(Lever), "Prefabs/Immobiles/Lever/Lever"},
		{typeof(UpdraftGoo), "Prefabs/Mobiles/AI/UpdraftGoo/UpdraftGoo"},
		{typeof(DependantPlatform), "Prefabs/Immobiles/DependantPlatform/DependantPlatform"},
		{typeof(Button), "Prefabs/Immobiles/Button/Button"}

	};

}
