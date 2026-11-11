using System;
using System.Collections.Generic;
using System.Linq;

namespace IE.Model
{
    public class Arena
    {
        private int width = 40;
        private int height = 40;
        private const int MaxAttemptsToFindFreeCell = 100;
        private Dictionary<Type, List<Entity>> entitiesByType;
        private Random random;
        private Cell[,] grid;

        public void Initialize(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.grid = new Cell[height, width];
            this.random = new Random();
            this.entitiesByType = new Dictionary<Type, List<Entity>>();

            InitializeCellGrid();
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public Cell GetCellAt(GridPosition position)
        {
            int row = position.Y;
            int column = position.X;

            return grid[row, column];
        }

        public bool AreSurroundingsFree(Cell cell, int radius = 1)
        {
            GridPosition center = cell.GetPosition();

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    GridPosition neighborPosition = center.Add(dx, dy);

                    if (IsInsideArena(neighborPosition))
                    {
                        Cell neighborCell = GetCellAt(neighborPosition);

                        if (neighborCell.HasEntities())
                            return false;
                    }
                }
            }

            return true;
        }

        public Cell GetRandomFreeCell(int radius = 0)
        {
            for (int attempts = 0; attempts < MaxAttemptsToFindFreeCell; attempts++)
            {
                int row = random.Next(0, height);
                int column = random.Next(0, width);

                GridPosition position = new GridPosition(column, row);
                Cell cell = GetCellAt(position);

                if (radius == 0 && !cell.HasEntities())
                    return cell;
                else if (radius > 0 && AreSurroundingsFree(cell, radius))
                    return cell;
            }

            throw new TimeoutException($"Couldn't find a random free cell after {MaxAttemptsToFindFreeCell} attempts.");
        }

        public bool HasNeighbourCell(Cell center, GridDirection direction)
        {
            GridPosition centerPosition = center.GetPosition();
            GridPosition neighborPosition = centerPosition.Add(direction.DeltaX, direction.DeltaY);

            return IsInsideArena(neighborPosition);
        }

        public Cell GetCellNeighbour(Cell center, GridDirection direction)
        {
            GridPosition centerPosition = center.GetPosition();
            GridPosition neighborPosition = centerPosition.Add(direction.DeltaX, direction.DeltaY);

            return GetCellAt(neighborPosition);
        }

        public bool AreThereAnyEntitiesOfType<EntityType>() where EntityType : Entity
        {
            var type = typeof(EntityType);

            if (!entitiesByType.ContainsKey(type))
                return false;

            var list = entitiesByType[type];

            return list.Count > 0;
        }

        public EntityType FindNearestEntity<EntityType>(Cell center) where EntityType : Entity
        {
            var type = typeof(EntityType);

            if (!AreThereAnyEntitiesOfType<EntityType>())
                throw new Exception($"Tried to find a {type.Name} but none existed (first ask if any exist with {nameof(AreThereAnyEntitiesOfType)})");

            var position = center.GetPosition();
            var matchingEntities = entitiesByType[type];

            Entity nearestEntity = default;
            float nearestDistance = width * height;

            foreach (var entity in matchingEntities)
            {
                var entityPosition = entity.GetPosition();
                var distance = position.DistanceTo(entityPosition);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEntity = entity;
                }
            }

            return (EntityType)nearestEntity;
        }
        
        public bool IsInsideArena(GridPosition position)
        {
            int row = position.Y;
            int column = position.X;

            bool rowIsInBounds = (row >= 0 && row < height);
            bool columnIsInBounds = (column >= 0 && column < width);

            return rowIsInBounds && columnIsInBounds;
        }

        private void InitializeCellGrid()
        {
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    GridPosition position = new GridPosition(column, row);
                    grid[row, column] = new Cell(this, position);
                }
            }
        }

        public void EntityCreated(Entity entity)
        {
            Type type = entity.GetType();

            if (!entitiesByType.ContainsKey(type))
            {
                entitiesByType[type] = new List<Entity>();
            }

            entitiesByType[type].Add(entity);
        }

        public void EntityDestroyed(Entity entity)
        {
            Type type = entity.GetType();
            entitiesByType[type].Remove(entity);
        }
    }
}