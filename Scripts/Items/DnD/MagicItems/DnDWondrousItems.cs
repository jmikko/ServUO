using System;

namespace Server.Items
{
	// Generated from Data/DnDMagicItems.xml - see the note in DnDWondrousTable.cs. These exist only
	// so [add and the spawner, which resolve by C# type name through reflection, can find them. All
	// the numbers live in the table.
	//
	// Regenerate after adding a row. The self-test fails loudly if a row has no class here, because
	// otherwise the item exists in the data and simply cannot be spawned - invisible until someone
	// tries to [add it.

	public class DnDRingOfProtection : DnDWondrousItem
	{
		public override string WondrousId { get { return "RingOfProtection"; } }

		[Constructable]
		public DnDRingOfProtection() : base("RingOfProtection") { }

		public DnDRingOfProtection(Serial serial) : base(serial) { }
	}

	public class DnDRingOfEvasion : DnDWondrousItem
	{
		public override string WondrousId { get { return "RingOfEvasion"; } }

		[Constructable]
		public DnDRingOfEvasion() : base("RingOfEvasion") { }

		public DnDRingOfEvasion(Serial serial) : base(serial) { }
	}

	public class DnDRingOfFreeAction : DnDWondrousItem
	{
		public override string WondrousId { get { return "RingOfFreeAction"; } }

		[Constructable]
		public DnDRingOfFreeAction() : base("RingOfFreeAction") { }

		public DnDRingOfFreeAction(Serial serial) : base(serial) { }
	}

	public class DnDAmuletOfHealth : DnDWondrousItem
	{
		public override string WondrousId { get { return "AmuletOfHealth"; } }

		[Constructable]
		public DnDAmuletOfHealth() : base("AmuletOfHealth") { }

		public DnDAmuletOfHealth(Serial serial) : base(serial) { }
	}

	public class DnDPeriaptOfWoundClosure : DnDWondrousItem
	{
		public override string WondrousId { get { return "PeriaptOfWoundClosure"; } }

		[Constructable]
		public DnDPeriaptOfWoundClosure() : base("PeriaptOfWoundClosure") { }

		public DnDPeriaptOfWoundClosure(Serial serial) : base(serial) { }
	}

	public class DnDAmuletOfProofAgainstDetection : DnDWondrousItem
	{
		public override string WondrousId { get { return "AmuletOfProofAgainstDetection"; } }

		[Constructable]
		public DnDAmuletOfProofAgainstDetection() : base("AmuletOfProofAgainstDetection") { }

		public DnDAmuletOfProofAgainstDetection(Serial serial) : base(serial) { }
	}

	public class DnDGauntletsOfOgrePower : DnDWondrousItem
	{
		public override string WondrousId { get { return "GauntletsOfOgrePower"; } }

		[Constructable]
		public DnDGauntletsOfOgrePower() : base("GauntletsOfOgrePower") { }

		public DnDGauntletsOfOgrePower(Serial serial) : base(serial) { }
	}

	public class DnDGlovesOfMissileSnaring : DnDWondrousItem
	{
		public override string WondrousId { get { return "GlovesOfMissileSnaring"; } }

		[Constructable]
		public DnDGlovesOfMissileSnaring() : base("GlovesOfMissileSnaring") { }

		public DnDGlovesOfMissileSnaring(Serial serial) : base(serial) { }
	}

	public class DnDHeadbandOfIntellect : DnDWondrousItem
	{
		public override string WondrousId { get { return "HeadbandOfIntellect"; } }

		[Constructable]
		public DnDHeadbandOfIntellect() : base("HeadbandOfIntellect") { }

		public DnDHeadbandOfIntellect(Serial serial) : base(serial) { }
	}

	public class DnDHatOfDisguise : DnDWondrousItem
	{
		public override string WondrousId { get { return "HatOfDisguise"; } }

		[Constructable]
		public DnDHatOfDisguise() : base("HatOfDisguise") { }

