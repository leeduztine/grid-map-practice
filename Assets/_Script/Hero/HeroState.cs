using UnityEngine;

namespace GridMap
{
    [CreateAssetMenu(fileName = "New Hero", menuName = "Hero", order = 0)]
    public class HeroState : ScriptableObject
    {
        // identity
        public int id;
        public string heroName;
        public Sprite icon;
        public Sprite img;

        // stats
        public int hp;
        public int atk;
        public int armor;
        public int range;
        public float reload;
    }
}