using System;
using System.Collections.Generic;

public class Example1_3
{
    public interface IEntityComponent { }

    public interface IEntity
    {
        void RegisterComponent<T>(T component) where T : IEntityComponent;
        T GetComponent<T>() where T : IEntityComponent;
    }
    
    public class HealsComponent : IEntityComponent
    {
        public int Heals;
    }
    
    public class Player : IEntity
    {
        private Dictionary<Type, IEntityComponent> components;

        public void RegisterComponent<T>(T component) where T : IEntityComponent
        {
            components.Add(typeof(T), component);
        }

        public T GetComponent<T>() where T : IEntityComponent
        {
            return (T)components[typeof(T)];
        }
    }
    
    public class DamageSystem
    {
        public bool TryDoDamage(IEntity entity, ComponentSettings settings)
        {
            var healsComponent = entity.GetComponent<HealsComponent>();

            if (healsComponent == null)
            {
                return false;
            }

            healsComponent.Heals -= settings.Damage;
            return true;
        }
    }
    
    [Serializable]
    public class ComponentSettings
    {
        public int Damage { get; }
    }
    
    [Serializable]
    public class EntitySettings
    {
        // string - name of Type for deserialization
        public Dictionary<string, IEntityComponent> Components;
    }
    
    class Program
    {
        private const string NewPlayerPath = "EntitySettings.json";
        private const string SettingsPath = "ComponentSettings.json";

        protected static Player player;

        public static void Main(string[] args)
        {

            player = new Player();
            var entitySettings = new EntitySettings(); //Serializer.LoadFromFile<EntitySettings>(NewPlayerPath);
            
            foreach (var component in entitySettings.Components.Values)
            {
                player.RegisterComponent(component);
            }

            var settings = new ComponentSettings(); //Serializer.LoadFromFile<Settings>(SettingsPath);
            var damageSystem = new DamageSystem();

            damageSystem.TryDoDamage(player, settings);
        }
    }
}
