using System;

namespace Server.Spells.DnD
{
	/// <summary>
	/// Makes a cast visible.
	/// <para>
	/// Until now a spell changed some numbers and printed a line of text, which reads as nothing
	/// happening. A bolt that travels, a flash on the target and a sound are what tell a player
	/// their Fire Bolt actually went off - and, just as importantly, what tells everyone else.
	/// </para>
	/// <para>
	/// Effects are chosen per spell where the data names one, and otherwise fall back to the
	/// school. A spell added tomorrow therefore looks like something without anyone having to
	/// remember to give it art.
	/// </para>
	/// </summary>
	public static class DnDSpellVisuals
	{
		/// <summary>One school's default look: a travelling projectile and a burst on arrival.</summary>
		private struct SchoolEffect
		{
			public int ProjectileId;
			public int ImpactId;
			public int Hue;
			public int Sound;

			public SchoolEffect(int projectile, int impact, int hue, int sound)
			{
				ProjectileId = projectile;
				ImpactId = impact;
				Hue = hue;
				Sound = sound;
			}
		}

		// Art borrowed from UO's own spell effects, picked so the schools read differently at a
		// glance: fire for evocation, green for necromancy, gold for abjuration and so on.
		private static readonly SchoolEffect[] m_BySchool =
		{
			new SchoolEffect(0x374A, 0x373A, 0x0000, 0x1F2), // Abjuration  - shimmer
			new SchoolEffect(0x36D4, 0x3728, 0x0000, 0x1EA), // Conjuration - swirl
			new SchoolEffect(0x374A, 0x375A, 0x0480, 0x1F3), // Divination  - pale blue
			new SchoolEffect(0x374A, 0x374A, 0x04EC, 0x1ED), // Enchantment - violet
			new SchoolEffect(0x36D4, 0x36BD, 0x0000, 0x160), // Evocation   - fire
			new SchoolEffect(0x3779, 0x3728, 0x0481, 0x1E5), // Illusion    - faint
			new SchoolEffect(0x374A, 0x374A, 0x0455, 0x1FB), // Necromancy  - sickly green
			new SchoolEffect(0x36D4, 0x373A, 0x0498, 0x1EB)  // Transmutation - amber
		};

		/// <summary>Plays the cast: a projectile from caster to target, then an impact.</summary>
		public static void Play(Mobile caster, Mobile target, DnDSpell spell)
		{
			if (caster == null || spell == null)
			{
				return;
			}

			SchoolEffect effect = GetEffect(spell.School);

			// Casting itself gets a small flourish, so a spell with no target still shows.
			Effects.SendTargetParticles(caster, 0x376A, 9, 32, effect.Hue, 0, 5005, EffectLayer.Waist, 0);

			if (target == null || target.Deleted || target == caster)
			{
				Effects.PlaySound(caster.Location, caster.Map, effect.Sound);
				return;
			}

			// A travelling bolt reads as direction and range - which of the two of you cast it, and
			// at whom. A flash on the target alone does not.
			Effects.SendMovingParticles(
				caster,
				target,
				effect.ProjectileId,
				7,
				0,
				false,
				true,
				effect.Hue,
				0,
				9502,
				4019,
				0x160,
				0);

			Effects.SendTargetParticles(target, effect.ImpactId, 10, 20, effect.Hue, 0, 5029, EffectLayer.Waist, 0);

			Effects.PlaySound(target.Location, target.Map, effect.Sound);
		}

		/// <summary>
		/// An area spell bursts where it lands rather than flying at one creature, so it gets a
		/// location effect scaled to the shape's size.
		/// </summary>
		public static void PlayArea(Mobile caster, Point3D centre, Map map, DnDSpell spell, int size)
		{
			if (caster == null || map == null || map == Map.Internal)
			{
				return;
			}

			SchoolEffect effect = GetEffect(spell.School);

			// SendLocationEffect takes a point directly. The alternative is stock ServUO's
			// EffectItem, which is a real item spawned and deleted for the sake of one animation -
			// and lives in the parked Scripts tree, which it is not worth reviving for this.
			Effects.SendLocationEffect(centre, map, effect.ImpactId, 30, 10, effect.Hue, 0);

			// A wide area gets a ring of bursts, so its extent is visible rather than implied by a
			// single flash in the middle.
			for (int i = 0; size > 1 && i < 4; ++i)
			{
				int offsetX = i == 0 ? size : i == 1 ? -size : 0;
				int offsetY = i == 2 ? size : i == 3 ? -size : 0;

				Effects.SendLocationEffect(
					new Point3D(centre.X + offsetX, centre.Y + offsetY, centre.Z),
					map,
					effect.ImpactId,
					24,
					10,
					effect.Hue,
					0);
			}

			Effects.PlaySound(centre, map, effect.Sound);
		}

		/// <summary>A beneficial spell glows on its target instead of striking it.</summary>
		public static void PlayBeneficial(Mobile target, DnDSpell spell)
		{
			if (target == null)
			{
				return;
			}

			SchoolEffect effect = GetEffect(spell.School);

			Effects.SendTargetParticles(target, 0x376A, 9, 32, effect.Hue, 0, 5030, EffectLayer.Waist, 0);
			Effects.PlaySound(target.Location, target.Map, 0x1F2);
		}

		private static SchoolEffect GetEffect(SpellSchool school)
		{
			int index = (int)school;

			return index >= 0 && index < m_BySchool.Length ? m_BySchool[index] : m_BySchool[4];
		}
	}
}
