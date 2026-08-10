using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
	/// <summary>
	/// A staff panel that runs the game's systems so they can be seen working.
	/// <para>
	/// Everything here is reachable by typing a command, and that is exactly the problem: knowing
	/// which command, and in what order, is knowledge that lives in one person's head and in a
	/// TODO file. Levelling a character to 5 to watch Extra Attack, or dropping yourself to 0 hit
	/// points to see the death-save pips, is half a dozen commands each and easy to get wrong in a
	/// way that looks like the feature is broken.
	/// </para>
	/// <para>
	/// Staff only, and deliberately blunt: it grants levels, spawns monsters and wounds you
	/// without confirmation, because it exists to reach an interesting state quickly. It is not
	/// meant to be reachable from a player account.
	/// </para>
	/// </summary>
	public static class DnDTestPanel
	{
		public static void Initialize()
		{
			CommandSystem.Register("testpanel", AccessLevel.GameMaster, TestPanel_OnCommand);
			CommandSystem.Register("dndhelp", AccessLevel.Player, DnDHelp_OnCommand);
		}

		[Usage("testpanel")]
		[Description("Opens the D&D staff testing panel: spawn, level, wound, rest, and the windows.")]
		private static void TestPanel_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			if (from == null)
			{
				return;
			}

			from.CloseGump(typeof(DnDTestPanelGump));
			from.SendGump(new DnDTestPanelGump(from));
		}

		/// <summary>
		/// The player-facing half: what can I actually type? Available to everyone, because a
		/// command nobody knows about is a feature nobody has.
		/// </summary>
		[Usage("dndhelp")]
		[Description("Lists the D&D commands you can use.")]
		private static void DnDHelp_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			if (from == null)
			{
				return;
			}

			from.SendMessage(0x35, "--- D&D commands ---");

			foreach (string line in PlayerCommands)
			{
				from.SendMessage(0x3B2, line);
			}

			if (from.AccessLevel >= AccessLevel.GameMaster)
			{
				from.SendMessage(0x35, "--- staff ---");

				foreach (string line in StaffCommands)
				{
					from.SendMessage(0x3B2, line);
				}

				from.SendMessage(0x40, "[testpanel opens the testing panel.");
			}
		}

		/// <summary>
		/// Every player command, with what it is for. Kept beside the panel rather than in a
		/// document so the two cannot drift apart without somebody noticing.
		/// </summary>
		public static readonly string[] PlayerCommands =
		{
			"[sheet - your character sheet: abilities, AC, hit points, proficiency.",
			"[spells - the spells you can cast and the slots you have left.",
			"[features - your class features, and how many uses remain of each.",
			"[use <feature> - spends an activated feature, e.g. [use Second Wind, [use Rage.",
			"[points - what is left in your ki, sorcery point or Lay on Hands pool.",
			"[choose [option] - takes a fighting style, expertise, invocation, pact boon or metamagic.",
			"[skill <name> - takes a skill proficiency you are owed.",
			"[hitdie - spends one hit die to heal.",
			"[shortrest - a short rest: some features and pools return.",
			"[rest - a long rest: hit points, slots, hit dice and everything else return.",
			"[attune - attunes to a magic item you are holding, up to three at once.",
			"[unattune - releases one.",
			"[dndhelp - this list."
		};

		public static readonly string[] StaffCommands =
		{
			"[XP <amount> - awards experience. Levels still have to be spent with [LevelUp.",
			"[LevelUp [class] - spends one pending level, multiclassing if you name another class.",
			"[add Srd<Name> - spawns a monster, e.g. [add SrdGoblin, [add SrdTroll, [add SrdWolf.",
			"[add DnD<Name> - spawns equipment, e.g. [add DnDLongsword, [add DnDPlateArmor.",
			"[props - inspects anything, including a monster's live stat block.",
			"[testpanel - the panel this list came from."
		};
	}

	public class DnDTestPanelGump : Gump
	{
		private readonly Mobile m_From;

		// Sized from the content rather than guessed. The first version was 480x470 while the rows
		// actually ran to y=504, so the last three collided with the footer, and the hint column
		// was 270px for text that needs about 400 - both visible the moment it was opened and
		// neither visible from the code.
		private const int Width = 660;
		private const int Height = 560;

		private const int LabelX = 48;
		private const int LabelWidth = 168;
		private const int HintX = 222;

		private const int RowHeight = 22;

		// Button ids. Grouped in hundreds so a new row in one section cannot silently take the
		// number another section was already using.
		private const int BtnClose = 0;
		private const int BtnSheet = 101;
		private const int BtnSpells = 102;
		private const int BtnFeatures = 103;
		private const int BtnPoints = 104;

		private const int BtnXp = 201;
		private const int BtnLevel = 202;
		private const int BtnLevelFive = 203;

		private const int BtnWound = 301;
		private const int BtnDown = 302;
		private const int BtnHeal = 303;
		private const int BtnShortRest = 304;
		private const int BtnLongRest = 305;

		private const int BtnGoblin = 401;
		private const int BtnWolfPack = 402;
		private const int BtnTroll = 403;
		private const int BtnSkeleton = 404;
		private const int BtnKit = 405;

		public DnDTestPanelGump(Mobile from) : base(60, 60)
		{
			m_From = from;

			Closable = true;
			Dragable = true;

			AddPage(0);
			AddBackground(0, 0, Width, Height, 5054);

			AddHtml(10, 10, Width - 20, 22, "<center>D&D Test Panel</center>", false, false);

			int y = 38;

			y = AddSection("Windows - check the client actually draws them", y);

			y = AddRow(BtnSheet, "Character sheet", "[sheet - the paperdoll Status button opens the same window.", y);
			y = AddRow(BtnSpells, "Spellbook", "[spells - a Fighter is sent an empty list and gets no window.", y);
			y = AddRow(BtnFeatures, "Features", "[features", y);
			y = AddRow(BtnPoints, "Resource pools", "[points - Monk ki, sorcery points, Lay on Hands.", y);

			y = AddSection("Advancement", y + 4);

			y = AddRow(BtnXp, "Award 1000 XP", "Enough for a level or two early on.", y);
			y = AddRow(BtnLevel, "Spend one level", "Raises the level-up window if anything is pending.", y);
			y = AddRow(BtnLevelFive, "Jump to level 5", "The interesting breakpoint: Extra Attack, 3rd-level spells.", y);

			y = AddSection("Damage, dying and rest", y + 4);

			y = AddRow(BtnWound, "Wound to 1 hit point", "So healing and hit dice have something to do.", y);
			y = AddRow(BtnDown, "Drop to 0 - start dying", "Watch the three pips. Six seconds a roll.", y);
			y = AddRow(BtnHeal, "Full heal", "Also clears any dying state.", y);
			y = AddRow(BtnShortRest, "Short rest", "[shortrest", y);
			y = AddRow(BtnLongRest, "Long rest", "[rest", y);

			y = AddSection("Spawn something to fight", y + 4);

			y = AddRow(BtnGoblin, "A goblin", "CR 1/4. Spawn two - one goblin cannot show pack tactics.", y);
			y = AddRow(BtnWolfPack, "Three wolves", "Pack tactics with enough bodies to actually trigger it.", y);
			y = AddRow(BtnTroll, "A troll", "Regenerates each round. Nothing else heals by waiting any more.", y);
			y = AddRow(BtnSkeleton, "A skeleton", "Vulnerable to bludgeoning, immune to poison. Try both.", y);
			y = AddRow(BtnKit, "Starting kit", "A longsword, a chain shirt and a shield.", y);

			AddHtml(
				12,
				Height - 30,
				Width - 24,
				20,
				"<basefont color=#888888>[dndhelp lists every command. [add Srd&lt;Name&gt; spawns any of 694 monsters.</basefont>",
				false,
				false);
		}

		private int AddSection(string title, int y)
		{
			AddHtml(12, y, Width - 24, 20, String.Format("<basefont color=#FFD700>{0}</basefont>", title), false, false);

			return y + 20;
		}

		private int AddRow(int buttonId, string label, string hint, int y)
		{
			AddButton(14, y + 1, 4005, 4007, buttonId, GumpButtonType.Reply, 0);
			AddHtml(LabelX, y, LabelWidth, 20, label, false, false);

			AddHtml(
				HintX,
				y,
				Width - HintX - 14,
				20,
				String.Format("<basefont color=#AAAAAA>{0}</basefont>", hint),
				false,
				false);

			return y + RowHeight;
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = m_From;

			if (from == null || from.Deleted || from.AccessLevel < AccessLevel.GameMaster)
			{
				return;
			}

			DnDPlayerMobile pm = from as DnDPlayerMobile;

			switch (info.ButtonID)
			{
				case BtnClose:
					return;

				case BtnSheet: Run(from, "sheet"); break;
				case BtnSpells: Run(from, "spells"); break;
				case BtnFeatures: Run(from, "features"); break;
				case BtnPoints: Run(from, "points"); break;

				case BtnXp: Run(from, "XP 1000"); break;
				case BtnLevel: Run(from, "LevelUp"); break;

				case BtnLevelFive:
					{
						// 6500 is the SRD threshold for 5th level. Awarded in one go, then each
						// pending level is spent, because XP alone only makes levels available.
						Run(from, "XP 6500");

						for (int i = 0; i < 4; ++i)
						{
							Run(from, "LevelUp");
						}

						break;
					}

				case BtnWound:
					{
						from.Hits = 1;
						from.SendMessage(0x35, "You are down to a single hit point.");
						break;
					}

				case BtnDown:
					{
						// Through Damage rather than by setting Hits, so the whole path runs -
						// OnBeforeDeath, the dying state, and the packet that draws the pips.
						from.Hits = 1;
						from.Damage(50, from);
						break;
					}

				case BtnHeal:
					{
						DnDDeath.Clear(from);

						from.Hits = from.HitsMax;
						from.SendMessage(0x40, "Healed, and no longer dying.");

						break;
					}

				case BtnShortRest: Run(from, "shortrest"); break;
				case BtnLongRest: Run(from, "rest"); break;

				case BtnGoblin: Spawn(from, "SrdGoblin", 1); break;
				case BtnWolfPack: Spawn(from, "SrdWolf", 3); break;
				case BtnTroll: Spawn(from, "SrdTroll", 1); break;
				case BtnSkeleton: Spawn(from, "SrdSkeleton", 1); break;

				case BtnKit:
					{
						GiveKit(from);
						break;
					}
			}

			// Reopened so the panel behaves like a control surface rather than something that
			// vanishes after one use.
			from.CloseGump(typeof(DnDTestPanelGump));
			from.SendGump(new DnDTestPanelGump(from));
		}

		/// <summary>Runs a registered command as the player, so the panel and typing agree.</summary>
		private static void Run(Mobile from, string command)
		{
			CommandSystem.Handle(from, CommandSystem.Prefix + command);
		}

		private static void Spawn(Mobile from, string typeName, int count)
		{
			Type type = ScriptCompiler.FindTypeByName(typeName);

			if (type == null)
			{
				from.SendMessage(0x22, "No such creature type '{0}'.", typeName);
				return;
			}

			int spawned = 0;

			for (int i = 0; i < count; ++i)
			{
				try
				{
					var creature = Activator.CreateInstance(type) as Mobile;

					if (creature == null)
					{
						continue;
					}

					// Beside the caller rather than on top of them, so a pack does not stack into
					// one square and hide the thing being demonstrated.
					var location = new Point3D(
						from.X + Utility.RandomMinMax(-2, 2),
						from.Y + Utility.RandomMinMax(-2, 2),
						from.Z);

					creature.MoveToWorld(location, from.Map);

					++spawned;
				}
				catch (Exception ex)
				{
					from.SendMessage(0x22, "{0} could not be spawned: {1}", typeName, ex.Message);
					return;
				}
			}

			from.SendMessage(0x40, "Spawned {0} x{1}.", typeName, spawned);
		}

		private static void GiveKit(Mobile from)
		{
			string[] kit = { "DnDLongsword", "DnDChainShirt", "DnDShield" };

			var given = new List<string>();

			foreach (string name in kit)
			{
				Type type = ScriptCompiler.FindTypeByName(name);

				if (type == null)
				{
					continue;
				}

				try
				{
					var item = Activator.CreateInstance(type) as Item;

					if (item != null)
					{
						from.AddToBackpack(item);
						given.Add(name);
					}
				}
				catch
				{
					// A kit item that cannot be built is worth reporting, not worth aborting the
					// rest of the kit for.
					from.SendMessage(0x22, "{0} could not be created.", name);
				}
			}

			from.SendMessage(0x40, "Added to your pack: {0}.", String.Join(", ", given));
		}
	}
}
