using System;
using System.Collections.Generic;
using System.Linq;

namespace IE.Model
{
    public static class Interactions
    {
        private static Dictionary<Type, List<Type>> interactionsPerEntityType = new Dictionary<Type, List<Type>>();

        static Interactions()
        {
            Type[] allTypes = typeof(Interactions).Assembly.GetTypes();
            Type[] interactionTypes = allTypes.Where(IsInteractionType).ToArray();

            foreach (Type interactionType in interactionTypes)
            {
                Type[] genericArguments = interactionType.BaseType.GetGenericArguments();
                Type entityType = genericArguments[0];

                RegisterInteractionType(entityType, interactionType);
            }
        }

        private static bool IsInteractionType(Type type)
        {
            return type.BaseType is not null && type.BaseType.Name == typeof(Interaction<,>).Name;
        }

        public static void CreateBetween(Entity entityA, IEnumerable<Entity> entities)
        {
            foreach (var entityB in entities)
            {
                CreateBetween(entityA, entityB);
            }
        }

        public static void CreateBetween(Entity entityA, Entity entityB)
        {
            Type typeA = entityA.GetType();
            List<Type> interactionTypes = GetInteractionsFor(typeA);

            foreach (Type interactionType in interactionTypes)
            {
                InteractionBase interaction = (InteractionBase)Activator.CreateInstance(interactionType);
                interaction.Interacted(entityA, entityB);
            }
        }

        private static List<Type> GetInteractionsFor(Type type)
        {
            List<Type> allInteractions = new List<Type>();

            Type wantedType = type;

            while (wantedType != typeof(Entity))
            {
                List<Type> interactionsForThisType;
                bool hasInteractions = interactionsPerEntityType.TryGetValue(wantedType, out interactionsForThisType);

                if (hasInteractions)
                {
                    allInteractions.AddRange(interactionsForThisType);
                }

                wantedType = wantedType.BaseType;
            }

            return allInteractions;
        }

        private static void RegisterInteractionType(Type entityType, Type interactionType)
        {
            List<Type> typeList;

            if (interactionsPerEntityType.ContainsKey(entityType))
            {
                typeList = interactionsPerEntityType[entityType];
            }
            else
            {
                typeList = new List<Type>();
                interactionsPerEntityType.Add(entityType, typeList);
            }

            typeList.Add(interactionType);
        }
    }

    public abstract class InteractionBase
    {
        public abstract void Interacted(Entity entityA, Entity entityB);
    }

    public abstract class Interaction<EntityA, EntityB> : InteractionBase where EntityA : Entity where EntityB : Entity
    {
        public override void Interacted(Entity entityA, Entity entityB)
        {
            try
            {
                EntityA castedEntityA = (EntityA)entityA;
                EntityB castedEntityB = (EntityB)entityB;

                Interacted(castedEntityA, castedEntityB);
            }
            catch (InvalidCastException) { }
        }

        public abstract void Interacted(EntityA entityA, EntityB entityB);
    }
}