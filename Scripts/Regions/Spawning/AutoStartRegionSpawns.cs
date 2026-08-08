using System;
using System.Collections;

namespace Server.Regions
{
	/// <summary>
	/// Starts region spawners automatically at boot.
	/// <para>
	/// Stock ServUO leaves every region spawner stopped until a GM runs [StartRegionSpawns or
	/// [StartAllRegionSpawns - reasonable for a shard whose world is populated from saved state,
	/// but unhelpful here: a fresh D&amp;D world has no saved spawns, so nothing would ever appear
	/// without manual intervention on every boot.
	/// </para>
	/// Controlled by <c>Spawning.AutoStartRegionSpawns</c> (default true).
	/// </summary>
	public static class AutoStartRegionSpawns
	{
		private static readonly bool Enabled = Config.Get("Spawning.AutoStartRegionSpawns", true);

		public static void Initialize()
		{
			if (!Enabled)
			{
				return;
			}

			// Region spawn definitions are parsed during Region.Load(), which has already run by
			// the time Initialize() is invoked, so the table is fully populated here.
			EventSink.ServerStarted += OnServerStarted;
		}

		private static void OnServerStarted()
		{
			int filled = 0, started = 0;

			foreach (SpawnEntry entry in SpawnEntry.Table.Values)
			{
				if (entry.Complete)
				{
					// Already at population from a loaded save - just let the timer top it up.
					if (!entry.Running)
					{
						entry.Start();
						++started;
					}

					continue;
				}

				// Start() only arms a timer that adds ONE creature every MinSpawnTime..MaxSpawnTime
				// (3-35 minutes in Regions.xml), so a world with no saved spawns would take hours to
				// populate. Respawn() fills the entry to Max immediately and starts it.
				entry.Respawn();
				++filled;
			}

			// SpawnDefinition.Spawn() returns null silently when a spawn's type name doesn't
			// resolve, so report what actually materialised rather than what we asked for.
			int live = 0;

			foreach (Mobile m in World.Mobiles.Values)
			{
				if (m is Server.Mobiles.DnDCreature)
				{
					++live;
				}
			}

			Console.WriteLine(
				"Region spawns: filled {0} spawner(s) to population, started {1} more; {2} creature(s) live.",
				filled,
				started,
				live);
		}
	}
}
