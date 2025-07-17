namespace peretc.AccountParser
{
    public struct BoardData : IEquatable<BoardData>
    {
        public string Name;
        public string Id;

        // Реализация IEquatable<BoardData>
        public bool Equals(BoardData other)
        {
            return Name == other.Name && Id == other.Id;
        }

        // Переопределение Equals(object)
        public override bool Equals(object obj)
        {
            return obj is BoardData other && Equals(other);
        }

        // Переопределение GetHashCode (важно для словарей и HashSet)
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id);
        }

        // Перегрузка оператора ==
        public static bool operator ==(BoardData left, BoardData right)
        {
            return left.Equals(right);
        }

        // Перегрузка оператора !=
        public static bool operator !=(BoardData left, BoardData right)
        {
            return !(left == right);
        }
    }
}
