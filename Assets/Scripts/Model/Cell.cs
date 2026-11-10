using System.Linq;
using System.Collections.Generic;
using System;

namespace IE.Model
{
    public class Cell
    {
        private Arena arena;
        private List<Entity> entities;
        private GridPosition position;

        public Cell(Arena arena, GridPosition position)
        {
            this.arena = arena;
            this.position = position;
            this.entities = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        public bool HasEntities()
        {
            return entities.Count > 0;
        }

        public Entity[] GetAllEntities()
        {
            return entities.ToArray();
        }

        public EntityType[] GetEntitiesOfType<EntityType>()
        {
            return entities.OfType<EntityType>().ToArray();
        }

        public Arena GetArena()
        {
            return arena;
        }

        public GridPosition GetPosition()
        {
            return position;
        }
    }
}
