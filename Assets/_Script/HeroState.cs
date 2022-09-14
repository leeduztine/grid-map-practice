using UnityEngine;

namespace GridMap
{
    [CreateAssetMenu(fileName = "New Hero", menuName = "Hero", order = 0)]
    public class HeroState : ScriptableObject
    {
        public int id;
        public string heroName;
        public Sprite sprite;

        public float hp;
        public float atk;
        public float armor;
        public float range;
        public float reload;
    }
}