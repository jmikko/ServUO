using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Spells.DnD
{
	public enum CastResult
	{
		Success,
		NotACaster,
		NotOnClassList,
		NoSlotAvailable,
		NoTarget,
		OutOfRange,
		WrongTargetType
	}

	/// <summary>
	/// The one way a spell gets cast. Everything that can refuse a cast is checked here, in one
	/// place, so a spell's own Effect never has to re-check whether it was allowed to happen.
	/// </summary>
	public static class DnDCasting
	{
		/// <summary>
		/// Casts <paramref name="spell"/>, spending the smallest slot that fits (cantrips spend
		/// nothing). Returns why it failed, if it did.
		/// </summary>
		public static CastResult Cast(DnDPlayerMobile caster, DnDSpell spell, Mobile target)
		{
			if (caster == null || spell == null)
			{
				return CastResult.NotACaster;
			}

			CharacterClass charClass = caster.CharacterClass;

			if (!caster.DnDInitialized || charClass == null || !charClass.CanCastSpells)
			{
				return CastResult.NotACaster;
			}

			if (!IsOnClassList(charClass, spell))
			{
				return CastResult.NotOnClassList;
			}

			if (spell.RequiresTarget)
			{
				if (target == null || target.Deleted || !target.Alive)
				{
					return CastResult.NoTarget;
				}

				// Fireballing yourself is a misclick, not a tactic. (Beneficial spells are left
				// free to target anyone, including the caster.)
				if (!spell.Beneficial && target == caster)
				{
					return CastResult.WrongTargetType;
				}

				if (target.Map != caster.Map || !caster.InRange(target, Math.Max(1, spell.Range)))
				{
					return CastResult.OutOfRange;
				}
			}

			int slotLevel = spell.Level;

			if (!spell.IsCantrip)
			{
				slotLevel = caster.FindSlotFor(spell.Level);

				if (slotLevel == 0 || !caster.ConsumeSpellSlot(slotLevel))
				{
					return CastResult.NoSlotAvailable;
				}
			}

			Mobile primary = target ?? caster;

			if (spell.RequiresConcentration)
			{
				DnDConcentration.Begin(caster, spell.Name, spell.Duration, null);
			}

			if (spell.AreaRadius <= 0)
			{
				spell.Effect(caster, caster, primary, slotLevel);

				return CastResult.Success;
			}

			foreach (Mobile affected in GetAreaTargets(caster, primary, spell))
			{
				spell.Effect(caster, caster, affected, slotLevel);
			}

			return CastResult.Success;
		}

		/// <summary>
		/// Everything an area spell catches. A harmful area spares its caster - SRD areas are shapes
		/// the caster places, and every one of them originates somewhere the caster is not - while a
		/// beneficial one includes them.
		/// </summary>
		private static List<Mobile> GetAreaTargets(Mobile caster, Mobile centre, DnDSpell spell)
		{
			var targets = new List<Mobile>();

			if (centre.Map == null || centre.Map == Map.Internal)
			{
				targets.Add(centre);
				return targets;
			}

			foreach (Mobile m in centre.GetMobilesInRange(spell.AreaRadius))
			{
				if (m == null || m.Deleted || !m.Alive)
				{
					continue;
				}

				if (m == caster && !spell.Beneficial)
				{
					continue;
				}

				targets.Add(m);
			}

			return targets;
		}

		private static bool IsOnClassList(CharacterClass charClass, DnDSpell spell)
		{
			return SpellRegistry.GetClassList(charClass.Name).Contains(spell);
		}

		/// <summary>
		/// Rolls a spell attack or a saving throw and reports how much of the spell's damage lands:
		/// all of it, half (a successful save against a spell that only halves), or none.
		/// </summary>
		public static int ApplySpellDamage(
			Mobile caster,
			IDnDCharacter character,
			Mobile target,
			DnDSpell spell,
			int damage)
		{
			switch (spell.Resolution)
			{
				case SpellResolution.SpellAttack:
					{
						// A spell attack is still an attack roll, so conditions on either side
						// swing it exactly as they do for a weapon swing.
						int roll = CombatRules.RollD20(DnDConditions.GetAttackRollMode(caster, target));

						if (roll == 1)
						{
							return 0;
						}

						if (roll == 20)
						{
							damage += damage; // a critical spell hit rolls its dice twice
						}
						else if (roll + Spellcasting.GetSpellAttackBonus(character) <
								 CombatRules.GetArmorClass(target))
						{
							return 0;
						}

						break;
					}
				case SpellResolution.SavingThrow:
					{
						if (CombatRules.CheckSave(target, spell.SaveAbility, Spellcasting.GetSaveDC(character)))
						{
							damage = spell.HalfDamageOnSave ? damage / 2 : 0;
						}

						break;
					}
			}

			if (damage > 0)
			{
				target.Damage(damage, caster);

				DnDConcentration.OnDamaged(target, damage);
			}

			return damage;
		}
	}
}
