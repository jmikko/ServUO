using System;

namespace Server.Spells.DnD
{
	/// <summary>
	/// Hand-written spells - the ones whose behaviour cannot be described as a row of dice in
	/// Data/DnDSpells.xml.
	/// <para>
	/// This file is deliberately empty for now: every spell implemented so far fits the data table,
	/// and the five that used to live here as classes (Fire Bolt, Sacred Flame, Magic Missile, Cure
	/// Wounds, Healing Word) moved into it unchanged. The extension point stays because area
	/// effects, conditions and concentration will need real code.
	/// </para>
	/// </summary>
	public static class SrdSpells
	{
		public static void Configure()
		{
			// Registration order determines spell ids on the wire, and DnDSpellTable.Configure has
			// already run by the time this does, so anything registered here lands after the data
			// table - appending, never renumbering it.
		}
	}
}
