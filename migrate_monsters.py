"""
Converts Data/DnDMonsters.xml from the flat one-element-per-monster format into the nested
schema with <abilityScores> and <actions>.

Why this file is written the way it is
--------------------------------------
The first version of this script carried attributes across using two hand-written allow-lists.
Anything not named on them was dropped silently, and the run that used it ate two attributes that
had been added earlier the same day: every monster lost its `flavour` sentence, and every monster
lost its `damageType` - which was then replaced by a hardcoded "Bludgeoning" on all 951 actions,
so resistances survived in the data and stopped discriminating between anything.

None of that failed the build. It produced valid XML the server loaded happily.

So the allow-lists are gone. This version copies EVERY attribute by default and names only the
handful that MOVE into the nested action. That inverts the failure mode: a new attribute somebody
adds next month rides along automatically, and the script cannot silently lose data it has never
heard of. The invariant is checked per monster rather than trusted - see verify_no_loss.

Run with no arguments to convert Data/DnDMonsters.xml to Data/DnDMonsters_v2.xml. Nothing is
overwritten; compare the two, then move the new one into place yourself.
"""

import os
import sys
import xml.etree.ElementTree as ET

# The only attributes that do not survive as attributes on <monster>, because they become part of
# the creature's first action instead. Everything else is copied verbatim.
RELOCATED_TO_ACTION = {"attackBonus", "damageDice", "damageType"}

DEFAULT_ABILITY_SCORES = {
    "str": "10",
    "dex": "10",
    "con": "10",
    "int": "10",
    "wis": "10",
    "cha": "10",
}


def already_migrated(monster):
    """A monster that already has an <actions> block is left exactly as it is.

    Re-running a migration over migrated data is the obvious way to lose everything a second
    time: the flat attributes are gone by then, so a naive second pass would build an empty
    action list and throw away the real one.
    """
    return monster.find("actions") is not None


def build_action(monster):
    """The creature's attack, moved out of the flat attributes.

    Returns None when there is nothing to move - a row with no attackBonus and no damageDice is
    a creature that never had an attack, and inventing one for it would be worse than leaving it
    without.
    """
    attack_bonus = monster.get("attackBonus")
    damage_dice = monster.get("damageDice")

    if not attack_bonus and not damage_dice:
        return None

    action = ET.Element("action")

    action.set("name", "Attack")
    action.set("type", "MeleeWeapon")
    action.set("toHit", attack_bonus or "0")
    action.set("reachOrRange", "5 ft.")
    action.set("primaryDamageDice", damage_dice or "1d4")

    # The creature's own damage type, not a constant. Hardcoding one here is what made every
    # attack in the game bludgeoning and quietly disarmed every resistance in the bestiary.
    damage_type = (monster.get("damageType") or "").strip()

    action.set("primaryDamageType", damage_type.capitalize() if damage_type else "Bludgeoning")
    action.set("usage", "AtWill")

    if not damage_type:
        print("  note: {0} has no damageType; its action defaults to Bludgeoning".format(
            monster.get("id", "?")))

    return action


def verify_no_loss(source, result):
    """Every attribute on the way in is on the way out, or was deliberately relocated.

    Checked for each monster rather than assumed, because "it produced valid XML" and "it kept
    the data" are different claims and only the first one is visible without looking.
    """
    expected = set(source.attrib) - RELOCATED_TO_ACTION
    actual = set(result.attrib)

    missing = expected - actual

    if missing:
        raise AssertionError(
            "monster '{0}' lost attribute(s): {1}".format(
                source.get("id", "?"), ", ".join(sorted(missing))))


def convert(input_path, output_path):
    # insert_comments keeps the explanatory header at the top of the data file. ElementTree
    # discards comments by default, which would quietly delete the documentation telling the
    # next person what these columns mean.
    parser = ET.XMLParser(target=ET.TreeBuilder(insert_comments=True))
    tree = ET.parse(input_path, parser=parser)
    root = tree.getroot()

    new_root = ET.Element(root.tag, dict(root.attrib))

    converted = 0
    untouched = 0
    without_attack = 0

    for child in list(root):
        if child.tag is ET.Comment:
            new_root.append(child)
            continue

        if child.tag != "monster":
            new_root.append(child)
            continue

        if already_migrated(child):
            new_root.append(child)
            untouched += 1
            continue

        new_monster = ET.Element("monster")

        # Everything, minus what moves into the action. No allow-list.
        for name, value in child.attrib.items():
            if name not in RELOCATED_TO_ACTION:
                new_monster.set(name, value)

        verify_no_loss(child, new_monster)

        scores = ET.SubElement(new_monster, "abilityScores")

        for name, value in DEFAULT_ABILITY_SCORES.items():
            scores.set(name, value)

        actions = ET.SubElement(new_monster, "actions")

        action = build_action(child)

        if action is not None:
            actions.append(action)
        else:
            without_attack += 1

        # Anything the row already carried as a child element - a hand-written action list, notes -
        # is kept rather than replaced by the generated one.
        for grandchild in list(child):
            if grandchild.tag not in ("abilityScores", "actions"):
                new_monster.append(grandchild)

        new_root.append(new_monster)
        converted += 1

    new_tree = ET.ElementTree(new_root)
    ET.indent(new_tree, space="\t", level=0)
    new_tree.write(output_path, encoding="utf-8", xml_declaration=True)

    print("converted:        {0}".format(converted))
    print("already migrated: {0} (left untouched)".format(untouched))
    print("no attack to move:{0}".format(without_attack))
    print("written to:       {0}".format(output_path))

    if converted == 0 and untouched > 0:
        print("\nNothing to do - every row is already in the new format.")


if __name__ == "__main__":
    here = os.path.dirname(os.path.abspath(__file__))

    input_file = sys.argv[1] if len(sys.argv) > 1 else os.path.join(here, "Data", "DnDMonsters.xml")
    output_file = sys.argv[2] if len(sys.argv) > 2 else os.path.join(here, "Data", "DnDMonsters_v2.xml")

    if not os.path.exists(input_file):
        sys.exit("No such file: {0}".format(input_file))

    if os.path.abspath(input_file) == os.path.abspath(output_file):
        sys.exit("Refusing to overwrite the input in place; give a different output path.")

    convert(input_file, output_file)
