<#
.SYNOPSIS
    Writes an art brief for every spell icon that does not exist yet.

.DESCRIPTION
    The icons themselves have to be drawn elsewhere. What this does is the laborious half: read
    every spell row, work out what the picture should show from the spell's own description,
    school and damage type, and pair it with the exact filename the client will look for.

    That last part is the reason this is a script rather than a list someone types. The loader
    matches a PNG to a spell by filename, so 'Magic Missile.png' works and 'MagicMissile.png'
    silently never appears - and there are 230 chances to make that mistake by hand.

    Re-run it after adding art and it lists only what is still missing.

.PARAMETER SpellData
    Data/DnDSpells.xml. Defaults to the copy beside this script's repository.

.PARAMETER IconDirectory
    Where the finished PNGs live, so already-drawn spells can be skipped.

.PARAMETER OutputDirectory
    Where to write SpellIconBrief.csv and SpellIconBrief.txt.

.EXAMPLE
    ./Tools/Write-SpellIconBrief.ps1
#>

[CmdletBinding()]
param(
    [string] $SpellData = (Join-Path $PSScriptRoot '..\Data\DnDSpells.xml'),
    [string] $IconDirectory = 'C:\ClassicUO\src\ClassicUO.Client\Data\SpellIcons',
    [string] $OutputDirectory = (Join-Path $PSScriptRoot '..\Data')
)

# The look every icon shares, written from the Fireball icon that already exists rather than
# invented - so a batch generated from this brief sits beside the ones already drawn instead of
# looking like a different game.
$style = 'Fantasy game spell icon, single centred subject glowing against a near-black background, ' +
         'painted digital art with strong rim light, no text, no border, no frame, square composition.'

function Get-Element {
    <#
        The colour a spell reads as, taken from what it does rather than its school - a player
        recognises a fire spell by it being orange, not by it being Evocation.
    #>
    param([string] $Name, [string] $Kind)

    $n = $Name.ToLowerInvariant()

    if ($n -match 'fire|flame|burn|scorch|meteor|firebolt') { return 'orange and gold flame' }
    if ($n -match 'frost|ice|cold|freez')                   { return 'pale blue ice and frost' }
    if ($n -match 'lightning|shock|thunder|storm|electr')   { return 'white-blue lightning' }
    if ($n -match 'acid|poison|venom|blight|contagion')     { return 'sickly green corrosion' }
    if ($n -match 'necro|death|undead|wither|grave|bone')   { return 'cold green-black necrotic energy' }
    if ($n -match 'holy|divine|sacred|radiant|sun|daylight|bless') { return 'warm golden radiance' }
    if ($n -match 'heal|cure|restor|revivify|resurrect|mend')      { return 'soft green-white healing light' }
    if ($n -match 'psychic|mind|charm|dominate|suggest|fear')      { return 'violet psychic energy' }

    switch ($Kind) {
        'Healing'    { return 'soft green-white healing light' }
        'Resistance' { return 'translucent blue protective light' }
        'ArmorClass' { return 'translucent blue protective light' }
        'Revive'     { return 'warm golden radiance' }
        'Light'      { return 'clear white light' }
    }

    return $null
}

function Get-SchoolMood {
    param([string] $School)

    switch ($School) {
        'Evocation'      { return 'Raw destructive energy.' }
        'Abjuration'     { return 'A protective ward or barrier.' }
        'Conjuration'    { return 'Something summoned into being.' }
        'Divination'     { return 'An eye, a sigil, or revealed knowledge.' }
        'Enchantment'    { return "An influence over another's will." }
        'Illusion'       { return 'Something half-real and shifting.' }
        'Necromancy'     { return 'Grim, touching on death.' }
        'Transmutation'  { return 'Matter or form being changed.' }
    }

    return ''
}

if (-not (Test-Path $SpellData)) {
    Write-Error "No spell data at $SpellData"
    exit 1
}

[xml] $doc = Get-Content -Path $SpellData -Raw

# Spells that already have art, so a rerun lists only what is left.
$existing = @{}

if (Test-Path $IconDirectory) {
    foreach ($file in Get-ChildItem -Path $IconDirectory -Filter *.png) {
        $existing[$file.BaseName] = $true
    }
}

$rows = New-Object System.Collections.Generic.List[object]

foreach ($spell in $doc.SelectNodes('//spell')) {
    $name = $spell.GetAttribute('name')

    if ([string]::IsNullOrEmpty($name)) { continue }

    # Blindness/Deafness and Enlarge/Reduce have a slash in their name and no file can. The client
    # applies the same substitution when it looks art up, so the two agree.
    $fileName = $name -replace '[\\/:*?"<>|]', '-'

    $done = $existing.ContainsKey($fileName)

    $parts = New-Object System.Collections.Generic.List[string]

    # The description is written for a player, which makes it better art direction than anything
    # derivable from the numbers.
    $description = $spell.GetAttribute('description')

    if (-not [string]::IsNullOrEmpty($description)) {
        $parts.Add($description.TrimEnd('.') + '.')
    }

    $element = Get-Element -Name $name -Kind $spell.GetAttribute('kind')

    if ($element) { $parts.Add("Rendered in $element.") }

    $shape = $spell.GetAttribute('shape')

    if ($shape -and $shape -ne 'Single') { $parts.Add("Suggest a $($shape.ToLowerInvariant()) of effect.") }

    $parts.Add((Get-SchoolMood -School $spell.GetAttribute('school')))
    $parts.Add($style)

    $rows.Add([pscustomobject]@{
        Filename = "$fileName.png"
        Spell    = $name
        Level    = $spell.GetAttribute('level')
        School   = $spell.GetAttribute('school')
        HasArt   = $done
        Prompt   = ($parts -join ' ')
    })
}

$pending = @($rows | Where-Object { -not $_.HasArt })

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$csvPath = Join-Path $OutputDirectory 'SpellIconBrief.csv'
$txtPath = Join-Path $OutputDirectory 'SpellIconBrief.txt'

$pending |
    Select-Object Filename, Spell, Level, School, Prompt |
    Export-Csv -Path $csvPath -NoTypeInformation -Encoding utf8

$text = New-Object System.Text.StringBuilder

[void] $text.AppendLine('Spell icon art brief')
[void] $text.AppendLine('====================')
[void] $text.AppendLine()
[void] $text.AppendLine('Save each image under the exact filename shown. The client matches art to a')
[void] $text.AppendLine('spell by filename, so spacing and punctuation have to match.')
[void] $text.AppendLine()
[void] $text.AppendLine("Target: 44 x 44 PNG with transparency, into $IconDirectory")
[void] $text.AppendLine()

foreach ($row in $pending) {
    [void] $text.AppendLine("=== $($row.Filename) ===")
    [void] $text.AppendLine("Level $($row.Level) $($row.School)")
    [void] $text.AppendLine($row.Prompt)
    [void] $text.AppendLine()
}

Set-Content -Path $txtPath -Value $text.ToString() -Encoding utf8

Write-Host "$($pending.Count) of $($rows.Count) spell(s) still need art."
Write-Host "Wrote $csvPath"
Write-Host "Wrote $txtPath"
