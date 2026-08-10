import xml.etree.ElementTree as ET
import os

def convert_monsters(input_path, output_path):
    tree = ET.parse(input_path)
    root = tree.getroot()
    
    new_root = ET.Element('monsters')
    
    for monster in root.findall('monster'):
        new_monster = ET.Element('monster')
        
        for attr in ['id', 'name', 'corpse', 'body', 'hue', 'sound', 'fightMode', 'ac', 'hp', 'cr', 'size', 'type', 'speed', 'flySpeed', 'swimSpeed', 'climbSpeed', 'burrowSpeed']:
            if attr in monster.attrib:
                new_monster.set(attr, monster.attrib[attr])
                
        for attr in ['immune', 'resist', 'vulnerable', 'conditionImmune', 'darkvision', 'blindsight', 'truesight', 'tremorsense', 'packTactics', 'magicResistance', 'regeneration', 'keenSenses', 'sunlightSensitivity', 'undeadFortitude', 'amphibious', 'incorporeal', 'multiattack']:
            if attr in monster.attrib:
                new_monster.set(attr, monster.attrib[attr])
                
        scores = ET.SubElement(new_monster, 'abilityScores')
        scores.set('str', '10')
        scores.set('dex', '10')
        scores.set('con', '10')
        scores.set('int', '10')
        scores.set('wis', '10')
        scores.set('cha', '10')
        
        actions = ET.SubElement(new_monster, 'actions')
        
        if 'attackBonus' in monster.attrib and 'damageDice' in monster.attrib:
            action = ET.SubElement(actions, 'action')
            action.set('name', 'Attack')
            action.set('type', 'MeleeWeapon')
            action.set('toHit', monster.attrib['attackBonus'])
            action.set('reachOrRange', '5 ft.')
            action.set('primaryDamageDice', monster.attrib['damageDice'])
            action.set('primaryDamageType', 'Bludgeoning')
            action.set('usage', 'AtWill')
                
        new_root.append(new_monster)

    new_tree = ET.ElementTree(new_root)
    ET.indent(new_tree, space="\t", level=0)
    new_tree.write(output_path, encoding='utf-8', xml_declaration=True)

if __name__ == '__main__':
    input_file = os.path.join(os.path.dirname(__file__), 'Data', 'DnDMonsters.xml')
    output_file = os.path.join(os.path.dirname(__file__), 'Data', 'DnDMonsters_v2.xml')
    convert_monsters(input_file, output_file)
    print("Conversion complete.")
