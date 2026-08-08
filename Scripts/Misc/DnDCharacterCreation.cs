using System;
using Server.Accounting;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Misc
{
	/// <summary>
	/// Turns a client's character-creation packet into a <see cref="DnDPlayerMobile"/>.
	/// <para>
	/// The vanilla creation packets (0x00 / 0xF8) are fixed-length with no spare capacity, and
	/// CharacterCreatedEventArgs has no slot for ability scores or a class - so the D&amp;D choices
	/// cannot be smuggled into creation. They are collected afterwards: this handler builds the
	/// character with placeholder scores, then the client is prompted (DnDCreationPrompt) and
	/// replies with DnDCharacterSetup, which ClassSystem applies via IDnDCharacter.ApplyDnDSetup.
	/// </para>
	/// </summary>
	public static class DnDCharacterCreation
	{
		public static void Initialize()
		{
			EventSink.CharacterCreated += EventSink_CharacterCreated;
		}

		private static void EventSink_CharacterCreated(CharacterCreatedEventArgs args)
		{
			if (args.Mobile != null)
			{
				return; // already handled
			}

			DnDPlayerMobile pm = new DnDPlayerMobile
			{
				Player = true,
				AccessLevel = args.Account.AccessLevel,
				Female = args.Female,
				Body = args.Female ? 0x191 : 0x190,
				Name = args.Name,
				Hue = args.Hue,
				HairItemID = args.HairID,
				HairHue = args.HairHue,
				FacialHairItemID = args.BeardID,
				FacialHairHue = args.BeardHue
			};

			if (args.Race != null)
			{
				pm.Race = args.Race;
				pm.Body = pm.Female ? args.Race.FemaleBody : args.Race.MaleBody;
			}

			// Legacy UO stats are not used by D&D combat resolution, but Mobile still needs
			// non-zero values for movement/carry weight until those are reworked.
			pm.Str = 40;
			pm.Dex = 40;
			pm.Int = 40;
			pm.Hits = pm.HitsMax;
			pm.Stam = pm.StamMax;
			pm.Mana = pm.ManaMax;

			AttachToAccount(args.Account, pm);
			args.Mobile = pm;

			pm.MoveToWorld(GetStartLocation(args), GetStartMap(args));

			AddStartingKit(pm);

			Console.WriteLine(
				"DnDCharacterCreation: created '{0}' on account '{1}' at {2} {3}",
				pm.Name,
				args.Account.Username,
				pm.Location,
				pm.Map);

			PromptForDnDSetup(args.State);
		}

		/// <summary>
		/// Asks the client to show the D&amp;D setup screen (species + class + ability scores).
		/// <para>
		/// Deliberately delayed: CharacterCreated fires BEFORE PacketHandlers.DoLogin sends the
		/// world-entry burst that moves the client into its in-game UI state. Sending the prompt
		/// immediately means the gump is added before the client's UIManager exists and is
		/// silently dropped on the scene transition - this cost real debugging time once already.
		/// </para>
		/// </summary>
		private static void PromptForDnDSetup(NetState state)
		{
			if (state == null)
			{
				return;
			}

			Timer.DelayCall(
				TimeSpan.FromSeconds(2.0),
				() =>
				{
					if (state.Running)
					{
						state.Send(new DnDCreationPrompt());
					}
				});
		}

		/// <summary>
		/// IAccount exposes character slots only through an indexer, so claim the first free one.
		/// </summary>
		private static bool AttachToAccount(IAccount account, Mobile m)
		{
			for (int i = 0; i < account.Length; ++i)
			{
				if (account[i] == null)
				{
					account[i] = m;
					return true;
				}
			}

			return false;
		}

		private static Map GetStartMap(CharacterCreatedEventArgs args)
		{
			Point3D ignored;

			if (TryGetStartOverride(out ignored))
			{
				Map map = Map.Parse(Config.Get("Startup.Map", "Trammel"));

				if (map != null && map != Map.Internal)
				{
					return map;
				}
			}

			return args.City != null && args.City.Map != null ? args.City.Map : Map.Trammel;
		}

		private static Point3D GetStartLocation(CharacterCreatedEventArgs args)
		{
			Point3D location;

			// Startup.Location ("x,y,z") overrides the client's starting-city pick. The vanilla
			// cities are UO landmarks with no D&D meaning, so a shard will normally want to point
			// this somewhere of its own.
			if (TryGetStartOverride(out location))
			{
				return location;
			}

			if (args.City != null && args.City.Location != Point3D.Zero)
			{
				return args.City.Location;
			}

			return new Point3D(1495, 1629, 10); // Britain bank, a safe universal fallback
		}

		private static bool TryGetStartOverride(out Point3D location)
		{
			location = Point3D.Zero;

			string value = Config.Get("Startup.Location", (string)null);

			if (String.IsNullOrEmpty(value))
			{
				return false;
			}

			string[] parts = value.Split(',');

			int x, y, z;

			if (parts.Length != 3 ||
				!Int32.TryParse(parts[0].Trim(), out x) ||
				!Int32.TryParse(parts[1].Trim(), out y) ||
				!Int32.TryParse(parts[2].Trim(), out z))
			{
				Console.WriteLine("DnDCharacterCreation: ignoring malformed Startup.Location '{0}'.", value);
				return false;
			}

			location = new Point3D(x, y, z);
			return true;
		}

		/// <summary>
		/// A minimal starting kit. Class-appropriate gear is granted later by ClassSystem once
		/// the player has actually picked a class, gated on proficiency.
		/// </summary>
		private static void AddStartingKit(DnDPlayerMobile pm)
		{
			Container pack = pm.Backpack;

			if (pack == null)
			{
				pack = new Backpack { Movable = false };
				pm.AddItem(pack);
			}
		}
	}
}
