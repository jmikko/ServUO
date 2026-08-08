namespace Server.Items
{
	/// <summary>
	/// The player's backpack. Server's own <see cref="Container"/> already provides the container
	/// behaviour, so this only fixes the item id, layer and capacity - the legacy BaseContainer
	/// hierarchy (engraving, dyeing, secure/lockdown, house storage) is not carried over.
	/// </summary>
	public class Backpack : Container
	{
		[Constructable]
		public Backpack()
			: base(0xE75)
		{
			Layer = Layer.Backpack;
			Weight = 3.0;
		}

		public Backpack(Serial serial)
			: base(serial)
		{
		}

		public override int DefaultMaxWeight { get { return 550; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt(); // version
		}
	}
}
