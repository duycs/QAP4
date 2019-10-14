using System;

namespace QAP4.Domain.Core.Models
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        /// <summary>
        /// Existing: when the posts be created
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Existing: when the posts be deleted
        /// </summary>
        public DateTime? DeletedDate { get; set; }
        /// <summary>
        /// Existing: when the posts be deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public bool IsTransient()
        {
            return Id == 0;
        }
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}