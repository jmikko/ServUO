import os
import re

directory = r"C:\ServUO\Scripts\Mobiles\DnD\AllMonsters"

for filename in os.listdir(directory):
    if filename.endswith(".cs"):
        filepath = os.path.join(directory, filename)
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()

        # Remove the PackItem lines
        content = re.sub(
            r'\s*// Unique and special flair\s*if \(Utility\.RandomDouble\(\) < [0-9.]+\)\s*PackItem\(new Gold\(Utility\.RandomMinMax\([0-9]+,\s*[0-9]+\)\)\);',
            '', content)
        
        # Or if it doesn't match exactly, just remove PackItem lines
        content = re.sub(r'\s*PackItem\(new Gold\([^\)]+\)\);', '', content)
        
        # Remove OnDeath override
        content = re.sub(
            r'\s*// Special unique behavior added by agents\s*public override void OnDeath\(Container [^)]+\)\s*\{\s*base\.OnDeath\([^)]+\);\s*SrdUniqueLootGenerator\.GenerateLoot\(this, [^)]+\);\s*\}',
            '', content)

        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
print("Finished processing AllMonsters.")
