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
		public static CastResult Cast(DnDPlayerMobile caster, DnDSpell spell, Mobile target, Point3D targetLocation = default(Point3D))
		{
			if (caster == null || spell == null)
			{
				return CastResult.NotACaster;
			}

			CharacterClass charClass = caster.PrimaryClass;

			if (!caster.DnDInitialized || charClass == null || !charClass.CanCastSpells)
			{
				return CastResult.NotACaster;
			}

			if (!IsOnClassList(charClass, spell))
			{
				return CastResult.NotOnClassList;
			}

			if (spell.TargetType == SpellTargetType.Location)
			{
				if (targetLocation == Point3D.Zero)
				{
					return CastResult.NoTarget;
				}

				if (!caster.InRange(targetLocation, Math.Max(1, spell.Range)))
				{
					return CastResult.OutOfRange;
				}
			}
			else if (spell.TargetType == SpellTargetType.Mobile && spell.RequiresTarget)
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

			if (spell.Shape == SpellShape.Single || spell.AreaSize <= 0)
			{
				// Show the cast before it resolves, so a killing blow still has its bolt.
				if (spell.Beneficial)
				{
					DnDSpellVisuals.PlayBeneficial(primary, spell);
				}
				else
				{
					DnDSpellVisuals.Play(caster, primary, spell);
				}

				spell.Effect(caster, caster, primary, targetLocation, slotLevel);

				return CastResult.Success;
			}

			List<Mobile> affected =
				DnDSpellArea.GetTargets(caster, primary, spell.Shape, spell.AreaSize, spell.Beneficial);

			DnDSpellVisuals.PlayArea(caster, primary.Location, primary.Map, spell, spell.AreaSize);

			foreach (Mobile m in affected)
			{
				spell.Effect(caster, caster, m, targetLocation, slotLevel);
			}

			caster.SendMessage("{0} catches {1} creature(s).", spell.Name, affected.Count);

			return CastResult.Success;
		}

		
		private static bool IsOnClassList(CharacterClass charClass, DnDSpell spell)
		{
			return SpellRegistry.GetClassList(charClass).Contains(spell);
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
						else if (roll + DnDRollModifiers.Roll(caster, RollKind.Attack) +
								 Spellcasting.GetSpellAttackBonus(character) <
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

			// The target's own nature answers last, after the save has decided how much of the
			// spell landed. Order matters: halving a resisted total is not the same as resisting a
			// halved one when both round down, and the SRD resolves the save first.
			IDnDTraited traited = target as IDnDTraited;

			if (traited != null && traited.Traits != null)
			{
				damage = traited.Traits.ApplyDamageType(damage, spell.DamageType);
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
