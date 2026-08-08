namespace Server.Engines.Plants
{
    // The plant-growing system (seeds, growth stages, cross-breeding for rare hues) was legacy
    // UO husbandry content with no D&D equivalent and was removed. The craft system still
    // threads a "required plant hue" through resource consumption for dye/pigment recipes, so
    // the hue vocabulary is kept here as an inert stub: only None is ever produced, which makes
    // every plant-hue branch in CraftItem fall through to the normal resource path.
    public enum PlantHue
    {
        None = 0
    }

    public enum PlantPigmentHue
    {
        None = 0
    }

    public interface IPlantHue
    {
        PlantHue PlantHue { get; set; }
    }

    public interface IPigmentHue
    {
        PlantPigmentHue PigmentHue { get; set; }
    }

    public static class PlantPigmentHueInfo
    {
        public static PlantPigmentHue HueFromPlantHue(PlantHue hue)
        {
            return PlantPigmentHue.None;
        }
    }
}
