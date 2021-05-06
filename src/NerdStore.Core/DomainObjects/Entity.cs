using System;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity entityA, Entity entityB)
        {
            var entityAIsNull = ReferenceEquals(entityA, null);
            var entityBIsNull = ReferenceEquals(entityB, null);

            if (entityAIsNull && entityBIsNull) return true;
            if (entityAIsNull || entityBIsNull) return false;

            return entityA.Equals(entityB);
        }

        public static bool operator !=(Entity entityA, Entity entityB)
        {
            return !(entityA == entityB);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 268) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