		public DnDHatOfDisguise(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack) && Parent != from)
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			if (from.BodyMod == 0)
			{
				from.BodyMod = from.Body.IsFemale ? 401 : 400;
				from.HueMod = Utility.RandomSkinHue();
				from.NameMod = "a disguised figure";
				from.SendMessage("You disguise yourself.");
			}
			else
			{
				from.BodyMod = 0;
				from.HueMod = -1;
				from.NameMod = null;
				from.SendMessage("You remove the disguise.");
			}
		}
	}

	public class DnDBeltOfGiantStrengthHill : DnDWondrousItem
	{
		public override string WondrousId { get { return "BeltOfGiantStrengthHill"; } }

		[Constructable]
		public DnDBeltOfGiantStrengthHill() : base("BeltOfGiantStrengthHill") { }

		public DnDBeltOfGiantStrengthHill(Serial serial) : base(serial) { }
	}

	public class DnDBeltOfDwarvenkind : DnDWondrousItem
	{
		public override string WondrousId { get { return "BeltOfDwarvenkind"; } }

		[Constructable]
		public DnDBeltOfDwarvenkind() : base("BeltOfDwarvenkind") { }

		public DnDBeltOfDwarvenkind(Serial serial) : base(serial) { }
	}

	public class DnDBootsOfElvenkind : DnDWondrousItem
	{
		public override string WondrousId { get { return "BootsOfElvenkind"; } }

		[Constructable]
		public DnDBootsOfElvenkind() : base("BootsOfElvenkind") { }

		public DnDBootsOfElvenkind(Serial serial) : base(serial) { }
	}

	public class DnDBootsOfSpeed : DnDWondrousItem
	{
		public override string WondrousId { get { return "BootsOfSpeed"; } }

		[Constructable]
		public DnDBootsOfSpeed() : base("BootsOfSpeed") { }

		public DnDBootsOfSpeed(Serial serial) : base(serial) { }

		public override bool OnEquip(Mobile from)
		{
			from.Send(new Server.Network.SpeedControl(Server.Network.SpeedControlType.MountSpeed));
			from.SendMessage("You feel incredibly fast.");
			return base.OnEquip(from);
		}

		public override void OnRemoved(object parent)
		{
			if (parent is Mobile m)
			{
				m.Send(new Server.Network.SpeedControl(Server.Network.SpeedControlType.Disable));
				m.SendMessage("Your speed returns to normal.");
			}
			base.OnRemoved(parent);
		}
	}

	public class DnDWingedBoots : DnDWondrousItem
	{
		public override string WondrousId { get { return "WingedBoots"; } }

		[Constructable]
		public DnDWingedBoots() : base("WingedBoots") { }

		public DnDWingedBoots(Serial serial) : base(serial) { }
	}

	public class DnDCloakOfProtection : DnDWondrousItem
	{
		public override string WondrousId { get { return "CloakOfProtection"; } }

		[Constructable]
		public DnDCloakOfProtection() : base("CloakOfProtection") { }

		public DnDCloakOfProtection(Serial serial) : base(serial) { }
	}

	public class DnDCloakOfElvenkind : DnDWondrousItem
	{
		public override string WondrousId { get { return "CloakOfElvenkind"; } }

		[Constructable]
		public DnDCloakOfElvenkind() : base("CloakOfElvenkind") { }

		public DnDCloakOfElvenkind(Serial serial) : base(serial) { }
	}

	public class DnDCloakOfDisplacement : DnDWondrousItem
	{
		public override string WondrousId { get { return "CloakOfDisplacement"; } }

		[Constructable]
		public DnDCloakOfDisplacement() : base("CloakOfDisplacement") { }

		public DnDCloakOfDisplacement(Serial serial) : base(serial) { }
	}

	public class DnDBracersOfDefense : DnDWondrousItem
	{
		public override string WondrousId { get { return "BracersOfDefense"; } }

		[Constructable]
		public DnDBracersOfDefense() : base("BracersOfDefense") { }

		public DnDBracersOfDefense(Serial serial) : base(serial) { }
	}

	public class DnDBracersOfArchery : DnDWondrousItem
	{
		public override string WondrousId { get { return "BracersOfArchery"; } }

		[Constructable]
		public DnDBracersOfArchery() : base("BracersOfArchery") { }

		public DnDBracersOfArchery(Serial serial) : base(serial) { }
	}

	public class DnDMedallionOfThoughts : DnDWondrousItem
	{
		public override string WondrousId { get { return "MedallionOfThoughts"; } }

		[Constructable]
		public DnDMedallionOfThoughts() : base("MedallionOfThoughts") { }

		public DnDMedallionOfThoughts(Serial serial) : base(serial) { }
	}

	public class DnDEyesOfCharming : DnDWondrousItem
	{
		public override string WondrousId { get { return "EyesOfCharming"; } }

		[Constructable]
		public DnDEyesOfCharming() : base("EyesOfCharming") { }

		public DnDEyesOfCharming(Serial serial) : base(serial) { }
	}

	public class DnDBagOfHolding : DnDWondrousItem
	{
		public override string WondrousId { get { return "BagOfHolding"; } }

		[Constructable]
		public DnDBagOfHolding() : base("BagOfHolding") { }

		public DnDBagOfHolding(Serial serial) : base(serial) { }
	}

	public class DnDRobeOfTheArchmagi : DnDWondrousItem
	{
		public override string WondrousId { get { return "RobeOfTheArchmagi"; } }

		[Constructable]
		public DnDRobeOfTheArchmagi() : base("RobeOfTheArchmagi") { }

		public DnDRobeOfTheArchmagi(Serial serial) : base(serial) { }
	}

	public class DnDDeckOfManyThings : DnDWondrousItem
	{
		public override string WondrousId { get { return "DeckOfManyThings"; } }

		[Constructable]
		public DnDDeckOfManyThings() : base("DeckOfManyThings") { }

		public DnDDeckOfManyThings(Serial serial) : base(serial) { }
	}

	public class DnDRingOfInvisibility : DnDWondrousItem
	{
		public override string WondrousId { get { return "RingOfInvisibility"; } }

		[Constructable]
		public DnDRingOfInvisibility() : base("RingOfInvisibility") { }

		public DnDRingOfInvisibility(Serial serial) : base(serial) { }

		public override bool OnEquip(Mobile from)
		{
			from.Hidden = true;
			from.SendMessage("You vanish from sight.");
			return base.OnEquip(from);
		}

		public override void OnRemoved(object parent)
		{
			if (parent is Mobile m)
			{
				m.Hidden = false;
				m.SendMessage("You become visible again.");
			}
			base.OnRemoved(parent);
		}
	}

	public class DnDBootsOfStridingAndSpringing : DnDWondrousItem
	{
		public override string WondrousId { get { return "BootsOfStridingAndSpringing"; } }

		[Constructable]
		public DnDBootsOfStridingAndSpringing() : base("BootsOfStridingAndSpringing") { }

		public DnDBootsOfStridingAndSpringing(Serial serial) : base(serial) { }
	}

	public class DnDCloakOfInvisibility : DnDWondrousItem
	{
		public override string WondrousId { get { return "CloakOfInvisibility"; } }

		[Constructable]
		public DnDCloakOfInvisibility() : base("CloakOfInvisibility") { }

		public DnDCloakOfInvisibility(Serial serial) : base(serial) { }
	}

	public class DnDPortableHole : DnDWondrousItem
	{
		public override string WondrousId { get { return "PortableHole"; } }

		[Constructable]
		public DnDPortableHole() : base("PortableHole") { }

		public DnDPortableHole(Serial serial) : base(serial) { }
	}

	public class DnDAmuletOfThePlanes : DnDWondrousItem
	{
		public override string WondrousId { get { return "AmuletOfThePlanes"; } }

		[Constructable]
		public DnDAmuletOfThePlanes() : base("AmuletOfThePlanes") { }

		public DnDAmuletOfThePlanes(Serial serial) : base(serial) { }
	}

	public class DnDSphereOfAnnihilation : DnDWondrousItem
	{
		public override string WondrousId { get { return "SphereOfAnnihilation"; } }

		[Constructable]
		public DnDSphereOfAnnihilation() : base("SphereOfAnnihilation") { }

		public DnDSphereOfAnnihilation(Serial serial) : base(serial) { }
	}

	public class DnDWellOfManyWorlds : DnDWondrousItem
	{
		public override string WondrousId { get { return "WellOfManyWorlds"; } }

		[Constructable]
		public DnDWellOfManyWorlds() : base("WellOfManyWorlds") { }

		public DnDWellOfManyWorlds(Serial serial) : base(serial) { }
	}

	public class DnDRingOfThreeWishes : DnDWondrousItem
	{
		public override string WondrousId { get { return "RingOfThreeWishes"; } }

		[Constructable]
		public DnDRingOfThreeWishes() : base("RingOfThreeWishes") { }

		public DnDRingOfThreeWishes(Serial serial) : base(serial) { }
	}

	public class DnDTalismanOfPureGood : DnDWondrousItem
	{
		public override string WondrousId { get { return "TalismanOfPureGood"; } }

		[Constructable]
		public DnDTalismanOfPureGood() : base("TalismanOfPureGood") { }

		public DnDTalismanOfPureGood(Serial serial) : base(serial) { }
	}

	public class DnDGemOfSeeing : DnDWondrousItem
	{
		public override string WondrousId { get { return "GemOfSeeing"; } }

		[Constructable]
		public DnDGemOfSeeing() : base("GemOfSeeing") { }

		public DnDGemOfSeeing(Serial serial) : base(serial) { }
	}

	public class DnDHornOfValhalla : DnDWondrousItem
	{
		public override string WondrousId { get { return "HornOfValhalla"; } }

		[Constructable]
		public DnDHornOfValhalla() : base("HornOfValhalla") { }

		public DnDHornOfValhalla(Serial serial) : base(serial) { }
	}

	public class DnDWandOfFireballs : DnDWondrousItem
	{
		public override string WondrousId { get { return "WandOfFireballs"; } }

		private int m_Charges = 7;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Charges { get { return m_Charges; } set { m_Charges = value; InvalidateProperties(); } }

		[Constructable]
		public DnDWandOfFireballs() : base("WandOfFireballs") { }

		public DnDWandOfFireballs(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack) && Parent != from)
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			if (m_Charges <= 0)
			{
				from.SendMessage("The wand is out of charges.");
				return;
			}

			from.Target = new FireballTarget(this);
			from.SendMessage("Target a location for the fireball.");
		}

		private class FireballTarget : Server.Targeting.Target
		{
			private DnDWandOfFireballs m_Wand;

			public FireballTarget(DnDWandOfFireballs wand) : base(12, true, Server.Targeting.TargetFlags.None)
			{
				m_Wand = wand;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				IPoint3D loc = targeted as IPoint3D;
				if (loc == null) return;
				
				Point3D p = new Point3D(loc);
				Server.Effects.SendLocationEffect(p, from.Map, 0x36D4, 30);
				Server.Effects.PlaySound(p, from.Map, 0x208);
				
				m_Wand.Charges--;
				from.SendMessage("You unleash a fireball from the wand. Charges left: {0}", m_Wand.Charges);
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1060584, m_Charges.ToString()); // charges: ~1_val~
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_Charges);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Charges = reader.ReadInt();
		}
	}

	public class DnDApparatusOfKwalish : DnDWondrousItem
	{
		public override string WondrousId { get { return "ApparatusOfKwalish"; } }

		[Constructable]
		public DnDApparatusOfKwalish() : base("ApparatusOfKwalish") { }

		public DnDApparatusOfKwalish(Serial serial) : base(serial) { }
	}

	public class DnDCarpetOfFlying : DnDWondrousItem
	{
		public override string WondrousId { get { return "CarpetOfFlying"; } }

		[Constructable]
		public DnDCarpetOfFlying() : base("CarpetOfFlying") { }

		public DnDCarpetOfFlying(Serial serial) : base(serial) { }
	}

	public class DnDHelmOfTeleportation : DnDWondrousItem
	{
		public override string WondrousId { get { return "HelmOfTeleportation"; } }

		[Constructable]
		public DnDHelmOfTeleportation() : base("HelmOfTeleportation") { }

		public DnDHelmOfTeleportation(Serial serial) : base(serial) { }
	}

	public class DnDPearlOfPower : DnDWondrousItem
	{
		public override string WondrousId { get { return "PearlOfPower"; } }

		[Constructable]
		public DnDPearlOfPower() : base("PearlOfPower") { }

		public DnDPearlOfPower(Serial serial) : base(serial) { }
	}

	public class DnDIounStone : DnDWondrousItem
	{
		public override string WondrousId { get { return "IounStone"; } }

		[Constructable]
		public DnDIounStone() : base("IounStone") { }

		public DnDIounStone(Serial serial) : base(serial) { }
	}

	public class DnDCrystalBall : DnDWondrousItem
	{
		public override string WondrousId { get { return "CrystalBall"; } }

		[Constructable]
		public DnDCrystalBall() : base("CrystalBall") { }

		public DnDCrystalBall(Serial serial) : base(serial) { }
	}

	public class DnDBagOfTricks : DnDWondrousItem
	{
		public override string WondrousId { get { return "BagOfTricks"; } }

		[Constructable]
		public DnDBagOfTricks() : base("BagOfTricks") { }

		public DnDBagOfTricks(Serial serial) : base(serial) { }
	}

	public class DnDCapeOfTheMountebank : DnDWondrousItem
	{
		public override string WondrousId { get { return "CapeOfTheMountebank"; } }

		[Constructable]
		public DnDCapeOfTheMountebank() : base("CapeOfTheMountebank") { }

		public DnDCapeOfTheMountebank(Serial serial) : base(serial) { }
	}

	public class DnDCircletOfBlasting : DnDWondrousItem
	{
		public override string WondrousId { get { return "CircletOfBlasting"; } }

		[Constructable]
		public DnDCircletOfBlasting() : base("CircletOfBlasting") { }

		public DnDCircletOfBlasting(Serial serial) : base(serial) { }
	}

}
