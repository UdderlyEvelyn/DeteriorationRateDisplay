using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Noise;
using UnityEngine;

namespace DRD
{
	[HarmonyPatch(typeof(Thing), "GetInspectStringLowPriority")]
    class HarmonyPatches
    {
		static void Postfix(ref Thing __instance, ref string __result)
		{
			var finalDeteriorationRate = SteadyEnvironmentEffects.FinalDeteriorationRate(__instance);
			float hpLossPerDay = 0;
			if (finalDeteriorationRate > .001) //If it's smaller then it takes no damage to leave it at 0.
			{
				//double deteriorationsPerDay = 12.37113402061856; //This was based on the %97==0 && rand.chance(.02) stuff but that was wrong.
				var deteriorationRate = Mathf.Lerp(1f, 5f, __instance.Map.weatherManager.RainRate);
				var deteriorationChance = deteriorationRate * finalDeteriorationRate / 36f;
				hpLossPerDay = (float)(deteriorationChance * 60000); //60k here used to be deteriorationsPerDay
				__result += "\nGlobal Deterioration Rate: " + deteriorationRate + "\nDeterioration Chance: " + deteriorationChance + "\nEstimated HP loss per day: " + Math.Round(hpLossPerDay, 2);
			}
		}
	}

	/*	The orginal we're patching
	public virtual string GetInspectStringLowPriority()
	{
		string result = null;
		tmpDeteriorationReasons.Clear();
		SteadyEnvironmentEffects.FinalDeteriorationRate(this, tmpDeteriorationReasons);
		if (tmpDeteriorationReasons.Count != 0)
		{
			result = string.Format("{0}: {1}", "DeterioratingBecauseOf".Translate(), tmpDeteriorationReasons.ToCommaList().CapitalizeFirst());
		}
		return result;
	}
*/
		}
