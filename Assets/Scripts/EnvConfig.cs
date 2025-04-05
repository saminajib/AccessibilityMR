    using UnityEngine;

    [CreateAssetMenu(fileName = "EnvConfig", menuName = "Configurations/EnvConfig")]
    public class EnvConfig : ScriptableObject
    {
        public string apiKey;
        public string environment;
    }
