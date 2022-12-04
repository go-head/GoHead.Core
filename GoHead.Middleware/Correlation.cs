using System;

namespace GoHead.Middleware
{
    public class Correlation
    {
        public static string PARAMNAME = "x-correlation-id";
        public Guid Id { get; private set; }

        public void SetId(Guid guid)
        {
            Id = guid;
        }

        public static implicit operator Correlation(Guid guid) => new() { Id = guid };

        public static implicit operator Guid(Correlation correlation) => correlation.Id;

        public override string ToString() => $"{Id}";
    }
}