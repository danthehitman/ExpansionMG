using System;
using HML.Expansion.Model;
using HML.Expansion.WorldMap.Tile;
using HML.Expansion.Common.Extensions;

namespace HML.Expansion.Entity
{
    public class HumanEntity : BaseEntity
    {
        private Random random = new Random();

        //Stats
        private float health;
        private float hunger;
        private float thirst;
        private float fatigue;
        private float morale;

        public const string HealthPropertyName = "Health";
        public float Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
                OnPropertyChanged(HealthPropertyName);
            }
        }

        public const string HungerPropertyName = "Hunger";
        public float Hunger
        {
            get
            {
                return hunger;
            }

            set
            {
                hunger = value;
                OnPropertyChanged(HungerPropertyName);
            }
        }

        public const string ThirstPropertyName = "Thirst";
        public float Thirst
        {
            get
            {
                return thirst;
            }

            set
            {
                thirst = value;
                OnPropertyChanged(ThirstPropertyName);
            }
        }

        public const string MoralePropertyName = "Morale";
        public float Morale
        {
            get
            {
                return morale;
            }

            set
            {
                morale = value;
                OnPropertyChanged(MoralePropertyName);
            }
        }

        public const string FatiguePropertyName = "Fatigue";
        public float Fatigue
        {
            get
            {
                return fatigue;
            }

            set
            {
                fatigue = value;
                OnPropertyChanged(FatiguePropertyName);
            }
        }

        //Skills
        public float AgricultureSkill { get; set; }
        public float ArchiologicalSkill { get; set; }
        public float ResearchSkill { get; set; }
        public float ExplorationSkill { get; set; }
        public float ForagingSkill { get; set; }
        public float HuntingSkill { get; set; }
        public float BuildingSkill { get; set; }
        public float FishingSkill { get; set; }
        public float FightingSkill { get; set; }
        public float MiningSkill { get; set; }
        public float HusbandrySkill { get; set; }
        public float SurvivalSkill { get; set; }
        public float CamoSkill { get; set; }
        public float NavigationSkill { get; set; }
        public float MountaineeringSkill { get; set; }
        public float SwimmingSkill { get; set; }

        public float AdjustedAgricultureSkill { get; set; }
        public float AdjustedArchiologicalSkill { get; set; }
        public float AdjustedResearchSkill { get; set; }
        public float AdjustedExplorationSkill { get; set; }
        public float AdjustedForagingSkill { get; set; }
        public float AdjustedHuntingSkill { get; set; }
        public float AdjustedBuildingSkill { get; set; }
        public float AdjustedFishingSkill { get; set; }
        public float AdjustedFightingSkill { get; set; }
        public float AdjustedMiningSkill { get; set; }
        public float AdjustedHusbandrySkill { get; set; }
        public float AdjustedSurvivalSkill { get; set; }
        public float AdjustedCamoSkill { get; set; }
        public float AdjustedNavigationSkill { get; set; }
        public float AdjustedMountaineeringSkill { get; set; }
        public float AdjustedSwimmingSkill { get; set; }

        //Traits
        public float ConstitutionTrait { get; set; }

        public HumanEntity()
        {
            EntityInventory = new Inventory();

            AdjustedAgricultureSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedArchiologicalSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedResearchSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedExplorationSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedForagingSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedHuntingSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedBuildingSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedFishingSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedFightingSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedMiningSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedHusbandrySkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedSurvivalSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedCamoSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedNavigationSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedMountaineeringSkill = random.NextFloatInRange(0.1f, 0.5f);
            AdjustedSwimmingSkill = random.NextFloatInRange(0.1f, 0.5f);

            //AdjustedAgricultureSkill = Random.Range(1f, 2f);
            //AdjustedArchiologicalSkill = Random.Range(1f, 2f);
            //AdjustedResearchSkill = Random.Range(1f, 2f);
            //AdjustedExplorationSkill = Random.Range(1f, 2f);
            //AdjustedForagingSkill = Random.Range(1f, 2f);
            //AdjustedHuntingSkill = Random.Range(1f, 2f);
            //AdjustedBuildingSkill = Random.Range(1f, 2f);
            //AdjustedFishingSkill = Random.Range(1f, 2f);
            //AdjustedFightingSkill = Random.Range(1f, 2f);
            //AdjustedMiningSkill = Random.Range(1f, 2f);
            //AdjustedHusbandrySkill = Random.Range(1f, 2f);
            //AdjustedSurvivalSkill = Random.Range(1f, 2f);
            //AdjustedCamoSkill = Random.Range(1f, 2f);
            //AdjustedNaviationSkill = Random.Random.Range(1f, 2f);

            X = 1;
            Y = 1;
            Health = 100f;
            Hunger = 100f;
            Thirst = 100f;
            Fatigue = 100f;
            Morale = 100f;
        }

        public void MoveEntityToTile(WorldTile tile)
        {
            SetPosition(tile.X, tile.Y);
        }
    }

}
